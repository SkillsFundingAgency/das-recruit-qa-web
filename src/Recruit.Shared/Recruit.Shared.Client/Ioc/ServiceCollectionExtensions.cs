using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Recruit.Vacancies.Client.Application.Aspects;
using Recruit.Vacancies.Client.Application.Cache;
using Recruit.Vacancies.Client.Application.CommandHandlers;
using Recruit.Vacancies.Client.Application.Events;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Queues;
using Recruit.Vacancies.Client.Application.Rules.Engine;
using Recruit.Vacancies.Client.Application.Services;
using Recruit.Vacancies.Client.Application.Services.NextVacancyReview;
using Recruit.Vacancies.Client.Application.Services.Reports;
using Recruit.Vacancies.Client.Application.Services.VacancyComparer;
using Recruit.Vacancies.Client.Application.Validation;
using Recruit.Vacancies.Client.Application.Validation.Fluent;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using Recruit.Vacancies.Client.Domain.Repositories;
using Recruit.Vacancies.Client.Infrastructure.ApplicationReview;
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.EventStore;
using Recruit.Vacancies.Client.Infrastructure.HttpRequestHandlers;
using Recruit.Vacancies.Client.Infrastructure.Messaging;
using Recruit.Vacancies.Client.Infrastructure.Mongo;
using Recruit.Vacancies.Client.Infrastructure.OuterApi;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Configurations;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.QueryStore;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.BannedPhrases;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.Profanities;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.Qualifications;
using Recruit.Vacancies.Client.Infrastructure.Reports;
using Recruit.Vacancies.Client.Infrastructure.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Services;
using Recruit.Vacancies.Client.Infrastructure.Services.EmployerAccount;
using Recruit.Vacancies.Client.Infrastructure.Services.Projections;
using Recruit.Vacancies.Client.Infrastructure.Services.ProviderRelationship;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProviderSummaryProvider;
using Recruit.Vacancies.Client.Infrastructure.Services.VacancySummariesProvider;
using Recruit.Vacancies.Client.Infrastructure.StorageQueue;
using Recruit.Vacancies.Client.Infrastructure.User;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.Http.MessageHandlers;
using SFA.DAS.Http.TokenGenerators;
using System;
using VacancyRuleSet = Recruit.Vacancies.Client.Application.Rules.VacancyRules.VacancyRuleSet;

namespace Recruit.Vacancies.Client.Ioc;

public static class ServiceCollectionExtensions
{
    public static void AddRecruitStorageClient(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpClient()
            .Configure<AccountApiConfiguration>(configuration.GetSection("AccountApiConfiguration"))
            .AddMemoryCache();
        RegisterClients(services);
        RegisterServiceDeps(services, configuration);
        RegisterAccountApiClientDeps(services);
        RegisterMongoQueryStores(services);
        RegisterRepositories(services, configuration);
        RegisterOutOfProcessEventDelegatorDeps(services);
        RegisterQueueStorageServices(services, configuration);
        AddValidation(services);
        AddRules(services);
        RegisterMediatR(services);
        RegisterProviderRelationshipsClient(services, configuration);
    }

    private static void RegisterProviderRelationshipsClient(IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("ProviderRelationshipsApiConfiguration").Get<ProviderRelationshipApiConfiguration>();
        if (config == null)
        {
            services.AddTransient<IProviderRelationshipsService, ProviderRelationshipsService>();
            return;
        }
        services
            .AddHttpClient<IProviderRelationshipsService, ProviderRelationshipsService>(options =>
            {
                options.BaseAddress = new Uri(config.ApiBaseUrl);
            })
            .AddHttpMessageHandler(() => new VersionHeaderHandler())
            .AddHttpMessageHandler(() => new ManagedIdentityHeadersHandler(new ManagedIdentityTokenGenerator(config)));
    }

    private static void RegisterAccountApiClientDeps(IServiceCollection services)
    {
        services.AddSingleton<IAccountApiConfiguration>(kernal => kernal.GetService<IOptions<AccountApiConfiguration>>().Value);
        services.AddTransient<IAccountApiClient, AccountApiClient>();
    }

    private static void RegisterServiceDeps(IServiceCollection services, IConfiguration configuration)
    {
        // Configuration
        services.AddSingleton(configuration);
        services.Configure<NextVacancyReviewServiceConfiguration>(o => o.VacancyReviewAssignationTimeoutMinutes = configuration.GetValue<int>("RecruitConfiguration:VacancyReviewAssignationTimeoutMinutes"));
        services.Configure<RecruitOuterApiConfiguration>(configuration.GetSection("OuterApiConfiguration"));
        services.Configure<RecruitQaOuterApiConfiguration>(configuration.GetSection("RecruitQaOuterApiConfiguration"));

        // Domain services
        services.AddTransient<ITimeProvider, CurrentUtcTimeProvider>();

        // Application Service
        services.AddTransient<INextVacancyReviewService, NextVacancyReviewService>();
        services.AddTransient<IVacancyComparerService, VacancyComparerService>();
        services.AddTransient<ICache, Cache>();
        services.AddTransient<IHtmlSanitizerService, HtmlSanitizerService>();

        //Reporting Service
        services.AddTransient<ICsvBuilder, CsvBuilder>();
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<ProviderApplicationsReportStrategy>();
        services.AddTransient<QaApplicationsReportStrategy>();
        services.AddTransient<Func<ReportType, IReportStrategy>>(serviceProvider => reportType =>
        {
            switch (reportType)
            {
                case ReportType.ProviderApplications:
                    return serviceProvider.GetService<ProviderApplicationsReportStrategy>();
                case ReportType.QaApplications:
                    return serviceProvider.GetService<QaApplicationsReportStrategy>();
                default:
                    throw new Exception($"No report strategy for {reportType}");
            }
        });

        // Infrastructure Services
        services.AddTransient<IEmployerAccountProvider, EmployerAccountProvider>();
        services.AddTransient<ITrainingProviderService, TrainingProviderService>();
        services.AddTransient<ITrainingProviderSummaryProvider, TrainingProviderSummaryProvider>();
        services.AddHttpClient<IRecruitOuterApiClient, RecruitOuterApiClient>();

        // Projection services
        services.AddTransient<IQaDashboardProjectionService, QaDashboardProjectionService>();
        services.AddTransient<IEditVacancyInfoProjectionService, EditVacancyInfoProjectionService>();
        services.AddTransient<IPublishedVacancyProjectionService, PublishedVacancyProjectionService>();
        services.AddTransient<IVacancyApplicationsProjectionService, VacancyApplicationsProjectionService>();
        services.AddTransient<IBlockedOrganisationsProjectionService, BlockedOrganisationsProjectionService>();

        // Reference Data Providers
        services.AddTransient<IMinimumWageProvider, NationalMinimumWageProvider>();
        services.AddTransient<IApprenticeshipProgrammeProvider, ApprenticeshipProgrammeProvider>();
        services.AddTransient<IQualificationsProvider, QualificationsProvider>();
        services.AddTransient<IProfanityListProvider, ProfanityListProvider>();
        services.AddTransient<IBannedPhrasesProvider, BannedPhrasesProvider>();
    }

    private static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        var mongoConnectionString = configuration.GetConnectionString("MongoDb");

        services.Configure<MongoDbConnectionDetails>(options =>
        {
            options.ConnectionString = mongoConnectionString;
        });

        MongoDbConventions.RegisterMongoConventions();

        services.AddTransient<MongoDbCollectionChecker>();
        //Repositories
        //----------------------------------------------------------------------------------------
        // WARNING: Do not change the order of these registrations
        //----------------------------------------------------------------------------------------
        services.AddKeyedTransient<IVacancyRepository, SqlVacancyRepository>("sql");
        services.AddKeyedTransient<IVacancyRepository, MongoDbVacancyRepository>("mongo");
        services.AddTransient<IVacancyRepository, MigrationVacancyRepository>();
        //----------------------------------------------------------------------------------------
            
        services.AddTransient<IVacancyReviewRepository, VacancyReviewService>();
        services.AddTransient<IVacancyReviewRepository, MongoDbVacancyReviewRepository>();
        services.AddTransient<IVacancyReviewRepositoryRunner, VacancyReviewRepositoryRunner>();

            
        services.AddTransient<IUserRepository, MongoDbUserRepository>();
        services.AddTransient<IUserRepositoryRunner, UserRepositoryRunner>();
        services.AddTransient<IUserWriteRepository, MongoDbUserRepository>();
        services.AddTransient<IUserWriteRepository, UserService>();
            

        services.AddTransient<IApplicationWriteRepository, ApplicationReviewService>();
        services.AddTransient<IApplicationWriteRepository, MongoDbApplicationReviewRepository>();
            
        services.AddTransient<ISqlDbRepository, ApplicationReviewService>();
        services.AddTransient<IMongoDbRepository, MongoDbApplicationReviewRepository>();

        services.AddTransient<IApplicationReviewRepository, MongoDbApplicationReviewRepository>();

        services.AddTransient<IApplicationReviewRepositoryRunner, ApplicationReviewRepositoryRunner>();


        services.AddTransient<IEmployerProfileRepository, MongoDbEmployerProfileRepository>();
        services.AddTransient<IReportRepository, MongoDbReportRepository>();
        services.AddTransient<IUserNotificationPreferencesRepository, MongoDbUserNotificationPreferencesRepository>();
        services.AddTransient<IBlockedOrganisationRepository, MongoDbBlockedOrganisationRepository>();

        //Queries
        services.AddTransient<IVacancyQuery, MongoDbVacancyRepository>();
        services.AddTransient<IVacancyReviewQuery, MongoDbVacancyReviewRepository>();
        services.AddTransient<IApplicationReviewQuery, MongoDbApplicationReviewRepository>();
        services.AddTransient<IBlockedOrganisationQuery, MongoDbBlockedOrganisationRepository>();

        services.AddTransient<IQueryStoreReader, QueryStoreClient>();
        services.AddTransient<IQueryStoreWriter, QueryStoreClient>();

        services.AddTransient<IReferenceDataReader, MongoDbReferenceDataRepository>();
        services.AddTransient<IReferenceDataWriter, MongoDbReferenceDataRepository>();
    }

    private static void RegisterOutOfProcessEventDelegatorDeps(IServiceCollection services)
    {
        services.AddTransient<IEventStore, StorageQueueEventStore>();
    }

    private static void RegisterQueueStorageServices(IServiceCollection services, IConfiguration configuration)
    {
        var recruitStorageConnectionString = configuration.GetConnectionString("QueueStorage");
        var communicationStorageConnectionString = configuration.GetConnectionString("CommunicationsStorage");

        services.AddTransient<IRecruitQueueService>(_ => new RecruitStorageQueueService(recruitStorageConnectionString));
        services.AddTransient<ICommunicationQueueService>(_ => new CommunicationStorageQueueService(communicationStorageConnectionString));
    }

    private static void RegisterMongoQueryStores(IServiceCollection services)
    {
        services.AddTransient<IQueryStore, MongoQueryStore>();
        services.AddTransient<IQueryStoreHouseKeepingService, MongoQueryStore>();
    }

    private static void AddValidation(IServiceCollection services)
    {
        services.AddTransient<AbstractValidator<Vacancy>, FluentVacancyValidator>();
        services.AddTransient(typeof(IEntityValidator<,>), typeof(EntityValidator<,>));

        services.AddTransient<AbstractValidator<ApplicationReview>, ApplicationReviewValidator>();
        services.AddTransient<AbstractValidator<VacancyReview>, VacancyReviewValidator>();

        services.AddTransient<AbstractValidator<UserNotificationPreferences>, UserNotificationPreferencesValidator>();
        services.AddTransient<AbstractValidator<Qualification>, QualificationValidator>();
    }

    private static void AddRules(IServiceCollection services)
    {
        services.AddTransient<RuleSet<Vacancy>, VacancyRuleSet>();
    }

    private static void RegisterClients(IServiceCollection services)
    {
        services
            .AddTransient<IRecruitVacancyClient, VacancyClient>()
            .AddTransient<IQaVacancyClient, QaVacancyClient>()
            .AddTransient<IRecruitOuterApiVacancyClient, RecruitOuterApiVacancyClient>()
            .AddTransient<IRecruitQaOuterApiVacancyClient, RecruitQaOuterApiVacancyClient>()
            .AddTransient<IRecruitQaOuterApiClient, RecruitQaOuterApiClient>();

    }


    private static void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(ApproveVacancyReviewCommandHandler).Assembly));
        services
            .AddTransient<IMessaging, MediatrMessaging>()
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
    }
}