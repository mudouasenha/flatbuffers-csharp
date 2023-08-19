namespace FlatBuffers.Domain.Results
{
    public record ServerSuccess();
    public record ServerError(int StatusCode, string Message);
}