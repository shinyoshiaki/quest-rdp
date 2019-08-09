import WebRTC from "../../lib/webrtc";
import { observer, action } from "../../server/signaling";

export function create(roomId: string, stream?: MediaStream) {
  return new Promise<WebRTC>(async resolve => {
    const rtc = new WebRTC({ trickle: true, stream });

    observer.subscribe(action => {
      console.log(action);
      switch (action.type) {
        case "join":
          rtc.makeOffer();
          break;
        case "sdp":
          const sdp = action.payload;
          rtc.setSdp(sdp);
          break;
      }
    });

    rtc.onSignal.subscribe((session: any) => {
      console.log({ session });
      const { type, sdp, ice } = session;

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
      console.log("connect");
      resolve(rtc);
    });

    rtc.onData.once(e => console.log("connected", e.data));
  });
}
