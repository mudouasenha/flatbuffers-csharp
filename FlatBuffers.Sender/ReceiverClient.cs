using Microsoft.Net.Http.Headers;

namespace FlatBuffers.Sender
{
    public class ReceiverClient
    {
        private const string ContentType = "application/octet-stream";
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReceiverClient> _logger;

        public ReceiverClient(HttpClient httpClient, ILogger<ReceiverClient> logger) =>
            (_httpClient, _logger) = (httpClient, logger);

        public async Task<Stream> GetAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Content = new ByteArrayContent(payload);
            request.Content.Headers.TryAddWithoutValidation(HeaderNames.ContentType, ContentType);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<byte[]> PostAsync(string path, byte[] payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Content = new ByteArrayContent(payload);
            request.Content.Headers.TryAddWithoutValidation(HeaderNames.ContentType, ContentType);

            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadAsByteArrayAsync();
        }

        /* public async Task<Result<ServerSuccess, ServerError>> PostAsync<T>(HttpClient httpClient, string path, T payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Content = new ByteArrayContent(MessagePackSerializer.Serialize(payload));
            request.Content.Headers.TryAddWithoutValidation(HeaderNames.ContentType, ContentType);

            var response = await httpClient.SendAsync(request).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return Result.Success<ServerSuccess, ServerError>(new ServerSuccess());

            return Result.Failure<ServerSuccess, ServerError>(new ServerError((int)response.StatusCode,
                                                              await response.Content.ReadAsStringAsync()));
        }*/

        /*public async Task<object> GetAsync<T>(HttpClient httpClient, string path)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            var response = await httpClient.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }*/
    }
}