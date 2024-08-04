using System.Text.Json.Serialization;

namespace MyApiProject.Tests
{
    public class ApiObject
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("data")]
        public DataObject? Data { get; set; }
    }

    public class DataObject
    {
        [JsonPropertyName("color")]
        public string? Color { get; set; }

        [JsonPropertyName("capacity")]
        public string? Capacity { get; set; }
    }
}
