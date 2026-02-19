using SignalaraAPI.Models;

namespace SignalaraAPI.Strategies;

/// <summary>
/// Moving Average Strategy - Simple trend follower
/// </summary>
public class MovingAverageStrategy : BaseStrategy
{
    private readonly int _period;
    
    public MovingAverageStrategy(int period = 5)
    {
        _period = period;
        Name = $"MA({period})";
    }
    
    public override Signal Calculate(List<PriceBar> bars)
    {
        if (!ValidateBars(bars, _period))
            return Signal.NEUTRAL;
        
        double[] closes = GetClosingPrices(bars);
        double ma = CalculateSMA(closes, _period);
        double currentPrice = closes[^1];
        
        if (double.IsNaN(ma))
            return Signal.NEUTRAL;
        
        if (currentPrice > ma)
            return Signal.BUY;
        else if (currentPrice < ma)
            return Signal.SELL;
        
        return Signal.NEUTRAL;
    }
}

/// <summary>
/// RSI Strategy - Momentum indicator
/// </summary>
public class RSIStrategy : BaseStrategy
{
    private readonly int _period;
    private readonly double _oversoldThreshold;
    private readonly double _overboughtThreshold;
    
    public RSIStrategy(int period = 7, double oversoldThreshold = 35, double overboughtThreshold = 65)
    {
        _period = period;
        _oversoldThreshold = oversoldThreshold;
        _overboughtThreshold = overboughtThreshold;
        Name = $"RSI({period})";
    }
    
    public override Signal Calculate(List<PriceBar> bars)
    {
        if (!ValidateBars(bars, _period + 1))
            return Signal.NEUTRAL;
        
        double[] closes = GetClosingPrices(bars);
        double rsi = CalculateRSI(closes, _period);
        
        if (double.IsNaN(rsi))
            return Signal.NEUTRAL;
        
        if (rsi < _oversoldThreshold)
            return Signal.BUY;
        else if (rsi > _overboughtThreshold)
            return Signal.SELL;
        
        return Signal.NEUTRAL;
    }
}

/// <summary>
/// MACD Strategy - Convergence/Divergence
/// </summary>
public class MACDStrategy : BaseStrategy
{
    private readonly int _fastPeriod;
    private readonly int _slowPeriod;
    private readonly int _signalPeriod;
    
    public MACDStrategy(int fastPeriod = 5, int slowPeriod = 13, int signalPeriod = 1)
    {
        _fastPeriod = fastPeriod;
        _slowPeriod = slowPeriod;
        _signalPeriod = signalPeriod;
        Name = $"MACD({fastPeriod},{slowPeriod},{signalPeriod})";
    }
    
    public override Signal Calculate(List<PriceBar> bars)
    {
        if (!ValidateBars(bars, _slowPeriod))
            return Signal.NEUTRAL;
        
        double[] closes = GetClosingPrices(bars);
        var (macd, signal, histogram) = CalculateMACD(closes, _fastPeriod, _slowPeriod, _signalPeriod);
        
        if (double.IsNaN(macd) || double.IsNaN(signal))
            return Signal.NEUTRAL;
        
        if (macd > signal)
            return Signal.BUY;
        else if (macd < signal)
            return Signal.SELL;
        
        return Signal.NEUTRAL;
    }
}

/// <summary>
/// EMA Crossover Strategy
/// </summary>
public class EMACrossoverStrategy : BaseStrategy
{
    private readonly int _shortPeriod;
    private readonly int _longPeriod;
    
    public EMACrossoverStrategy(int shortPeriod = 5, int longPeriod = 13)
    {
        _shortPeriod = shortPeriod;
        _longPeriod = longPeriod;
        Name = $"EMA({shortPeriod},{longPeriod})";
    }
    
    public override Signal Calculate(List<PriceBar> bars)
    {
        if (!ValidateBars(bars, _longPeriod))
            return Signal.NEUTRAL;
        
        double[] closes = GetClosingPrices(bars);
        double emaShort = CalculateEMA(closes, _shortPeriod);
        double emaLong = CalculateEMA(closes, _longPeriod);
        
        if (double.IsNaN(emaShort) || double.IsNaN(emaLong))
            return Signal.NEUTRAL;
        
        if (emaShort > emaLong)
            return Signal.BUY;
        else if (emaShort < emaLong)
            return Signal.SELL;
        
        return Signal.NEUTRAL;
    }
}

/// <summary>
/// Supertrend Strategy - Volatility based
/// </summary>
public class SupertrendStrategy : BaseStrategy
{
    private readonly int _period;
    private readonly double _multiplier;
    
    public SupertrendStrategy(int period = 7, double multiplier = 2)
    {
        _period = period;
        _multiplier = multiplier;
        Name = $"Supertrend({period},{multiplier})";
    }
    
    public override Signal Calculate(List<PriceBar> bars)
    {
        if (!ValidateBars(bars, _period))
            return Signal.NEUTRAL;
        
        // Simplified Supertrend using ATR
        double atr = CalculateATR(bars, _period);
        double currentPrice = bars[^1].Close;
        double highAvg = bars.TakeLast(_period).Average(b => b.High);
        
        double basicUpperBand = highAvg + (_multiplier * atr);
        double basicLowerBand = bars.TakeLast(_period).Average(b => b.Low) - (_multiplier * atr);
        
        if (currentPrice > basicUpperBand)
            return Signal.BUY;
        else if (currentPrice < basicLowerBand)
            return Signal.SELL;
        
        return Signal.NEUTRAL;
    }
    
    private double CalculateATR(List<PriceBar> bars, int period)
    {
        if (bars.Count < period)
            return 0;
        
        double sumTR = 0;
        for (int i = bars.Count - period; i < bars.Count; i++)
        {
            double highLow = bars[i].High - bars[i].Low;
            double highClose = Math.Abs(bars[i].High - bars[i - 1].Close);
            double lowClose = Math.Abs(bars[i].Low - bars[i - 1].Close);
            
            double tr = Math.Max(highLow, Math.Max(highClose, lowClose));
            sumTR += tr;
        }
        
        return sumTR / period;
    }
}

/// <summary>
/// Stochastic Strategy
/// </summary>
public class StochasticStrategy : BaseStrategy
{
    private readonly int _kPeriod;
    private readonly int _dPeriod;
    
    public StochasticStrategy(int kPeriod = 5, int dPeriod = 3)
    {
        _kPeriod = kPeriod;
        _dPeriod = dPeriod;
        Name = $"Stoch({kPeriod},{dPeriod},3)";
    }
    
    public override Signal Calculate(List<PriceBar> bars)
    {
        if (!ValidateBars(bars, _kPeriod))
            return Signal.NEUTRAL;
        
        List<PriceBar> recentBars = bars.TakeLast(_kPeriod).ToList();
        double highest = recentBars.Max(b => b.High);
        double lowest = recentBars.Min(b => b.Low);
        double currentClose = bars[^1].Close;
        
        double k = ((currentClose - lowest) / (highest - lowest)) * 100;
        
        if (k < 20)
            return Signal.BUY;
        else if (k > 80)
            return Signal.SELL;
        
        return Signal.NEUTRAL;
    }
}

/// <summary>
/// ADX Strategy - Trend Strength
/// </summary>
public class ADXStrategy : BaseStrategy
{
    private readonly int _period;
    private readonly double _trendThreshold;
    
    public ADXStrategy(int period = 14, double trendThreshold = 25)
    {
        _period = period;
        _trendThreshold = trendThreshold;
        Name = $"ADX({period})";
    }
    
    public override Signal Calculate(List<PriceBar> bars)
    {
        if (!ValidateBars(bars, _period + 1))
            return Signal.NEUTRAL;
        
        // Simplified ADX calculation
        List<PriceBar> recentBars = bars.TakeLast(_period).ToList();
        
        double upSum = 0, downSum = 0;
        for (int i = 1; i < recentBars.Count; i++)
        {
            double upMove = recentBars[i].High - recentBars[i - 1].High;
            double downMove = recentBars[i - 1].Low - recentBars[i].Low;
            
            if (upMove > downMove && upMove > 0)
                upSum += upMove;
            if (downMove > upMove && downMove > 0)
                downSum += downMove;
        }
        
        double diPlus = (upSum / _period) * 100 / (recentBars.Average(b => b.Close));
        double diMinus = (downSum / _period) * 100 / (recentBars.Average(b => b.Close));
        
        if (diPlus > diMinus && diPlus > _trendThreshold)
            return Signal.BUY;
        else if (diMinus > diPlus && diMinus > _trendThreshold)
            return Signal.SELL;
        
        return Signal.NEUTRAL;
    }
}
