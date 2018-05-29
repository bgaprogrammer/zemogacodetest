## Zemoga Code Test

### Data Modeling Porcess

The first step was to create the data storage based on the kaggle dataset provided. To do that I followed the next steps:

1. I created an empty database in my local SQL Server 2017 Express instance.
2. I used the SQL Server Data Import Export utility to convert the .csv file to a table inside the previous created database.
3. Then I defined the ER model that my application will use, the final design looks like this:

![alt text](https://github.com/bgaprogrammer/zemogacodetest/blob/master/Images/DbDiag.png "ER model")

4. With the ER model created I wrote a stored procedure to read the records from the single table and tranforms them to the ER model. SPROC name is: **usp_CopyDataFromDumpTable**

> This loading process took about 24 hours in my local machine

5. Then I created another stored procedure that calculates a possible transaction date based on the step value. Basically it starts from 01/04/2018 and adds hours depending on the step value, it also adds a gap of milliseconds betwwen records to the create a more accurate look up process. SPROC name is: **usp_AdjustTransactionDateInConvertedRecords**

> This calculation took about 6 hours in my local machine

### Development Process

To accomplish the test I used the following technologies/frameworks:

#### Backend

* A net standard library that holds the business definitions and concrete implementations
* Entity Framework core
* Automapper
* ASP.NET Core Identity
* ASP.NET Core MVC - WebApi

#### Front End

* ASP.NET Core MVC
* Bootstrap
* Jquery
* Telerik Kendo UI

> The MVC controller and WebApi controllers live in the same project

The Net Standard library class hierachy looks like this:

![alt text](https://github.com/bgaprogrammer/zemogacodetest/blob/master/Images/CoreLibClassDiag.png "Core Library classes")

The web application is basically a SPA (Single Page Application) so there is just a single .cshtml view. The concrete views, authorization a routing are handled by the Kendo UI SPA Framework.

### How to run

1. In the *Database* folder of the repo you can find a SQL Server 2017 .bak file, so it's just needed to restore it in a SQL Server instance and you are ready to go with the data storage.

> Note: The .bak file was uploaded using the Git LFS extension, it's possible you need to install it before download it. Also its protected by a password, it's: **j0rg3r4m1r3z**

2. Modify the connection string values in the **appsettings.json** file for the web project to target your SQL Server instance.

3. You can run the web project locally in debug mode or simple right click it and use the wizard to publish it to a remote service like Azure App Service.

> The database backup already includes 3 users, one per role

| User        | Password           | Role  |
| ------------- |-------------| -------------|
| assistant@jrzemogacodetest.com | 1234 | Assitant |
| manager@jrzemogacodetest.com | 1234 | Manager |
| administrator@jrzemogacodetest.com | 1234 | Administrator |

Assistant: Can create a new transaction but can't mark the new transaction as fraud.

Manager: Can mark any transaction as fraud or remove the fraud flag. Can't create new transactions.

Administrator: Can create new transactions and change fraud flag in existing transactions.

### Reach me out

You can find me in Twitter as *@bgaprogrammer* and in skype as *@jramirezdev*
