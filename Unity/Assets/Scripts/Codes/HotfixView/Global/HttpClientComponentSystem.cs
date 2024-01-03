using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ET.Client
{
    [FriendOf(typeof(HttpClientTask))]
    public static class HttpClientComponentSystem
    {
        public class AwakeSystem : AwakeSystem<HttpClientComponent>
        {
            protected override void Awake(HttpClientComponent self)
            {
                self.HttpClient = new HttpClient();
            }
        }
        
        public static async ETTask<HttpClientTask> AddDownload(this HttpClientComponent self, HttpDownloadContext context)
        {
            var task = self.AddChild<HttpClientTask>();
            task.Context = context;
            task.DownloadBegin = true;
            task.TotalLength = await task.GetLength();
            task.Download().Coroutine();
            return task;
        }

        private static async ETTask Download(this HttpClientTask self)
        {
            var content = await self.Response.Content.ReadAsStreamAsync();
            FileHelper.CreateFile(self.Context.WriteToFile);
            var fileStream = new FileStream(self.Context.WriteToFile, FileMode.OpenOrCreate, FileAccess.Write);
            var buffer = new byte[1024 * 10];
            int length = 0;
            await Task.Run(async () =>
            {
                while ((length = await content.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    self.Length += length;
                    fileStream.Write(buffer, 0, length);
                }
            });
            self.DownloadOver = true;
            await TimerComponent.Instance.WaitAsync(1000 * 60);
            self.Dispose();
        }
        private static async ETTask<long> GetLength(this HttpClientTask self)
        {
            self.Response = await self.HttpClient.GetAsync(self.Context.Url, HttpCompletionOption.ResponseHeadersRead);
            var length = self.Response.Content.Headers.ContentLength;
            if (length != null) return length.Value;
            return 0;
        }
    }
}