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
import UpdateSplitModal from "../Update/UpdateSplitModal";

function SplitList(props) {
  const { stateFilter } = props;

  const [listState, setListState] = useState({
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
    SplitService.getSplits(listState.page, stateFilter)
      .then(res => {
        setListState(prev => ({
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
        setListState(prev => ({
          ...prev,
          isInitial: true,
          message: err.message,
          showMessage: true,
          isLoading: false,
          splits: null
        }));
      });
  }, [listState.page, stateFilter]);

  const setPage = page => {
    setListState(prev => ({
      ...prev,
      page: page
    }));
  };

  const closeAlert = () => {
    setListState(prev => ({
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

  const [editModalState, setEditModalState] = useState({
    isOpen: false,
    id: 1
  });

  const toggleEditModal = () => {
    setEditModalState(prev => ({
      ...prev,
      isOpen: false
    }));
  };

  const onUpdate = id => {
    setEditModalState({
      id: id,
      isOpen: true
    });
  };

  return (
    <>
      {listState.totalCount === 0 && (
        <Alert color="primary" isOpen={true} style={{ marginTop: "2rem" }}>
          No results found
        </Alert>
      )}
      {listState.totalCount > 0 && (
        <>
          <PaginationControl
            page={listState.page}
            between={4}
            total={listState.totalCount}
            limit={PAGE_SIZE}
            changePage={page => {
              setPage(page);
            }}
            ellipsis={1}
          />

          <div>
            {listState.isLoading && (
              <span className="spinner-border spinner-border-sm"></span>
            )}
            {listState.showMessage && (
              <Alert
                color="danger"
                isOpen={listState.showMessage}
                toggle={closeAlert}
              >
                {listState.message}
              </Alert>
            )}
          </div>

          {listState.splits && (
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
              {listState.splits.map(s => {
                return (
                  <SplitItem
                    item={s}
                    onUpdate={e => {
                      onUpdate(s.id);
                    }}
                    key={`${s.id}`}
                    changeStatus={changeStatus}
                  />
                );
              })}
            </Container>
          )}

          {!listState.splits && !listState.isInitial && (
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

          <UpdateSplitModal
            id={editModalState.id}
            isOpen={editModalState.isOpen}
            toggle={toggleEditModal}
          />

          <Modal isOpen={modal.show} toggle={closeModal}>
            <ModalHeader toggle={closeModal}>Error occurred</ModalHeader>
            <ModalBody>{modal.message}</ModalBody>
          </Modal>
        </>
      )}
    </>
  );
}

export default SplitList;
