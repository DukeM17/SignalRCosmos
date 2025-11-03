import React, { useEffect, useState } from "react";
import { useSignalR } from "./useSignalR";

export default function MainBatchApp() {
  const { connected, messages } = useSignalR();

  // Keep stage → status map
  const [stageStatus, setStageStatus] = useState({});
  const [order, setOrder] = useState([]);

  // When a new message arrives
  useEffect(() => {
    if (!messages.length) return;

    // Get the latest batch update (the last message)
    const latest = messages[messages.length - 1];

    // Loop through its statusUpdates array
    if (Array.isArray(latest.statusUpdates)) {
      latest.statusUpdates.forEach(({ stage, status, updatedAt, additionalInformation }) => {
        setStageStatus((prev) => ({
          ...prev,
          [stage]: { status, updatedAt, additionalInformation },
        }));

        setOrder((prev) => (prev.includes(stage) ? prev : [...prev, stage]));
      });
    }
  }, [messages]);

  return (
    <div style={{ padding: "20px" }}>
      <h2>Batch Updates</h2>
      <p>Status: {connected ? "🟢 Connected to Cosmos" : "🔴 Disconnected from Cosmos"}</p>

      <div style={{ display: "flex", gap: "16px" }}>
        {order.map((stageName) => {
          const info = stageStatus[stageName] ?? { status: "Pending" };
          return (
            <div
              key={stageName}
              style={{
                border: "1px solid #ccc",
                borderRadius: "8px",
                padding: "10px 14px",
                minWidth: "200px",
              }}
            >
              <div style={{ fontWeight: 600 }}>{stageName}</div>
              <div
                style={{
                  color:
                    info.status === "Completed"
                      ? "green"
                      : info.status === "Failed"
                      ? "red"
                      : "gray",
                }}
              >
                {info.status}
              </div>
              <div>{info.updatedAt}</div>
              <div
              style={{
                color:
                  info.status === "Completed"
                    ? "green"
                    : info.status === "Failed"
                    ? "red"
                    : "gray",
              }}><strong>{info.additionalInformation}</strong></div>
            </div>
          );
        })}
      </div>
    </div>
  );
}
