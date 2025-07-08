# syntax=docker/dockerfile:1.4.3

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine as builder
WORKDIR /build
COPY ../../. .
RUN apk add --no-cache nodejs npm
RUN npm install -g yarn
RUN dotnet publish src/Api/Api.csproj -c Release -o /build/publish

#########################################################################

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
EXPOSE 80
COPY --from=builder /build/publish .
ENTRYPOINT ["dotnet", "Api.dll"]