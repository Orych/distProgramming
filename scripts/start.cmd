@echo off


cd "../RankCalculator/RankCalculator"
start "" dotnet run

cd "../../EventsLogger"
start "" dotnet run

cd "../Valuator"

start dotnet run --urls "http://0.0.0.0:5001"
start dotnet run --urls "http://0.0.0.0:5002"

cd "../nats-server"
start "" "nats-server.exe"

cd "D:\nginx-1.25.4"
start "" "nginx.exe"