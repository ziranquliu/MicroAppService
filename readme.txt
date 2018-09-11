dotnet WebApp.dll --ip 127.0.0.1 --port 8889

dotnet WebAppService.dll --ip 127.0.0.1 --port 5001
dotnet WebAppService.dll --ip 127.0.0.1 --port 5002

cmd启动方式
set ASPNETCORE_URLS=http://127.0.0.1:5001
dotnet smsservice1.dll

set ASPNETCORE_URLS=http://127.0.0.1:5002
dotnet emailservice1.dll


powershell启动方式
# Unix:
ASPNETCORE_URLS="https://*:5123" dotnet run

# Windows PowerShell:
$env:ASPNETCORE_URLS="https://*:5123" ; dotnet run

# Windows CMD (note: no quotes):
SET ASPNETCORE_URLS=https://*:5123 && dotnet run


dotnet ocelotserver1.dll



dotnet smsservice2.dll --ip 127.0.0.1 --port 5001
dotnet smsservice2.dll --ip 127.0.0.1 --port 5002


dotnet ocelotserver2.dll




set ASPNETCORE_URLS=http://127.0.0.1:5001

dotnet JWTTokenServer1.dll
http://127.0.0.1:5001




set ASPNETCORE_URLS=http://127.0.0.1:5002
dotnet calcservice3.dll




set ASPNETCORE_URLS=http://127.0.0.1:5000
dotnet ocelotserver3.dll






dotnet ID4.IdServer.dll




set ASPNETCORE_URLS=http://127.0.0.1:10000
dotnet ocelot_id4server.dll



dotnet MsgService.dll --ip 127.0.0.1 --port 5001
dotnet ProductService.dll --ip 127.0.0.1 --port 5002
