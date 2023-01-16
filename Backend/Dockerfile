# syntax=docker/dockerfile:1.4.3

FROM mcr.microsoft.com/dotnet/sdk:7.0 as builder
WORKDIR /build
COPY --from=registry.swerik.dev/poegamblinghelper/model:latest . ./packages
COPY . .
RUN dotnet publish -c Release -o /build/publish

#########################################################################

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
WORKDIR /app
EXPOSE 80
COPY --from=builder /build/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]