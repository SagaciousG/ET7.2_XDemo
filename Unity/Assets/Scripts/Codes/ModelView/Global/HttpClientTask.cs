using System.Net.Http;

namespace ET.Client
{
    [ChildOf(typeof(HttpClientComponent))]
    public class HttpClientTask : Entity, IAwake
    {
        public HttpClient HttpClient => GetParent<HttpClientComponent>().HttpClient;
        public HttpDownloadContext Context;
        public HttpResponseMessage Response;
        public bool DownloadBegin { get; set; }
        public long Length { get; set; }
        public long TotalLength { get; set; }
        public bool DownloadOver { get; set; }
        
    }
}