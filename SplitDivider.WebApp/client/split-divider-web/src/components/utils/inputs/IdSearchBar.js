import React, { useState, useEffect } from "react";
import { Button, Col, FormGroup, Input, Label } from "reactstrap";
import PropTypes from "prop-types";

function IdSearchBar(props) {
  const { onSubmit, validations, onChange } = props;

  const [state, setState] = useState({
    id: undefined,
    isValid: true,
    feedback: null
  });

  const onChangeId = e => {
    onChange();

    for (let validate of validations) {
      const feedback = validate(e.target.value);

      if (feedback) {
        setState(prev => ({
          ...prev,
          id: e.target.value,
          isValid: false,
          feedback: feedback
        }));

        return;
      }

      setState(prev => ({
        ...prev,
        id: e.target.value,
        isValid: true,
        feedback: null
      }));
    }
  };

  const onButtonClick = e => {
    e.preventDefault();

    if (!state.id) {
      setState(prev => ({
        ...prev,
        isValid: false
      }));

      return;
    }

    onSubmit(state.id);
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
      <Label for="id" sm={2}>
        Enter participant id:{" "}
      </Label>
      <Col sm={7}>
        <Input
          type="number"
          placeholder={"1234567"}
          className="form-control"
          name="id"
          value={state.id}
          onChange={onChangeId}
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

IdSearchBar.propTypes = {
  onSubmit: PropTypes.func.isRequired,
  validations: PropTypes.arrayOf(PropTypes.func)
};

export default IdSearchBar;
