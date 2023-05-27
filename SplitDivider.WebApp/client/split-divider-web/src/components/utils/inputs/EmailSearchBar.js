import React, { useState, useEffect } from "react";
import { Button, Col, FormGroup, Input, Label } from "reactstrap";
import PropTypes from "prop-types";

function EmailSearchBar(props) {
  const { onSubmit, validations, onChange } = props;

  const [state, setState] = useState({
    email: undefined,
    isValid: true,
    feedback: null
  });

  const onChangeEmail = e => {
    onChange();

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

  const onButtonClick = e => {
    e.preventDefault();

    if (!state.email) {
      setState(prev => ({
        ...prev,
        isValid: false
      }));

      return;
    }

    onSubmit(state.email);
  };

  useEffect(() => {
    const handleKeyPress = event => {
      if (event.key === "Enter") {
        onButtonClick(event);
      }
    };

    document.addEventListener("keydown", handleKeyPress);

    return () => {
      document.removeEventListener("keydown", handleKeyPress);
    };
  }, [onButtonClick]);

  return (
    <FormGroup row>
      <Label for="email" sm={2}>
        Enter email:{" "}
      </Label>
      <Col sm={7}>
        <Input
          type="email"
          placeholder={"email@example.com"}
          className="form-control"
          name="email"
          value={state.email}
          onChange={onChangeEmail}
          invalid={!state.isValid}
        />
        {!state.isValid && state.feedback}
      </Col>
      <Col sm={1}>
        <Button onClick={onButtonClick} disabled={!state.isValid}>
          Search
        </Button>
      </Col>
    </FormGroup>
  );
}

EmailSearchBar.propTypes = {
  onSubmit: PropTypes.func.isRequired,
  validations: PropTypes.arrayOf(PropTypes.func)
};

export default EmailSearchBar;
