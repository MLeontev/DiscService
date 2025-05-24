using System.Text.Json.Serialization;

namespace DiscService.Messaging.Models;

public class ServiceRegistrationRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("commands")]
    public List<CommandInfo> Commands { get; set; }
}