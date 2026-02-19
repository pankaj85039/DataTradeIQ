// API service for backend communication

const API_BASE_URL = import.meta.env.VITE_API_URL || "http://localhost:5000";

export const apiService = {
  /**
   * Fetch welcome message from API
   */
  getWelcome: async () => {
    const response = await fetch(`${API_BASE_URL}/api/signal/welcome`);
    if (!response.ok) throw new Error("Failed to fetch welcome");
    return response.json();
  },

  /**
   * Fetch all signals
   */
  getSignals: async () => {
    const response = await fetch(`${API_BASE_URL}/api/signal`);
    if (!response.ok) throw new Error("Failed to fetch signals");
    return response.json();
  },

  /**
   * Fetch signal by ID
   */
  getSignalById: async (id) => {
    const response = await fetch(`${API_BASE_URL}/api/signal/${id}`);
    if (!response.ok) throw new Error(`Failed to fetch signal ${id}`);
    return response.json();
  },

  /**
   * Create a new signal
   */
  createSignal: async (name, value) => {
    const response = await fetch(`${API_BASE_URL}/api/signal`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ name, value }),
    });
    if (!response.ok) throw new Error("Failed to create signal");
    return response.json();
  },

  /**
   * Health check
   */
  healthCheck: async () => {
    const response = await fetch(`${API_BASE_URL}/api/health`);
    if (!response.ok) throw new Error("API is not healthy");
    return response.json();
  },
};
