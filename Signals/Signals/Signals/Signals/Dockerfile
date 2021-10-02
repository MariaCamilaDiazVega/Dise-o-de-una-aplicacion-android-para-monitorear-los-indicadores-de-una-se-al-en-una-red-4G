FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Signals/Signals.csproj", "Signals/"]
RUN dotnet restore "Signals/Signals.csproj"
COPY . .
WORKDIR "/src/Signals"
RUN dotnet build "Signals.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Signals.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Signals.dll"]