CREATE DATABASE "mya.cohousing.sql.dev";

--USE [mya.cohousing.sql.dev]
CREATE USER "mya.cohousing.sql.user.dev" WITH ENCRYPTED PASSWORD '[PASSWORD]';

GRANT INSERT, UPDATE, DELETE ON DATABASE "mya.cohousing.sql.dev" TO "mya.cohousing.sql.user.dev";
GRANT SELECT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.dev";
GRANT UPDATE ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.dev";
GRANT INSERT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.dev";