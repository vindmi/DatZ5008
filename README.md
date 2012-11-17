# This is my README
Project for "Web Programming" course at Latvian University, masters, 2nd year.

This software imports data from Google+ service and saves it to the database.

Technologies used:
1) .NET 4.0, C# 4,
2) Entity Framework 5 Code First and Database Migrations,
3) Microsoft SQL Server 2008 Express,
4) Spring.NET,
5) log4net,
6) NUnit,
7) Moq.

Preconditions for compiling:
1) .NET 4.0 or higher,
2) MSBuild 4 or higher.

Preconditions for running on local machine:
1) Microsoft SQL Server 2008 Express or Microsoft SQL Server 2008 
(connection string in GooglePlus.DataImporter project's App.config file should be changed accordingly)

How to build this software:
1) Download sources,
2) Execute "datz5008\GooglePlus.Build\BuildSolution.bat".
(solution should be built and unit tests should be executed)

Solution description:
- GooglePlus.ApiClient - contains service clients implementations.
- GooglePlus.Data - contains database / persistance related functionality.
- GooglePlus.DataImporter - console application which executes data import.
- GooglePlus.ApiClient.Test - contains tests for service clients implementations.
- GooglePlus.Data.Test - contains tests for database / persistance related functionality.
- GooglePlus.Build - contains tools and scripts for automated build.