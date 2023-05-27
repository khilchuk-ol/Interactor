import React from "react";
import { css } from "@emotion/css";

import EditPencil from "../../images/pencil.svg";
import PlusSign from "../../images/plus.svg";
import CloseSign from "../../images/close.svg";

export const iconTypes = {
  editPencil: "EDIT_PENCIL",
  plusSign: "PLUS_SIGN",
  closeSign: "CLOSE_SIGN"
};

const iconSrc = {
  EDIT_PENCIL: EditPencil,
  PLUS_SIGN: PlusSign,
  CLOSE_SIGN: CloseSign
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
