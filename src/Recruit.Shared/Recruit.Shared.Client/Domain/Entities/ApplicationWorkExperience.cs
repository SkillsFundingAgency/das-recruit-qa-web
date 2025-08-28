using System;

namespace Recruit.Vacancies.Client.Domain.Entities;

public class ApplicationWorkExperience
{
    public string Employer { get; set; }
    public string JobTitle { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}
public class ApplicationJob
{
    public string Employer { get; set; }
    public string JobTitle { get; set; }
    public string Description { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}