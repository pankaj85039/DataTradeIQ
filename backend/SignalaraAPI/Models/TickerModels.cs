namespace SignalaraAPI.Models;

/// <summary>
/// Live ticker data for an index
/// </summary>
public class TickerData
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public double Change { get; set; }
    public double ChangePct { get; set; }
    public double PrevClose { get; set; }
    public DateTime UpdatedAt { get; set; }
}
