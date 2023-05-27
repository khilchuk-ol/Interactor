import React, { useEffect, useState } from "react";
import {
  Button,
  Col,
  Container,
  Dropdown,
  DropdownItem,
  DropdownMenu,
  DropdownToggle,
  Row,
  Table
} from "reactstrap";
import Icon, { iconTypes } from "../utils/Icon";
import adminService from "../../services/admin.service";
import AdminService from "../../services/admin.service";

export default function UserAndRoles(props) {
  const { user, userRoles, setUserRoles, onError } = props;

  const [addableRoles, setAddableRoles] = useState([]);
  const [isOpenDropdown, setIsOpenDropdown] = useState(false);

  useEffect(() => {
    AdminService.fetchRoles()
      .then(resp => {
        let addableRolesR = resp.filter(
          r => !userRoles.map(r => r.id).includes(r.id)
        );
        setAddableRoles(addableRolesR);
      })
      .catch(err => {
        onError(err);
      });
  }, [userRoles]);

  const removeRole = role => {
    adminService
      .deleteRoleForUser(user.id, role.name)
      .then(res => {
        let updatedRoles = [];
        userRoles.forEach(role => {
          if (role.id !== role.id) {
            updatedRoles.push(role);
          } else {
            addableRoles.push(role);
          }
        });

        setUserRoles(updatedRoles);
        setAddableRoles(addableRoles);
      })
      .catch(err => {
        onError(err.msg);
      });
  };

  const addRole = role => {
    adminService
      .addRoleForUser(user.id, role.name)
      .then(res => {
        userRoles.push(role);
        setUserRoles(userRoles);

        setAddableRoles(addableRoles.filter(r => r.id !== role.id));
      })
      .catch(err => {
        onError(err.msg);
      });
  };

  return (
    <Container className={"text-start"} style={{ paddingLeft: "20%" }}>
      <Row style={{ padding: "1rem" }}>
        <Col tag={"h5"}>{user.email}</Col>
      </Row>
      <Row>
        <Col>
          <Table className={"border"} style={{ width: "65%" }}>
            <tbody>
              {userRoles.map(role => (
                <tr>
                  <td>
                    <Row>
                      <Col>{role.name}</Col>
                      <Col className={"text-end"}>
                        <Button
                          className={"btn border btn-light btn-block"}
                          onClick={() => {
                            removeRole(role);
                          }}
                        >
                          <Icon type={iconTypes.closeSign} />
                        </Button>
                      </Col>
                    </Row>
                  </td>
                </tr>
              ))}
              {addableRoles.length > 0 && (
                <tr>
                  <td>
                    <div style={{ overflow: "hidden" }}>
                      <Dropdown
                        isOpen={isOpenDropdown}
                        toggle={() => {
                          setIsOpenDropdown(!isOpenDropdown);
                        }}
                      >
                        <DropdownToggle caret>Add role</DropdownToggle>
                        <DropdownMenu container="body">
                          {addableRoles.map(r => (
                            <DropdownItem onClick={() => addRole(r)}>
                              {r.name}
                            </DropdownItem>
                          ))}
                        </DropdownMenu>
                      </Dropdown>
                    </div>
                  </td>
                </tr>
              )}
            </tbody>
          </Table>
        </Col>
      </Row>
    </Container>
  );
}
