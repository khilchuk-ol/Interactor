import React, { useState } from "react";

import { useNavigate } from "react-router-dom";
import { Button, Col, Row } from "reactstrap";
import SplitHelper from "../../../services/split.helper";
import Icon, { iconTypes } from "../../utils/Icon";
import { patchStatusName } from "../../../services/split.service";

function SplitItem(props) {
  const { item, isEditable, changeStatus } = props;

  const navigate = useNavigate();

  const navigateToInfo = () => {
    navigate(`/splits/${item.id}`);
  };

  const navigateToEdit = () => {
    navigate(`/splits/${item.id}/edit`);
  };

  const originalStyle = "bg-light text-secondary border text-start";
  const [dataClassName, setDataClassName] = useState(originalStyle);

  const addStyle = () => {
    setDataClassName(prev => prev + " like-btn-hover");
  };

  const removeStyle = () => {
    setDataClassName(originalStyle);
  };

  return (
    <>
      <Row>
        <Col
          className={dataClassName}
          xs={1}
          onClick={navigateToInfo}
          onMouseEnter={addStyle}
          onMouseLeave={removeStyle}
        >
          {item.id}
        </Col>
        <Col
          className={dataClassName}
          xs={5}
          onClick={navigateToInfo}
          onMouseEnter={addStyle}
          onMouseLeave={removeStyle}
        >
          {item.name}
        </Col>
        <Col
          className={dataClassName}
          xs={2}
          onClick={navigateToInfo}
          onMouseEnter={addStyle}
          onMouseLeave={removeStyle}
        >
          {SplitHelper.getStateName(item.state)}
        </Col>
        <Col className="text-secondary text-start">
          <Button
            className={"bg-light border btn-in-row"}
            onClick={navigateToEdit}
            disabled={!isEditable}
          >
            <Icon type={iconTypes.editPencil} />
          </Button>
          {SplitHelper.canBeActivated(item.state) && (
            <Button
              color={"success"}
              className={"btn-in-row"}
              onClick={() => {
                changeStatus(item.id, patchStatusName.activate);
              }}
            >
              Activate
            </Button>
          )}
          {SplitHelper.canBeSuspended(item.state) && (
            <Button
              color={"primary"}
              className={"btn-in-row"}
              onClick={() => {
                changeStatus(item.id, patchStatusName.suspend);
              }}
            >
              Suspend
            </Button>
          )}
          {SplitHelper.canBeClosed(item.state) && (
            <Button
              color={"warning"}
              className={"btn-in-row"}
              onClick={() => {
                changeStatus(item.id, patchStatusName.close);
              }}
            >
              Close
            </Button>
          )}
        </Col>
      </Row>
    </>
  );
}

export default SplitItem;
