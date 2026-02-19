import { useState, useEffect } from "react";
import { RoleCard } from "./RoleCard";
import "./SignalsDisplay.css";

export function SignalsDisplay() {
  const [signals, setSignals] = useState(null);
  const [signalsByRole, setSignalsByRole] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [selectedSymbol, setSelectedSymbol] = useState("^NSEI");
  const [indices, setIndices] = useState({});

  const API_BASE = "http://localhost:5136/api";

  // Fetch available indices
  useEffect(() => {
    const fetchIndices = async () => {
      try {
        const res = await fetch(`${API_BASE}/signals/indices`);
        const data = await res.json();
        setIndices(data);
      } catch (err) {
        console.error("Failed to fetch indices:", err);
      }
    };

    fetchIndices();
  }, []);

  // Fetch signals
  const fetchSignals = async (symbol) => {
    try {
      setLoading(true);
      setError("");

      const res = await fetch(`${API_BASE}/signals?symbol=${symbol}`);

      if (!res.ok) {
        throw new Error(`API error: ${res.status}`);
      }

      const data = await res.json();

      setSignals(data);
      setSignalsByRole(data.signalsByRole || {});
    } catch (err) {
      console.error("Error fetching signals:", err);
      setError("Failed to fetch signals. Make sure backend is running.");
    } finally {
      setLoading(false);
    }
  };

  // Auto-fetch on symbol change
  useEffect(() => {
    fetchSignals(selectedSymbol);
    const interval = setInterval(() => fetchSignals(selectedSymbol), 60000); // Refresh every minute
    return () => clearInterval(interval);
  }, [selectedSymbol]);

  const getConsensusColor = (consensus) => {
    switch (consensus) {
      case "BUY":
        return "#00d480";
      case "SELL":
        return "#ff4757";
      case "NEUTRAL":
        return "#ffc107";
      default:
        return "#888";
    }
  };

  if (loading) {
    return <div className="signals-loading">Loading trading signals...</div>;
  }

  if (error) {
    return (
      <div className="signals-error">
        <p>{error}</p>
        <button onClick={() => fetchSignals(selectedSymbol)}>Retry</button>
      </div>
    );
  }

  if (!signals) {
    return <div className="signals-empty">No signals available</div>;
  }

  return (
    <div className="signals-container">
      {/* Symbol Selector */}
      <div className="signals-top-bar">
        <div className="symbol-selector">
          <label>Select Index:</label>
          <select
            value={selectedSymbol}
            onChange={(e) => setSelectedSymbol(e.target.value)}
          >
            {Object.entries(indices).map(([symbol, name]) => (
              <option key={symbol} value={symbol}>
                {name}
              </option>
            ))}
          </select>
        </div>

        <div className="price-info">
          <div className="price-label">Price</div>
          <div className="price-value">₹{signals.price?.toFixed(2) || "-"}</div>
        </div>

        <div
          className="consensus-badge"
          style={{ backgroundColor: getConsensusColor(signals.consensus) }}
        >
          <div className="consensus-label">Consensus</div>
          <div className="consensus-value">{signals.consensus}</div>
          <div className="consensus-votes">
            {signals.buyVotes} B / {signals.sellVotes} S /{" "}
            {signals.neutralVotes} N
          </div>
        </div>

        <div className="vix-indicator">
          <div className="vix-label">India VIX</div>
          <div className="vix-value">
            {signals.indiaVix?.value?.toFixed(2) || "-"}
          </div>
          <div
            className="vix-change"
            style={{
              color: signals.indiaVix?.changePct > 0 ? "#ff4757" : "#00d480",
            }}
          >
            {signals.indiaVix?.changePct > 0 ? "↑" : "↓"}{" "}
            {Math.abs(signals.indiaVix?.changePct || 0).toFixed(2)}%
          </div>
        </div>

        <div className="atr-indicator">
          <div className="atr-label">ATR (14)</div>
          <div className="atr-value">{signals.atr?.toFixed(2) || "-"}</div>
          <div className="atr-unit">pts</div>
        </div>
      </div>

      {/* Role Cards */}
      <div className="signals-cards-container">
        {Object.entries(signalsByRole).map(([role, indicators]) => (
          <RoleCard key={role} role={role} indicators={indicators} />
        ))}
      </div>

      {/* Footer */}
      <div className="signals-footer">
        <div className="update-time">
          Last updated: {new Date().toLocaleTimeString()}
        </div>
        <button
          onClick={() => fetchSignals(selectedSymbol)}
          className="refresh-btn"
        >
          🔄 Refresh
        </button>
      </div>
    </div>
  );
}

export default SignalsDisplay;
