#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /src
COPY ["Teapot.Web/Teapot.Web.csproj", "Teapot.Web/"]
RUN dotnet restore "Teapot.Web/Teapot.Web.csproj"
COPY . .
WORKDIR "/src/Teapot.Web"
RUN dotnet build "Teapot.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Teapot.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Teapot.Web.dll"]