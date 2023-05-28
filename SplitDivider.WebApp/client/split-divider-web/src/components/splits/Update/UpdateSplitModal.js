import React, { useState } from "react";
import { ALL_GENDERS } from "../../../services/gender.helper";
import SplitService from "../../../services/split.service";
import { Button, Modal, ModalBody, ModalFooter, ModalHeader } from "reactstrap";
import NameInput from "../inputs/NameInput";
import { validText } from "../../utils/ValidationFeedback";
import ActionsWeightsTableInput from "../inputs/ActionsWeightsInput";
import CountryIdsInput from "../inputs/CountryIdsInput";
import GenderInput from "../inputs/GenderInput";
import MinRegDtInput from "../inputs/MinRegDtInput";
import splitService from "../../../services/split.service";
import { useEffect } from "react";
import { prepareDatetime } from "../../../services/datetime.helper";

export default function UpdateSplitModal(props) {
  const [msgState, setMsgState] = useState({
    show: false,
    message: null
  });

  const { isOpen, id, toggle } = props;

  const [nameState, setNameState] = useState({
    value: "",
    isValid: true,
    feedback: null
  });

  const [actionsWeightsState, setActionsWeightsState] = useState([]);

  const onChangeInput = e => {
    e.preventDefault();

    let { value, id } = e.target;

    value = parseInt(value);

    if (value < 1) value = 1;
    if (value > 30) value = 30;

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

  useEffect(() => {
    splitService.getSplit(id).then(data => {
      setNameState({
        value: data.name,
        isValid: true,
        feedback: null
      });
      setCountryIdsState(data.countryIds.map(id => id + ""));
      setGenderState(data.gender ? data.gender : ALL_GENDERS);

      setMinRegDtState(
        data.minRegDt ? prepareDatetime(data.minRegDt) : undefined
      );

      let i = 0;
      let actionsWeights = Object.keys(data.actionsWeights).map(awk => ({
        id: i++,
        name: awk,
        value: data.actionsWeights[awk]
      }));

      setActionsWeightsState(actionsWeights);
    });
  }, [id]);

  const updateSplit = e => {
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

    SplitService.patchSplit(
      id,
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
        <ModalHeader>Update split ID={id}</ModalHeader>
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
          <Button color="success" onClick={updateSplit}>
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
