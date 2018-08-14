USE [master]
CREATE DATABASE [mya.cohousing.sql.prd]
GO

USE [mya.cohousing.sql.prd]
CREATE LOGIN [mya.cohousing.sql.user.prd] WITH PASSWORD = 'SHOULD_BE_SOMETHING_SECRET'
CREATE USER [mya.cohousing.sql.user.prd] FOR LOGIN [mya.cohousing.sql.user.prd] WITH DEFAULT_SCHEMA=[dbo]
ALTER ROLE [db_owner] ADD MEMBER [mya.cohousing.sql.user.prd]
GO