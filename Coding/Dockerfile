FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /source

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

COPY *.csproj .
RUN dotnet restore

COPY ./ ./

RUN dotnet publish "./Coding.csproj" --output "./dist" --configuration Release --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=builder /source/dist .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "Coding.dll"]
