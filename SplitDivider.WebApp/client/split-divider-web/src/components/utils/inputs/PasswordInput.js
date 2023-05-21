import React from "react";
import { Input } from "reactstrap";
import PropTypes from "prop-types";

function PasswordInput(props) {
  const { state, setState, validations, title } = props;

  const onChangePassword = e => {
    for (let validate of validations) {
      const feedback = validate(e.target.value);

      if (feedback) {
        setState(prev => ({
          ...prev,
          password: e.target.value,
          isValid: false,
          feedback: feedback
        }));

        return;
      }

      setState(prev => ({
        ...prev,
        password: e.target.value,
        isValid: true,
        feedback: null
      }));
    }
  };

  return (
    <div className="form-group">
      <label htmlFor="password" className="input-title">
        {title}
      </label>
      <Input
        type="password"
        className="form-control"
        name="password"
        value={state.password}
        onChange={onChangePassword}
        invalid={!state.isValid}
      />
      {!state.isValid && state.feedback}
    </div>
  );
}

PasswordInput.propTypes = {
  title: PropTypes.string,
  state: PropTypes.object.isRequired,
  setState: PropTypes.func.isRequired,
  validations: PropTypes.arrayOf(PropTypes.func)
};

PasswordInput.defaultProps = {
  title: "Password",
  validations: []
};

export default PasswordInput;
