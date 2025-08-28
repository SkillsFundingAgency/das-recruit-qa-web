using Recruit.Vacancies.Client.Domain.Messaging;
using MediatR;

namespace Recruit.Vacancies.Client.Application.Commands;

public class SetupProviderCommand : ICommand, IRequest<Unit>
{
    public long Ukprn { get; private set; }

    public SetupProviderCommand(long ukprn)
    {
        Ukprn = ukprn;
    }
}