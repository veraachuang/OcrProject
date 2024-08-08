using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.Storage.Blobs;
using OcrProject;
using static OcrProject.DataConverter;

// Initialize clients
var credential = new AzureKeyCredential(Settings.Key);
var client = new DocumentAnalysisClient(new Uri(Settings.Endpoint), credential);
var blobContainerClient = new BlobContainerClient(Settings.BlobConnectionString, Settings.ContainerName);

await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
{
    var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
    using var stream = new MemoryStream(); 
    await blobClient.DownloadToAsync(stream);
    stream.Position = 0;

    await WriteJsonFile(blobItem.Name, await Scanner(stream,"prebuilt-invoice"));
}


async Task<IReadOnlyList<InvoiceFields>> Scanner(Stream stream, string modelId)
{
    var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, modelId, stream);
    var result = operation.Value;
    return result.Documents.Select(Convert).ToList();    
}