using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Recruit.Vacancies.Client.Domain.Entities;

public record QaCsvReport
{
    [JsonProperty("Vacancy title")]
    public string? VacancyTitle { get; set; }
    [JsonProperty("Vacancy reference number")]
    public long VacancyReference { get; set; }
    [JsonProperty("Submission number")]
    public int SubmissionNumber { get; set; }
    [JsonProperty("Date submitted")]
    public DateTime? DateSubmitted { get; set; }
    [JsonProperty("SLA deadline")]
    public DateTime? SlaDeadline { get; set; }
    [JsonProperty("Review started")]
    public DateTime? ReviewStarted { get; set; }
    [JsonProperty("Review completed")]
    public DateTime? ReviewCompleted { get; set; }
    [JsonProperty("Time taken to review")]
    public string TimeTakenToReview { get; set; } = "";
    [JsonProperty("SLA exceeded by (hours)")]
    public string SlaExceededByHours { get; set; } = "";
    [JsonProperty("Outcome")]
    public string? Outcome { get; set; }
    [JsonProperty("Number of issues reported")]
    public int NumberOfIssuesReported { get; set; }
    [JsonProperty("Vacancy submitted by")]
    public string? VacancySubmittedBy { get; set; }
    [JsonProperty("Vacancy submitted by user")]
    public string? VacancySubmittedByUser { get; set; }
    [JsonProperty("Employer")]
    public string? Employer { get; set; }
    [JsonProperty("Display name")]
    public string? DisplayName { get; set; }
    [JsonProperty("Training provider")]
    public string? TrainingProvider { get; set; }
    [JsonProperty("Vacancy postcode")]
    public string? VacancyPostcode { get; set; }
    [JsonProperty("Standard / Framework")]
    public string? CourseName { get; set; }
    [JsonProperty("level")]
    public string? CourseLevel { get; set; }
    [JsonProperty("Referred Fields")]
    public List<string> ReferredFields { get; set; } = [];
    [JsonProperty("Reviewed by")]
    public string? ReviewedBy { get; set; }
    [JsonProperty("Reviewer Comment")]
    public string? ReviewerComment { get; set; }
    

    public static implicit operator QaCsvReport(QaReport report) =>
        new()
        {
            VacancyTitle = report.VacancyTitle,
            VacancyReference = report.VacancyReference,
            SubmissionNumber = report.SubmissionNumber,
            DateSubmitted = report.DateSubmitted,
            SlaDeadline = report.SlaDeadline,
            ReviewStarted = report.ReviewStarted,
            ReviewCompleted = report.ReviewCompleted,
            Outcome = report.Outcome,
            SlaExceededByHours = report.SlaExceededByHours,
            TimeTakenToReview = report.TimeTakenToReview,
            NumberOfIssuesReported = report.NumberOfIssuesReported,
            VacancySubmittedBy = report.VacancySubmittedBy,
            VacancySubmittedByUser = report.VacancySubmittedByUser,
            Employer = report.Employer,
            DisplayName = report.DisplayName,
            TrainingProvider = report.TrainingProvider,
            VacancyPostcode = report.VacancyPostcode,
            ReferredFields = report.ReferredFields,
            ReviewedBy = report.ReviewedBy,
            ReviewerComment = report.ReviewerComment,
            CourseName = report.CourseName,
            CourseLevel = report.CourseLevel
        };
}