#!/bin/bash
echo "Preparing application directory..."

APP_DIR="/home/ec2-user/app"

# Ensure directory exists
mkdir -p $APP_DIR

# Ensure correct ownership
chown -R ec2-user:ec2-user $APP_DIR

# Ensure proper file permissions
chmod -R 755 $APP_DIR

echo "Dependencies installed and directory ready."
