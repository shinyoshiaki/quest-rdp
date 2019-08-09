import React, { FC, useState, useEffect } from "react";
import ipv4 from "../../server/ip";
import styled from "styled-components";

const ShowIP: FC = () => {
  const [ip, setIp] = useState("");

  useEffect(() => {
    (async () => {
      const res = await ipv4();
      setIp(res);
    })();
  }, []);

  return <Text>{ip}</Text>;
};

export default ShowIP;

const Text = styled.p`
  width: 100%;
  font-size: 30px;
  text-align: center;
`;
