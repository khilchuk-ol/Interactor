import React from "react";
import { Button, ButtonGroup, Label } from "reactstrap";
import { COUNTRIES_MAP } from "../../../../services/geo.helper";

export default function CountryIdsInput(props) {
  const { state, setState } = props;

  const onCheckboxBtnClick = selected => {
    const index = state.indexOf(selected);

    if (index < 0) {
      state.push(selected);
    } else {
      state.splice(index, 1);
    }

    setState([...state]);
  };

  return (
    <>
      <div>
        <Label
          htmlFor="country_selector"
          className="input-title"
          style={{ paddingBottom: ".5rem" }}
        >
          Countries
        </Label>
      </div>
      <ButtonGroup id={"country_selector"}>
        {Object.keys(COUNTRIES_MAP).map(k => (
          <Button
            color="secondary"
            outline
            key={k}
            onClick={() => onCheckboxBtnClick(k)}
            active={state.includes(k)}
          >
            {COUNTRIES_MAP[k]}
          </Button>
        ))}
      </ButtonGroup>
    </>
  );
}
