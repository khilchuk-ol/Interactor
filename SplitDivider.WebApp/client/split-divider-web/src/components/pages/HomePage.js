import React from "react";
import { Container } from "reactstrap";

import "../../styles/animation.css";
import { Link } from "react-router-dom";

export default class HomePage extends React.Component {
  componentDidMount() {
    window.animate();
  }

  render() {
    return (
      <div>
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            flexWrap: "wrap",
            justifyContent: "flex-start"
          }}
        >
          <div
            style={{
              paddingTop: "20rem",
              paddingRight: "40rem"
            }}
          >
            <Container>
              <h1 className="display-3">Split Divider</h1>
              <p className="lead">
                Internal tool for handling A/B tests. For more information read{" "}
                <Link to="/documentation">Documentation</Link>
              </p>
              <p className="lead">
                Log in to have full access to all pages. Or register a new
                account and contact your manager to receive the appropriate role
                in the system
              </p>
            </Container>
          </div>
          <div>
            <canvas id="graphic" width="600" height="600"></canvas>
          </div>
        </div>
      </div>
    );
  }
}
