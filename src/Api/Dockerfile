# syntax=docker/dockerfile:1.4.3

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as builder
WORKDIR /build
COPY ../../. .
RUN dotnet publish -c Release -o /build/publish src/Api/Api.csproj

#########################################################################

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
WORKDIR /app
EXPOSE 80
COPY --from=builder /build/publish .
ENTRYPOINT ["dotnet", "Api.dll"]