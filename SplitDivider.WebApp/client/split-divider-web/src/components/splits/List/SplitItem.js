import React, { useState } from "react";

import { useNavigate } from "react-router-dom";
import { Col, Row } from "reactstrap";
import SplitHelper from "../../../services/split.helper";
import ButtonsGroup from "../inputs/ButtonsGroup";

function SplitItem(props) {
  const { item, changeStatus, onUpdate } = props;

  const navigate = useNavigate();

  const navigateToInfo = () => {
    navigate(`/splits/${item.id}`);
  };

  const onEditSplit = e => {
    onUpdate(e);
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
          <ButtonsGroup
            state={item.state}
            id={item.id}
            changeStatus={changeStatus}
            onEditSplit={onEditSplit}
          />
        </Col>
      </Row>
    </>
  );
}

export default SplitItem;
