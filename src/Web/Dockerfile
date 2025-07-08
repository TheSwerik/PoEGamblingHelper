# syntax=docker/dockerfile:1.4.3

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine as builder
WORKDIR /build
COPY ../../. .
RUN dotnet publish -c Release -o /build/publish src/Web/Web.csproj

#########################################################################

FROM nginx:alpine AS final
EXPOSE 80
EXPOSE 443
WORKDIR /usr/share/nginx/html
COPY --from=builder /build/publish/wwwroot .
COPY src/Web/nginx.conf /etc/nginx/nginx.conf