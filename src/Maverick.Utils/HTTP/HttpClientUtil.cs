using Flurl;
using Flurl.Http;
using Maverick.Utils.Contants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Maverick.Utils.HTTP
{
    public class HttpClientUtil
    {
        public async Task<T> GetJSON<T>(string path, object queryParams = null, object headers = null, object cookies = null)
        {
            try
            {
                return await new Url(path)
                    .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { })
                    .WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                    .WithHeaders(headers ?? new { })
                    .GetAsync().ReceiveJson<T>();
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
        }
        public async Task<string> GetString(string path, object queryParams = null, object headers = null,
            object cookies = null)
        {
            try
            {
                return await new Url(path)
                    .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { })
                    .WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                    .WithHeaders(headers ?? new { })
                    .GetStringAsync();
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public async Task<T> PostJSONAsync<T>(string path, object payload = null,
            int timeout = Configs.REST_REQUEST_TIMEOUT,
            object headers = null, object cookies = null)
        {
            try
            {
                var result = await new Url(path)
                    .WithTimeout(timeout)
                    .WithCookies(cookies ?? new { })
                                   .WithHeaders(headers ?? new { })
                                    .PostJsonAsync(payload ?? new object()).ReceiveJson<T>();
                return result;
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.InternalServerError)
                    throw ex;

                //var response = ex.GetResponseJson<T>();
                return default(T);
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
        }
        public async Task<string> PostJSONForString(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                var result = await new Url(path)
                    .WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                    .WithCookies(cookies ?? new { })
                                   .WithHeaders(headers ?? new { })
                    .PostJsonAsync(payload ?? new object()).ReceiveString();
                return result;
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public async Task<HttpResponseMessage> PostJSONAsync(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                return await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                                  .WithHeaders(headers ?? new { })
                                   .PostJsonAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {
                return default(HttpResponseMessage);
            }
            catch (IOException)
            {
                return default(HttpResponseMessage);
            }
        }


        public async Task<String> UploadByteArrayAsync(string path, byte[] imageBytes, byte[] secondaryImageBytes, string token, ICollection<KeyValuePair<String, String>> payload = null)
        {
            try
            {
                using (Stream primaryFileStream = new MemoryStream(imageBytes))
                {
                    using (Stream secondaryFileStream = secondaryImageBytes == null ? new MemoryStream() : new MemoryStream(secondaryImageBytes))
                        return await new Url(path).WithTimeout(Configs.REST_REQUEST_TIMEOUT).PostMultipartAsync((mp) =>
                        {
                            mp.AddFile("File", primaryFileStream, "my_uploaded_image.jpg");
                            if (secondaryImageBytes != null)
                            {
                                mp.AddFile("BackFile", secondaryFileStream, "my_secondary_uploaded_image.jpg");
                            }
                            if (payload != null)
                            {
                                foreach (var item in payload)
                                {
                                    mp.AddString(item.Key, item.Value);
                                }
                            }
                        }).ReceiveJson<String>();
                }
            }
            catch (TaskCanceledException)
            {
                return string.Empty;
            }
            catch (IOException)
            {
                return string.Empty;
            }
        }
        public async Task<T> PostUrlEncodedAsync<T>(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                return await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                                   .WithHeaders(headers ?? new { })
                                          .PostUrlEncodedAsync(payload ?? new object()).ReceiveJson<T>();
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
        }
        public async Task PostUrlEncodedAsync(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                                  .WithHeaders(headers ?? new { })
                                   .PostUrlEncodedAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public async Task<T> PutJSONAsync<T>(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                return await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                                   .WithHeaders(headers ?? new { })
                                          .PutJsonAsync(payload ?? new object()).ReceiveJson<T>();
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
        }

        public async Task PutJSONAsync(string path, object payload = null, object headers = null,
                                                  object cookies = null)
        {
            try
            {
                await new Url(path).WithCookies(cookies ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                                  .WithHeaders(headers ?? new { })
                                   .PutJsonAsync(payload ?? new object());
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public async Task<T> DeleteAsync<T>(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                return await new Url(path)
                         .SetQueryParams(queryParams ?? new { })
                    .WithCookies(cookies ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                                   .WithHeaders(headers ?? new { })
                    .DeleteAsync().ReceiveJson<T>();
            }
            catch (TaskCanceledException)
            {
                return default(T);
            }
            catch (IOException)
            {
                return default(T);
            }
        }
        public async Task DeleteAsync(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                await new Url(path)
                   .SetQueryParams(queryParams ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                   .WithCookies(cookies ?? new { })
                                  .WithHeaders(headers ?? new { })
                                         .DeleteAsync();
            }
            catch (TaskCanceledException)
            {

            }
            catch (IOException)
            {

            }
        }
        public async Task<byte[]> GetBytesAsync(string path,
                               object queryParams = null, object headers = null,
                              object cookies = null)
        {
            try
            {
                return await new Url(path)
                   .SetQueryParams(queryParams ?? new { }).WithTimeout(Configs.REST_REQUEST_TIMEOUT)
                   .WithCookies(cookies ?? new { })
                    .WithHeaders(headers ?? new { })
                    .GetBytesAsync();
            }
            catch (TaskCanceledException)
            {
                return default(byte[]);
            }
            catch (IOException)
            {
                return default(byte[]);
            }
        }

    }
}
