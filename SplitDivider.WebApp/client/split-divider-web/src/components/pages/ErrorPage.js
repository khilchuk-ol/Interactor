import React from "react";
import { Container } from "reactstrap";

import "../../styles/animation.css";
import { Link } from "react-router-dom";
import { Image } from "react-bootstrap";

export default class ErrorPage extends React.Component {
  render() {
    return (
      <div
        style={{
          paddingTop: "20rem"
        }}
      >
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            justifyContent: "flex-start"
          }}
        >
          <div>
            <Container>
              <h1 className="display-3">Oops, something went wrong</h1>
              <p className="lead">
                It seems that you don't have access to this page ot such page
                does not exist. Please, <Link to="/login">Log in</Link> to have
                full access
              </p>
            </Container>
          </div>
          <div>
            <Image
              height={"50%"}
              src={require("../../images/charts-error.png")}
            ></Image>
          </div>
        </div>
      </div>
    );
  }
}
