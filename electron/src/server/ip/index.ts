import internalIp_ from "internal-ip";
const load = (window as any).require;
const internalIp: typeof internalIp_ = load("internal-ip");

export default async function ipv4() {
  return await internalIp.v4();
}
