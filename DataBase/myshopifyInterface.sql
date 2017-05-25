create database myshopifyInterface
go
use myshopifyInterface
go
create table tiendas(
	tienda_id varchar(50) primary key,
	vchNombreTienda varchar(100) not null,
	vchUrlOrdenes NVARCHAR(MAX) not null,
	vchUrlTransacciones NVARCHAR(max) not null,
	vchUsername varchar(500) not null,
	vchPassword varchar(500) not null,
	bitActiva bit not null
)
create table ordenes(
	order_id bigint primary key,
	tienda_id varchar(50),
	foreign key(tienda_id) references tiendas(tienda_id),
	datFechaEnviada datetime 
)
go