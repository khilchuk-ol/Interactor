import React, { useEffect, useRef, useState } from "react";
import cytoscape from "cytoscape";
import jquery from "jquery";
import graphml from "cytoscape-graphml";

import "../../../styles/graph.css";
import { Button } from "reactstrap";
import GraphmlHelper from "../../../services/graphml.helper";
import SplitService from "../../../services/split.service";

graphml(cytoscape, jquery);

export default function Graph(props) {
  const { id, groups, onError } = props;

  const [isDisbaled, setIsDisabled] = useState(false);
  const [graphmlStr, setGraphmlStr] = useState("");

  const containerRef = useRef();
  const [cy, setCy] = useState({});

  useEffect(() => {
    SplitService.getSplitGraph(id)
      .then(data => {
        setGraphmlStr(GraphmlHelper.prepareGraphML(data));
      })
      .catch(err => {
        onError(err.message);
      });

    const config = {
      container: containerRef.current,
      style: [
        {
          selector: "node",
          style: {
            label: "data(id)",
            width: "8px",
            height: "8px",
            fontSize: "4px",
            textValign: "center"
          }
        },
        {
          selector: "edge",
          style: {
            width: "data(weight)"
          }
        },
        {
          selector: "node.control",
          style: {
            label: "data(id)",
            width: "8px",
            height: "8px",
            fontSize: "4px",
            textValign: "center",
            backgroundColor: "#78d65e"
          }
        },
        {
          selector: "node.test",
          style: {
            label: "data(id)",
            width: "8px",
            height: "8px",
            fontSize: "4px",
            textValign: "center",
            backgroundColor: "orange"
          }
        }
      ]
    };
    setCy(cytoscape(config));

    return () => {
      setCy({});
    };
  }, []);

  return (
    <div className={"text-start"}>
      <Button
        disabled={isDisbaled}
        onClick={() => {
          setIsDisabled(true);

          cy.graphml({
            layoutBy: "cose",
            node: {
              data: true,
              position: true
            },
            edge: {
              // css: true,
              data: true
            }
          });
          cy.graphml(graphmlStr);

          groups[0].forEach(id => {
            cy.getElementById("" + id).addClass("control");
          });

          groups[1].forEach(id => {
            cy.getElementById("" + id).addClass("test");
          });

          const eles = cy.elements();

          cy.elements().remove();

          cy.add(eles);
        }}
      >
        Show graph
      </Button>
      <div ref={containerRef} style={{ height: "60vh" }} />
    </div>
  );
}
