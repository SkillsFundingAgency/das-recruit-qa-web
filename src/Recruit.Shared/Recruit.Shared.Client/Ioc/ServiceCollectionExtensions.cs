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
using Recruit.Vacancies.Client.Infrastructure.Client;
using Recruit.Vacancies.Client.Infrastructure.EventStore;
using Recruit.Vacancies.Client.Infrastructure.Messaging;
using Recruit.Vacancies.Client.Infrastructure.OuterApi;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Configurations;
using Recruit.Vacancies.Client.Infrastructure.OuterApi.Interfaces;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.ApprenticeshipProgrammes;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.BannedPhrases;
using Recruit.Vacancies.Client.Infrastructure.ReferenceData.Profanities;
using Recruit.Vacancies.Client.Infrastructure.Reports;
using Recruit.Vacancies.Client.Infrastructure.Repositories;
using Recruit.Vacancies.Client.Infrastructure.Services;
using Recruit.Vacancies.Client.Infrastructure.Services.Report;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProvider;
using Recruit.Vacancies.Client.Infrastructure.Services.TrainingProviderSummaryProvider;
using Recruit.Vacancies.Client.Infrastructure.StorageQueue;
using Recruit.Vacancies.Client.Infrastructure.VacancyReview;
using SFA.DAS.EAS.Account.Api.Client;
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
        RegisterRepositories(services);
        RegisterOutOfProcessEventDelegatorDeps(services);
        RegisterQueueStorageServices(services, configuration);
        AddValidation(services);
        AddRules(services);
        RegisterMediatR(services);
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

        // Infrastructure Services
        services.AddTransient<ITrainingProviderService, TrainingProviderService>();
        services.AddTransient<ITrainingProviderSummaryProvider, TrainingProviderSummaryProvider>();
        services.AddHttpClient<IRecruitOuterApiClient, RecruitOuterApiClient>();
        services.AddTransient<IQaReportService, QaReportService>();

        // Reference Data Providers
        services.AddTransient<IApprenticeshipProgrammeProvider, ApprenticeshipProgrammeProvider>();
        services.AddTransient<IProfanityListProvider, ProfanityListProvider>();
        services.AddTransient<IBannedPhrasesProvider, BannedPhrasesProvider>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddTransient<IVacancyRepository, SqlVacancyRepository>();
            
        services.AddTransient<IVacancyReviewRepository, VacancyReviewService>();
        services.AddTransient<IVacancyReviewQuery, VacancyReviewService>();
    }

    private static void RegisterOutOfProcessEventDelegatorDeps(IServiceCollection services)
    {
        services.AddTransient<IEventStore, StorageQueueEventStore>();
    }

    private static void RegisterQueueStorageServices(IServiceCollection services, IConfiguration configuration)
    {
        var recruitStorageConnectionString = configuration.GetConnectionString("QueueStorage");
        services.AddTransient<IRecruitQueueService>(_ => new RecruitStorageQueueService(recruitStorageConnectionString));
    }

    private static void AddValidation(IServiceCollection services)
    {
        services.AddTransient(typeof(IEntityValidator<,>), typeof(EntityValidator<,>));

        services.AddTransient<AbstractValidator<ApplicationReview>, ApplicationReviewValidator>();
        services.AddTransient<IValidator<VacancyReview>, VacancyReviewValidator>();
    }

    private static void AddRules(IServiceCollection services)
    {
        services.AddTransient<RuleSet<Vacancy>, VacancyRuleSet>();
    }

    private static void RegisterClients(IServiceCollection services)
    {
        services
            .AddTransient<IQaVacancyClient, QaVacancyClient>()
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