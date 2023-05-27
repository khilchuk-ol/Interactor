import React from "react";
import SplitHelper from "../../../services/split.helper";
import { Button } from "reactstrap";
import { patchStatusName } from "../../../services/split.service";
import Icon, { iconTypes } from "../../utils/Icon";

export default function ButtonsGroup(props) {
  const { state, onEditSplit, changeStatus, id } = props;

  return (
    <>
      <Button
        className={"bg-light border btn-in-row"}
        onClick={onEditSplit}
        disabled={!SplitHelper.canBeEdited(state)}
      >
        <Icon type={iconTypes.editPencil} />
      </Button>
      {SplitHelper.canBeActivated(state) && (
        <Button
          color={"success"}
          className={"btn-in-row"}
          onClick={() => {
            changeStatus(id, patchStatusName.activate);

            window.location.reload();
          }}
        >
          Activate
        </Button>
      )}
      {SplitHelper.canBeSuspended(state) && (
        <Button
          color={"primary"}
          className={"btn-in-row"}
          onClick={() => {
            changeStatus(id, patchStatusName.suspend);

            window.location.reload();
          }}
        >
          Suspend
        </Button>
      )}
      {SplitHelper.canBeClosed(state) && (
        <Button
          color={"warning"}
          className={"btn-in-row"}
          onClick={() => {
            changeStatus(id, patchStatusName.close);

            window.location.reload();
          }}
        >
          Close
        </Button>
      )}
    </>
  );
}
