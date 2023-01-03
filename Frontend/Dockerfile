# syntax=docker/dockerfile:1.4.3

FROM alpine:3.17.0 as file-loader
WORKDIR /data
COPY scripts/clean-files-for-caching.sh /scripts/
RUN chmod -R 555 /scripts

#########################################################################

FROM registry.swerik.dev/poegamblinghelper/model:$MODEL_TAG as model
RUN echo "This is build with Shared-Lib ${MODEL_TAG}"

FROM file-loader as base-files
COPY --from=model /build/publish ./packages
COPY Frontend.csproj .
RUN /scripts/clean-files-for-caching.sh .

#########################################################################

FROM file-loader as source-files
COPY . .
RUN /scripts/clean-files-for-caching.sh .

#########################################################################
#########################################################################

FROM mcr.microsoft.com/dotnet/sdk:7.0 as dependencyfetcher
WORKDIR /build
COPY --from=base-files /data .
RUN dotnet restore

#########################################################################

FROM dependencyfetcher as tester
WORKDIR /build
COPY --from=source-files /data .
RUN dotnet test

#########################################################################

FROM dependencyfetcher as builder
COPY --from=source-files /data .
RUN dotnet publish -c Release -o /build/publish

#########################################################################
#########################################################################

FROM nginx:alpine AS final
EXPOSE 80
EXPOSE 443
WORKDIR /usr/share/nginx/html
COPY --from=builder /build/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf