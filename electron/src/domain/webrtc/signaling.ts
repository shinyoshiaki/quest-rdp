import WebRTC from "webrtc4me";
import { observer, action } from "../../server/signaling";

export const create = (stream: MediaStream) =>
  new Promise<WebRTC>(resolve => {
    const rtc = new WebRTC({ trickle: true, stream });

    observer.subscribe(({ type, payload }) => {
      switch (type) {
        case "join":
          rtc.makeOffer();
          break;
        case "sdp":
          rtc.setSdp(payload);
          break;
      }
    });

    rtc.onSignal.subscribe(({ type, sdp, ice }) => {
      if (sdp) {
        const payload = type + "%" + sdp;
        action.execute({ type: "offer", payload });
      } else if (ice) {
        const { candidate, sdpMLineIndex, sdpMid } = ice;
        const payload =
          type + "%" + candidate + "%" + sdpMLineIndex + "%" + sdpMid;
        action.execute({ type: "ice", payload });
      }
    });

    rtc.onConnect.once(() => resolve(rtc));
    rtc.onData.once(e => console.log("connected", e.data));
  });
