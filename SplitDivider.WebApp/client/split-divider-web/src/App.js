import React, { useState } from "react";
import "./styles/App.css";
import AuthService from "./services/auth.service";
import { HttpStatusCode } from "axios";
import Router from "./components/Router";
import HeaderNavbar from "./components/HeaderNavbar";

function App() {
  const [user, setUser] = useState(null);

  AuthService.getCachedUser().then(
    user => {
      setUser(user);
    },
    () => {
      AuthService.getCurrentUser()
        .then(resp => {
          if (resp.status !== HttpStatusCode.Ok) return;

          setUser(resp.result);
        })
        .catch(err => {
          console.log(err);
        });
    }
  );

  return (
    <div className="App">
      <HeaderNavbar user={user} />

      <Router user={user} setUser={setUser} />
    </div>
  );
}

export default App;
