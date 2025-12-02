#!/bin/bash
echo "Validating StarGateAPI service..."

# Allow service a moment to warm up
sleep 5

# Health endpoint should return 200 OK
curl -f http://localhost:5000/health || exit 1

echo "Validation successful."
