using System;
using System.IO;
using System.Net.Http;

namespace ET.Client
{
    public static class HttpClientHelper
    {
        public static async ETTask<string> Get(string link)
        {
            try
            {
                HttpResponseMessage response =  await Init.HttpClient.GetAsync(link);
                string result = await response.Content.ReadAsStringAsync();
                
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"http request fail: {link.Substring(0,link.IndexOf('?'))}\n{e}");
            }
        }


        public static async ETTask<string> Post(string url, byte[] bytes) //post异步请求方法
        {
            try
            {
                HttpContent content = new ByteArrayContent(bytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //由HttpClient发出异步Post请求
                HttpResponseMessage res = await Init.HttpClient.PostAsync(url, content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}