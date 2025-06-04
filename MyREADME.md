Prüfen:
dotnet--list-sdks

Installieren: 
Install dotnet 8 sdk
 sudo apt-get update
 sudo apt-get install-y dotnet-sdk-8.0

Erstellen Sie ein .NET Projekt WebApi mit Template web:
dotnet new web--name WebApi--framework net8.0

Gitignore:
dotnet new gitignore

Öffne in VS:
code . 

Navigiere ins WebApi:
cd WebApi

Starte deine Appliaktion:
dotnet run

das launchSettings.json sollte wie folgt aussehen:
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:{hier kannst du dein Port angeben}",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
  }
}

nun sollte deine Applikation auf deiner URL verfügbar sein und Hello Worl sollte zu sehen sein.

Erstelle ein Dockerfile: 
1. Build compile image

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

2. Build runtime image

FROM mcr.microsoft.com/dotnet/aspnet:8.0
LABEL description="Minimal API with MongoDB"
LABEL organisation="GBS St. Gallen"
LABEL author="Martin Früh"
WORKDIR /app
COPY --from=build-env /build/out .
ENV ASPNETCORE_URLS=http://*:5002
EXPOSE 5001
ENTRYPOINT ["dotnet", "WebApi.dll"]

Erstelle ein docker-compose.yml:
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

gehe wieder ins Verzeichnis indem dein docker-compose file ist:
cd ..
nun solltest du docker comose up machen und die anwendung sollte starten

oder wenn von uns verlangt wird einfach ein Container starten ohne Docker und compose file kann der Command um es zu laden wie folgt aussehen:
docker run -d \
  -p 27017:27017 \
  -v mongo-data:/data/db \
  -e MONGO_INITDB_ROOT_USERNAME=gbs \
  -e MONGO_INITDB_ROOT_PASSWORD=geheim \
  --name my-mongo \
  mongo

Installiere C# extantion:
![alt text](image.png)

Installieren Sie das Nuget-Package mit folgendem .NET CLI Command:
dotnet add package MongoDB.Driver

Erzeugen Sie in WebApi/Program.cs unter app.MapGet(...) einen weiteren Endpunkt check:
 app.MapGet("/check", () => {
 /* Code zur Prüfung der DB ...*/
 return "Zugriff auf MongDB ok.";
 });

Nun sollte wenn du den Endpunkt /check aufrufst "Zugriff auf MongDB ok." stehen.

 Erweitern Sie die eben eingefügte Methode so, dass folgende Anforderungen erfüllt werden
 • DieVerbindungzurMongoDBwirdüberMongoDB.Driver.MongoClient aufgebaut.
 • dievorhandenenDatenbankenwerdenabgefragtundinderAntwortausgegeben.
 • Exceptions sind mit try/catch abgefangen und werdenals Fehlermeldungzurückgegeben.

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

 Erstellen Sie unter min-api-with-mongo/WebApi ein neues File DatabaseSettings.cs mit folgendem
 Inhalt:
 public class DatabaseSettings
 {
 public string ConnectionString { get; set; } = "";
 }

 Erweitern Sie min-api-with-mongo/WebApi/appsettings.json um den Abschnitt DatabaseSettings und
 weisen Sie ConnectionString den bis jetzt fix codierten Wert zu:

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

Füge im File Program.cs einen weiteren Checkpoint hinzu welcher die Connectiosn zur Datenbank überprüft, Also das mit DataeseSettings:

Falls ein Port besetzt ist so kannst du ihn löschen und freigeben:
lsof -i :{port}
kill -9 11631

Manuel auf die DB zugreiffen:
docker exec -it my-mongo mongosh -u gbs -p geheim --authenticationDatabase admin




