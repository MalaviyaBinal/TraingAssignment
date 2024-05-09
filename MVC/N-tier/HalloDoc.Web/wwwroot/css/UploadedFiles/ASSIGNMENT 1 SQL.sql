CREATE TABLE "Products" (
	"ProductID" "int" IDENTITY (1, 1) NOT NULL ,
	"ProductName" nvarchar (40) NOT NULL ,
	"SupplierID" "int" NULL ,
	"CategoryID" "int" NULL ,
	"QuantityPerUnit" nvarchar (20) NULL ,
	"UnitPrice" "money" NULL,
	"UnitsInStock" "smallint" NULL,
	"UnitsOnOrder" "smallint" NULL,
	"ReorderLevel" "smallint" NULL,
	"Discontinued" "bit" NOT NULL,
	CONSTRAINT "PK_Products" PRIMARY KEY  CLUSTERED 
	(
		"ProductID"
	),
	CONSTRAINT "CK_Products_UnitPrice" CHECK (UnitPrice >= 0),
	CONSTRAINT "CK_ReorderLevel" CHECK (ReorderLevel >= 0),
	CONSTRAINT "CK_UnitsInStock" CHECK (UnitsInStock >= 0),
	CONSTRAINT "CK_UnitsOnOrder" CHECK (UnitsOnOrder >= 0)
)
GO
set identity_insert "Products" on
GO
INSERT "Products"("ProductID","ProductName","SupplierID","CategoryID","QuantityPerUnit","UnitPrice","UnitsInStock","UnitsOnOrder","ReorderLevel","Discontinued")
VALUES
(1,'Chai',1,1,'10 boxes x 20 bags',18,39,0,10,0),
(2,'Chang',1,1,'24 - 12 oz bottles',19,17,40,25,0),
(3,'Aniseed Syrup',1,2,'12 - 550 ml bottles',10,13,70,25,0),
(4,'Chef Anton''s Cajun Seasoning',2,2,'48 - 6 oz jars',22,53,0,0,0),
(5,'Chef Anton''s Gumbo Mix',2,2,'36 boxes',21.35,0,0,0,1),
(6,'Grandma''s Boysenberry Spread',3,2,'12 - 8 oz jars',25,120,0,25,0),
(7,'Uncle Bob''s Organic Dried Pears',3,7,'12 - 1 lb pkgs.',30,15,0,10,0),
(8,'Northwoods Cranberry Sauce',3,2,'12 - 12 oz jars',40,6,0,0,0),
(9,'Mishi Kobe Niku',4,6,'18 - 500 g pkgs.',97,29,0,0,1),
(10,'Ikura',4,8,'12 - 200 ml jars',31,31,0,0,0),
(11,'Queso Cabrales',5,4,'1 kg pkg.',21,22,30,30,0),
(12,'Queso Manchego La Pastora',5,4,'10 - 500 g pkgs.',38,86,0,0,0),
(13,'Konbu',6,8,'2 kg box',6,24,0,5,0),
(14,'Tofu',6,7,'40 - 100 g pkgs.',23.25,35,0,0,0),
(15,'Genen Shouyu',6,2,'24 - 250 ml bottles',15.5,39,0,5,0),
(16,'Pavlova',7,3,'32 - 500 g boxes',17.45,29,0,10,0),
(17,'Alice Mutton',7,6,'20 - 1 kg tins',39,0,0,0,1),
(18,'Carnarvon Tigers',7,8,'16 kg pkg.',62.5,42,0,0,0),
(19,'Teatime Chocolate Biscuits',8,3,'10 boxes x 12 pieces',9.2,25,0,5,0),
(20,'Sir Rodney''s Marmalade',8,3,'30 gift boxes',81,40,0,0,0),
(21,'Sir Rodney''s Scones',8,3,'24 pkgs. x 4 pieces',10,3,40,5,0),
(22,'Gustaf''s Kn�ckebr�d',9,5,'24 - 500 g pkgs.',21,104,0,25,0),
(23,'Tunnbr�d',9,5,'12 - 250 g pkgs.',9,61,0,25,0),
(24,'Guaran� Fant�stica',10,1,'12 - 355 ml cans',4.5,20,0,0,1),
(25,'NuNuCa Nu�-Nougat-Creme',11,3,'20 - 450 g glasses',14,76,0,30,0),
(26,'Gumb�r Gummib�rchen',11,3,'100 - 250 g bags',31.23,15,0,0,0),
(27,'Schoggi Schokolade',11,3,'100 - 100 g pieces',43.9,49,0,30,0),
(28,'R�ssle Sauerkraut',12,7,'25 - 825 g cans',45.6,26,0,0,1),
(29,'Th�ringer Rostbratwurst',12,6,'50 bags x 30 sausgs.',123.79,0,0,0,1),
(30,'Nord-Ost Matjeshering',13,8,'10 - 200 g glasses',25.89,10,0,15,0),
(31,'Gorgonzola Telino',14,4,'12 - 100 g pkgs',12.5,0,70,20,0),
(32,'Mascarpone Fabioli',14,4,'24 - 200 g pkgs.',32,9,40,25,0),
(33,'Geitost',15,4,'500 g',2.5,112,0,20,0),
(34,'Sasquatch Ale',16,1,'24 - 12 oz bottles',14,111,0,15,0),
(35,'Steeleye Stout',16,1,'24 - 12 oz bottles',18,20,0,15,0),
(36,'Inlagd Sill',17,8,'24 - 250 g  jars',19,112,0,20,0),
(37,'Gravad lax',17,8,'12 - 500 g pkgs.',26,11,50,25,0),
(38,'C�te de Blaye',18,1,'12 - 75 cl bottles',263.5,17,0,15,0),
(39,'Chartreuse verte',18,1,'750 cc per bottle',18,69,0,5,0),
(40,'Boston Crab Meat',19,8,'24 - 4 oz tins',18.4,123,0,30,0);
GO
SELECT * FROM Products;
GO
set identity_insert "Products" off
GO 
ALTER TABLE "Products" CHECK CONSTRAINT ALL
GO
--1. Write a query to get a Product list (id, name, unit price) where current products cost less than $20.
SELECT ProductID, ProductName, UnitPrice
FROM Products
WHERE ((Unitprice <= 20) AND (Discontinued=0))
GO
--2. Write a query to get Product list (id, name, unit price) where products cost between $15 and $25
SELECT ProductID, ProductName, UnitPrice
FROM Products
WHERE UnitPrice BETWEEN 15 AND 25 AND Discontinued=0
GO
--3. Write a query to get Product list (name, unit price) of above average price.
SELECT DISTINCT ProductName, UnitPrice
FROM Products
WHERE (UnitPrice > (SELECT avg(UnitPrice) FROM Products) AND Discontinued=0);
GO
--4. Write a query to get Product list (name, unit price) of ten most expensive products
SELECT TOP 10 ProductName, UnitPrice
FROM Products
WHERE Discontinued=0
ORDER BY UnitPrice DESC
GO
--5. Write a query to count current and discontinued products
SELECT
    COUNT(CASE WHEN Discontinued = 0 THEN 1 END) AS CurrentProducts,
    COUNT(CASE WHEN Discontinued = 1 THEN 1 END) AS DiscontinuedProducts
FROM Products
GO
--6. Write a query to get Product list (name, units on order , units in stock) of stock is less than the quantity on order
SELECT ProductName,  UnitsOnOrder , UnitsInStock
FROM Products
WHERE (((Discontinued)=0) AND ((UnitsInStock)<UnitsOnOrder))
GO