import WebRTC from "webrtc4me";
import { observer, action } from "../../server/signaling";

export function create(stream: MediaStream) {
  return new Promise<WebRTC>(async resolve => {
    const rtc = new WebRTC({ trickle: true, stream });

    observer.subscribe(({ type, payload }) => {
      switch (type) {
        case "join":
          rtc.makeOffer();
          break;
        case "sdp":
          const sdp = payload;
          rtc.setSdp(sdp);
          break;
      }
    });

    rtc.onSignal.subscribe(({ type, sdp, ice }) => {
      if (sdp) {
        const data = type + "%" + sdp;
        action.execute({ type: "offer", payload: data });
      } else if (ice) {
        const { candidate, sdpMLineIndex, sdpMid } = ice;
        const data =
          type + "%" + candidate + "%" + sdpMLineIndex + "%" + sdpMid;
        action.execute({ type: "ice", payload: data });
      }
    });

    rtc.onConnect.once(() => {
      resolve(rtc);
    });

    rtc.onData.once(e => console.log("connected", e.data));
  });
}
