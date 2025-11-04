// src/useSignalR.js
import { useEffect, useRef, useState } from "react";
import * as signalR from "@microsoft/signalr";

export function useSignalR() {
  const [connected, setConnected] = useState(false);
  const [messages, setMessages] = useState([]);
  const connectionRef = useRef(null);

  useEffect(() => {
    let disposed = false;

    async function start() {
      const res = await fetch("https://function-app-001-drcscpegdzfjfjhn.uksouth-01.azurewebsites.net/api/negotiate", {method: "POST"});
      if (!res.ok) throw new Error("Failed to negotiate");
      const { url, accessToken } = await res.json();

      const conn = new signalR.HubConnectionBuilder()
        .withUrl(url, { accessTokenFactory: () => accessToken })
        .withAutomaticReconnect()
        .build();

      // 👇 flatten payloads so messages = array of objects, not array-of-array
      conn.on("batches", (payload) => {
        const normalized = Array.isArray(payload) ? payload : [payload];
        setMessages((prev) => [...prev, ...normalized]);
      });

      await conn.start();
      if (disposed) {
        await conn.stop();
        return;
      }
      connectionRef.current = conn;
      setConnected(true);
    }

    start().catch(console.error);

    return () => {
      disposed = true;
      if (connectionRef.current) {
        connectionRef.current.stop();
      }
    };
  }, []);

  return { connected, messages };
}
