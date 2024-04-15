@REM cd ..\RankCalculator\
@REM start dotnet run

@REM cd ..\nats-server\
@REM start nats-server.exe

cd ..\Valuator\
start dotnet run --urls "http://localhost:5001"
start dotnet run --urls "http://localhost:5002"

cd D:\nginx-1.25.4
start nginx -c D:\nginx-1.25.4\conf\nginx.conf

@REM cd ..\..\..\..\..\..\nginx-1.25.4
@REM start nginx -c ..\..\..\..\..\..\nginx-1.25.4\conf\nginx.conf