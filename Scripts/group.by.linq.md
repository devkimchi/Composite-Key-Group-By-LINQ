# Playing `Group By` and `Having` Clauses with LINQ #
playing-group-by-and-having-clauses-with-linq

![](http://blob.devkimchi.com/devkimchiwp/2014/09/93148.png)

[LINQ (Language INtegrated Query)](http://msdn.microsoft.com/en-us/library/bb397926.aspx) is one of the most powerful tools in .NET world. With this, code can be much simpler than before, especially for iterating objects. Using LINQ gives developers very similar experience when they send a `SELECT` query to a database server. This is a very basic example of `SELECT` query on SQL Server and LINQ statement.

<div>
<pre lang="sql">
SELECT *
  FROM dbo.Cars
 WHERE Manufacturer = 'Hyundai'
</pre>
</div>

<div>
<pre lang="csharp">
// LINQ
var cars1 = (from c in context.Cars
             where c.Manufacturer == "Hyundai"
             select c);

// Fluent API
var cars2 = context.Cars
                   .Where(c => c.Manufacturer == "Hyundai");
</pre>
</div>

Then, how can we use aggregation using `GROUP BY` and `HAVING` clauses? We can easily achieve this goal in SQL query. However, it's not that easy in LINQ at the first look. Basically the philosophy in both SQL query and LINQ are the same as each other, so once we get used to it, we can make use of this as easy as SQL query. In this article, I'd like to introduce several approaches of using `GROUP BY` and `HAVING` clauses in LINQ. You can find this sample codes on [GitHub](https://github.com):

[https://github.com/devkimchi/Composite-Key-Group-By-LINQ](https://github.com/devkimchi/Composite-Key-Group-By-LINQ)


## Preparation ##

Before start, we need data ingredients for fermentation. In the sample repository stated above, there's a local DB that contains sample data. Logon to your local DB by accessing to `(localdb)\v11.0` with your Windows account and attach the database or simply run the following script below.

<div>
<pre lang="sql">
CREATE TABLE [dbo].[Cars](
	[CarId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Manufacturer] [nvarchar](50) NOT NULL,
	[Year] [int] NULL,
	[Price] [int] NULL,
 CONSTRAINT [PK_Car] PRIMARY KEY CLUSTERED (
	[CarId] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

SET IDENTITY_INSERT [dbo].[Cars] ON 

INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (1, N'Lantra', N'Hyundai', 2011, 100)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (2, N'Lantra', N'Hyundai', 2012, 100)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (3, N'Lantra', N'Hyundai', 2013, 101)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (4, N'Lantra', N'Hyundai', 2014, 110)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (5, N'Golf', N'VW', 2011, NULL)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (6, N'Golf', N'VW', 2012, 110)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (7, N'Golf', N'VW', 2013, 115)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (8, N'Golf', N'VW', 2014, 120)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (9, N'118', N'BMW', 2013, 110)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (10, N'118', N'BMW', 2013, 115)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (11, N'118', N'BMW', 2014, 115)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (12, N'118', N'BMW', 2014, 116)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (13, N'Yaris', N'Toyota', 2012, 105)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (14, N'Yaris', N'Toyota', 2013, 106)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (15, N'Yaris', N'Toyota', 2013, 107)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (16, N'Yaris', N'Toyota', 2014, NULL)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (17, N'Yaris', N'Toyota', 2014, 108)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (18, N'i30', N'Hyundai', 2012, 107)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (19, N'i30', N'Hyundai', 2013, 107)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (20, N'i30', N'Hyundai', 2014, 108)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (21, N'i30', N'Hyundai', 2014, 109)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (22, N'Lantra', N'Hyundai', NULL, 120)
INSERT [dbo].[Cars] ([CarId], [Name], [Manufacturer], [Year], [Price]) VALUES (23, N'i30', N'Hyundai', NULL, 125)

SET IDENTITY_INSERT [dbo].[Cars] OFF
</pre>
</div>


## Basic `GROUP BY` Clause ##

With data above, let's populate a basic `GROUP BY` clause using an SQL query.

<div>
<pre lang="sql">
SELECT Manufacturer, MAX(Price) AS MaxPrice, MIN(Price) AS MinPrice
  FROM [dbo].[Cars]
 GROUP BY Manufacturer
</pre>
</div>

Its result will be:

<div>
<pre>
|Manufacturer|MaxPrice|MinPrice|
|---|---|---|
|BMW|116|110|
|Hyundai|125|100|
|Toyota|108|105|
|VW|120|110|
</pre>
</div>

With LINQ, try to achieve the same result. There are two different types of using LINQ:

<div>
<pre lang="csharp">
// LINQ
var results1 = (from c in this._context.Cars
                group c by c.Manufacturer into g
                select new CarViewModel()
                       {
                           Manufacturer = g.Key,
                           MaxPrice = g.Max(q => q.Price),
                           MinPrice = g.Min(q => q.Price)
                       });

// Fluent API
var results2 = this._repository
                   .Get()
                   .GroupBy(c => c.Manufacturer, r => new { Manufacturer = r.Manufacturer, Price = r.Price })
                   .Select(g => new CarViewModel()
                                {
                                    Manufacturer = g.Key,
                                    MaxPrice = g.Max(q => q.Price),
                                    MinPrice = g.Min(q => q.Price)
                                });
</pre>
</div>

When running both LINQ statements, the auto-generated SQL scripts will look like:

<div>
<pre lang="sql">
SELECT 
    1 AS [C1], 
    [GroupBy1].[K1] AS [Manufacturer], 
    [GroupBy1].[A1] AS [C2], 
    [GroupBy1].[A2] AS [C3]
    FROM ( SELECT 
        [Extent1].[Manufacturer] AS [K1], 
        MAX([Extent1].[Price]) AS [A1], 
        MIN([Extent1].[Price]) AS [A2]
        FROM [dbo].[Cars] AS [Extent1]
        GROUP BY [Extent1].[Manufacturer]
    )  AS [GroupBy1]
</pre>
</div>

And its result will be:

<div>
<pre>
|C1|Manufacturer|C2|C3|
|---|---|---|---|
|1|BMW|116|110|
|1|Hyundai|125|100|
|1|Toyota|108|105|
|1|VW|120|110|
</pre>
</div>

As you can see the result retrieved by LINQ is literally the same as the one by SQL query. Now let's move on to the next query using `GROUP BY` clause with a composite key.


## Composite `GROUP BY` Clause ##

We're getting a maximum and minimum car price by car name and manufacturer. With SQL query, this can be:

<div>
<pre lang="sql">
SELECT Manufacturer, Name, MAX(Price) AS MaxPrice, MIN(Price) AS MinPrice
  FROM [dbo].[Cars]
 GROUP BY Manufacturer, Name
</pre>
</div>

It results in:

<div>
<pre>
|Manufacturer|Name|MaxPrice|MinPrice|
|---|---|---|---|
|BMW|118|116|110|
|Hyundai|i30|125|107|
|Hyundai|Lantra|120|100|
|Toyota|Yaris|108|105|
|VW|Golf|120|110|
</pre>
</div>

How can we achieve this in c# with LINQ? Let's see the following code example:

<div>
<pre lang="csharp">
// LINQ
var results1 = (from c in this._context.Cars
                group c by new { Manufacturer = c.Manufacturer, Name = c.Name } into g
                select new CarViewModel()
                       {
                           Manufacturer = g.Key.Manufacturer,
                           Name = g.Key.Name,
                           MaxPrice = g.Max(q => q.Price),
                           MinPrice = g.Min(q => q.Price)
                       });

// Fluent API
var results2 = this._repository
                   .Get()
                   .GroupBy(c => new { Manufacturer = c.Manufacturer, Name = c.Name },
                            (g, r) => new CarViewModel()
                                      {
                                          Manufacturer = g.Manufacturer,
                                          Name = g.Name,
                                          MaxPrice = r.Max(q => q.Price),
                                          MinPrice = r.Min(q => q.Price)
                                      });
</pre>
</div>

As you can see the above code, grouping by two keys &ndash; `Manufacturer` and `Name` in this example &ndash; can be done by creating an anonymous object containing those two keys. I personally prefer to the latter one using Fluent API approach as it does look clear in terms of grouping. Both codes auto-generate the same SQL query of:

<div>
<pre lang="sql">
SELECT 
    1 AS [C1], 
    [GroupBy1].[K2] AS [Manufacturer], 
    [GroupBy1].[K1] AS [Name], 
    [GroupBy1].[A1] AS [C2], 
    [GroupBy1].[A2] AS [C3]
    FROM ( SELECT 
        [Extent1].[Name] AS [K1], 
        [Extent1].[Manufacturer] AS [K2], 
        MAX([Extent1].[Price]) AS [A1], 
        MIN([Extent1].[Price]) AS [A2]
        FROM [dbo].[Cars] AS [Extent1]
        GROUP BY [Extent1].[Name], [Extent1].[Manufacturer]
    )  AS [GroupBy1]
</pre>
</div>

And it results in:

<div>
<pre>
|C1|Manufacturer|Name|C2|C3|
|---|---|---|---|---|
|1|BMW|118|116|110|
|1|Hyundai|i30|125|107|
|1|Hyundai|Lantra|120|100|
|1|Toyota|Yaris|108|105|
|1|VW|Golf|120|110|
</pre>
</div>

We can confirm that both SQL query and LINQ statements returns the same result set. Now let's move on to the next query with `WHERE` clause to filter out `NULL` values.


## Combination of `WHERE` and `GROUP BY` Clause ##

The original data has two records having `NULL` values on their `Year` field. We need to filter them out to get more accurate maximum/minimum price range. For this, the simplq SQL query looks like:

<div>
<pre lang="sql">
SELECT Manufacturer, Name, MAX(Price) AS MaxPrice, MIN(Price) AS MinPrice
  FROM [dbo].[Cars]
 WHERE [Year] IS NOT NULL
 GROUP BY Manufacturer, Name
</pre>
</div>

It returns the record set of:

|Manufacturer|Name|MaxPrice|MinPrice|
|---|---|---|---|
|BMW|118|116|110|
|Hyundai|i30|109|107|
|Hyundai|Lantra|110|100|
|Toyota|Yaris|108|105|
|VW|Golf|120|110|

Let's see the c# code example to get the same results.

<div>
<pre lang="csharp">
// LINQ
var results1 = (from c in this._context.Cars
                where c.Year != null
                group c by new { Manufacturer = c.Manufacturer, Name = c.Name } into g
                select new CarViewModel()
                       {
                           Manufacturer = g.Key.Manufacturer,
                           Name = g.Key.Name,
                           MaxPrice = g.Max(q => q.Price),
                           MinPrice = g.Min(q => q.Price)
                       });

// Fluent API
var results2 = this._repository
                   .Get()
                   .Where(c => c.Year != null)
                   .GroupBy(c => new { Manufacturer = c.Manufacturer, Name = c.Name },
                            (g, r) => new CarViewModel()
                                      {
                                          Manufacturer = g.Manufacturer,
                                          Name = g.Name,
                                          MaxPrice = r.Max(q => q.Price),
                                          MinPrice = r.Min(q => q.Price)
                                      });
</pre>
</div>

As you can see above, either `where` keyword or `Where()` method has been added. Both also generate the same SQL queries like:

<div>
<pre lang="sql">
SELECT 
    1 AS [C1], 
    [GroupBy1].[K2] AS [Manufacturer], 
    [GroupBy1].[K1] AS [Name], 
    [GroupBy1].[A1] AS [C2], 
    [GroupBy1].[A2] AS [C3]
    FROM ( SELECT 
        [Extent1].[Name] AS [K1], 
        [Extent1].[Manufacturer] AS [K2], 
        MAX([Extent1].[Price]) AS [A1], 
        MIN([Extent1].[Price]) AS [A2]
        FROM [dbo].[Cars] AS [Extent1]
        WHERE [Extent1].[Year] IS NOT NULL
        GROUP BY [Extent1].[Name], [Extent1].[Manufacturer]
    )  AS [GroupBy1]
</pre>
</div>

It returns the result of:

|C1|Manufacturer|Name|C2|C3|
|---|---|---|---|---|
|1|BMW|118|116|110|
|1|Hyundai|i30|109|107|
|1|Hyundai|Lantra|110|100|
|1|Toyota|Yaris|108|105|
|1|VW|Golf|120|110|

