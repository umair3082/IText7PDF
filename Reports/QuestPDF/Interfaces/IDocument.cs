using QuestPDF.Infrastructure;

namespace IText7PdfPOC.Reports.QuestPDF.Interfaces
{
    public interface IDocument
    {
        DocumentMetadata GetMetadata();
        DocumentSettings GetSettings();
        void Compose(IDocumentContainer container);
    }
}
