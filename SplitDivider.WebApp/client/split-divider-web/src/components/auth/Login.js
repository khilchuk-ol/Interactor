import React, { useState } from "react";
import { Form } from "reactstrap";

import {
  required,
  validEmail,
  validPassword
} from "../utils/ValidationFeedback.js";
import AuthService from "../../services/auth.service.js";

import EmailInput from "../utils/inputs/EmailInput.js";
import PasswordInput from "../utils/inputs/PasswordInput.js";
import { Link, useNavigate } from "react-router-dom";
import PropTypes from "prop-types";
import TokenService from "../../services/token.service";

function Login(props) {
  const { setUser } = props;

  const navigate = useNavigate();

  const [formState, setFormState] = useState({
    loading: false,
    message: ""
  });

  const [emailState, setEmailState] = useState({
    email: "",
    isValid: true,
    feedback: null
  });

  const [passwordState, setPasswordState] = useState({
    password: "",
    isValid: true,
    feedback: null
  });

  const handleLogin = e => {
    e.preventDefault();

    setFormState(prev => ({ ...prev, message: "", loading: true }));

    if (!emailState.email || !passwordState.password) {
      setFormState(prev => ({
        ...prev,
        loading: false,
        message: "Fields cannot be empty"
      }));
      return;
    }

    if (passwordState.isValid && emailState.isValid) {
      AuthService.login(emailState.email, passwordState.password).then(
        data => {
          TokenService.saveToken(data["token"]);
          setUser(data["user"]);

          setFormState(prev => ({
            ...prev,
            loading: false
          }));

          navigate("/");
        },
        err => {
          let resMsg =
            (err.response && err.response.data && err.response.data.message) ||
            err.message ||
            err.toString();

          if (resMsg.search("401") !== -1) {
            resMsg = "Incorrect email or password";
          }

          setFormState({
            loading: false,
            message: resMsg
          });
        }
      );
    } else {
      setFormState(prev => ({ ...prev, loading: false }));
    }
  };

  return (
    <div className="col-md-12" style={{ paddingTop: "9rem" }}>
      <div className="card card-container mx-auto" id="light-card-with-input">
        <Form onSubmit={handleLogin}>
          <EmailInput
            state={emailState}
            setState={setEmailState}
            validations={[required, validEmail]}
          />

          <div id="last-div">
            <PasswordInput
              state={passwordState}
              setState={setPasswordState}
              validations={[required, validPassword]}
            />
          </div>

          <div className="form-group">
            <button
              type="submit"
              className="btn btn-primary btn-block"
              disabled={formState.loading}
            >
              {formState.loading && (
                <span className="spinner-border spinner-border-sm"></span>
              )}
              <span>Login</span>
            </button>
          </div>
          <div
            style={{
              paddingTop: "0.5rem"
            }}
          >
            <Link to="/register">Don't have an account?</Link>
          </div>

          {formState.message && (
            <div className="form-group" style={{ padding: "0.5rem" }}>
              <div className="alert alert-danger" role="alert">
                {formState.message}
              </div>
            </div>
          )}
        </Form>
      </div>
    </div>
  );
}

Login.propTypes = {
  setUser: PropTypes.func.isRequired
};

export default Login;
