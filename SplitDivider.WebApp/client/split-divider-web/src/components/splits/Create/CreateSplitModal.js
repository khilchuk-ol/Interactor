import React, { useState } from "react";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import SplitService from "../../../services/split.service";
import { validText } from "../../utils/ValidationFeedback";
import NameInput from "./inputs/NameInput";
import ActionsWeightsTableInput from "./inputs/ActionsWeightsInput";
import { InteractionTypes } from "../../../services/split.helper";
import CountryIdsInput from "./inputs/CountryIdsInput";
import { ALL_GENDERS } from "../../../services/gender.helper";
import GenderInput from "./inputs/GenderInput";
import MinRegDtInput from "./inputs/MinRegDtInput";

export default function CreateSplitModal(props) {
  const [msgState, setMsgState] = useState({
    show: false,
    message: null
  });

  const { isOpen, toggle } = props;

  const [nameState, setNameState] = useState({
    value: "",
    isValid: true,
    feedback: null
  });

  let initialActionsWeightsData = [];
  let i = 0;

  Object.keys(InteractionTypes).forEach(k => {
    initialActionsWeightsData.push({
      id: i++,
      name: InteractionTypes[k],
      value: 1
    });
  });

  const [actionsWeightsState, setActionsWeightsState] = useState(
    initialActionsWeightsData
  );

  const onChangeInput = e => {
    e.preventDefault();

    let { value, id } = e.target;

    value = parseInt(value);
    id = parseInt(id);

    setActionsWeightsState(prev => {
      return prev.map(o => {
        if (o.id === id) {
          return {
            id: o.id,
            name: o.name,
            value: value
          };
        } else {
          return o;
        }
      });
    });
  };

  const [countryIdsState, setCountryIdsState] = useState([]);

  const [genderState, setGenderState] = useState(ALL_GENDERS);

  const [minRegDtState, setMinRegDtState] = useState(new Date());

  const createSplit = e => {
    e.preventDefault();

    if (!nameState.value || !nameState.isValid) {
      setMsgState({
        show: true,
        message: "Name value is not valid"
      });

      return;
    }

    let actionsWeight = {};
    actionsWeightsState.forEach(v => {
      actionsWeight[v.name] = v.value;
    });

    SplitService.postSplit(
      nameState.value,
      genderState === ALL_GENDERS ? null : genderState,
      minRegDtState,
      countryIdsState.map(id => parseInt(id)),
      actionsWeight
    )
      .then(_ => {
        window.location.reload();
      })
      .catch(err => {
        setMsgState({
          show: true,
          message: err.message
        });
      });
  };

  const closeMsg = () => {
    setMsgState(_ => ({
      show: false,
      message: ""
    }));
  };

  return (
    <>
      <Modal isOpen={isOpen} toggle={toggle} fullscreen>
        <ModalHeader>Create new split</ModalHeader>
        <ModalBody style={{ width: "30%" }} className={"mx-auto"}>
          <NameInput
            state={nameState}
            setState={setNameState}
            validations={[validText]}
          />
          <ActionsWeightsTableInput
            state={actionsWeightsState}
            setState={setActionsWeightsState}
            onChangeInput={onChangeInput}
          />
          <CountryIdsInput
            state={countryIdsState}
            setState={setCountryIdsState}
          />
          <GenderInput state={genderState} setState={setGenderState} />
          <MinRegDtInput state={minRegDtState} setState={setMinRegDtState} />
        </ModalBody>
        <ModalFooter>
          <Button color="success" onClick={createSplit}>
            Submit
          </Button>{" "}
          <Button color="secondary" onClick={toggle}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>

      <Modal isOpen={msgState.show} toggle={closeMsg}>
        <ModalHeader toggle={closeMsg} className={"text-bg-danger"}>
          Error
        </ModalHeader>
        <ModalBody>{msgState.message}</ModalBody>
      </Modal>
    </>
  );
}
