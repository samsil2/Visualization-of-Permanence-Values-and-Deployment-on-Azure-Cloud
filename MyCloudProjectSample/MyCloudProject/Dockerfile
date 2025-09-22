# Stage 1: Base runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyCloudProject/MyCloudProject.csproj", "MyCloudProject/"]
COPY ["MyCloudProject.Common/MyCloudProject.Common.csproj", "MyCloudProject.Common/"]
COPY ["MyExperiment/MyExperiment.csproj", "MyExperiment/"]
RUN dotnet restore "./MyCloudProject/MyCloudProject.csproj"
COPY . .
WORKDIR "/src/MyCloudProject"
RUN dotnet build "./MyCloudProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyCloudProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime
FROM base AS final
WORKDIR /app

# Install required native libs for System.Drawing
RUN apt-get update && apt-get install -y --no-install-recommends \
    libgdiplus \
    libc6-dev \
    libx11-6 \
 && rm -rf /var/lib/apt/lists/*

# Ensure libdl.so symlink exists
RUN [ -e /usr/lib/x86_64-linux-gnu/libdl.so ] || \
    ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /usr/lib/x86_64-linux-gnu/libdl.so

ENV LD_LIBRARY_PATH=/usr/lib:/usr/lib/x86_64-linux-gnu
ENV OUTPUT_ROOT=/tmp/outputfile
ENV DOTNET_ENVIRONMENT=Production

COPY --from=publish /app/publish .
COPY MyCloudProject/appsettings.json ./appsettings.json

ENTRYPOINT ["dotnet", "MyCloudProject.dll"]
