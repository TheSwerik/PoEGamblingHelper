# syntax=docker/dockerfile:1.4.3
ARG MODEL_TAG=latest

FROM alpine:3.17.0 as file-loader
WORKDIR /data
COPY scripts/clean-files-for-caching.sh /scripts/
RUN chmod -R 555 /scripts

#########################################################################

FROM registry.swerik.dev/poegamblinghelper/model:$MODEL_TAG as model
RUN echo "This is build with Shared-Lib ${MODEL_TAG}"

FROM file-loader as base-files
COPY --from=model /build/publish ./packages
COPY Backend.csproj .
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

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final
WORKDIR /app
EXPOSE 80
COPY --from=builder /build/publish .
ENTRYPOINT ["dotnet", "Backend.dll"]