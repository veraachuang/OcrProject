namespace OcrProject;

public interface IOcrScanner
{
    Task<IReadOnlyList<InvoiceFields>> Scanner(Stream stream, string modelId);
}