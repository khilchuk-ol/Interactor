import React from "react";
import { Input } from "reactstrap";
import PropTypes from "prop-types";

function NameInput(props) {
  const { state, setState, validations } = props;

  const onChangeText = e => {
    for (let validate of validations) {
      const feedback = validate(e.target.value);

      if (feedback) {
        setState(prev => ({
          ...prev,
          value: e.target.value,
          isValid: false,
          feedback: feedback
        }));

        return;
      }

      setState(prev => ({
        ...prev,
        value: e.target.value,
        isValid: true,
        feedback: null
      }));
    }
  };

  return (
    <div className="form-group">
      <label htmlFor="text" className="input-title">
        Name
      </label>
      <Input
        type="text"
        className="form-control"
        name="text"
        value={state.value}
        onChange={onChangeText}
        invalid={!state.isValid}
      />
      {!state.isValid && state.feedback}
    </div>
  );
}

NameInput.propTypes = {
  state: PropTypes.object.isRequired,
  setState: PropTypes.func.isRequired,
  validations: PropTypes.arrayOf(PropTypes.func)
};

export default NameInput;
