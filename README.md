# Application Exercises

There are 3 exercises in this repo:
## 1. CSV Read & Write

Write a C#. NET application which does the following:

1.	Read in the CSV file from: https://apps.waterconnect.sa.gov.au/file.csv
2.	Remove the column “Unit_No”
3.	Add a new column with the label “calc”
4.	Set the value of the new column as the sum of the columns “swl” and “rswl”
5.	Save the new CSV file to the file system

> Solution requirements:
>-	The solution must be .NET C# (can be a web application, console app etc)

## 2. Weather ConsoleApp

Write a C# program that consumes Bureau of Meteorology weather observation data for the Adelaide Airport weather observation station, and calculates the average temperature from this data for the previous 72 hours.

The data is available at http://www.bom.gov.au/products/IDS60901/IDS60901.94672.shtml,
OR
the JSON feed for this data is available at http://www.bom.gov.au/fwo/IDS60901/IDS60901.94672.json.

> Solution requirements:
> -	The solution must be an ASP.NET C# **console** application (.NET Framework or .NET Core) that outputs the calculated average temperature.


## 3. Weather WebApp

Create a C# web service that provides weather observation data for any requested weather observation station in the Adelaide Area.

A sample table of four weather observation stations follows below.
The data formats available for each are constructed using the following patterns:
* <a>http://www.bom.gov.au/products/IDS60901/IDS60901.{WMO}.shtml</a>
* <a>http://www.bom.gov.au/fwo/IDS60901/IDS60901.{WMO}.json</a>

Weather Observation Station |	WMO
--- | ---
Adelaide Airport | 94672
Edinburgh	| 95676
Hindmarsh Island | 94677
Kuitpo | 94683


> Solution requirements:
> -	The web service API shall be able to fetch all weather observation data for any weather observation station.
> -	The web service API shall be able to fetch a specific piece of weather observation data (e.g. Temp, App Temp, Dew Point etc.) for any weather observation station.
> -	The solution must be a RESTful HTTP service, ASP.NET web application, API project (.NET Framework or .NET Core) deployable to IIS web server.
> -	Please provide a short paragraph explaining your solution

# Technical Features

1. Back-end & console apps are built with .NET 5;
2. Front-end apps are built with Angular 12;
3. xUnit for .NET Unit Testing;

# How to Run
