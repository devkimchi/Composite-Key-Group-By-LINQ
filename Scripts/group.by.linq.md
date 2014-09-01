# Playing `Group By` and `Having` Clauses with LINQ #
playing-group-by-and-having-clauses-with-linq

![](http://blob.devkimchi.com/devkimchiwp/2014/09/93148.png)

[LINQ (Language INtegrated Query)](http://msdn.microsoft.com/en-us/library/bb397926.aspx) is one of the most powerful tools in .NET world. With this, code can be much simpler than before, especially for iterating objects. Using LINQ gives developers very similar experience when they send a `SELECT` query to a database server. This is a very basic example of `SELECT` query on SQL Server.

<div>
<pre lang="sql">
SELECT *
  FROM dbo.Cars
 WHERE Manufacturer = 'Hyundai'
</pre>
</div>

<div>
<pre lang="csharp">
var cars = (from c in context.Cars
            where c.Manufacturer == "Hyundai"
            select c);
</pre>
</div>

Then, how can we use aggregation using `GROUP BY` and `HAVING` clauses? We can easily achieve this goal in SQL query. However, it's not that easy in LINQ at the first look. Basically the philosophy in both SQL query and LINQ are the same as each other, so once we get used to it, we can make use of this as easy as SQL query. In this article, I'd like to introduce several approaches of using `GROUP BY` and `HAVING` clauses in LINQ. You can find this sample codes on [GitHub](https://github.com):

[https://github.com/devkimchi/Composite-Key-Group-By-LINQ](https://github.com/devkimchi/Composite-Key-Group-By-LINQ)


## Preparation ##

Before start, we need data ingredients for fermentation. In the sample repository stated above, there's a local DB that contains sample data. Logon to your local db by accessing to `(localdb)\v11.0` with your Windows account and attach the database or simply run the following script below.

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
                   .GroupBy(p => p.Manufacturer, q => new {Manufacturer = q.Manufacturer, Price = q.Price})
                   .Select(p => new CarViewModel()
                                {
                                    Manufacturer = p.Key,
                                    MaxPrice = p.Max(q => q.Price),
                                    MinPrice = p.Min(q => q.Price)
                                });
</pre>
</div>

When running both LINQ statements, the auto-generated SQL scripts are the same as each other.

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

Its result will be:

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

As you can see the result retrieved by LINQ is literally the same as the one by SQL query.
