namespace FlatBuffers.Sender
{
    public class ReceiverClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReceiverClient> _logger;

        public ReceiverClient(HttpClient httpClient, ILogger<ReceiverClient> logger) =>
            (_httpClient, _logger) = (httpClient, logger);

        public async Task<object> GetAsync<T>(HttpClient httpClient, string path)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, path);

            var response = await httpClient.SendAsync(request);
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<Stream> PostAsync<T>(string path, T payload)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, path);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            return await response.Content.ReadAsStreamAsync();
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
    }
}