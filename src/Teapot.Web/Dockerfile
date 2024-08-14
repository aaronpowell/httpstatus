#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Teapot.Web/Teapot.Web.csproj", "Teapot.Web/"]
COPY [".git", ".git"]
RUN dotnet restore "Teapot.Web/Teapot.Web.csproj"
COPY src/Teapot.Web Teapot.Web
WORKDIR "/src/Teapot.Web"
RUN dotnet build "Teapot.Web.csproj" -c Release -o /app/build

FROM build AS publish
ARG TARGETARCH
RUN dotnet publish "Teapot.Web.csproj" -c Release -o /app/publish -a $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teapot.Web.dll"]
