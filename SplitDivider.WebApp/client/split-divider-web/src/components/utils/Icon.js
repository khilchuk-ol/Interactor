import React from "react";
import { css } from "@emotion/css";

import EditPencil from "../../images/pencil.svg";

export const iconTypes = {
  editPencil: "EDIT_PENCIL"
};

const iconSrc = {
  EDIT_PENCIL: EditPencil
};

const circleStyles = css({
  width: 25,
  height: 25,
  borderRadius: "50%",
  backgroundColor: "#f7f7f9",
  display: "flex",
  justifyContent: "center"
});

export default function Icon(props) {
  const { type } = props;

  return (
    <div className={circleStyles}>
      <img src={iconSrc[type]} alt={""} />
    </div>
  );
}
