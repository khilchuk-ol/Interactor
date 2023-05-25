import React, { useEffect, useState } from "react";
import "./styles/App.css";
import AuthService from "./services/auth.service";
import Router from "./components/Router";
import HeaderNavbar from "./components/HeaderNavbar";

function App() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    AuthService.getCurrentUser()
      .then(data => {
        setUser(data);
      })
      .catch(err => {});
  }, []);

  return (
    <div className="App">
      <HeaderNavbar user={user} />

      <Router user={user} setUser={setUser} />
    </div>
  );
}

export default App;
