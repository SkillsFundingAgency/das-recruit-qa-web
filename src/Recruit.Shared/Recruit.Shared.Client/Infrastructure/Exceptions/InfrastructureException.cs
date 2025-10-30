using System;

namespace Recruit.Vacancies.Client.Infrastructure.Exceptions;

[Serializable]
public class InfrastructureException(string message) : Exception(message);