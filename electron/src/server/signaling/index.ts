import WS_ from "ws";
import Event from "rx.mini";
const load = (window as any).require;
const WS: typeof WS_ = load("ws");

type Action = { type: string; payload: any };

const action = new Event<Action>();
const observer = new Event<Action>();

export { action, observer };

export default function signalingServer() {
  const wss = new WS.Server({ port: 8080 });

  console.log("start");

  let unity: WS_;

  action.subscribe(action => {
    unity.send(JSON.stringify(action));
  });

  wss.on("connection", ws => {
    console.log("connection");
    unity = ws;
    ws.on("message", data => {
      console.log("message", data);
      try {
        const action: Action = JSON.parse(data as string);
        observer.execute(action);
      } catch (error) {}
    });
  });
}
