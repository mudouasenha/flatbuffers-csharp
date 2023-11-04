using System.Net.Http.Headers;

namespace Serialization.Services
{
    public class RestClient
    {
        private const string OctetStreamContentType = "application/octet-stream";
        private readonly HttpClient _httpClient = new();
        private const string BaseUrl = "http://127.0.0.1:5020/";

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

            throw new NotImplementedException($"Deserialization for payload not implemented!");
        }

        private async Task<byte[]> PostAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + path);
            request.Content = new ByteArrayContent(payload);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(OctetStreamContentType);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}