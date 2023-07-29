using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Todo.Services.GravatarServices.Client.Models
{
    public class GravatarProfileResponse
    {
        [JsonPropertyName("entry")]
        public Entry[] Entries { get; set; }
    }
    public class Entry
    {
        [JsonPropertyName("hash")]
        public string Hash { get; set; }
        [JsonPropertyName("requestHash")]
        public string RequestHash { get; set; }
        [JsonPropertyName("profileUrl")]
        public string ProfileUrl { get; set; }
        [JsonPropertyName("preferredUsername")]
        public string PreferredUsername { get; set; }
        [JsonPropertyName("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }
        [JsonPropertyName("photos")]
        public Photo[] Photos { get; set; }
        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }
        [JsonPropertyName("urls")]
        public string[] Urls { get; set; }

    }

    public class Photo
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
