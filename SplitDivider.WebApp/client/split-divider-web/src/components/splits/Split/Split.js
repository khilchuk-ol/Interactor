import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import SplitService from "../../../services/split.service";
import {
  Card,
  CardHeader,
  Col,
  Container,
  ListGroup,
  ListGroupItem,
  Modal,
  ModalBody,
  ModalHeader,
  Row,
  Table
} from "reactstrap";
import UpdateSplitModal from "../Update/UpdateSplitModal";
import SplitHelper from "../../../services/split.helper";
import ButtonsGroup from "../inputs/ButtonsGroup";
import { GENDERS_MAP } from "../../../services/gender.helper";
import { COUNTRIES_MAP } from "../../../services/geo.helper";
import { prepareDatetimeForDisplay } from "../../../services/datetime.helper";
import Graph from "./Graph";

export default function Split() {
  const { id } = useParams();

  const [split, setSplit] = useState({
    id: 1,
    actionsWeights: {}
  });

  const [groupsState, setGroupsState] = useState([[], []]);

  const [msgState, setMsgState] = useState({
    isOpen: false,
    msg: ""
  });

  const closeMsg = () => {
    setMsgState(prev => ({
      ...prev,
      isOpen: false
    }));
  };

  useEffect(() => {
    SplitService.getSplit(id)
      .then(data => {
        setSplit(data);

        if (SplitHelper.splitHasGroups(data.state)) {
          SplitService.getSplitUsers(id).then(data => {
            let groups = [[], []];

            data.forEach(e => {
              const group = parseInt(e.group);
              groups[group].push(parseInt(e.userId));
            });

            setGroupsState(groups);
          });
        }
      })
      .catch(err => {
        setMsgState({
          isOpen: true,
          msg: err.message
        });
      });
  }, []);

  const [isEditModalOpen, setIsEditModalOpen] = useState(false);

  const onEditSplit = e => {
    setIsEditModalOpen(true);
  };

  const toggleEditModal = () => {
    setIsEditModalOpen(false);
  };

  const changeStatus = (id, status) => {
    SplitService.patchStatus(id, status)
      .then(() => {
        window.location.reload();
      })
      .catch(err => {
        setMsgState({
          isOpen: true,
          msg: err.message
        });
      });
  };

  return (
    <Container style={{ paddingTop: "2rem" }}>
      <Row>
        <Col xs={5}>
          <Card
            style={{
              width: "100%"
            }}
            className={"text-start"}
          >
            <CardHeader tag="h5">{split.name}</CardHeader>
            <ListGroup flush>
              <ListGroupItem>
                <b>ID:</b> {split.id}
              </ListGroupItem>
              <ListGroupItem>
                <b>State:</b> {SplitHelper.getStateName(split.state)}
              </ListGroupItem>
              <ListGroupItem>
                <b>Actions' weights: </b>
                <table className={"table table-light border"}>
                  <thead className={"table-dark"}>
                    <tr>
                      <th scope={"col"}>Name</th>
                      <th scope={"col"}>Weight</th>
                    </tr>
                  </thead>
                  <tbody>
                    {Object.keys(split.actionsWeights).map(k => (
                      <tr key={k}>
                        <th scope={"row"} key={k}>
                          {k}
                        </th>
                        <td key={k}>{split.actionsWeights[k]}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </ListGroupItem>
              <ListGroupItem>
                <b>Gender:</b> {GENDERS_MAP[split.gender]}
              </ListGroupItem>
              <ListGroupItem>
                <b>Countries:</b>{" "}
                {split.countryIds && split.countryIds.length > 0 ? (
                  <Table className={"border"}>
                    <tbody>
                      {split.countryIds.map(id => (
                        <tr>
                          <td>{COUNTRIES_MAP[id]}</td>
                        </tr>
                      ))}
                    </tbody>
                  </Table>
                ) : (
                  "All"
                )}
              </ListGroupItem>
              <ListGroupItem>
                <b>Min registration time:</b>{" "}
                {split.minRegDt
                  ? prepareDatetimeForDisplay(split.minRegDt)
                  : "Not specified"}
              </ListGroupItem>
              {SplitHelper.splitHasGroups(split.state) && (
                <ListGroupItem>
                  <b>Groups: </b>{" "}
                  <Table bordered>
                    <thead>
                      <tr>
                        <th>#</th>
                        <th>Total</th>
                        <th style={{ backgroundColor: "#78d65e" }}>
                          Control (0)
                        </th>
                        <th style={{ backgroundColor: "orange" }}>Test (1)</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr>
                        <th scope="row">Number of users</th>
                        <td>{groupsState[0].length + groupsState[1].length}</td>
                        <td>{groupsState[0].length}</td>
                        <td>{groupsState[1].length}</td>
                      </tr>
                      <tr>
                        <th scope="row">Percentage</th>
                        <td>100%</td>
                        <td>
                          {(
                            (groupsState[0].length /
                              (groupsState[0].length + groupsState[1].length)) *
                            100
                          ).toFixed(2)}
                          %
                        </td>
                        <td>
                          {(
                            (groupsState[1].length /
                              (groupsState[0].length + groupsState[1].length)) *
                            100
                          ).toFixed(2)}
                          %
                        </td>
                      </tr>
                    </tbody>
                  </Table>
                </ListGroupItem>
              )}
            </ListGroup>
          </Card>
          <Row style={{ paddingTop: "1rem" }} className={"text-start"}>
            <Col>
              <ButtonsGroup
                state={split.state}
                id={split.id}
                changeStatus={changeStatus}
                onEditSplit={onEditSplit}
              />
            </Col>
          </Row>
        </Col>
        <Col>
          {SplitHelper.splitHasGroups(split.state) ? (
            <Graph
              id={split.id}
              groups={groupsState}
              onError={msg => {
                setMsgState({
                  isOpen: true,
                  msg: msg
                });
              }}
            />
          ) : (
            "Groups are not formed yet"
          )}
        </Col>

        <Modal isOpen={msgState.show} toggle={closeMsg}>
          <ModalHeader toggle={closeMsg}>Error occurred</ModalHeader>
          <ModalBody>{msgState.msg}</ModalBody>
        </Modal>

        <UpdateSplitModal
          id={split.id}
          isOpen={isEditModalOpen}
          toggle={toggleEditModal}
        />
      </Row>
    </Container>
  );
}
