# syntax=docker/dockerfile:1.4.3

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as builder
WORKDIR /build
COPY . .
RUN dotnet publish -c Release -o /build/publish

FROM scratch as final
COPY --from=builder /build/publish .