# NPO-Client

This is a proof of concept made for my diploma thesis based on my studies at [TEKO Schweizerische Fachschule AG](https://www.teko.ch/) commissioned by [Post CH AG](https://www.post.ch/). The goal was to show how an application written based on .NET Core runs as a Windows Service and in a Linux Docker Container. The application itself subscribes an MQTT topic and publishes the received messages on an Apache Kafka topic.

## Prerequisites

To run the NPO-Client as a Windows Service all the following software is required. To run the NPO-Client in a Linux Docker Container only Docker is required.

- [Windows 10 Version 1903](https://www.microsoft.com/windows)
- [.NET Core SDK 2.2](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

To publish some messages on an MQTT topic "my/topic" and to read the messages on an Apache Kafka topic "test-topic", clients like these are helpful:

- [MQTT Explorer](https://mqtt-explorer.com/)
- [Kafka Tool](https://www.kafkatool.com/)

## Docker Compose

There are two versions of Docker Compose files.

To run all parts at once (MQTT, Apache Kafka and NPO-Client) set the docker-compose project in Visual Studio as startup project and start it or execute the following command in PowerShell:

```powershell
docker-compose up
```

To run only MQTT and Apache Kafka execute the following command in PowerShell. This version is used to test the NPO-Client as a Windows Service or in a Linux Docker Container, independent from the Docker Compose file, for faster testing without stopping all services:

```powershell
docker-compose -f docker-compose.services.yml up
```

## Windows Service

### Publish client

To register the NPO-Client the project has to be published with the following command:

```powershell
dotnet publish -c Release -o "C:\Services\Release\netcoreapp2.2\publish\" -r win10-x64 --self-contained false
```

### Register client as Windows Service

To be able to run the application as a Windows Service register it with the [SC-Tool](https://docs.microsoft.com/windows-server/administration/windows-commands/sc-create) by the following command. To execute the following command, use PowerShell as administrator:

```powershell
sc.exe create NPO-Client binPath="C:\Services\Release\netcoreapp2.2\publish\NPO-Client.exe --environment=Development"
```

### Delete client from Windows Services

To remove the NPO-Client from Windows Services run the following command:

```powershell
sc.exe delete NPO-Client
```

## Linux Docker Container

## Build image

To build an image from the source use the following command:

```powershell
docker build -t "npoclient:latest" -f .\NPO-Client\Dockerfile .
```

## Run image

To run a container from the previously created image use the following command:

```powershell
docker run -it --name "npo_client" --network "npoclient_default" -e "DOTNET_ENVIRONMENT=Development" npoclient:latest
```

## License

[MIT licensed](https://en.wikipedia.org/wiki/MIT_License)

Copyright (c) 2019 Roman Stocker

Please consider that some files are from the [.NET Extensions](https://github.com/aspnet/Extensions) project of the .NET Foundation and may have some other licenses.
