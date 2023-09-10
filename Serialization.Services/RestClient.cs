namespace Serialization.Services
{
    public class RestClient
    {
        private const string OctetStreamContentType = "application/octet-stream";
        private readonly HttpClient _httpClient = new();
        private const string BaseUrl = "https://localhost:5021/";

        public async Task<Stream> GetAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + path);
            request.Content = new ByteArrayContent(payload);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task<byte[]> PostAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + path);
            request.Content = new ByteArrayContent(payload);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(OctetStreamContentType);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<object> PostAsync(string path, object payload)
        {
            if (payload is byte[] byteArrayPayload)
            {
                return await PostAsync(path, byteArrayPayload);
            }

            throw new NotImplementedException($"Deserialization for payload not implemented!");
        }
    }
}
