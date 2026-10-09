# --- Etapa 1: compilación (imagen grande con todo lo del SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Se copia primero solo el .csproj para que Docker reutilice la caché
# de "restore" mientras no cambien las dependencias
COPY AppGestionReconocimientos.Api/AppGestionReconocimientos.Api.csproj AppGestionReconocimientos.Api/
RUN dotnet restore AppGestionReconocimientos.Api/AppGestionReconocimientos.Api.csproj

COPY . .
RUN dotnet publish AppGestionReconocimientos.Api/AppGestionReconocimientos.Api.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false


# --- Etapa 2: ejecutar (imagen pequeña solo con el runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "AppGestionReconocimientos.Api.dll"]