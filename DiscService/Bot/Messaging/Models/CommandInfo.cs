using System.Text.Json.Serialization;

namespace DiscService.Bot.Messaging.Models;

public class CommandInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("right")]
    public string Right { get; set; }
    
    public CommandInfo(string name, string description, string action, string right)
    {
        Name = name;
        Description = description;
        Action = action;
        Right = right;
    }
}