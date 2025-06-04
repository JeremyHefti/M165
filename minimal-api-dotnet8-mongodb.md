# Minimal API mit .NET 8 und MongoDB – Komplettanleitung

---

## 1. .NET SDK prüfen und installieren

### Prüfen installierter SDKs
```bash
dotnet --list-sdks
```

### Installieren .NET 8 SDK (Ubuntu/Debian)
```bash
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

---

## 2. Neues .NET WebApi Projekt erstellen

```bash
dotnet new web --name WebApi --framework net8.0
```

---

## 3. Gitignore hinzufügen

```bash
dotnet new gitignore
```

---

## 4. Projekt in Visual Studio Code öffnen

```bash
code .
```

---

## 5. Ins Projektverzeichnis wechseln und Anwendung starten

```bash
cd WebApi
dotnet run
```

---

## 6. launchSettings.json anpassen

```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:{dein-port}",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

---

## 7. Dockerfile erstellen

### 7.1 Build-Stage (Compile Image)
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out
```

### 7.2 Runtime-Stage (Runtime Image)
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL description="Minimal API with MongoDB"
LABEL organisation="GBS St. Gallen"
LABEL author="Martin Früh"
WORKDIR /app
COPY --from=build-env /build/out .
ENV ASPNETCORE_URLS=http://*:5002
EXPOSE 5001
ENTRYPOINT ["dotnet", "WebApi.dll"]
```

---

## 8. docker-compose.yml erstellen

```yaml
services:
  webapi:
    build: WebApi
    restart: always
    depends_on:
      - mongodb
    environment:
      DatabaseSettings__ConnectionString: "mongodb://gbs:geheim@mongodb:27017"
    ports:
      - 5001:5001
  mongodb:
    image: mongo
    restart: always
    environment:
      MONGO_INITDB_ROOT_USERNAME: gbs
      MONGO_INITDB_ROOT_PASSWORD: geheim
    volumes:
      - mongoData:/data/db
volumes:
  mongoData:
```

---

## 9. Docker Compose starten

```bash
cd ..
docker-compose up
```

---

## 10. MongoDB Container manuell starten (optional)

```bash
docker run -d   -p 27017:27017   -v mongo-data:/data/db   -e MONGO_INITDB_ROOT_USERNAME=gbs   -e MONGO_INITDB_ROOT_PASSWORD=geheim   --name my-mongo   mongo
```

---

## 11. C# Extension für VS Code installieren

(Füge hier ein Screenshot ein oder verlinke die Extension)

---

## 12. MongoDB NuGet-Package installieren

```bash
dotnet add package MongoDB.Driver
```

---

## 13. Program.cs: Minimal API mit MongoDB-Verbindung

```csharp
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Root-Endpunkt anpassen
app.MapGet("/", () => "Minimal API Version 1.0");

// Neuer Endpunkt: /check
app.MapGet("/check", () =>
{
    try
    {
        // Fix definierter Connection-String (localhost, Port 27017, Benutzername/Passwort wie beim Container)
        var connectionString = "mongodb://gbs:geheim@localhost:27017";

        // Verbindung aufbauen
        var client = new MongoClient(connectionString);

        // Datenbanken abrufen
        var dbList = client.ListDatabaseNames().ToList();

        // Ergebnis als String zurückgeben
        return Results.Ok($"Zugriff auf MongoDB ok. Datenbanken: {string.Join(", ", dbList)}");
    }
    catch (Exception ex)
    {
        // Fehlerbehandlung
        return Results.Problem($"Fehler beim Zugriff auf MongoDB: {ex.Message}");
    }
});

app.Run();
```

---

## 14. DatabaseSettings.cs erstellen

```csharp
public class DatabaseSettings
{
    public string ConnectionString { get; set; } = "";
}
```

---

## 15. appsettings.json erweitern

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DatabaseSettings": {
    "ConnectionString": "mongodb://gbs:geheim@localhost:27017/?authSource=admin"
  }
}
```

---

## 16. Port freigeben bei Belegung (Linux)

```bash
lsof -i :{port}
kill -9 {PID}
```

---

## 17. Manuell auf MongoDB zugreifen

```bash
docker exec -it my-mongo mongosh -u gbs -p geheim --authenticationDatabase admin
```

---

# Fertig! Deine Minimal API mit MongoDB ist einsatzbereit.
