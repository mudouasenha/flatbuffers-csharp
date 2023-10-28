using System.Text;

namespace Serialization.Services
{
    public class RestClient
    {
        private const string OctetStreamContentType = "application/octet-stream";
        private readonly HttpClient _httpClient = new();
        private const string BaseUrl = "http://localhost:5020/";

        public async Task<Stream> GetAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + path);
            request.Content = new ByteArrayContent(payload);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<object> PostAsync(string path, object payload)
        {
            if (payload is byte[] byteArrayPayload)
            {
                return await PostAsync(path, byteArrayPayload);
            }

            if (payload is MemoryStream memoryStreamPayload)
            {
                return await PostAsync(path, memoryStreamPayload);
            }

            throw new NotImplementedException($"Deserialization for payload not implemented!");
        }

        private async Task<byte[]> PostAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + path);
            request.Content = new ByteArrayContent(payload);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(OctetStreamContentType);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsByteArrayAsync();
        }

        private async Task<byte[]> PostAsync(string path, MemoryStream payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + path);
            Stream requestStream = null;
            payload.Seek(0, SeekOrigin.Begin);

            payload.WriteTo(requestStream);
            request.Content = new StreamContent(requestStream);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(OctetStreamContentType);

            requestStream.Flush();
            requestStream.Close();

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var result = await response.Content.ReadAsStreamAsync();


            return Encoding.UTF8.GetBytes(result.ToString());
        }


    }
}
