using Microsoft.Extensions.Configuration;

namespace OcrProject;

public class Settings
{
    public static string Endpoint => "https://di-poc-free-seasia-001.cognitiveservices.azure.com/";
    public static string Key => "358d2370e07c46ae862b45abf90d00f6";
    public static string BlobConnectionString => "DefaultEndpointsProtocol=https;AccountName=docintelpoceasia001;AccountKey=IelmCQ4a2WqGwSz6ZDFc3CtpWXcBXcN9OEluWHnLxi8clV0sYF/EiVkWlQMKD9ZsBM8HkVEf+Yx5+AStTYZK0w==;EndpointSuffix=core.windows.net";
    public const string ContainerName = "testing";


}