namespace OcrProject;

public class AuditFields
{
    public string? MonitoringID { get; set; }
    public string? MonitoredParty { get; set; }
    public string? amforiID { get; set; }
    public string? ClientAddress { get; set; }
    public string? MonitoringActivity { get; set; }
    public string? MonitoringType { get; set; }
    public string? MonitoringPartner { get; set; }
    public DateTimeOffset? MonitoringStartDate { get; set; }
    public DateTimeOffset? FinishedDate { get; set; }
    public DateTimeOffset? SubmissionDate { get; set; }
    public DateTimeOffset? ExpirationDate { get; set; }
    public string? AnnouncementType { get; set; }
    public string? Site { get; set; }
    public string? SiteID { get; set; }
    public string? OverallRating { get; set; }
    public List<SectionRatings> SectionRatings { get; set; } = new();
}

public class SectionRatings
{
    public string? Findings { get; set; }
    public string? Rating { get; set; }
}