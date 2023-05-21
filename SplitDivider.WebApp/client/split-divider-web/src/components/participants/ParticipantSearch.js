import React from "react";

import ParticipantService from "../../services/participant.service";

import { useState } from "react";
import IdSearchBar from "../utils/inputs/IdSearchBar";
import { required, valuePositive } from "../utils/ValidationFeedback";
import { Alert, Col, Container, Row } from "reactstrap";

export default function ParticipantsSearch(props) {
  const [state, setState] = useState({
    isLoading: false,
    isInitial: true,
    results: null,
    message: null,
    showMessage: false
  });

  const doSearch = id => {
    setState(prev => ({
      ...prev,
      isInitial: true,
      isLoading: true
    }));

    ParticipantService.getParticipantsSplitGroups(id)
      .then(res => {
        setState(prev => ({
          isInitial: false,
          message: null,
          showMessage: false,
          isLoading: false,
          results: res
        }));
      })
      .catch(err => {
        setState(prev => ({
          isInitial: true,
          message: err.message,
          showMessage: true,
          isLoading: false,
          results: null
        }));
      });
  };

  const closeAlert = () => {
    setState(prev => ({
      ...prev,
      showMessage: false
    }));
  };

  const onChangeInput = () => {
    setState(prev => ({
      ...prev,
      isInitial: true
    }));
  };

  return (
    <>
      <div>
        <IdSearchBar
          validations={[required, valuePositive]}
          onSubmit={doSearch}
          onChange={onChangeInput}
        />
      </div>
      <div>
        {state.isLoading && (
          <span className="spinner-border spinner-border-sm"></span>
        )}
        {state.showMessage && (
          <Alert color="danger" isOpen={state.showMessage} toggle={closeAlert}>
            {state.message}
          </Alert>
        )}
      </div>

      <Container
        style={{
          width: "75%"
        }}
      >
        <Row>
          <Col className="bg-dark border text-light text-start">Split Id</Col>
          <Col className="bg-dark border text-light text-start">Group</Col>
        </Row>
        {state.results !== null && state.results.length > 0
          ? state.results.map(p => (
              <Row>
                <Col className="bg-light border text-start">{p.splitId}</Col>
                <Col className="bg-light border align-content-end text-start">
                  {p.group}
                </Col>
              </Row>
            ))
          : !state.isInitial && (
              <div
                style={{
                  paddingTop: "1rem"
                }}
              >
                <Alert color="primary" isOpen={true}>
                  No results found
                </Alert>
              </div>
            )}
      </Container>
    </>
  );
}
