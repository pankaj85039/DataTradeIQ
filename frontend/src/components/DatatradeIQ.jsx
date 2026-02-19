import { SignalsDisplay } from "./SignalsDisplay";
import { MarketTicker } from "./MarketTicker";
import "./DatatradeIQ.css";

export function DatatradeIQ({ userId, onLogout }) {
  return (
    <div className="datatradeiq-container">
      <header className="datatradeiq-header">
        <div className="header-content">
          <h1>DataTradeIQ Dashboard</h1>
          <div className="header-right">
            <span className="user-info">
              Logged in as: <strong>{userId}</strong>
            </span>
            <button className="logout-btn" onClick={onLogout}>
              Logout
            </button>
          </div>
        </div>
      </header>
      <main className="datatradeiq-main">
        <MarketTicker />
        <SignalsDisplay />
      </main>
    </div>
  );
}

export default DatatradeIQ;
