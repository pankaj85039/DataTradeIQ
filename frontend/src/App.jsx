import { useState, useEffect } from "react";
import "./App.css";
import { Login } from "./components/Login";
import { DatatradeIQ } from "./components/DatatradeIQ";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [userId, setUserId] = useState(null);

  // Check if user was previously logged in
  useEffect(() => {
    const savedUserId = localStorage.getItem("userId");
    if (savedUserId) {
      setUserId(savedUserId);
      setIsLoggedIn(true);
    }
  }, []);

  const handleLogin = (username) => {
    setUserId(username);
    setIsLoggedIn(true);
    localStorage.setItem("userId", username);
  };

  const handleLogout = () => {
    setUserId(null);
    setIsLoggedIn(false);
    localStorage.removeItem("userId");
  };

  return (
    <>
      {isLoggedIn ?
        <DatatradeIQ userId={userId} onLogout={handleLogout} />
      : <Login onLogin={handleLogin} />}
    </>
  );
}

export default App;
