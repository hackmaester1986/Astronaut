#!/bin/bash
echo "Stopping StarGateAPI service..."
systemctl stop myapp || true

# Give systemd a second to fully stop the service
sleep 2
