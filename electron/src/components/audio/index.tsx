import React, { FC } from "react";
import useActive from "../../hooks/useActive";
import WebRTC from "webrtc4me";

const framesPerPacket = 2048;

const Audio: FC<{ stream?: MediaStream; peer?: WebRTC }> = ({
  stream,
  peer
}) => {
  useActive(() => {
    const audioCtx = new AudioContext();
    const source = audioCtx.createMediaStreamSource(stream!);
    const processor = audioCtx.createScriptProcessor(framesPerPacket, 1, 1);
    source.connect(processor);
    const destinationNode = audioCtx.createMediaStreamDestination();
    processor.onaudioprocess = e => {
      const channelData = e.inputBuffer.getChannelData(0);
      const data = {
        pcm: channelData.map(v => Number(v)),
        pcmLength: channelData.length
      };
      console.log(data);
      peer!.send(JSON.stringify(data));
    };
    processor.connect(destinationNode);
  }, [stream, peer]);

  return <div />;
};

export default Audio;
