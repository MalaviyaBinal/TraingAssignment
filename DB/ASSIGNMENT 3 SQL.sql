CREATE TABLE Department(
	dept_id INT PRIMARY KEY IDENTITY(1,1),
	dept_name VARCHAR(30) NOT NULL,
);
GO

CREATE TABLE Employee(
	emp_id INT PRIMARY KEY IDENTITY(1,1),
	dept_id INT,
	mngr_id INT,
	emp_name NVARCHAR(40) NOT NULL,
	salary MONEY NOT NULL
);
GO

INSERT INTO Department([dept_name])
VALUES 
('Information Technology'),
('Civil'),
('Electrical'),
('Mechanical'),
('Auto Mobile'),
('IC');
GO

SELECT * FROM Department;
GO

insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 3, 'Jedidiah Handyside', 78235);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 4, 'Ingar Kindell', 74251);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Goober Castelin', 119488);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 4, 'Nert Cockburn', 95603);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 4, 'Bobbe Maclean', 81960);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 4, 'Dagny O''Growgane', 104814);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Jerrold Mulcaster', 61808);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 1, 'Erny Frissell', 91041);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (6, 1, 'Cindee Adamczyk', 38461);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Misty Boutton', 118240);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 1, 'Nani Tutton', 117490);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 3, 'Pattie Castillon', 58195);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 2, 'Morse Resun', 144015);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 2, 'Kare Jakov', 71200);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 2, 'Debi Spawforth', 136313);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 4, 'Noreen Bonnier', 42009);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (5, 4, 'Lucky Osbaldeston', 38689);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (5, 1, 'Garrard Simkin', 26274);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 1, 'Hamnet McClifferty', 43230);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (5, 1, 'Helaine Sholem', 78503);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (6, 1, 'Kakalina Denning', 37964);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (5, 4, 'Amitie McNeice', 38228);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 2, 'Valeria Snashall', 21167);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 1, 'Cecile Kopec', 143741);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (5, 4, 'Hatty Skirvane', 34536);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 4, 'Enos Pechet', 94804);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 4, 'Corby Nannizzi', 32034);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Olenolin Bernhardi', 25385);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 1, 'Tiff Milam', 20831);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 2, 'Kliment Martellini', 101550);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 3, 'Chere Pethick', 24902);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 1, 'Silvia Antonoyev', 67928);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 3, 'Maryellen Gale', 92417);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Jaine Billsberry', 72349);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 3, 'Lettie Bax', 116622);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Birgit Jepps', 101711);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (5, 3, 'Moina Baldry', 95059);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 2, 'Blisse Earlam', 144395);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 1, 'Jule Asif', 116074);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 3, 'Donia Drewe', 84931);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 3, 'Ag Tomaszkiewicz', 90724);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 4, 'Nikkie Jovis', 136617);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (4, 4, 'Cobb Rickertsen', 130084);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 4, 'Duky McMackin', 78767);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 1, 'Maxwell Geekin', 89810);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (3, 3, 'Davina Wollen', 122041);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Moyna Frood', 82396);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Sean Tranter', 119370);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (2, 3, 'Nick Kinavan', 100247);
insert into Employee (dept_id, mngr_id, emp_name, salary) values (1, 3, 'Alicia Brealey', 118022);
GO

SELECT * FROM Employee;
GO

--1. write a SQL query to find Employees who have the biggest salary in their Department
select max(E.salary) AS "Maximum salary in  department",E.dept_id ,D.dept_name from Employee E JOIN Department D ON E.dept_id=D.dept_id
GROUP BY E.dept_id,D.dept_name;

--2. write a SQL query to find Departments that have less than 3 people in it
select * from(
	select COUNT(E.emp_id) as noOfEmp,E.dept_id ,D.dept_name from Employee E JOIN Department D ON E.dept_id=D.dept_id GROUP BY E.dept_id,D.dept_name) T
 where noOfEmp <3;

 --3. write a SQL query to find All Department along with the number of people there
 select COUNT(E.emp_id) as noOfEmp,E.dept_id ,D.dept_name from Employee E JOIN Department D ON E.dept_id=D.dept_id GROUP BY E.dept_id,D.dept_name;

 --4. write a SQL query to find All Department along with the total salary there
select sum(E.salary) AS "Total salary in  department",E.dept_id ,D.dept_name from Employee E JOIN Department D ON E.dept_id=D.dept_id
GROUP BY E.dept_id,D.dept_name;
