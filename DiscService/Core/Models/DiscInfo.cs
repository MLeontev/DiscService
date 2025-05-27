namespace DiscService.Core.Models;

public class DiscInfo
{
    public DiscType DiscType { get; set; }
    public string DiscName { get; set; }
    public List<string> Keywords {get; set; }
    public string Description { get; set; }

    public DiscInfo(DiscType discType, string discName, List<string> keywords, string description)
    {
        DiscType = discType;
        DiscName = discName;
        Keywords = keywords;
        Description = description;
    }
}