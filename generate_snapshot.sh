#! /bin/bash
msbuild.exe workers/Managed/Snapshots.csproj /property:Configuration=Snapshot /property:Platform=x64
workers/Managed/bin/snapshot/snapshot.exe snapshots/default.snapshot 