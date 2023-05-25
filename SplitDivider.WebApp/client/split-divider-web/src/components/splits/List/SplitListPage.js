import React, { useState } from "react";
import {
  Button,
  Col,
  Dropdown,
  DropdownItem,
  DropdownMenu,
  DropdownToggle,
  Row
} from "reactstrap";
import SplitList from "./SplitList";
import SplitHelper, { StateNames } from "../../../services/split.helper";
import Icon, { iconTypes } from "../../utils/Icon";
import CreateSplitModal from "../Create/CreateSplitModal";

export default function SplitListPage() {
  const [filterState, setFilterState] = useState({
    isOpen: false,
    selected: null,
    isInitial: true
  });

  const [isModalOpen, setIsModalOpen] = useState(false);

  const toggleModal = () => {
    setIsModalOpen(!isModalOpen);
  };

  const onClickSetState = (e, state) => {
    e.preventDefault();

    setFilterState(prev => ({
      ...prev,
      selected: state,
      isInitial: false
    }));
  };

  const toggleFilter = () => {
    setFilterState(prev => ({
      ...prev,
      isOpen: !prev.isOpen
    }));
  };

  return (
    <>
      <Row>
        <Col className={"text-start"}>
          <Dropdown isOpen={filterState.isOpen} toggle={toggleFilter}>
            <DropdownToggle caret>
              {filterState.isInitial
                ? "Select state"
                : filterState.selected
                ? SplitHelper.getStateName(filterState.selected)
                : "All"}
            </DropdownToggle>
            <DropdownMenu>
              <DropdownItem header>State</DropdownItem>
              <DropdownItem onClick={e => onClickSetState(e, null)}>
                All
              </DropdownItem>
              {Object.keys(StateNames).map(k => (
                <DropdownItem onClick={e => onClickSetState(e, k)} key={k}>
                  {StateNames[k]}
                </DropdownItem>
              ))}
              <DropdownItem></DropdownItem>
            </DropdownMenu>
          </Dropdown>
        </Col>
        <Col className={"text-end"}>
          <Button
            className={"btn border btn-light btn-block"}
            onClick={toggleModal}
          >
            <Icon type={iconTypes.plusSign} />
          </Button>
        </Col>
      </Row>
      <SplitList stateFilter={filterState.selected} />

      <CreateSplitModal isOpen={isModalOpen} toggle={toggleModal} />
    </>
  );
}
