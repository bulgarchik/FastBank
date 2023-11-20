namespace FastBank.Domain
{
    public class TransactionsReport
    {
        public TransactionsReport(Guid reportId, DateTime createdOn, string pathToFile, User createdBy)
        {
            ReportId = reportId;
            CreatedOn = createdOn;
            PathToFile = pathToFile;
            CreatedBy = createdBy;
        }
        public Guid ReportId { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string PathToFile { get; private set; }
        public User CreatedBy { get; private set; }
    }
}
