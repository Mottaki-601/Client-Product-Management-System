CREATE DATABASE r67_MSD
GO
USE r67_MSD
GO

CREATE TABLE products
(
	productId int identity primary key,
	productName nvarchar(50) not null
)
go

CREATE TABLE clients
(
	clientId int identity primary key,
	clientName nvarchar(50) not null,
	birthDate date not null,
	age int not null,
	picture nvarchar(max) null,
	insideDhaka bit not null
)
go 

create table orders
(
	orderId int identity primary key,
	clientId int references clients(clientId),
	productId int references products(productId)
)
go 