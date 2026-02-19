import { useState, useEffect } from 'react';
import { apiService } from '../services/apiService';
import '../styles/ApiDemo.css';

export function ApiDemo() {
  const [welcome, setWelcome] = useState(null);
  const [signals, setSignals] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [apiStatus, setApiStatus] = useState('checking...');

  useEffect(() => {
    // Check API health on component mount
    checkApiHealth();
    // Optionally fetch welcome message
    fetchWelcome();
  }, []);

  const checkApiHealth = async () => {
    try {
      await apiService.healthCheck();
      setApiStatus('🟢 Connected');
    } catch (err) {
      setApiStatus('🔴 Disconnected');
      setError('Backend API is not available');
    }
  };

  const fetchWelcome = async () => {
    try {
      setLoading(true);
      const data = await apiService.getWelcome();
      setWelcome(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchSignals = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await apiService.getSignals();
      setSignals(data.signals || []);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const createNewSignal = async () => {
    try {
      setLoading(true);
      setError(null);
      const newSignal = await apiService.createSignal(
        `New Signal ${Date.now()}`,
        Math.floor(Math.random() * 1000)
      );
      setSignals([...signals, newSignal]);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="api-demo-container">
      <div className="api-demo-header">
        <h2>🔌 Signalara API Demo</h2>
        <div className={`api-status ${apiStatus.includes('Connected') ? 'connected' : 'disconnected'}`}>
          {apiStatus}
        </div>
      </div>

      {error && <div className="error-message">⚠️ {error}</div>}

      {welcome && (
        <div className="api-section">
          <h3>Welcome Message</h3>
          <div className="api-response">
            <p><strong>Message:</strong> {welcome.message}</p>
            <p><strong>Status:</strong> {welcome.status}</p>
            <p><strong>Version:</strong> {welcome.version}</p>
            <p><strong>Time:</strong> {new Date(welcome.timestamp).toLocaleString()}</p>
          </div>
        </div>
      )}

      <div className="api-section">
        <h3>Signals</h3>
        <div className="button-group">
          <button onClick={fetchSignals} disabled={loading}>
            {loading ? 'Loading...' : 'Fetch Signals'}
          </button>
          <button onClick={createNewSignal} disabled={loading}>
            {loading ? 'Creating...' : 'Create Signal'}
          </button>
        </div>

        {signals.length > 0 && (
          <div className="signals-grid">
            {signals.map((signal) => (
              <div key={signal.id} className="signal-card">
                <h4>{signal.name}</h4>
                <p className="signal-value">{signal.value}</p>
                <p className="signal-id">ID: {signal.id}</p>
              </div>
            ))}
          </div>
        )}
      </div>

      <div className="api-section info-section">
        <h3>ℹ️ API Information</h3>
        <ul>
          <li>React Frontend: http://localhost:5173</li>
          <li>.NET Backend: http://localhost:5000</li>
          <li>Try fetching data from the backend API</li>
          <li>Implement your own components using apiService</li>
        </ul>
      </div>
    </div>
  );
}
