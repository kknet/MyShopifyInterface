create database myshopifyInterface
go
use myshopifyInterface
go
create table ordenes(
	order_id bigint primary key,
	datFechaEnviada datetime 
)
go