namespace SignalaraAPI.Models;

/// <summary>
/// Represents a trading signal (BUY, SELL, NEUTRAL)
/// </summary>
public enum Signal
{
    BUY,
    SELL,
    NEUTRAL
}

/// <summary>
/// OHLCV bar data structure
/// </summary>
public class PriceBar
{
    public DateTime Timestamp { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }
    public long Volume { get; set; }
}

/// <summary>
/// Result from a trading strategy
/// </summary>
public class StrategyResult
{
    public string Name { get; set; } = string.Empty;
    public Signal Signal { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Signals grouped by role (Trend, Momentum, Strength)
/// </summary>
public class SignalsByRole
{
    public List<StrategyResult> Trend { get; set; } = new();
    public List<StrategyResult> Momentum { get; set; } = new();
    public List<StrategyResult> Strength { get; set; } = new();
}

/// <summary>
/// Complete signals response
/// </summary>
public class SignalsResponse
{
    public string Symbol { get; set; } = string.Empty;
    public string IndexName { get; set; } = string.Empty;
    public string Timeframe { get; set; } = "5m";
    public double Price { get; set; }
    public Signal Consensus { get; set; }
    public int BuyVotes { get; set; }
    public int SellVotes { get; set; }
    public int NeutralVotes { get; set; }
    public int TotalStrategies { get; set; }
    public List<StrategyResult> Signals { get; set; } = new();
    public SignalsByRole SignalsByRole { get; set; } = new();
    public IndiaVixData IndiaVix { get; set; } = new();
    public double ATR { get; set; }
}

/// <summary>
/// India VIX data
/// </summary>
public class IndiaVixData
{
    public double Value { get; set; }
    public double Change { get; set; }
    public double ChangePct { get; set; }
    public double PrevClose { get; set; }
}
