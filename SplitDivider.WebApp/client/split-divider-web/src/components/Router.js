import React from "react";

import { Route, Routes } from "react-router-dom";
import Login from "./auth/Login";
import Register from "./auth/Register";
import Logout from "./auth/Logout";
import PropTypes from "prop-types";
import HomePage from "./pages/HomePage";
import ErrorPage from "./pages/ErrorPage";
import ParticipantsSearch from "./participants/ParticipantSearch";
import SplitList from "./splits/List/SplitList";
import SplitListPage from "./splits/List/SplitListPage";
import Split from "./splits/Split/Split";
import AdminPage from "./admin/AdminPage";

export default function Router(props) {
  const { user, setUser } = props;

  return (
    <div className="container mt-3">
      <Routes>
        <Route exact path="/documentation" element={<>Documentation</>} />

        {!user && (
          <>
            <Route exact path="/login" element={<Login setUser={setUser} />} />
            <Route
              exact
              path="/register"
              element={<Register setUser={setUser} />}
            />
          </>
        )}
        {user && (
          <>
            <Route
              exact
              path="/logout"
              element={<Logout setUser={setUser} />}
            />
            <Route
              exact
              path="/participants"
              element={<ParticipantsSearch />}
            />
            <Route exact path="/splits" element={<SplitListPage />} />
            <Route path="/splits/:id" element={<Split />} />
            <Route exact path="/admin" element={<AdminPage />} />
          </>
        )}
        <Route path="/" element={<HomePage />} />
        <Route path="*" element={<ErrorPage />} />
      </Routes>
    </div>
  );
}

Router.propTypes = {
  user: PropTypes.object,
  setUser: PropTypes.func.isRequired
};
