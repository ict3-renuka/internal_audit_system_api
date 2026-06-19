using InternalAuditSystem.Models.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InternalAuditSystem.Services.Report
{
    public class ObservationReportDocument : IDocument
    {
        private readonly List<CombinedObservation> _data;

        public ObservationReportDocument(List<CombinedObservation> data)
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
                            .Text("Internal Audit Observation Report")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Darken4);

                        col.Item().PaddingBottom(10);
                        });

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(60);    // Review Reference
                        columns.ConstantColumn(60);   // Area
                        columns.RelativeColumn(2);     // Subject
                        columns.RelativeColumn(2);     // Risk
                        columns.RelativeColumn(2);     // Recommendation
                        columns.ConstantColumn(80);  // Department
                        columns.ConstantColumn(80);  // Internal Dept
                        columns.RelativeColumn(2);  // Management Res
                        columns.RelativeColumn(2);  // Corrective Action Plan
                        columns.ConstantColumn(50);   // Status
                        columns.ConstantColumn(60);   //Remark
                        columns.ConstantColumn(70);  // Date
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Review Ref.").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Area").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Subject").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Risk").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Recommendation").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Department").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Internal Dept").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Mgmt Response").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Action Plan").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Status").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Remark").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                        header.Cell().Text("Date").Bold().FontSize(11).FontColor(Colors.Blue.Darken2);
                    });

                    foreach (var item in _data)
                    {
                        table.Cell().Padding(2).Text(item.ReviewReference ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.Area ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.Subject ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.RiskAndRootCause ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.Recommendation ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.DepartmentName ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.InternalDepartmentName ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.ManagementResponse ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.CorrectiveActionPlan ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.Status ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.Remark ?? "-").FontSize(8);
                        table.Cell().Padding(2).Text(item.ObservationCreationDate.ToString("yyyy-MM-dd")).FontSize(8);
                    }
                });
            });
        }
    }
}
