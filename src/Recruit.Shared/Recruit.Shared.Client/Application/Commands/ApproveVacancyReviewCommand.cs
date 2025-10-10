using System;
using System.Collections.Generic;
using Recruit.Vacancies.Client.Domain.Entities;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class ApproveVacancyReviewCommand(
    Guid reviewId,
    string manualQaComment,
    List<ManualQaFieldIndicator> manualQaFieldIndicators,
    List<Guid> selectedAutomatedQaRuleOutcomeIds,
    List<ManualQaFieldEditIndicator> manualQaFieldEditIndicators)
    : ICommand, IRequest<Unit>
{
    public Guid ReviewId { get; } = reviewId;
    public string ManualQaComment { get; } = manualQaComment;
    public List<ManualQaFieldIndicator> ManualQaFieldIndicators { get; } = manualQaFieldIndicators;
    public List<Guid> SelectedAutomatedQaRuleOutcomeIds { get; } = selectedAutomatedQaRuleOutcomeIds;
    public List<ManualQaFieldEditIndicator> ManualQaFieldEditIndicators { get; } = manualQaFieldEditIndicators;
}