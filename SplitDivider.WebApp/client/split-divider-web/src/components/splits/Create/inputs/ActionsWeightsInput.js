import React, { useState } from "react";

export default function ActionsWeightsTableInput(props) {
  const { state, setState, onChangeInput } = props;

  return (
    <div style={{ paddingTop: "1.5rem" }}>
      <label
        htmlFor="input_table"
        className="input-title"
        style={{ paddingBottom: ".5rem" }}
      >
        Interactions' weights
      </label>
      <table id={"input_table"} className={"table table-light border"}>
        <thead className={"table-dark"}>
          <tr>
            <th scope={"col"}>Name</th>
            <th scope={"col"}>Weight</th>
          </tr>
        </thead>
        <tbody>
          {state.map(v => (
            <tr key={v.i}>
              <th scope={"row"}>{v.name}</th>
              <td>
                <input
                  name="value"
                  value={v.value}
                  type="number"
                  key={v.id}
                  id={v.id}
                  min={1}
                  max={30}
                  onChange={onChangeInput}
                  placeholder="Enter interaction's weight"
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
