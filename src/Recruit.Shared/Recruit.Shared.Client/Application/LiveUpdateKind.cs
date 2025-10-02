using System;

namespace Recruit.Vacancies.Client.Application;

[Flags]
public enum LiveUpdateKind
{
    None,
    ClosingDate,
    StartDate
}