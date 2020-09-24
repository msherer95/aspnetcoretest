# escape=` 

FROM mcr.microsoft.com/powershell:nanoserver-1803 AS downloadnodejs
SHELL ["pwsh", "-Command", "$ErrorActionPreference = 'Stop';$ProgressPreference='silentlyContinue';"]
RUN Invoke-WebRequest -OutFile nodejs.zip -UseBasicParsing "https://nodejs.org/dist/v12.18.3/node-v12.18.3-win-x64.zip"; `
Expand-Archive nodejs.zip -DestinationPath C:\; `
Rename-Item "C:\node-v12.18.3-win-x64" c:\nodejs


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1803 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1803 AS debug-test
EXPOSE 5000
EXPOSE 5001
WORKDIR /src

COPY --from=downloadnodejs C:\nodejs\ C:\Windows\system32\
WORKDIR "/src/Webapp"
ENTRYPOINT ["dotnet", "run", "Webapp.dll"]


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1803 AS debug
EXPOSE 5000
EXPOSE 5001
WORKDIR /src

COPY --from=downloadnodejs C:\nodejs\ C:\Windows\system32\
COPY ["Webapp/Webapp.csproj", "Webapp/"]
RUN dotnet restore "Webapp/Webapp.csproj"
COPY . .
WORKDIR "/src/Webapp/ClientApp"
#RUN npm install
WORKDIR "/src/new_src"
WORKDIR "/src/Webapp"
ENTRYPOINT ["dotnet", "run", "Webapp.dll"]


FROM build AS publish
RUN dotnet publish "Webapp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "run", "Webapp.dll"]
