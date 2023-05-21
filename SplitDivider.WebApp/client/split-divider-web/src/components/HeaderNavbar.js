import React, { useState } from "react";
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink,
  NavbarText
} from "reactstrap";

export default function HeaderNavbar(props) {
  const { user } = props;

  const [isOpen, setIsOpen] = useState(false);

  const toggle = () => {
    setIsOpen(!isOpen);
  };

  return (
    <div>
      <Navbar
        color="dark"
        dark
        expand="md"
        style={{ height: "5rem", fontSize: "120%", paddingLeft: "2rem" }}
      >
        <NavbarBrand href="/" style={{ fontSize: "120%" }}>
          Split Divider
        </NavbarBrand>
        <NavbarToggler onClick={toggle} />
        <Collapse isOpen={isOpen} navbar>
          <Nav className="me-auto" navbar>
            <NavItem>
              <NavLink href="/splits">Splits</NavLink>
            </NavItem>
            <NavItem>
              <NavLink href="/participants">Participants</NavLink>
            </NavItem>
            <NavItem>
              <NavLink href="/documentation">Documentation</NavLink>
            </NavItem>
            <NavItem>
              {!!user ? (
                <NavLink href="/logout">Log out</NavLink>
              ) : (
                <NavLink href="/login">Log in</NavLink>
              )}
            </NavItem>
          </Nav>
          {user && (
            <NavbarText style={{ paddingRight: "2rem" }}>
              {user.email}
            </NavbarText>
          )}
        </Collapse>
      </Navbar>
    </div>
  );
}
