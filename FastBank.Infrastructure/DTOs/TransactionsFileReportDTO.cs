using FastBank.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    [Table("TransactionsReports")]
    public class TransactionsFileReportDTO
    {
        private TransactionsFileReportDTO() { }
        public TransactionsFileReportDTO(TransactionsFileReport transactionsFileReport)
        {
            ReportId = transactionsFileReport.ReportId;
            CreatedOn = transactionsFileReport.CreatedOn;
            PathToFile = transactionsFileReport.PathToFile;
            UserId = transactionsFileReport.CreatedBy.Id;
            CreatedBy = transactionsFileReport.CreatedBy;
        }
        [Key]
        public Guid ReportId { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public string PathToFile { get; private set; } = string.Empty;
        public Guid UserId { get; private set; }
        [ForeignKey(nameof(UserId))]
        public User CreatedBy { get; private set; }


        public TransactionsFileReport ToDomainObj()
        {
            return new TransactionsFileReport(ReportId, CreatedOn, PathToFile, CreatedBy);
        }
    }


}
