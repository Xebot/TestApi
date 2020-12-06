declare @Customers table(
	Name varchar(max) null,
	Address nvarchar(max) null,
	IsVip bit not null
)
 
-- Добавление значений в таблицу Customers
insert @Customers values
	('Customer1', 'Москва, ул.Пушкина, д.Колотушкина', 0),
	('Customer2', 'Севастополь, ул.Б.Морская, 1', 0),
	('Customer3', 'Симферополь, ул. Севастопольская, д.2', 1);
	
merge [dbo].[Customers] as target
using @Customers as source on
	target.Name = source.Name
when not matched by target then
insert (Name, Address, IsVip)
values (source.Name, source.Address, source.IsVip)
when matched then
update set
	target.Name = source.Name,
	target.Address = source.Address,
	target.IsVip = source.IsVip;
	

declare @Products table(
	Name varchar(max) null,
	Price decimal(18,2) not null
)
 
-- Добавление значений в таблицу Customers
insert @Products values
	('Ручка шариковая', 100),
	('Ластик', 150),
	('Тетрадь 12 листов клетка', 20),
	('Тетрадь 24 листа клетка', 35),
	('Тетрадь 36 листов линейка', 40),
	('Ежедневник на 2021 год', 200),
	('Календарь на 2021 год', 10.50);
	
merge [dbo].[Products] as target
using @Products as source on
	target.Name = source.Name
when not matched by target then
insert (Name, Price)
values (source.Name, source.Price);