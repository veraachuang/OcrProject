using System.Text.Json;
using System.Text.Json.Serialization;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.Storage.Blobs;
using OcrProject;

// initialize documentanalysisclient
var endpoint = "https://di-poc-free-seasia-001.cognitiveservices.azure.com/";
var key = "358d2370e07c46ae862b45abf90d00f6";
var blobStorageConnnectionString =
    "DefaultEndpointsProtocol=https;AccountName=docintelpoceasia001;AccountKey=IelmCQ4a2WqGwSz6ZDFc3CtpWXcBXcN9OEluWHnLxi8clV0sYF/EiVkWlQMKD9ZsBM8HkVEf+Yx5+AStTYZK0w==;EndpointSuffix=core.windows.net";
var containerName = "testing";

var credential = new AzureKeyCredential(key);
var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

// initialize blobcontainerclient
var blobContainerClient = new BlobContainerClient(blobStorageConnnectionString, containerName);

await foreach (var blobItem in blobContainerClient.GetBlobsAsync())
{
    var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);

    // download blob content
    using var stream = new MemoryStream();
    await blobClient.DownloadToAsync(stream);
    stream.Position = 0;

    // submit for analysis
    var operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", stream);
    var result = operation.Value;

    var documentIndex = 0;
    var outputPath = "/Users/toponaute/RiderProjects/OcrProject/outputs";

    foreach (var document in result.Documents)
    {
        var invoiceData = new InvoiceFields();
        //var audits = new AuditFields();
        DataConverter.Convert(document, invoiceData);
        //ExtractAuditData.extract(document, audits);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new CurrencyConverter() }
        };
        var jsonString = JsonSerializer.Serialize(invoiceData, options);

        var outputJsonPath = $"{outputPath}/{blobItem.Name}_{documentIndex}.json";
        await File.WriteAllTextAsync(outputJsonPath, jsonString);

        Console.WriteLine($"Analysis result saved to: {outputJsonPath}");
        // var path = Path.GetFullPath(outputJsonPath);
        // Console.WriteLine(path);
        
        documentIndex++;
    }
}