// src/MainApp.jsx
import React from "react";
import { useSignalR } from "./useSignalR";

const MainApp = () => {
  const { connected, messages } = useSignalR();

  return (
    <div style={{ maxWidth: 720, margin: "40px auto", fontFamily: "sans-serif" }}>
      <h1>Order Updates</h1>
      <p>Status: {connected ? "Connected" : "Connecting..."}</p>

      <ul style={{ listStyle: "none", padding: 0 }}>
        {messages.map((m, idx) => (
          <li key={idx} style={{ padding: "12px 16px", border: "1px solid #ddd", borderRadius: 8, marginBottom: 10 }}>
            <pre style={{ margin: 0, whiteSpace: "pre-wrap", wordBreak: "break-word" }}>
{typeof m === "string" ? m : JSON.stringify(m, null, 2)}
            </pre>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default MainApp;
