using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace UserLogNET.Response
{
    public class UserLogFailureResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        
        [JsonPropertyName("error")]
        public UserLogFailureResponseError Error { get; set; } = new UserLogFailureResponseError();
    }

    public class UserLogFailureResponseError
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("issues")]
        public List<UserLogFailureResponseErrorIssue> Issues { get; set; } =
            Enumerable.Empty<UserLogFailureResponseErrorIssue>().ToList();
    }

    public class UserLogFailureResponseErrorIssue
    {
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;
        
        [JsonPropertyName("minimum")]
        public int Minimum { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        
        [JsonPropertyName("inclusive")]
        public bool Inclusive { get; set; }
        
        [JsonPropertyName("exact")]
        public bool Exact { get; set; }
        
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("path")] public string[] Path { get; set; } = Array.Empty<string>();
    }
}