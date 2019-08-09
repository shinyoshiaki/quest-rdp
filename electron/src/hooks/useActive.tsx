import { useEffect } from "react";

export default function useActive(fun: () => void, bind: any[]) {
  useEffect(() => {
    const unexist = bind.filter(v => v === undefined || null);
    if (unexist.length === 0) fun();
  }, [...bind]);
}
