import React from "react";
import { Button, ButtonGroup, Label } from "reactstrap";
import { GENDERS_MAP } from "../../../services/gender.helper";

export default function GenderInput(props) {
  const { state, setState } = props;

  return (
    <>
      <div>
        <Label
          htmlFor="gender_selector"
          className="input-title"
          style={{ paddingBottom: ".5rem", paddingTop: ".5rem" }}
        >
          Gender
        </Label>
      </div>
      <ButtonGroup id={"gender_selector"}>
        {Object.keys(GENDERS_MAP).map(k => (
          <Button
            color="secondary"
            outline
            key={k}
            onClick={() => setState(k)}
            active={state === k}
          >
            {GENDERS_MAP[k]}
          </Button>
        ))}
      </ButtonGroup>
    </>
  );
}
