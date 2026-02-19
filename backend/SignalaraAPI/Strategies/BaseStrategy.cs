using SignalaraAPI.Models;

namespace SignalaraAPI.Strategies;

/// <summary>
/// Base class for all trading strategies
/// </summary>
public abstract class BaseStrategy
{
    public string Name { get; protected set; } = string.Empty;
    
    /// <summary>
    /// Calculate trading signal based on price data
    /// </summary>
    public abstract Signal Calculate(List<PriceBar> bars);
    
    /// <summary>
    /// Validate that we have minimum required data
    /// </summary>
    protected bool ValidateBars(List<PriceBar> bars, int minRequired = 5)
    {
        return bars != null && bars.Count >= minRequired;
    }
    
    /// <summary>
    /// Get closing prices as array
    /// </summary>
    protected double[] GetClosingPrices(List<PriceBar> bars)
    {
        return bars.Select(b => b.Close).ToArray();
    }
    
    /// <summary>
    /// Calculate Simple Moving Average
    /// </summary>
    protected double CalculateSMA(double[] prices, int period)
    {
        if (prices.Length < period)
            return double.NaN;
        
        return prices.TakeLast(period).Average();
    }
    
    /// <summary>
    /// Calculate Exponential Moving Average
    /// </summary>
    protected double CalculateEMA(double[] prices, int period)
    {
        if (prices.Length < period)
            return double.NaN;
        
        double multiplier = 2.0 / (period + 1);
        double ema = prices.Take(period).Average();
        
        for (int i = period; i < prices.Length; i++)
        {
            ema = (prices[i] * multiplier) + (ema * (1 - multiplier));
        }
        
        return ema;
    }
    
    /// <summary>
    /// Calculate RSI (Relative Strength Index)
    /// </summary>
    protected double CalculateRSI(double[] prices, int period = 14)
    {
        if (prices.Length < period + 1)
            return double.NaN;
        
        double gain = 0, loss = 0;
        
        // Calculate average gain and loss
        for (int i = prices.Length - period; i < prices.Length; i++)
        {
            double diff = prices[i] - prices[i - 1];
            if (diff > 0)
                gain += diff;
            else
                loss += Math.Abs(diff);
        }
        
        double avgGain = gain / period;
        double avgLoss = loss / period;
        
        if (avgLoss == 0)
            return avgGain == 0 ? 50 : 100;
        
        double rs = avgGain / avgLoss;
        double rsi = 100 - (100 / (1 + rs));
        
        return rsi;
    }
    
    /// <summary>
    /// Calculate MACD (Moving Average Convergence Divergence)
    /// </summary>
    protected (double macd, double signal, double histogram) CalculateMACD(double[] prices, int fast = 12, int slow = 26, int signalPeriod = 9)
    {
        if (prices.Length < slow)
            return (double.NaN, double.NaN, double.NaN);
        
        double ema12 = CalculateEMA(prices, fast);
        double ema26 = CalculateEMA(prices, slow);
        double macd = ema12 - ema26;
        
        // For signal line, we'd need to calculate EMA of MACD values
        // Simplified version - use just the MACD value
        double signal = macd; // Simplified
        double histogram = macd - signal;
        
        return (macd, signal, histogram);
    }
}
