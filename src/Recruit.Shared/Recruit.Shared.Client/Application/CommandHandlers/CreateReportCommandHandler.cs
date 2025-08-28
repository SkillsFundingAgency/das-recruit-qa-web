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

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Unit>
{
    private readonly ILogger<CreateReportCommandHandler> _logger;
    private readonly IReportRepository _repository;
    private readonly ITimeProvider _timeProvider;
    private readonly IRecruitQueueService _queue;

    public CreateReportCommandHandler(
        ILogger<CreateReportCommandHandler> logger,
        IReportRepository repository,
        ITimeProvider timeProvider,
        IRecruitQueueService queue)
    {
        _logger = logger;
        _repository = repository;
        _timeProvider = timeProvider;
        _queue = queue;
    }

    public async Task<Unit> Handle(CreateReportCommand message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating report '{reportType}' with parameters '{reportParameters}' requested by {userId}", message.ReportType, message.Parameters, message.RequestedBy.UserId);

        var now = _timeProvider.Now;

        var report = new Report(
            message.ReportId,
            message.Owner,
            ReportStatus.New,
            message.ReportName,
            message.ReportType,
            message.Parameters,
            message.RequestedBy,
            now);

        await _repository.CreateAsync(report);

        var queueMessage = new ReportQueueMessage
        {
            ReportId = report.Id
        };

        await _queue.AddMessageAsync(queueMessage);

        _logger.LogInformation("Finished create report '{reportType}' with parameters '{reportParameters}' requested by {userId}", message.ReportType, message.Parameters, message.RequestedBy.UserId);
            
        return Unit.Value;
    }
}