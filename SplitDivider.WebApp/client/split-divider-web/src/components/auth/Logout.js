import React from "react";
import { Navigate } from "react-router-dom";
import authService from "../../services/auth.service";
import PropTypes from "prop-types";

function Logout(props) {
  const { setUser } = props;

  setUser(null);
  authService.clearCache();
  authService
    .logout()
    .then(_ => {
      window.location.reload();
    })
    .catch(err => {
      console.log(err);
    });

  return <Navigate to="/" />;
}

Logout.propTypes = {
  setUser: PropTypes.func.isRequired
};

export default Logout;
