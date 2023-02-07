#!/bin/sh
set -e

TARGET_DIR=$1

if [ -z "$TARGET_DIR" ]; then
    echo "Usage $0 <dir with files to clear>"
    exit 1
fi

chmod -R 555 "$TARGET_DIR"
chown -R root:root "$TARGET_DIR"

find "$TARGET_DIR" -exec touch -a -m -t 201512180130.09 {} \;