## Install Microsoft .NET Framework SDK (version 4.7.2)
FROM microsoft/dotnet-framework:4.7.2-sdk AS builder
WORKDIR /src

## Restore the default Windows shell for correct batch processing below
SHELL ["cmd", "/S", "/C"]

## Copy .sln, .snk and .csproj files
COPY ["*.sln", "*.snk", "*.csproj", "./"]

## Copy everything else
COPY [".", "./"]

## Copy and install DbAx
COPY ["DbAx/DB-AX.CNT", "DbAx/DB-AX.GID", "DbAx/DBAX.LIC", "DbAx/DBAX.OCX", "DbAx/DSI.REG", "DbAx/DSIUSR32.DLL", "DbAx/IMPBORL.DLL", "/Windows/SysWOW64/"]
RUN /Windows/SysWOW64/regsvr32.exe /s C:\Windows\SysWOW64\DBAX.OCX

## Download the Build Tools bootstrapper
ARG CHANNEL_URL=https://aka.ms/vs/15/release/channel
ADD ${CHANNEL_URL} /Temp/VisualStudio.chman
ADD https://aka.ms/vs/15/release/vs_buildtools.exe /Temp/vs_buildtools.exe

## Install needed components from Build Tools
RUN /Temp/vs_buildtools.exe --quiet --wait --norestart --nocache --channelUri C:\Temp\VisualStudio.chman --installChannelUri C:\Temp\VisualStudio.chman --installPath C:\BuildTools --add Microsoft.Net.Component.4.7.2.SDK --add Microsoft.Net.Component.4.7.2.TargetingPack

## Build the application
WORKDIR /src/OSDevGrp.OSIntranet.DataAccess.Services
RUN MSBuild.exe OSDevGrp.OSIntranet.DataAccess.Services.csproj /p:Configuration=Release /p:OutputPath=out

## Install Microsoft .NET Framework (version 4.7.2)
FROM microsoft/dotnet-framework:4.7.2-runtime
WORKDIR /OSDevGrp.OSIntranet.DataAccess
COPY --from=builder /src/OSDevGrp.OSIntranet.DataAccess.Services/out ./

## Copy and install DbAx
COPY ["DbAx/DB-AX.CNT", "DbAx/DB-AX.GID", "DbAx/DBAX.LIC", "DbAx/DBAX.OCX", "DbAx/DSI.REG", "DbAx/DSIUSR32.DLL", "DbAx/IMPBORL.DLL", "/Windows/SysWOW64/"]
RUN /Windows/SysWOW64/regsvr32.exe /s C:\Windows\SysWOW64\DBAX.OCX

# Install the service
RUN /Windows/Microsoft.NET/Framework/v4.0.30319/InstallUtil C:\OSDevGrp.OSIntranet.DataAccess\OSDevGrp.OSIntranet.DataAccess.Services.exe

EXPOSE 7000
VOLUME ["C:/Data"]

ENTRYPOINT ["powershell.exe", "-NoLogo", "-NoExit", "-ExecutionPolicy", "Bypass", "-Command", "Start-Service -Name 'OS Intranet - Data Access'"]