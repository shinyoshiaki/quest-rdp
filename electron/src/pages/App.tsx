import React, { FC, useState } from "react";
import Display from "../components/display";
import { create } from "../domain/webrtc/signaling";
import { moveMouse, clickMouse, keyTap } from "../server/robot";
import ShowIP from "../components/showip";
import ScreenList from "../components/screenlist";
import { Grow } from "@material-ui/core";
import Reload from "../components/reload";
import { getScreen } from "../domain/screen/screen";
import styled from "styled-components";
import Audio from "../components/audio";
import WebRTC from "webrtc4me";

const Cast: FC = () => {
  const [screenStream, setscreenStream] = useState<MediaStream>();
  const [msg, setmsg] = useState("ipアドレスを入力してください");
  const [peer, setpeer] = useState<WebRTC>();

  const onSelectScreen = async (id: string) => {
    const res = await getScreen(id);
    setscreenStream(res);
    onStream(res);
  };

  const onStream = async (stream: MediaStream) => {
    const peer = await create(stream);
    setmsg("接続完了");
    setpeer(peer);

    peer.onData.subscribe((msg: any) => {
      console.log(msg);
      const data = JSON.parse(msg.data);
      switch (data.type) {
        case "move":
          moveMouse.execute(data.payload);
          break;
        case "click":
          clickMouse.execute(null);
          break;
        case "key":
          keyTap.execute(data.payload);
          break;
      }
    });

    peer.onDisconnect.once(() => {
      window.location.reload();
    });
  };

  return (
    <div>
      {!screenStream && (
        <div>
          <p style={{ fontSize: 23 }}>シェアする画面を選択してください</p>
          <ScreenList onClick={onSelectScreen} />
        </div>
      )}
      {screenStream && <Reload>{"再選択"}</Reload>}
      {screenStream && (
        <Base in={true} timeout={1000}>
          <div>
            <div style={{ width: "70vw" }}>
              <p style={{ fontSize: 23 }}>{msg}</p>
              <ShowIP />
              <Display strem={screenStream} />
            </div>
            <Audio stream={screenStream} peer={peer} />
          </div>
        </Base>
      )}
    </div>
  );
};

export default Cast;

const Base = styled(Grow)`
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
`;
