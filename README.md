# WPF ZooManager

## Überblick
WPF ZooManager ist eine Windows Presentation Foundation (WPF) Anwendung zur Verwaltung eines Zoos. Sie ermöglicht die Verwaltung von Zoos und Tieren und ist mit einer Microsoft SQL Server 2022 Express Datenbank verknüpft.

## Funktionen
- Anzeigen aller Zoos in der Datenbank
- Anzeigen aller Tiere in der Datenbank
- Anzeigen der einem Zoo zugeordneten Tiere
- Hinzufügen, Löschen und Bearbeiten von Zoos
- Hinzufügen, Löschen und Bearbeiten von Tieren
- Zuordnen und Entfernen von Tieren zu einem Zoo

## Voraussetzungen
- Windows-Betriebssystem
- Microsoft SQL Server 2022 Express
- .NET Framework (empfohlen: .NET 6 oder höher)
- Visual Studio mit WPF-Unterstützung

## Einrichtung
### 1. Datenbank einrichten
1. Installiere Microsoft SQL Server 2022 Express und SQL Server Management Studio (SSMS).
2. Erstelle eine Datenbank mit folgendem Schema:
   ```sql
   CREATE TABLE [dbo].[Zoo] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Location] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
    );

   CREATE TABLE [dbo].[Animal] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
    );

   CREATE TABLE [dbo].[ZooAnimal] (
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [Zooid]    INT NOT NULL,
    [Animalid] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ZooAnimal_Animal] FOREIGN KEY ([Animalid]) REFERENCES [dbo].[Animal] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ZooAnimal_Zoo] FOREIGN KEY ([Zooid]) REFERENCES [dbo].[Zoo] ([Id]) ON DELETE CASCADE
    );
   ```

### 2. Anwendung starten
1. Klone oder lade das Projekt herunter.
2. Öffne das Projekt in Visual Studio.
3. Stelle sicher, dass die Verbindungszeichenfolge in `App.config` korrekt auf die SQL Server-Datenbank verweist:
   ```xml
   <connectionStrings>
       <add name="WPF_ZooManager.Properties.Settings.CSharpTutorialDBConnectionString"
            connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=YOUR_DATABASE_NAME;Integrated Security=True"
            providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```
4. Baue und starte das Projekt.

## Bedienung
- Beim Start der Anwendung werden alle Zoos und Tiere aus der Datenbank geladen.
- Durch Auswahl eines Zoos werden die mit diesem Zoo verknüpften Tiere angezeigt.
- Neue Zoos oder Tiere können über das Textfeld und die entsprechenden Buttons hinzugefügt werden.
- Zoos oder Tiere können bearbeitet oder gelöscht werden.
- Tiere können einem Zoo zugeordnet oder entfernt werden.

## Fehlerbehandlung
- Falls SQL Server nicht erreichbar ist, erscheint eine Fehlermeldung.
- Falls ungültige Eingaben gemacht werden (z. B. leere Namen), erfolgt keine Datenbankänderung.

## Videopräsentation des Projektes 

[![WPFZooManager](https://youtu.be/7aecUFGySpM)](https://www.youtube.com/watch?v=7aecUFGySpM))



