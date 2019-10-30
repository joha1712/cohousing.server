CREATE DATABASE "mya.cohousing.sql.loc";

--USE [mya.cohousing.sql.loc]
CREATE USER "mya.cohousing.sql.user.loc" WITH ENCRYPTED PASSWORD 'local1234';

GRANT INSERT, UPDATE, DELETE ON DATABASE "mya.cohousing.sql.loc" TO "mya.cohousing.sql.user.loc";
GRANT SELECT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.loc";
GRANT UPDATE ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.loc";
GRANT INSERT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.loc";