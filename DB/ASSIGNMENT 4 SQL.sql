
--1. Create a stored procedure in the Northwind database that will calculate the average 
--value of Freight for a specified customer.Then, a business rule will be added that will 
--be triggered before every Update and Insert command in the Orders controller,and 
--will use the stored procedure to verify that the Freight does not exceed the average 
--freight. If it does, a message will be displayed and the command will be cancelled.
	--SELECT AVG(Freight)as sumF,CustomerID as customer FROM orders GROUP BY CustomerID ORDER BY CustomerID DESC;
CREATE TRIGGER updateTrigger  ON orders
AFTER	
	insert,UPDATE
as 
begin
	declare  @avg money,@cus_id nchar(5),@id int,@frieght money
	select * into #tempTable from inserted
	
	while(Exists (select OrderID FROM #tempTable))
	begin 
		select top 1 @id = OrderID, @cus_id = CustomerID,@frieght = Freight from #tempTable
		set @avg = (SELECT  AVG(Freight)FROM orders   where customerID = @cus_id GROUP BY CustomerID)
		if @frieght > @avg
		begin
			print 'You can not make changes.....frienght value is greater than average'
			
			rollback;
		end
		delete from #tempTable where OrderID = @id
	end
end	
GO

insert into orders([CustomerID], [EmployeeID], [OrderDate], [RequiredDate], [ShippedDate], [ShipVia], [Freight], [ShipName], [ShipAddress], [ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry])
		values('ALFKI',6,'1996-07-05 00:00:00.000','1996-08-16 00:00:00.000','1996-07-10 00:00:00.000',3,5.00,'abcdefgh','abnhdbcuyc','xyz',null,'12345','Inida');
GO

update orders set Freight = 53 where OrderID = 11083;
GO

select * from orders order by customerID;
GO





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

