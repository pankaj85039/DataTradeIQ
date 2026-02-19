import { useState, useEffect } from "react";
import "./MarketTicker.css";

export function MarketTicker() {
  const [tickers, setTickers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const API_BASE = "http://localhost:5136/api";

  useEffect(() => {
    const fetchTickers = async () => {
      try {
        const res = await fetch(`${API_BASE}/ticker`);
        if (!res.ok) throw new Error("Failed to fetch tickers");
        const data = await res.json();
        setTickers(data);
        setLoading(false);
      } catch (err) {
        console.error("Ticker fetch error:", err);
        setError("Failed to fetch live tickers");
        setLoading(false);
      }
    };

    // Fetch immediately
    fetchTickers();

    // Update every 2 seconds (live ticker)
    const interval = setInterval(fetchTickers, 2000);
    return () => clearInterval(interval);
  }, []);

  const getChangeColor = (changePct) => {
    if (changePct > 0) return "#00d480";
    if (changePct < 0) return "#ff4757";
    return "#ffc107";
  };

  if (error) {
    return <div className="ticker-error">{error}</div>;
  }

  return (
    <div className="market-ticker">
      <div className="ticker-label">📈 LIVE MARKET TICKER</div>
      <div className="ticker-scroll">
        {loading ?
          <div className="ticker-loading">Loading...</div>
        : <div className="ticker-items">
            {tickers.map((ticker) => (
              <div key={ticker.symbol} className="ticker-item">
                <span className="ticker-name">{ticker.name}</span>
                <span className="ticker-price">₹{ticker.price.toFixed(2)}</span>
                <span
                  className="ticker-change"
                  style={{ color: getChangeColor(ticker.changePct) }}
                >
                  {ticker.changePct > 0 ?
                    "↑"
                  : ticker.changePct < 0 ?
                    "↓"
                  : "→"}
                  {ticker.change.toFixed(2)} ({ticker.changePct.toFixed(2)}%)
                </span>
                <span className="ticker-divider">|</span>
              </div>
            ))}
          </div>
        }
      </div>
    </div>
  );
}

export default MarketTicker;
