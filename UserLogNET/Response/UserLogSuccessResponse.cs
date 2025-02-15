using System.Text.Json.Serialization;

namespace UserLogNET.Response
{
    public class UserLogSuccessResponse
    {
        [JsonPropertyName("success")] public bool Success { get; set; }

        [JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
    }
}