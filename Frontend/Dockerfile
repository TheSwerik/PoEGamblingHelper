# syntax=docker/dockerfile:1.4.3

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine as builder
WORKDIR /build
COPY --from=registry.swerik.dev/poegamblinghelper/shared:latest . ./packages
COPY . .
RUN dotnet publish -c Release -o /build/publish

#########################################################################

FROM nginx:alpine AS final
EXPOSE 80
EXPOSE 443
WORKDIR /usr/share/nginx/html
COPY --from=builder /build/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf