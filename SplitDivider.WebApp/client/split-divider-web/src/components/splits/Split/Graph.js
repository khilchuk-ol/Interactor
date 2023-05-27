import React, { useEffect, useRef, useState } from "react";
import cytoscape from "cytoscape";
import jquery from "jquery";
import graphml from "cytoscape-graphml";

import "../../../styles/graph.css";
import { Button } from "reactstrap";
import GraphmlHelper from "../../../services/graphml.helper";

graphml(cytoscape, jquery);

export default function Graph(props) {
  const { id, groups } = props;

  const [isDisbaled, setIsDisabled] = useState(false);

  let graphmlStr =
    '<?xml version="1.0" encoding="utf-8"?><graphml xmlns="http://graphml.graphdrawing.org/xmlns"><key id="weight" for="edge" attr.name="weight" attr.type="int" /><graph id="G" edgedefault="undirected" parse.nodes="14" parse.edges="182" parse.order="nodesfirst" parse.nodeids="free" parse.edgeids="free"><node id="39" /><node id="40" /><node id="41" /><node id="42" /><node id="43" /><node id="44" /><node id="48" /><node id="51" /><node id="56" /><node id="57" /><node id="59" /><node id="60" /><node id="61" /><node id="62" /><edge id="0" source="39" target="40"><data key="weight">0</data></edge><edge id="1" source="39" target="41"><data key="weight">0</data></edge><edge id="2" source="39" target="42"><data key="weight">0</data></edge><edge id="3" source="39" target="43"><data key="weight">0</data></edge><edge id="4" source="39" target="44"><data key="weight">0</data></edge><edge id="5" source="39" target="48"><data key="weight">0</data></edge><edge id="6" source="39" target="51"><data key="weight">0</data></edge><edge id="7" source="39" target="56"><data key="weight">0</data></edge><edge id="8" source="39" target="57"><data key="weight">0</data></edge><edge id="9" source="39" target="59"><data key="weight">0</data></edge><edge id="10" source="39" target="60"><data key="weight">0</data></edge><edge id="11" source="39" target="61"><data key="weight">0</data></edge><edge id="12" source="39" target="62"><data key="weight">0</data></edge><edge id="13" source="40" target="39"><data key="weight">0</data></edge><edge id="14" source="40" target="41"><data key="weight">0</data></edge><edge id="15" source="40" target="42"><data key="weight">0</data></edge><edge id="16" source="40" target="43"><data key="weight">0</data></edge><edge id="17" source="40" target="44"><data key="weight">0</data></edge><edge id="18" source="40" target="48"><data key="weight">0</data></edge><edge id="19" source="40" target="51"><data key="weight">0</data></edge><edge id="20" source="40" target="56"><data key="weight">0</data></edge><edge id="21" source="40" target="57"><data key="weight">0</data></edge><edge id="22" source="40" target="59"><data key="weight">0</data></edge><edge id="23" source="40" target="60"><data key="weight">0</data></edge><edge id="24" source="40" target="61"><data key="weight">0</data></edge><edge id="25" source="40" target="62"><data key="weight">0</data></edge><edge id="26" source="41" target="39"><data key="weight">0</data></edge><edge id="27" source="41" target="40"><data key="weight">0</data></edge><edge id="28" source="41" target="42"><data key="weight">0</data></edge><edge id="29" source="41" target="43"><data key="weight">0</data></edge><edge id="30" source="41" target="44"><data key="weight">0</data></edge><edge id="31" source="41" target="48"><data key="weight">0</data></edge><edge id="32" source="41" target="51"><data key="weight">0</data></edge><edge id="33" source="41" target="56"><data key="weight">0</data></edge><edge id="34" source="41" target="57"><data key="weight">0</data></edge><edge id="35" source="41" target="59"><data key="weight">0</data></edge><edge id="36" source="41" target="60"><data key="weight">0</data></edge><edge id="37" source="41" target="61"><data key="weight">0</data></edge><edge id="38" source="41" target="62"><data key="weight">0</data></edge><edge id="39" source="42" target="39"><data key="weight">0</data></edge><edge id="40" source="42" target="40"><data key="weight">0</data></edge><edge id="41" source="42" target="41"><data key="weight">0</data></edge><edge id="42" source="42" target="43"><data key="weight">0</data></edge><edge id="43" source="42" target="44"><data key="weight">0</data></edge><edge id="44" source="42" target="48"><data key="weight">0</data></edge><edge id="45" source="42" target="51"><data key="weight">0</data></edge><edge id="46" source="42" target="56"><data key="weight">0</data></edge><edge id="47" source="42" target="57"><data key="weight">0</data></edge><edge id="48" source="42" target="59"><data key="weight">0</data></edge><edge id="49" source="42" target="60"><data key="weight">0</data></edge><edge id="50" source="42" target="61"><data key="weight">0</data></edge><edge id="51" source="42" target="62"><data key="weight">0</data></edge><edge id="52" source="43" target="39"><data key="weight">0</data></edge><edge id="53" source="43" target="40"><data key="weight">0</data></edge><edge id="54" source="43" target="41"><data key="weight">0</data></edge><edge id="55" source="43" target="42"><data key="weight">0</data></edge><edge id="56" source="43" target="44"><data key="weight">0</data></edge><edge id="57" source="43" target="48"><data key="weight">0</data></edge><edge id="58" source="43" target="51"><data key="weight">0</data></edge><edge id="59" source="43" target="56"><data key="weight">1</data></edge><edge id="60" source="43" target="57"><data key="weight">0</data></edge><edge id="61" source="43" target="59"><data key="weight">0</data></edge><edge id="62" source="43" target="60"><data key="weight">0</data></edge><edge id="63" source="43" target="61"><data key="weight">1</data></edge><edge id="64" source="43" target="62"><data key="weight">0</data></edge><edge id="65" source="44" target="39"><data key="weight">0</data></edge><edge id="66" source="44" target="40"><data key="weight">0</data></edge><edge id="67" source="44" target="41"><data key="weight">0</data></edge><edge id="68" source="44" target="42"><data key="weight">0</data></edge><edge id="69" source="44" target="43"><data key="weight">0</data></edge><edge id="70" source="44" target="48"><data key="weight">0</data></edge><edge id="71" source="44" target="51"><data key="weight">0</data></edge><edge id="72" source="44" target="56"><data key="weight">0</data></edge><edge id="73" source="44" target="57"><data key="weight">0</data></edge><edge id="74" source="44" target="59"><data key="weight">0</data></edge><edge id="75" source="44" target="60"><data key="weight">0</data></edge><edge id="76" source="44" target="61"><data key="weight">0</data></edge><edge id="77" source="44" target="62"><data key="weight">0</data></edge><edge id="78" source="48" target="39"><data key="weight">0</data></edge><edge id="79" source="48" target="40"><data key="weight">0</data></edge><edge id="80" source="48" target="41"><data key="weight">0</data></edge><edge id="81" source="48" target="42"><data key="weight">0</data></edge><edge id="82" source="48" target="43"><data key="weight">0</data></edge><edge id="83" source="48" target="44"><data key="weight">0</data></edge><edge id="84" source="48" target="51"><data key="weight">0</data></edge><edge id="85" source="48" target="56"><data key="weight">0</data></edge><edge id="86" source="48" target="57"><data key="weight">0</data></edge><edge id="87" source="48" target="59"><data key="weight">0</data></edge><edge id="88" source="48" target="60"><data key="weight">0</data></edge><edge id="89" source="48" target="61"><data key="weight">0</data></edge><edge id="90" source="48" target="62"><data key="weight">0</data></edge><edge id="91" source="51" target="39"><data key="weight">0</data></edge><edge id="92" source="51" target="40"><data key="weight">0</data></edge><edge id="93" source="51" target="41"><data key="weight">0</data></edge><edge id="94" source="51" target="42"><data key="weight">0</data></edge><edge id="95" source="51" target="43"><data key="weight">0</data></edge><edge id="96" source="51" target="44"><data key="weight">0</data></edge><edge id="97" source="51" target="48"><data key="weight">0</data></edge><edge id="98" source="51" target="56"><data key="weight">0</data></edge><edge id="99" source="51" target="57"><data key="weight">0</data></edge><edge id="100" source="51" target="59"><data key="weight">0</data></edge><edge id="101" source="51" target="60"><data key="weight">0</data></edge><edge id="102" source="51" target="61"><data key="weight">0</data></edge><edge id="103" source="51" target="62"><data key="weight">0</data></edge><edge id="104" source="56" target="39"><data key="weight">0</data></edge><edge id="105" source="56" target="40"><data key="weight">0</data></edge><edge id="106" source="56" target="41"><data key="weight">0</data></edge><edge id="107" source="56" target="42"><data key="weight">0</data></edge><edge id="108" source="56" target="43"><data key="weight">0</data></edge><edge id="109" source="56" target="44"><data key="weight">0</data></edge><edge id="110" source="56" target="48"><data key="weight">0</data></edge><edge id="111" source="56" target="51"><data key="weight">0</data></edge><edge id="112" source="56" target="57"><data key="weight">0</data></edge><edge id="113" source="56" target="59"><data key="weight">0</data></edge><edge id="114" source="56" target="60"><data key="weight">0</data></edge><edge id="115" source="56" target="61"><data key="weight">0</data></edge><edge id="116" source="56" target="62"><data key="weight">0</data></edge><edge id="117" source="57" target="39"><data key="weight">0</data></edge><edge id="118" source="57" target="40"><data key="weight">0</data></edge><edge id="119" source="57" target="41"><data key="weight">0</data></edge><edge id="120" source="57" target="42"><data key="weight">0</data></edge><edge id="121" source="57" target="43"><data key="weight">0</data></edge><edge id="122" source="57" target="44"><data key="weight">0</data></edge><edge id="123" source="57" target="48"><data key="weight">0</data></edge><edge id="124" source="57" target="51"><data key="weight">0</data></edge><edge id="125" source="57" target="56"><data key="weight">0</data></edge><edge id="126" source="57" target="59"><data key="weight">0</data></edge><edge id="127" source="57" target="60"><data key="weight">0</data></edge><edge id="128" source="57" target="61"><data key="weight">0</data></edge><edge id="129" source="57" target="62"><data key="weight">0</data></edge><edge id="130" source="59" target="39"><data key="weight">0</data></edge><edge id="131" source="59" target="40"><data key="weight">0</data></edge><edge id="132" source="59" target="41"><data key="weight">0</data></edge><edge id="133" source="59" target="42"><data key="weight">0</data></edge><edge id="134" source="59" target="43"><data key="weight">0</data></edge><edge id="135" source="59" target="44"><data key="weight">0</data></edge><edge id="136" source="59" target="48"><data key="weight">0</data></edge><edge id="137" source="59" target="51"><data key="weight">0</data></edge><edge id="138" source="59" target="56"><data key="weight">0</data></edge><edge id="139" source="59" target="57"><data key="weight">0</data></edge><edge id="140" source="59" target="60"><data key="weight">0</data></edge><edge id="141" source="59" target="61"><data key="weight">0</data></edge><edge id="142" source="59" target="62"><data key="weight">0</data></edge><edge id="143" source="60" target="39"><data key="weight">0</data></edge><edge id="144" source="60" target="40"><data key="weight">0</data></edge><edge id="145" source="60" target="41"><data key="weight">0</data></edge><edge id="146" source="60" target="42"><data key="weight">0</data></edge><edge id="147" source="60" target="43"><data key="weight">0</data></edge><edge id="148" source="60" target="44"><data key="weight">0</data></edge><edge id="149" source="60" target="48"><data key="weight">0</data></edge><edge id="150" source="60" target="51"><data key="weight">0</data></edge><edge id="151" source="60" target="56"><data key="weight">0</data></edge><edge id="152" source="60" target="57"><data key="weight">0</data></edge><edge id="153" source="60" target="59"><data key="weight">0</data></edge><edge id="154" source="60" target="61"><data key="weight">0</data></edge><edge id="155" source="60" target="62"><data key="weight">0</data></edge><edge id="156" source="61" target="39"><data key="weight">0</data></edge><edge id="157" source="61" target="40"><data key="weight">0</data></edge><edge id="158" source="61" target="41"><data key="weight">0</data></edge><edge id="159" source="61" target="42"><data key="weight">0</data></edge><edge id="160" source="61" target="43"><data key="weight">0</data></edge><edge id="161" source="61" target="44"><data key="weight">0</data></edge><edge id="162" source="61" target="48"><data key="weight">0</data></edge><edge id="163" source="61" target="51"><data key="weight">1</data></edge><edge id="164" source="61" target="56"><data key="weight">0</data></edge><edge id="165" source="61" target="57"><data key="weight">0</data></edge><edge id="166" source="61" target="59"><data key="weight">0</data></edge><edge id="167" source="61" target="60"><data key="weight">0</data></edge><edge id="168" source="61" target="62"><data key="weight">0</data></edge><edge id="169" source="62" target="39"><data key="weight">0</data></edge><edge id="170" source="62" target="40"><data key="weight">0</data></edge><edge id="171" source="62" target="41"><data key="weight">0</data></edge><edge id="172" source="62" target="42"><data key="weight">0</data></edge><edge id="173" source="62" target="43"><data key="weight">0</data></edge><edge id="174" source="62" target="44"><data key="weight">0</data></edge><edge id="175" source="62" target="48"><data key="weight">0</data></edge><edge id="176" source="62" target="51"><data key="weight">0</data></edge><edge id="177" source="62" target="56"><data key="weight">0</data></edge><edge id="178" source="62" target="57"><data key="weight">0</data></edge><edge id="179" source="62" target="59"><data key="weight">0</data></edge><edge id="180" source="62" target="60"><data key="weight">0</data></edge><edge id="181" source="62" target="61"><data key="weight">0</data></edge></graph></graphml>';
  graphmlStr = GraphmlHelper.prepareGraphML(graphmlStr);

  const containerRef = useRef();
  const [cy, setCy] = useState({});

  useEffect(() => {
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
