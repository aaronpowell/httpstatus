#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
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
EXPOSE 443
EXPOSE 80
ENTRYPOINT ["dotnet", "Teapot.Web.dll"]
