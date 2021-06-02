FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Teapot.Web/Teapot.Web.csproj", "Teapot.Web/"]
RUN dotnet restore "Teapot.Web/Teapot.Web.csproj"
COPY . .
WORKDIR "/src/Teapot.Web"
RUN dotnet build "Teapot.Web.csproj" -c Release -o /app/build
RUN dotnet publish "Teapot.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine-amd64
WORKDIR /app
COPY --from=build /app/publish .
# Install OpenSSH and set the password for root to "Docker!". In this example, "apk add" is the install instruction for an Alpine Linux-based image.
RUN apk add openssh \
     && echo "root:Docker!" | chpasswd 

# Copy the sshd_config file to the /etc/ssh/ directory
COPY sshd_config /etc/ssh/
# Open port 2222 for SSH access
EXPOSE 80 2222
ENTRYPOINT ["dotnet", "Teapot.Web.dll"]
