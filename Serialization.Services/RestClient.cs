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
            request.Content = new StreamContent(payload);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(OctetStreamContentType);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsByteArrayAsync();
        }


    }
}
