# syntax=docker/dockerfile:1.4.3

FROM alpine:3.17.0 as file-loader
WORKDIR /data
COPY scripts/clean-files-for-caching.sh /scripts/
RUN chmod -R 555 /scripts

#########################################################################

FROM file-loader as base-files
COPY Model.csproj .
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

FROM dependencyfetcher as final
COPY --from=source-files /data .
RUN dotnet publish -c Release -o /build/publish