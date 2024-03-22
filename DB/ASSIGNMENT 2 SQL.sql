CREATE TABLE Salesman(
	salesman_id INT PRIMARY KEY IDENTITY(1,1),
	name NVARCHAR(40) NOT NULL,
	city NVARCHAR(40),
	commission INT NOT NULL
);
GO

CREATE TABLE customer(
	customer_id INT PRIMARY KEY IDENTITY(1,1),
	cust_name NVARCHAR(40) NOT NULL,
	city NVARCHAR(40),
	grade INT NOT NULL,
	salesman_id INT,
	FOREIGN KEY (salesman_id) REFERENCES Salesman (salesman_id)
);
GO

CREATE TABLE orders(
	ord_no INT PRIMARY KEY IDENTITY(1,1),
	purch_amt MONEY NOT NULL,
	ord_date DATE NOT NULL,
	customer_id INT NOT NULL,
	salesman_id INT,
	FOREIGN KEY (customer_id) REFERENCES customer (customer_id),
	FOREIGN KEY (salesman_id) REFERENCES Salesman (salesman_id)
);
GO

set identity_insert "Salesman" on
GO

insert into Salesman (salesman_id, name, city, commission) values (1, 'Joe', 'Wenfu', 7);
insert into Salesman (salesman_id, name, city, commission) values (2, 'Ofella', 'Kakata', 2);
insert into Salesman (salesman_id, name, city, commission) values (3, 'Eugine', 'Chengmen', 15);
insert into Salesman (salesman_id, name, city, commission) values (4, 'Cyrillus', 'Nezhinka', 9);
insert into Salesman (salesman_id, name, city, commission) values (5, 'Jill', 'Ujazd', 11);
insert into Salesman (salesman_id, name, city, commission) values (6, 'Adey', 'Novoderevyankovskaya', 8);
insert into Salesman (salesman_id, name, city, commission) values (7, 'Lynna', 'Sanchahe', 13);
insert into Salesman (salesman_id, name, city, commission) values (8, 'Kylie', 'Ciudad Nueva', 8);
insert into Salesman (salesman_id, name, city, commission) values (9, 'Randall', 'Wailolung', 4);
insert into Salesman (salesman_id, name, city, commission) values (10, 'Genvieve', 'Tankhoy', 2);

GO

set identity_insert "Salesman" OFF
GO


SELECT * FROM Salesman;
GO


set identity_insert "customer" on
GO

insert into customer (customer_id, cust_name, city, grade, salesman_id) values (1, 'Birgit', 'Wenfu', 444, null);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (2, 'Blake', 'Chengmen', 466, 10);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (3, 'Wit', 'Kakata', 302, 9);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (4, 'Hollie', 'Tankhoy', 233, 7);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (5, 'Abby', 'Tankhoy', 331, 8);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (6, 'Leigh', 'Nehe', 378, 9);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (7, 'Lorilyn', 'Ilinden', 394, 1);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (8, 'Gregoire', 'Chengmen', 478, 1);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (9, 'Al', 'Tankhoy', 177, 10);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (10, 'Byron', 'Kohila', 83, 6);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (11, 'Christye', 'Wenfu', 340, 3);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (12, 'Juanita', 'Dera Murād Jamāli', 215, 10);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (13, 'Rosella', 'Lac-Brome', 480, 7);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (14, 'Ivie', 'Dana', 450, 4);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (15, 'Berna', 'Wang Yang', 149, 4);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (16, 'Brigit', 'Tankhoy', 98, 9);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (17, 'Lanita', 'Kakata', 351, 10);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (18, 'Julee', 'Rakičan', 68, 4);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (19, 'Marlon', 'Chengmen', 64, 4);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (20, 'Ansley', 'Wenfu', 63, 7);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (21, 'Simon', 'Chengmen', 117, 7);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (22, 'Abrahan', 'Oslo', 119, null);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (23, 'Juli', 'Itapicuru', 465, 9);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (24, 'Deena', 'Durham', 201, 1);
insert into customer (customer_id, cust_name, city, grade, salesman_id) values (25, 'Dalli', 'Tankhoy', 189, 10);

GO

set identity_insert "customer" off
GO

select * from customer;
GO

set identity_insert "orders" on
GO
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (1, 1296, '2022-04-25', 8, 2);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (2, 4613, '2024-01-19', 20, 1);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (3, 1961, '2023-12-24', 21, 8);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (4, 898, '2023-06-28', 25, 9);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (5, 1812, '2022-09-11', 6, 6);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (6, 359, '2023-02-08', 11, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (7, 4288, '2022-09-21', 19, 1);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (8, 4262, '2022-05-25', 2, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (9, 3749, '2024-01-07', 16, 1);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (10, 351, '2022-03-07', 15, 3);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (11, 1834, '2023-11-30', 25, 2);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (12, 463, '2023-07-11', 16, 6);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (13, 860, '2023-05-03', 10, null);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (14, 1435, '2023-08-11', 22, 2);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (15, 4684, '2022-05-06', 20, 9);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (16, 2896, '2023-02-15', 16, 4);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (17, 1881, '2022-11-30', 11, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (18, 4462, '2022-10-18', 16, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (19, 1146, '2022-10-19', 8, 8);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (20, 4934, '2022-08-30', 21, 3);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (21, 3667, '2022-07-22', 15, 8);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (22, 2174, '2023-12-06', 11, null);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (23, 2374, '2022-11-24', 6, 2);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (24, 4379, '2022-08-26', 11, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (25, 3379, '2022-11-09', 14, 2);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (26, 3899, '2022-07-12', 14, 1);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (27, 3318, '2023-10-22', 21, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (28, 1971, '2022-03-31', 23, null);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (29, 1408, '2023-03-13', 21, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (30, 4513, '2023-11-16', 5, 1);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (31, 4953, '2024-01-19', 24, 4);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (32, 2524, '2022-07-28', 6, 2);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (33, 1541, '2022-04-22', 3, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (34, 3479, '2022-10-10', 19, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (35, 338, '2024-01-11', 21, 3);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (36, 4146, '2023-04-08', 10, 9);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (37, 2979, '2022-03-21', 21, 10);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (38, 4571, '2023-01-23', 15, 4);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (39, 1869, '2023-06-15', 25, 2);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (40, 4741, '2023-11-16', 4, null);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (41, 3237, '2023-11-11', 11, 9);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (42, 620, '2022-07-21', 14, 5);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (43, 3563, '2023-12-31', 22, 1);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (44, 3308, '2023-06-19', 13, 5);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (45, 3519, '2022-03-27', 21, 4);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (46, 1558, '2023-01-07', 21, 5);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (47, 1011, '2023-06-29', 13, 1);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (48, 4134, '2023-05-01', 5, 4);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (49, 2392, '2023-08-14', 22, 3);
insert into orders (ord_no, purch_amt, ord_date, customer_id, salesman_id) values (50, 2163, '2023-12-28', 2, 4);

set identity_insert "orders" off
GO
SELECT * FROM orders;
GO

--1. write a SQL query to find the salesperson and customer who reside in the same city. Return Salesman, cust_name and city

SELECT S.name AS Salesman,C.cust_name AS Customer,S.city AS CITY 
FROM 
	Salesman S  JOIN customer C ON S.city = C.city;
GO

--2. write a SQL query to find those orders where the order amount exists between 500 and 2000. Return ord_no, purch_amt, cust_name, city

SELECT T.ord_no as orderNo, T.purch_amt as amount , C.cust_name , C.city
FROM 
	customer C JOIN 
		(SELECT * FROM orders WHERE purch_amt >=500 AND purch_amt <= 2000) T 
	ON C.customer_id =  T.customer_id 
ORDER BY amount ;
GO

--3. write a SQL query to find the salesperson(s) and the customer(s) he represents.Return Customer Name, city, Salesman, commission

SELECT C.cust_name as Customer_name, C.city as city ,S.name as salesman ,S.commission as commission 
FROM 
	customer C JOIN Salesman S 
ON C.salesman_id = S.salesman_id
ORDER BY salesman;
GO

--4. write a SQL query to find salespeople who received commissions of more than 12 percent from the company. Return Customer Name, customer city, Salesman, commission
SELECT * FROM 
	(SELECT C.cust_name as Customer_name, C.city as city ,S.name as salesman ,S.commission as commission 
	FROM 
		customer C JOIN Salesman S 
	ON C.salesman_id = S.salesman_id
	) T WHERE commission > 12
ORDER BY salesman;
GO

--5. write a SQL query to locate those salespeople who do not live in the same city where their customers live and have received a commission of more than 12% from the company. 
--Return Customer Name, customer city, Salesman, salesman city, commission
select * from
	(SELECT C.cust_name as Customer_name, C.city as city ,S.name as salesman, S.city as salesman_city ,S.commission as commission 
	FROM 
		customer C JOIN Salesman S 
	ON C.salesman_id = S.salesman_id) T WHERE city != salesman_city AND commission > 12
ORDER BY salesman
GO

--6. write a SQL query to find the details of an order. Return ord_no, ord_date, purch_amt, Customer Name, grade, Salesman, commission
SELECT O.ord_no AS orderNo, O.ord_date as orderDate ,O.purch_amt as amount , C.cust_name AS CustomerName , C.grade as grade  , S.name Salesman , S.commission as commission
FROM orders O JOIN customer C on O.customer_id = C.customer_id
		JOIN Salesman S ON O.salesman_id = S.salesman_id ;

--7. Write a SQL statement to join the tables salesman, customer and orders so that the same column of each table appears once and only the relational rows are returned. 
SELECT O.ord_no AS orderNo, O.ord_date as orderDate ,O.purch_amt as amount , C.cust_name AS CustomerName , C.grade as grade  , S.name Salesman , S.commission as commission
FROM orders O 
	left JOIN customer C on O.customer_id = C.customer_id
	left JOIN Salesman S ON O.salesman_id = S.salesman_id ;


--8. write a SQL query to display the customer name, customer city, grade, salesman, salesman city. The results should be sorted by ascending customer_id

SELECT C.cust_name as name, C.city as CustomerCity , C.grade as grade, S.name Salesman, S.city SalesmanCity 
FROM customer C left join Salesman S ON C.salesman_id = S.salesman_id
ORDER BY C.customer_id ASC;

--9. write a SQL query to find those customers with a grade less than 300. Return cust_name, customer city, grade, Salesman, salesmancity. The result should be ordered by ascending customer_id.
SELECT C.cust_name as name, C.city as CustomerCity , C.grade as grade, S.name Salesman, S.city SalesmanCity 
FROM customer C left join Salesman S ON C.salesman_id = S.salesman_id WHERE C.grade < 300
ORDER BY C.customer_id ASC;


--10. Write a SQL statement to make a report with customer name, city, order number, order date, and order amount in ascending order according to the order date to 
--determine whether any of the existing customers have placed an order or not
SELECT C.cust_name as name, C.city as CustomerCity ,O.ord_no AS orderNo , O.ord_date as date , O.purch_amt as  orderAmount
FROM orders O join customer C ON C.customer_id = O.customer_id where C.salesman_id IS NOT NULL
ORDER BY  O.ord_date ASC;

--11. Write a SQL statement to generate a report with customer name, city, order number, order date, order amount, salesperson name, and commission to determine if any of 
--the existing customers have not placed orders or if they have placed orders through their salesman or by themselves
SELECT C.cust_name AS CustomerName , O.ord_no AS orderNo, O.ord_date as orderDate ,O.purch_amt as amount , S.name Salesman , S.commission as commission
FROM orders O 
	left JOIN customer C on O.customer_id = C.customer_id
	left JOIN Salesman S ON O.salesman_id = S.salesman_id WHERE C.customer_id != O.customer_id;

--12. Write a SQL statement to generate a list in ascending order of salespersons who work either for one or more customers or have not yet joined any of the customers

SELECT COUNT(C.cust_name) as noOfCustomer,S.name as salesman
FROM 
	customer C JOIN Salesman S 
ON C.salesman_id = S.salesman_id
GROUP BY S.name
ORDER BY noOfCustomer asc;
GO

--13. write a SQL query to list all salespersons along with customer name, city, grade, order number, date, and amount
SELECT C.cust_name AS CustomerName ,C.city as city,C.grade as grade ,O.ord_no AS orderNo, O.ord_date as orderDate ,O.purch_amt as amount, S.name Salesman 
FROM orders O JOIN customer C on O.customer_id = C.customer_id
		JOIN Salesman S ON O.salesman_id = S.salesman_id
ORDER BY Salesman;

--14. Write a SQL statement to make a list for the salesmen who either work for one or more customers or yet to join any of the customers. The customer may have placed, 
--either one or more orders on or above order amount 2000 and must have a grade, or he may not have placed any order to the associated supplier.


SELECT C.cust_name AS NAME , C.city AS CITY ,C.grade AS GRADE , S.name AS Salesman ,O.ord_no AS OrderNo , O.ord_date AS Date , O.purch_amt
FROM 
	customer C RIGHT OUTER JOIN Salesman S ON C.salesman_id = S.salesman_id 
	LEFT OUTER JOIN orders O ON O.customer_id = C.customer_id
	WHERE O.purch_amt >2000 AND C.grade IS NOT NULL;

--Write a SQL statement to generate a list of all the salesmen who either work for one or more customers or have yet to join any of them. The customer may have placed 
--one or more orders at or above order amount 2000, and must have a grade, or he may not have placed any orders to the associated supplier.

SELECT C.cust_name AS NAME , C.city AS CITY ,C.grade AS GRADE , S.name AS Salesman ,O.ord_no AS OrderNo , O.ord_date AS Date , O.purch_amt
FROM 
	customer C RIGHT OUTER JOIN Salesman S ON C.salesman_id = S.salesman_id 
	LEFT OUTER JOIN orders O ON O.customer_id = C.customer_id
	WHERE O.purch_amt >2000 AND C.grade IS NOT NULL;

--16. Write a SQL statement to generate a report with the customer name, city, order no. order date, purchase amount for only those customers on the list who must have a 
--grade and placed one or more orders or which order(s) have been placed by the customer who neither is on the list nor has a grade.
SELECT c.cust_name AS customerName,
       c.city,
       o.ord_no,
       o.ord_date,
       o.purch_amt
  FROM customer c
  FULL OUTER JOIN orders o
    ON c.customer_id= o.customer_id
      AND c.grade IS NOT NULL;


--17. Write a SQL query to combine each row of the salesman table with each row of the customer tablE
SELECT * FROM salesman S CROSS JOIN customer C;


--19. Write a SQL statement to create a Cartesian product between salesperson and customer, i.e. each salesperson will appear for every customer and vice versa for those salesmen who belong to a city and customers who require a grade 
SELECT * FROM salesman S CROSS JOIN customer C
WHERE S.city = C.city;

--20. Write a SQL statement to make a Cartesian product between salesman and customer i.e. each salesman will appear for all customers 
--and vice versa for those salesmen who must belong to a city which is not the same as his customer and the customers should have their own grade
SELECT * FROM salesman s CROSS JOIN customer c
WHERE s.city IS NOT NULL AND c.grade IS NOT NULL AND s.city <> c.city;

