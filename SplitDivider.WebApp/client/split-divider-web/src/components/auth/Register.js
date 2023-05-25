import React, { useState } from "react";
import { Form } from "reactstrap";

import AuthService from "../../services/auth.service.js";

import {
  required,
  validEmail,
  validPassword,
  valueEquals
} from "../utils/ValidationFeedback.js";
import PasswordInput from "../utils/inputs/PasswordInput.js";
import EmailInput from "../utils/inputs/EmailInput.js";
import PropTypes from "prop-types";
import { useNavigate } from "react-router-dom";
import TokenService from "../../services/token.service";

function Register(props) {
  const { setUser } = props;

  const navigate = useNavigate();

  const [formState, setFormState] = useState({
    success: false,
    message: ""
  });

  const [passwordState, setPasswordState] = useState({
    password: "",
    isValid: true,
    feedback: null
  });

  const [confirmPasswordState, setConfirmPasswordState] = useState({
    password: "",
    isValid: true,
    feedback: null
  });

  const [emailState, setEmailState] = useState({
    email: "",
    isValid: true,
    feedback: null
  });

  const handleRegister = e => {
    e.preventDefault();

    setFormState(prev => ({
      ...prev,
      success: false,
      message: ""
    }));

    if (
      !emailState.email ||
      !passwordState.password ||
      !confirmPasswordState.password
    ) {
      setFormState(prev => ({
        ...prev,
        success: false,
        message: "Fields cannot be empty"
      }));
      return;
    }

    if (
      passwordState.isValid &&
      confirmPasswordState.isValid &&
      emailState.isValid
    ) {
      AuthService.register(
        emailState.email,
        passwordState.password,
        confirmPasswordState.password
      ).then(
        data => {
          setFormState(prev => ({
            ...prev,
            message: "Your account has been registered",
            success: true
          }));

          setUser(data["user"]);
          TokenService.saveToken(data["token"]);

          navigate("/");
        },
        err => {
          setFormState(prev => ({
            ...prev,
            message: err.message
          }));
        }
      );
    }
  };

  return (
    <div className="col-md-12" style={{ paddingTop: "9rem" }}>
      <div className="card card-container mx-auto" id="light-card-with-input">
        <Form onSubmit={handleRegister}>
          <EmailInput
            state={emailState}
            setState={setEmailState}
            validations={[required, validEmail]}
          />

          <div
            style={{
              paddingTop: "0.4rem"
            }}
          >
            <PasswordInput
              state={passwordState}
              setState={setPasswordState}
              validations={[required, validPassword]}
            />
          </div>

          <div id="last-div">
            <PasswordInput
              title="Confirm password"
              state={confirmPasswordState}
              setState={setConfirmPasswordState}
              validations={[
                required,
                validPassword,
                valueEquals(passwordState.password)
              ]}
            />
          </div>

          <div className="form-group">
            <button type="submit" className="btn btn-primary btn-block">
              <span>Register</span>
            </button>
          </div>

          {formState.message && (
            <div className="form-group" style={{ paddingTop: "0.5rem" }}>
              <div
                className={
                  formState.success
                    ? "alert alert-success"
                    : "alert alert-danger"
                }
                role="alert"
              >
                {formState.message}
              </div>
            </div>
          )}
        </Form>
      </div>
    </div>
  );
}

Register.propTypes = {
  setUser: PropTypes.func.isRequired
};

export default Register;
