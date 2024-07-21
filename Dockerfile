# Step 1: Build Angular application
FROM node:16 AS angular-builder
WORKDIR /app

# Copy package.json and install dependencies
COPY /client/package.json /client/package-lock.json ./
RUN npm install

# Copy the rest of the application code and build it
COPY . ./

# 切換到 /client 資料夾
WORKDIR /app/client
RUN npm run build --prod

# Step 2: Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY --from=angular-builder /app ./

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Step 3: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "API.dll"]