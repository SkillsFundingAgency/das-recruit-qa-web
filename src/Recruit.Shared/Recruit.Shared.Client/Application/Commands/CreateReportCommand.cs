using System;
using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class CreateReportCommand(
    Guid reportId,
    ReportOwner owner,
    ReportType reportType,
    Dictionary<string, object> parameters,
    VacancyUser requestedBy,
    string reportName)
    : ICommand, IRequest<Unit>
{
    public Guid ReportId { get; set; } = reportId;
    public ReportOwner Owner { get; set; } = owner;
    public ReportType ReportType { get; set; } = reportType;
    public Dictionary<string, object> Parameters { get; set; } = parameters;
    public VacancyUser RequestedBy { get; set; } = requestedBy;
    public string ReportName { get; set; } = reportName;
}