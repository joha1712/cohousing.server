CREATE DATABASE "mya.cohousing.sql.prd";

--USE [mya.cohousing.sql.prd]
CREATE USER "mya.cohousing.sql.user.prd" WITH ENCRYPTED PASSWORD '[PASSWORD]';

GRANT INSERT, UPDATE, DELETE ON DATABASE "mya.cohousing.sql.prd" TO "mya.cohousing.sql.user.prd";
GRANT SELECT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.prd";
GRANT UPDATE ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.prd";
GRANT INSERT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.prd";
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public TO "mya.cohousing.sql.user.prd";