FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Teapot.Web/Teapot.Web.csproj", "Teapot.Web/"]
RUN dotnet restore "Teapot.Web/Teapot.Web.csproj"
COPY . .
WORKDIR "/src/Teapot.Web"
RUN dotnet build "Teapot.Web.csproj" -c Release -o /app/build
RUN dotnet publish "Teapot.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Teapot.Web.dll"]
