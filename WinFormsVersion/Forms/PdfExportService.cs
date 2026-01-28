using System;
using System.Collections.Generic;
using System.IO;
using SimsAppJournal.Models;
using SimsAppJournal.Services;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;

namespace SimsAppJournal.Services
{
    public class PdfExportService
    {
        private readonly JournalService journalService;

        public PdfExportService()
        {
            journalService = new JournalService();
        }

        public void ExportEntries(DateTime startDate, DateTime endDate, string filePath)
        {
            List<JournalEntry> entries = journalService.GetAllEntries();
            entries = entries.FindAll(e => e.CreatedAt.Date >= startDate.Date && e.CreatedAt.Date <= endDate.Date);

            if (entries.Count == 0)
                throw new Exception("No journal entries found in this date range.");

            using (var writer = new PdfWriter(filePath))
            using (var pdf = new PdfDocument(writer))
            {
                var doc = new Document(pdf);

                // Use built-in fonts
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont italic = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
                PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                foreach (var entry in entries)
                {
                    // Title (bold)
                    doc.Add(new Paragraph(entry.Title).SetFont(bold).SetFontSize(16));

                    // Date & Mood (italic)
                    string moodLine = $"Date: {entry.CreatedAt:yyyy-MM-dd} | Mood: {entry.PrimaryMood}";
                    if (!string.IsNullOrEmpty(entry.SecondaryMood1))
                        moodLine += $", {entry.SecondaryMood1}";
                    if (!string.IsNullOrEmpty(entry.SecondaryMood2))
                        moodLine += $", {entry.SecondaryMood2}";
                    doc.Add(new Paragraph(moodLine).SetFont(italic).SetFontSize(10));

                    // Category & Tags 
                    if (!string.IsNullOrEmpty(entry.Category))
                        doc.Add(new Paragraph("Category: " + entry.Category).SetFont(normal).SetFontSize(10));
                    if (!string.IsNullOrEmpty(entry.Tags))
                        doc.Add(new Paragraph("Tags: " + entry.Tags).SetFont(normal).SetFontSize(10));

                    // Content 
                    doc.Add(new Paragraph(entry.Content).SetFont(normal).SetFontSize(12));

                    // Divider
                    doc.Add(new Paragraph("\n--------------------------------------\n").SetFont(normal));
                }

                doc.Close();
            }
        }
    }
}

