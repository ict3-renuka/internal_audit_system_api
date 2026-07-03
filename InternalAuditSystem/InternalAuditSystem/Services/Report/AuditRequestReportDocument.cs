using InternalAuditSystem.Models.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InternalAuditSystem.Services.Report
{
    public class AuditRequestReportDocument : IDocument
    {
        private readonly List<AuditRequestReportDto> _data;

        public AuditRequestReportDocument(List<AuditRequestReportDto> data)
        {
            _data = data;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(20);

                page.Header()
                    .Column(col =>
                    {
                        col.Item()
                            .AlignCenter()
                            .Text("Audit Request Report")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Darken4);

                        col.Item().PaddingBottom(10);
                    });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // Meeting Date
                        columns.RelativeColumn(2); // Audit Name
                        columns.RelativeColumn(2); // Preliminary Start
                        columns.RelativeColumn(2); // Audit Firm
                        //columns.RelativeColumn(2); // Audit Manager
                        columns.RelativeColumn(2); // Department
                        columns.RelativeColumn(2); // Info Request
                        columns.RelativeColumn(2); // Info Submit
                        columns.RelativeColumn(2); // Field Work Start
                        columns.RelativeColumn(2); // Field Work End
                        columns.RelativeColumn(2); // Exit Meeting
                        columns.RelativeColumn(2); // Management Discussion
                        columns.RelativeColumn(2); // Report Issued
                        columns.RelativeColumn(2); // Shared To Board
                        columns.RelativeColumn(2); // Audit Committee Date
                        columns.RelativeColumn(2); // Review Reference
                        columns.RelativeColumn(2); // Sector
                        columns.RelativeColumn(2); // Company
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Review Ref").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Sector").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Company").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Department").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Audit Firm").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Meeting Date").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Audit Name").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Preliminary Start").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        //header.Cell().Text("Audit Manager").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Info Request").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Info Submit").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Field Work Start").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Field Work End").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Exit Meeting").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Management Discussion").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Report Issued").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Shared To Board").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Committee Date").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                    });

                    foreach (var item in _data)
                    {
                        table.Cell().Padding(2).Text(item.ReviewReference ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.Sector ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.CompanyName ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.DepartmentName ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.AuditFirm ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.MeetingDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.AuditName ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.PreliminaryStartDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        //table.Cell().Padding(2).Text(item.AuditManager ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.InfoRequestDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.InfoSubmitDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.FieldWorkStartDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.FieldWorkEndDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.ExitMeetingDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.ManagementDiscussionDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.ReportIssuedDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.SharedToBoardDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.AuditCommitteeTableDate?.ToString("yyyy-MM-dd") ?? "-").FontSize(8);
                    }
                });
            });
        }
    }
}