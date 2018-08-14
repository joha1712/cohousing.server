USE [master]
CREATE DATABASE [mya.cohousing.sql.tst]
GO

USE [mya.cohousing.sql.tst]
CREATE LOGIN [mya.cohousing.sql.user.tst] WITH PASSWORD = 'SHOULD_BE_SOMETHING_SECRET'
CREATE USER [mya.cohousing.sql.user.tst] FOR LOGIN [mya.cohousing.sql.user.tst] WITH DEFAULT_SCHEMA=[dbo]
ALTER ROLE [db_owner] ADD MEMBER [mya.cohousing.sql.user.tst]
GO