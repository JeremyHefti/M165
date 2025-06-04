# Minimal Api with MongoDB

## Beschreibung

Das mit ASP.NET 8 realisierte [Minimal WebApi](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/overview?view=aspnetcore-8.0) unterstützt CRUD-Operationen für eine Film-Collection. Für die Persistenz der Daten wird eine MongoDb angebunden.

Die möglichst einfach gehaltenen Anwendung soll das Wissen aus den Modulen [165 NoSQL-Datenbanken einsetzen](https://www.modulbaukasten.ch/module/165/1/de-DE) und [347 Dienst mit Container anwenden](https://www.modulbaukasten.ch/module/347/1/de-DE) aus dem [Modulbaukasten](https://www.modulbaukasten.ch/) von [ICT-Berufsbildung](https://www.ict-berufsbildung.ch/) in einem gemeinsamen Praxisprojekt vertiefen.

> **Hinweis**: Es handelt sich hier um ein Schulungsprojekt, das nicht für den produktiven Einsatz geeignet ist. Wichtige Themen wie Sicherheit (Authentication, Authorization, HTTPS, etc.), Asynchronität, Exception-Handling, Logging, etc. wurden bewusst weggelassen.

Folgende CRUD-Operationen werden unterstützt:

**Insert**

Fügt einen Film hinzu.
Die Angabe einer *id* ist zwingend und sie darf nicht bereits vorhanden sein.

```
POST /api/movies HTTP/1.1
Host: localhost:5001
Content-Type: application/json
Content-Length: 224

{
    "id": "1",
    "title": "The Imitation Game",
    "year": "2014",
    "summary": "Das wahre Rätsel war der Mann, der den Code knackte",
    "actors": ["Benedict Cumberbatch", "Keira Knightley"]
}
```
**Get**

Gibt alle vorhandenen Filme zurück.

```
GET /api/movies HTTP/1.1
Host: localhost:5001
```

**GetById**

Gibt den Film mit der im Pfad übergebenen *id* zurück.
Exisitert kein Film mit der übergebenen *id*, wird Status *404 Not Found* zurückgegeben.

```
GET /api/movies/1 HTTP/1.1
Host: localhost:5001
```

**Delete**

Löscht den Film mit der im Pfad übergebenen *id*
Exisitert kein Film mit der übergebenen *id*, wird Status *404 Not Found* zurückgegeben.

```
DELETE /api/movies/2 HTTP/1.1
Host: localhost:5001
```

**Update**

Aktualisiert den Film mit der im PFad übergebenen *id* mit den übergebenen Daten.

```
PUT /api/movies/1 HTTP/1.1
Host: localhost:5001
Content-Type: application/json
Content-Length: 188

{
    "title": "The Imitation Game 2",
    "year": "2022",
    "summary": "Das wahre Rätsel war der Mann, der den Code knackte",
    "actors": ["Benedict Cumberbatch", "Keira Knightley"]
}
```

## Getting started

Laden Sie das Repo auf Ihren lokalen Rechner

```
git clone https://gitlab.com/gbssg/min-api-with-mongo.git
cd min-api-with-mongo
```

### Konsole

Damit das Projekt auf Ihrem lokalen Rechner ausführbar ist, müssen folgende Voraussetzungen erfüllt sein:

**.NET 8**

Damit das Projekt kompiliert und ausgeführt werden kann, muss der [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) auf Ihrem Rechner installiert sein.
Der SDK ist für Windows, Linux und macOS verfügbar.

**MongoDB**

Stellen Sie sicher, dass Sie Zugriff auf eine MongoDB haben. Sie können dazu einen MongoDB-Container mit folgendem Befehl starten. 

```
docker run --name mongodb -d -p 27017:27017 -v data:/data/db -e MONGO_INITDB_ROOT_USERNAME=gbs -e MONGO_INITDB_ROOT_PASSWORD=geheim mongo
```

Passen Sie bei Bedarf den ConnectionString in [appsettings.json](./WebApi/appsettings.json) an.
Im [MongoDB Connection Guide](https://www.mongodb.com/docs/drivers/go/current/fundamentals/connection/) ist beschrieben, wie ein MongoDB ConnectionString aufgebaut ist.

Starten Sie das WebApi in einer Konsole wie folgt:

```
dotnet run --project ./WebApi/WebApi.csproj
```

### Docker Compose

Damit das Projekt auf Ihrem lokalen Rechner ausführbar ist, muss die [Docker Engine](https://docs.docker.com/engine/install/) auf Ihrem Rechner gestartet sein.

Starten Sie die Anwendung (Web API und MongoDB-Container) in einer Konsole wie folgt:

```
docker compose up 
```
## Beispiel-Requests

Für den Test des WebApis eignet sich die API-Plattform [Postman](https://www.postman.com/)
Importieren Sie in Ihren Workspace die Collection MinimalApiWithMongoDb von File [PostmanCollection.json](./PostmanCollection.json).

Folgende Anwendungsfälle lassen sich mit einer leeren Datenbank durchspielen:
- InsertMovie-Imitation Game
- InsertMovie-Top Gun
- GetAllMovies
- GetMovieById-Top Gun
- DeleteMovieById-Top Gun
- Update Movie-Imitation Game

> **Wichtig**: Wenn Sie in Postman eigene Request erfassen, achten Sie darauf, dass nebst der URL auch die Http-Methode (GET, POST, DEL, PUT) korrekt gewählt ist. Bei POST-Requests ist der Body mit Option *raw* und Format *JSON* zu erfassen.

## Arbeitsaufträge

Das Übungsprojekt kann mit 4 Arbeitsaufträgen von Grund auf erstellt werden.
Die Arbeitsaufträge sind wie folgt gegliedert.

- Arbeitsauftrag 1: Grundgerüst eines Minimal API (mit Dockerfile und docker-compose.yml)
- Arbeitsauftrag 2: Einbindung MongoDB
- Arbeitsauftrag 3: Endpunkte mit CRUD-Operationen
- Arbeitsauftrag 4: MovieService mit CRUD-Operationen

Die Arbeitsaufträge sind in Verzeichnis [_Arbeisauftraege](./_Arbeitsauftraege/) als MD-Files abgelegt. 

Mit folgender Anweisung werden alle vier PDF-Aufträge aus den MD-Files erstellt:
```
./_Arbeitsauftraege/buildPdf.sh
```

## Quellen

- [Building a Web API with ASP.NET Core and MongoDB](https://www.youtube.com/watch?v=VSsAsA6_-GE)
- [Tutorial: Create a minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-8.0&tabs=visual-studio)
