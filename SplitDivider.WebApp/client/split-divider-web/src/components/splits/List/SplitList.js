import React, { useEffect, useState } from "react";
import SplitService, { PAGE_SIZE } from "../../../services/split.service";
import { PaginationControl } from "react-bootstrap-pagination-control";
import {
  Alert,
  Col,
  Container,
  Modal,
  ModalBody,
  ModalHeader,
  Row
} from "reactstrap";
import SplitItem from "./SplitItem";
import SplitHelper from "../../../services/split.helper";

function SplitList(props) {
  const { stateFilter } = props;

  const [state, setState] = useState({
    splits: [],
    totalCount: 0,
    page: 1,
    isLoading: false,
    isInitial: true,
    message: null,
    showMessage: false
  });

  const [modal, setModal] = useState({
    show: false,
    message: null
  });

  useEffect(() => {
    SplitService.getSplits(state.page, stateFilter)
      .then(res => {
        setState(prev => ({
          ...prev,
          isInitial: false,
          message: null,
          isLoading: false,
          showMessage: false,
          splits: res.items,
          totalCount: res.totalCount
        }));
      })
      .catch(err => {
        setState(prev => ({
          ...prev,
          isInitial: true,
          message: err.message,
          showMessage: true,
          isLoading: false,
          splits: null
        }));
      });
  }, [state.page]);

  const setPage = page => {
    setState(prev => ({
      ...prev,
      page: page
    }));
  };

  const closeAlert = () => {
    setState(prev => ({
      ...prev,
      showMessage: false
    }));
  };

  const changeStatus = (id, status) => {
    SplitService.patchStatus(id, status)
      .then(() => {
        window.location.reload();
      })
      .catch(err => {
        setModal({
          show: true,
          message: err.message
        });
      });
  };

  const closeModal = () => {
    setModal({
      show: false,
      message: null
    });
  };

  return (
    <>
      <PaginationControl
        page={state.page}
        between={4}
        total={state.totalCount}
        limit={PAGE_SIZE}
        changePage={page => {
          setPage(page);
        }}
        ellipsis={1}
      />

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

      {state.splits && (
        <Container className={"mt-auto"}>
          <Row>
            <Col
              className="bg-dark border text-light text-start narrow-row"
              xs={1}
            >
              Split Id
            </Col>
            <Col
              className="bg-dark border text-light text-start narrow-row"
              xs={5}
            >
              Name
            </Col>
            <Col
              className="bg-dark border text-light text-start narrow-row"
              xs={2}
            >
              State
            </Col>
            <Col className="text-start narrow-row"></Col>
          </Row>
          {state.splits.map(s => {
            return (
              <SplitItem
                item={s}
                isEditable={SplitHelper.canBeEdited(s.state)}
                key={`${s.id}`}
                changeStatus={changeStatus}
              />
            );
          })}
        </Container>
      )}

      {!state.splits && !state.isInitial && (
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

      <Modal isOpen={modal.show} toggle={closeModal}>
        <ModalHeader toggle={closeModal}>Error occurred</ModalHeader>
        <ModalBody>{modal.message}</ModalBody>
      </Modal>
    </>
  );
}

export default SplitList;
