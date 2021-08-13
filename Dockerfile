#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Help/Help.csproj", "Help/"]
RUN dotnet restore "Help/Help.csproj"
COPY . .
COPY ["Data/Data.csproj", "Data/"]
RUN dotnet restore "Data/Data.csproj"
COPY . .
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "Domain/Domain.csproj"
COPY . .
COPY ["Web/Web.csproj", "Web/"]
RUN dotnet restore "Web/Web.csproj"
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Web.dll"]