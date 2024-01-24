
--1. Create a stored procedure in the Northwind database that will calculate the average 
--value of Freight for a specified customer.Then, a business rule will be added that will 
--be triggered before every Update and Insert command in the Orders controller,and 
--will use the stored procedure to verify that the Freight does not exceed the average 
--freight. If it does, a message will be displayed and the command will be cancelled.
	--SELECT AVG(Freight)as sumF,CustomerID as customer FROM orders 
	--	GROUP BY CustomerID ORDER BY CustomerID DESC;

CREATE PROCEDURE AvgFrieght 
@cus_id nchar(5) = 0, @emp_id int ,@ord_date datetime , @req_date datetime, @ship_date datetime, @ship_via int , @frieght money , @shi_name nvarchar(40) , @shi_adr nvarchar(60) ,
@shi_city nvarchar(15) , @shi_region nvarchar(15), @postal_code nvarchar(10),@shi_country nvarchar(15)
AS
	declare  @ch money
	set @ch = (SELECT  AVG(Freight)FROM orders   where customerID = @cus_id GROUP BY CustomerID )
		
	if @ch > @frieght
	begin 
		insert into orders([CustomerID], [EmployeeID], [OrderDate], [RequiredDate], [ShippedDate], [ShipVia], [Freight], [ShipName], [ShipAddress], [ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry])
		values(@cus_id,@emp_id,@ord_date,@req_date,@ship_date,@ship_via,@frieght,@shi_name,@shi_adr ,@shi_city ,@shi_region , @postal_code ,@shi_country);

	end
	else
	begin 
		print 'can not insert'
	end
GO
exec AvgFrieght 'HANAR',6,'1996-07-05 00:00:00.000','1996-08-16 00:00:00.000','1996-07-10 00:00:00.000',3,12.34,'abcdefgh','abnhdbcuyc','xyz',null,'12345','Inida'


drop procedure AvgFrieght

select * from orders order by customerID;
GO
 
insert into orders([CustomerID], [EmployeeID], [OrderDate], [RequiredDate], [ShippedDate], [ShipVia], [Freight], [ShipName], [ShipAddress], [ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry])
values();



--2. write a SQL query to Create Stored procedure in the Northwind database to retrieve 
--Employee Sales by Country

CREATE PROCEDURE "Employee Sales by Country" 
@Beginning_Date DateTime, @Ending_Date DateTime AS
SELECT Employees.Country, Employees.LastName, Employees.FirstName, Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal AS SaleAmount
FROM Employees  INNER JOIN 
	(Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID) 
	ON Employees.EmployeeID = Orders.EmployeeID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
GO

exec  "Employee Sales by Country" "1996-07-16 00:00:00.000","1997-08-08 00:00:00.000"

--3. write a SQL query to Create Stored procedure in the Northwind database to retrieve 
--Sales by Year
CREATE PROCEDURE "Sales by Year" 
	@Beginning_Date DateTime, @Ending_Date DateTime AS
SELECT Orders.ShippedDate, Orders.OrderID, "Order Subtotals".Subtotal, DATENAME(yy,ShippedDate) AS Year
FROM Orders INNER JOIN "Order Subtotals" ON Orders.OrderID = "Order Subtotals".OrderID
WHERE Orders.ShippedDate Between @Beginning_Date And @Ending_Date
GO

exec  "Sales by Year" "1996-07-16 00:00:00.000","1997-08-08 00:00:00.000"


--4. write a SQL query to Create Stored procedure in the Northwind database to retrieve 
--Sales By Category
SELECT * FROM Categories
GO

CREATE PROCEDURE SalesByCategory
    @CategoryName nvarchar(15), @OrdYear nvarchar(4) = '1998'
AS
	IF @OrdYear != '1996' AND @OrdYear != '1997' AND @OrdYear != '1998' 
	BEGIN
		SELECT @OrdYear = '1998'
	END

	SELECT ProductName,
		TotalPurchase=ROUND(SUM(CONVERT(decimal(14,2), OD.Quantity * (1-OD.Discount) * OD.UnitPrice)), 0)
	FROM [Order Details] OD, Orders O, Products P, Categories C
	WHERE OD.OrderID = O.OrderID 
		AND OD.ProductID = P.ProductID 
		AND P.CategoryID = C.CategoryID
		AND C.CategoryName = @CategoryName
		AND SUBSTRING(CONVERT(nvarchar(22), O.OrderDate, 111), 1, 4) = @OrdYear
	GROUP BY ProductName
	ORDER BY ProductName
GO

EXEC SalesByCategory "Beverages",1996;

--5. write a SQL query to Create Stored procedure in the Northwind database to retrieve Ten Most Expensive Products


CREATE PROCEDURE TOP10
AS 
SELECT TOP 10 Products.ProductName AS TenMostExpensiveProducts, Products.UnitPrice
FROM Products
ORDER BY Products.UnitPrice DESC
GO

EXEC TOP10
GO

--6. write a SQL query to Create Stored procedure in the Northwind database to insert Customer Order Details 

CREATE PROCEDURE InsertCustomerOrder
 @ord_id int ,@prd_id int,@amt money,@quality smallint ,@disc real
AS
	insert into "Order Details"([OrderID], [ProductID], [UnitPrice], [Quantity], [Discount]) values
	(@ord_id,@prd_id,@amt,@quality,@disc);
GO

EXEC InsertCustomerOrder 10248,43,13.4,23,0;
GO

SELECT * FROM "Order Details";
GO


--7. write a SQL query to Create Stored procedure in the Northwind database to update Customer Order Details

CREATE PROCEDURE UpdateCustomerOrder
 @purch_amt money ,@ord_id int ,@prd_id int
AS
	UPDATE [Order Details] SET [Order Details].UnitPrice = @purch_amt where [Order Details].OrderID = @ord_id and [Order Details].ProductID = @prd_id;	
GO

EXEC UpdateCustomerOrder 15,10248,11;
GO

SELECT * FROM orders;
GO

