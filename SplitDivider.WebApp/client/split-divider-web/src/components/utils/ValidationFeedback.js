import React from "react";
import { FormFeedback } from "reactstrap";
import { isEmail } from "validator";

export const required = value => {
  if (!value) {
    return (
      <FormFeedback style={{ display: "block" }}>
        Value cannot be empty
      </FormFeedback>
    );
  }
};

export const validEmail = value => {
  //if (!isEmail(value)) {
  if(!value) {
    return (
      <FormFeedback style={{ display: "block" }}>
        This is not a valid email.
      </FormFeedback>
    );
  }
};

export const validText = value => {
  if (value.length < 5 || value.length > 30) {
    return (
      <FormFeedback style={{ display: "block" }}>
        Length should be between 5 and 30 characters
      </FormFeedback>
    );
  }
};

export const validPassword = value => {
  if (value.search(/^(?=.*[A-Za-z!])(?=.*\d)[A-Za-z!\d]{8,}$/) === -1) {
    return (
      <FormFeedback style={{ display: "block" }}>
        The password must be at least 8 characters and it must contain at least
        one letter and one number.
      </FormFeedback>
    );
  }
};

export const valueEquals = expected => {
  return value => {
    if (value !== expected) {
      return (
        <FormFeedback style={{ display: "block" }}>
          Values don't match.
        </FormFeedback>
      );
    }
  };
};

export const valuePositive = value => {
  if (parseInt(value) < 0) {
    return (
      <FormFeedback style={{ display: "block" }}>
        Value should be positive
      </FormFeedback>
    );
  }
};

export const wrapInFeedback = msg => {
  return <FormFeedback style={{ display: "block" }}>{msg}</FormFeedback>;
};
