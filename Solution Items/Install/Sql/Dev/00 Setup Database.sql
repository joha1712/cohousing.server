USE [master]
CREATE DATABASE [mya.cohousing.sql.user.dev]
GO

USE [mya.cohousing.sql.user.dev]
CREATE LOGIN [mya.cohousing.sql.user.dev] WITH PASSWORD = 'Development1234'
CREATE USER [mya.cohousing.sql.user.dev] FOR LOGIN [mya.cohousing.sql.user.dev]
GO