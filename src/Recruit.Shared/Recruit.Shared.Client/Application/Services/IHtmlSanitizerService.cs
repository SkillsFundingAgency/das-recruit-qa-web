namespace Recruit.Vacancies.Client.Application.Services;

public interface IHtmlSanitizerService
{
    bool IsValid(string html);
}