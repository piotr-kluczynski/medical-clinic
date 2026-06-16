CREATE ROLE db_procexecutor;

CREATE USER ApplicationIdentity IDENTIFIED BY Admin123;
GRANT CONNECT, CREATE SESSION TO ApplicationIdentity;
GRANT db_procexecutor TO ApplicationIdentity;

CREATE USER dev_piotr IDENTIFIED BY Admin123;
GRANT CONNECT, CREATE SESSION TO dev_piotr;

CREATE USER dev_michal IDENTIFIED BY Admin123;
GRANT CONNECT, CREATE SESSION TO dev_michal;

GRANT SELECT, INSERT, UPDATE, DELETE ON "Departments" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Rooms" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Patients" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Workers" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Schedules" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Equipment" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Supplies" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Visits" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Diagnosis" TO ApplicationIdentity;

GRANT SELECT, INSERT, UPDATE, DELETE ON "Visits_HIST" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "Supplies_HIST" TO ApplicationIdentity;
GRANT SELECT, INSERT, UPDATE, DELETE ON "__EFMigrationsHistory" TO ApplicationIdentity;


GRANT SELECT ON "Departments" TO dev_piotr;
GRANT SELECT ON "Rooms" TO dev_piotr;
GRANT SELECT ON "Patients" TO dev_piotr;
GRANT SELECT ON "Workers" TO dev_piotr;
GRANT SELECT ON "Schedules" TO dev_piotr;
GRANT SELECT ON "Equipment" TO dev_piotr;
GRANT SELECT ON "Supplies" TO dev_piotr;
GRANT SELECT ON "Visits" TO dev_piotr;
GRANT SELECT ON "Diagnosis" TO dev_piotr;

GRANT SELECT ON "Departments" TO dev_michal;
GRANT SELECT ON "Rooms" TO dev_michal;
GRANT SELECT ON "Patients" TO dev_michal;
GRANT SELECT ON "Workers" TO dev_michal;
GRANT SELECT ON "Schedules" TO dev_michal;
GRANT SELECT ON "Equipment" TO dev_michal;
GRANT SELECT ON "Supplies" TO dev_michal;
GRANT SELECT ON "Visits" TO dev_michal;
GRANT SELECT ON "Diagnosis" TO dev_michal;