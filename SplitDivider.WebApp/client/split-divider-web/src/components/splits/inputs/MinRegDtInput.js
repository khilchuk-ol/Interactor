import React from "react";
import { Input, Label } from "reactstrap";

import "react-datetime-picker/dist/DateTimePicker.css";

export default function MinRegDtInput(props) {
  const { state, setState } = props;

  const onDtInput = e => {
    setState(e.target.value);
  };

  return (
    <>
      <div>
        <Label
          htmlFor="dt_picker"
          className="input-title"
          style={{ paddingBottom: ".5rem", paddingTop: ".5rem" }}
        >
          Minimal Registration Time
        </Label>
      </div>
      <div id={"dt_picker"}>
        <div>
          <Input type={"datetime-local"} value={state} onInput={onDtInput} />
        </div>
      </div>
    </>
  );
}
