import React from "react";
import { Input } from "reactstrap";
import PropTypes from "prop-types";

function EmailInput(props) {
  const { state, setState, validations } = props;

  const onChangeEmail = e => {
    for (let validate of validations) {
      const feedback = validate(e.target.value);

      if (feedback) {
        setState(prev => ({
          ...prev,
          email: e.target.value,
          isValid: false,
          feedback: feedback
        }));

        return;
      }

      setState(prev => ({
        ...prev,
        email: e.target.value,
        isValid: true,
        feedback: null
      }));
    }
  };

  return (
    <div className="form-group">
      <label htmlFor="email" className="input-title">
        Email
      </label>
      <Input
        type="email"
        className="form-control"
        name="email"
        value={state.email}
        onChange={onChangeEmail}
        invalid={!state.isValid}
      />
      {!state.isValid && state.feedback}
    </div>
  );
}

EmailInput.propTypes = {
  state: PropTypes.object.isRequired,
  setState: PropTypes.func.isRequired,
  validations: PropTypes.arrayOf(PropTypes.func)
};

export default EmailInput;
