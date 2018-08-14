USE [master]
CREATE DATABASE [mya.cohousing.sql.dev]
GO

USE [mya.cohousing.sql.dev]
CREATE LOGIN [mya.cohousing.sql.user.dev] WITH PASSWORD = 'Development1234'
CREATE USER [mya.cohousing.sql.user.dev] FOR LOGIN [mya.cohousing.sql.user.dev] WITH DEFAULT_SCHEMA=[dbo]
ALTER ROLE [db_owner] ADD MEMBER [mya.cohousing.sql.user.dev]
GO