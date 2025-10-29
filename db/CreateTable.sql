if DB_ID('UsuariosDB') is null
begin
create database UsuariosDB;
end
go

use UsuariosDB;
go


if OBJECT_ID('dbo.Usuario', 'U') is not null
begin
drop table dbo.Usuario;
end
go

create table dbo.Usuario (
Id int identity(1,1) primary key,
Descripcion varchar(100) not null, -- Nombre completo o alias descriptivo
Tipo varchar(20) not null, -- Administrador | Cliente | Agente
CorreoElectronico varchar(120) not null,
Telefono varchar(30) null, -- se valida desde app que sean dígitos
Activo bit not null default 1,
FechaAlta datetime2 not null default sysdatetime()
);

create unique index UX_Usuario_Correo on dbo.Usuario(CorreoElectronico);


alter table dbo.Usuario add constraint CK_Usuario_Tipo
check (Tipo in ('Administrador','Cliente','Agente'));


insert into dbo.Usuario (Descripcion, Tipo, CorreoElectronico, Telefono, Activo)
values
('César Rómulo', 'Administrador', 'cesar@validacion.com', '1133445566', 1),
('Ana Gómez', 'Cliente', 'ana.gomez@validacion.com', '1122334455', 1);

select * from dbo.Usuario;