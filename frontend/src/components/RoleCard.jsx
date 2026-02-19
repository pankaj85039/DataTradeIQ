import "./RoleCard.css";

export function RoleCard({ role, indicators }) {
  if (!indicators || indicators.length === 0) {
    return null;
  }

  const roleConfig = {
    Trend: {
      icon: "📊",
      borderColor: "#00d4ff",
      accentColor: "#00d4ff",
      description: "Price Direction",
    },
    Momentum: {
      icon: "⚡",
      borderColor: "#b41ef2",
      accentColor: "#b41ef2",
      description: "Market Speed",
    },
    Strength: {
      icon: "👍",
      borderColor: "#00d480",
      accentColor: "#00d480",
      description: "Trend Power",
    },
  };

  const config = roleConfig[role] || roleConfig.Trend;

  // Calculate consensus for this role
  const buyCount = indicators.filter((i) => i.signal === "BUY").length;
  const sellCount = indicators.filter((i) => i.signal === "SELL").length;
  const neutralCount = indicators.filter((i) => i.signal === "NEUTRAL").length;

  let roleSignal = "NEUTRAL";
  if (buyCount > sellCount) {
    roleSignal = "BUY";
  } else if (sellCount > buyCount) {
    roleSignal = "SELL";
  }

  const getSignalColor = (signal) => {
    switch (signal) {
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

  return (
    <div className="role-card" style={{ borderColor: config.borderColor }}>
      <div className="role-header">
        <span className="role-icon">{config.icon}</span>
        <div style={{ flex: 1 }}>
          <div className="role-title">{role}</div>
          <div className="role-subtitle">{config.description}</div>
        </div>
      </div>

      <div className="indicators-list">
        {indicators.map((indicator, idx) => (
          <div key={idx} className="indicator-row">
            <span className="indicator-name">{indicator.name}</span>
            <span
              className="indicator-signal"
              style={{ color: getSignalColor(indicator.signal) }}
            >
              {indicator.signal}
            </span>
          </div>
        ))}
      </div>

      <div
        className="role-consensus"
        style={{ backgroundColor: getSignalColor(roleSignal) }}
      >
        {roleSignal}
      </div>
    </div>
  );
}

export default RoleCard;
