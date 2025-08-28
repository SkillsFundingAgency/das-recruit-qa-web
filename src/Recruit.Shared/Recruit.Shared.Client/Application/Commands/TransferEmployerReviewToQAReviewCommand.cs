using System;
using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class TransferEmployerReviewToQAReviewCommand : ICommand, IRequest<Unit>
{
    public TransferEmployerReviewToQAReviewCommand(Guid vacancyId, Guid userRef, string userEmailAddress, string userName)
    {
        VacancyId = vacancyId;
        UserRef = userRef;
        UserEmailAddress = userEmailAddress;
        UserName = userName;
    }

    public Guid VacancyId { get;}
    public Guid UserRef { get; }
    public string UserEmailAddress { get; }
    public string UserName { get; }
}