using System.Net.Http;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class HttpClientComponent : Entity, IAwake
    {
        public HttpClient HttpClient { get; set; }
    }

    public struct HttpDownloadContext
    {
        public string Url;
        public string WriteToFile;
    }
}