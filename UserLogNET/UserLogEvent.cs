using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserLogNET
{
    public class UserLogEvent
    {
        /// <summary>
        /// Project name <para></para>
        /// Set when Event is being sent
        /// </summary>
        [JsonPropertyName("project"), Required]
        public string Project { get; internal set; } = string.Empty;
        
        /// <summary>
        /// Channel name
        /// </summary>
        [JsonPropertyName("channel"), Required]
        public string Channel { get; set; } = string.Empty;
        
        /// <summary>
        /// Event name
        /// </summary>
        [JsonPropertyName("event"), Required]
        public string Event { get; set; } = string.Empty;
        
        /// <summary>
        /// User ID, e.g. abc@example.com
        /// </summary>
        [JsonPropertyName("user_id"), Required]
        public string UserId { get; set; } = string.Empty;
        
        /// <summary>
        /// Event description
        /// </summary>
        [JsonPropertyName("description"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        /// <summary>
        /// Event tags
        /// </summary>
        [JsonPropertyName("tags"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object>? Tags { get; set; }
        
        /// <summary>
        /// Single Emoji, e.g. 💰 <para></para>
        /// HAS to be a valid Emoji if set
        /// </summary>
        [JsonPropertyName("icon"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Icon { get; set; }
        
        /// <summary>
        /// Send push notification
        /// </summary>
        [JsonPropertyName("notify"), Required]
        public bool Notify { get; set; }

        /// <summary>
        /// Add new Tag to GetUserLogEvent
        /// </summary>
        /// <param name="name">Tag name</param>
        /// <param name="value">Tag value (only string, number, bool allowed)</param>
        public void AddTag(string name, object value)
        {
            if (ValidateTag(name, value))
                Tags!.Add(name, value);
        }

        public void AddTags(params (string name, object value)[] tags)
        {
            foreach (var (name, value) in tags)
            {
                if (ValidateTag(name, value))
                    Tags!.Add(name, value);
            }
        }

        private bool ValidateTag(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            if (!(value is int || value is long || value is float || value is double || value is decimal || value is bool || value is string))
                throw new ArgumentException($"Tag invalid {{{name}}} Value type {{{value.GetType()}}} is not supported. Only string, int and bool types are supported.");

            Tags ??= new Dictionary<string, object>();
            
            return true;
        }
    }
}