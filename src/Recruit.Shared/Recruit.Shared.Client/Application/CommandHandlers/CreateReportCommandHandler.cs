using Recruit.Vacancies.Client.Application.Commands;
using Recruit.Vacancies.Client.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Recruit.Vacancies.Client.Application.Providers;
using Recruit.Vacancies.Client.Application.Queues;
using Recruit.Vacancies.Client.Application.Queues.Messages;
using Recruit.Vacancies.Client.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Recruit.Vacancies.Client.Application.CommandHandlers;

public class CreateReportCommandHandler(
    ILogger<CreateReportCommandHandler> logger,
    IReportRepository repository,
    ITimeProvider timeProvider,
    IRecruitQueueService queue)
    : IRequestHandler<CreateReportCommand, Unit>
{
    public async Task<Unit> Handle(CreateReportCommand message, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating report '{reportType}' with parameters '{reportParameters}' requested by {userId}", message.ReportType, message.Parameters, message.RequestedBy.UserId);

        var now = timeProvider.Now;

        var report = new Report(
            message.ReportId,
            message.Owner,
            ReportStatus.New,
            message.ReportName,
            message.ReportType,
            message.Parameters,
            message.RequestedBy,
            now);

        await repository.CreateAsync(report);

        var queueMessage = new ReportQueueMessage
        {
            ReportId = report.Id
        };

        await queue.AddMessageAsync(queueMessage);

        logger.LogInformation("Finished create report '{reportType}' with parameters '{reportParameters}' requested by {userId}", message.ReportType, message.Parameters, message.RequestedBy.UserId);
            
        return Unit.Value;
    }
}