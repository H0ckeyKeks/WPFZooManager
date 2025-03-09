# WPF ZooManager

## Overview
WPF ZooManager is a Windows Presentation Foundation (WPF) application for managing a zoo. It enables the management of zoos and animals and is linked to a Microsoft SQL Server 2022 Express database.

## Functions
- Show all zoos in the database
- Show all animals in the database
- Display the animals assigned to a zoo
- Adding, deleting and editing zoos
- Adding, deleting and editing animals
- Assigning and removing animals to a zoo

## Requirements
- Windows operating system
- Microsoft SQL Server 2022 Express
- .NET Framework (recommended: .NET 6 or higher)
- Visual Studio with WPF support

## Setup
### 1. set up the database
1. install Microsoft SQL Server 2022 Express and SQL Server Management Studio (SSMS).
2. create a database with the following schema:
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

### 2. start the application
1. clone or download the project.
2. open the project in Visual Studio.
3. make sure that the connection string in `App.config` points correctly to the SQL Server database:
   ```xml
   <connectionStrings>
       <add name="WPF_ZooManager.Properties.Settings.CSharpTutorialDBConnectionString"
            connectionString="Data Source=YOUR_SERVER_NAME;Initial Catalog=YOUR_DATABASE_NAME;Integrated Security=True"
            providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```
4. build and start the project.

## Operation
- When the application is started, all zoos and animals are loaded from the database.
- By selecting a zoo, the animals linked to this zoo are displayed.
- New zoos or animals can be added using the text field and the corresponding buttons.
- Zoos or animals can be edited or deleted.
- Animals can be assigned to a zoo or removed.

## Error handling
- If SQL Server is not available, an error message appears.
- If invalid entries are made (e.g. empty names), the database will not be changed.

## Video presentation of the project 
[![WPFZooManager](https://youtu.be/7aecUFGySpM)](https://www.youtube.com/watch?v=7aecUFGySpM)



