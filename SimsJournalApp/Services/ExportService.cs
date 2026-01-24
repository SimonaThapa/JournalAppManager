using Microsoft.UI.Xaml.Documents;
using System.Reflection.Metadata;
using Windows.Data.Pdf;

public class ExportService
{
    private readonly IWebHostEnvironment _env;

    public ExportService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> ExportJournalEntriesAsPdf(List<JournalEntry> entries)
    {
        string folder = Path.Combine(_env.ContentRootPath, "Exports");
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        string fileName = $"JournalExport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string filePath = Path.Combine(folder, fileName);

        using (var writer = new PdfWriter(filePath))
        using (var pdf = new PdfDocument(writer))
        using (var doc = new Document(pdf))
        {
            foreach (var e in entries)
            {
                doc.Add(new Paragraph($"Date: {e.JournalDate:yyyy-MM-dd}"));
                doc.Add(new Paragraph($"Mood: {e.PrimaryMood}"));
                doc.Add(new Paragraph($"Tags: {string.Join(", ", e.Tags.Select(t => t.Name))}"));
                doc.Add(new Paragraph("Content:"));
                doc.Add(new Paragraph(e.Content));
                doc.Add(new Paragraph("----------------------------------------------------"));
            }
        }

        return filePath;
    }
}

