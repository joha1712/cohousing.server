CREATE DATABASE "mya.cohousing.sql.tst";

--USE [mya.cohousing.sql.tst]
CREATE USER "mya.cohousing.sql.user.tst" WITH ENCRYPTED PASSWORD '[PASSWORD]';

GRANT INSERT, UPDATE, DELETE ON DATABASE "mya.cohousing.sql.tst" TO "mya.cohousing.sql.user.tst";
GRANT SELECT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.tst";
GRANT UPDATE ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.tst";
GRANT INSERT ON ALL TABLES IN SCHEMA public TO "mya.cohousing.sql.user.tst";
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public TO "mya.cohousing.sql.user.tst";