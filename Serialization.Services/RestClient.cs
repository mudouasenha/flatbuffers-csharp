namespace Serialization.Services
{
    public class RestClient
    {
        private const string ContentType = "application/octet-stream";
        private readonly HttpClient _httpClient = new();
        private const string BaseUrl = "https://localhost:5021/";

        public async Task<Stream> GetAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + path);
            request.Content = new ByteArrayContent(payload);
            //request.Content.Headers.TryAddWithoutValidation(HeaderNames.ContentType, ContentType);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<byte[]> PostAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + path);
            request.Content = new ByteArrayContent(payload);
            request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            //request.Content.Headers.TryAddWithoutValidation(HeaderNames.ContentType, ContentType);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
