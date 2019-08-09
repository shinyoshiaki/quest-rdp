import remote from "./robot";
import signalingServer from "./signaling";

export default async function server() {
  remote();
  signalingServer();
}
