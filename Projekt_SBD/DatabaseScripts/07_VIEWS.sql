-- 1. Widok dostępności lekarzy
CREATE OR REPLACE VIEW "v_DoctorAvailability" AS
SELECT 
    w."Id" AS "DoctorId",
    w."FirstName",
    w."LastName",
    w."Position" AS "Specialization",
    v."Start",
    v."End"
FROM "Workers" w
JOIN "Visits" v ON w."Id" = v."WorkerId"
-- Wykluczamy personel niemedyczny (zakładając przykładowe nazwy ról)
WHERE w."Position" NOT IN ('Reception', 'Administrator', 'Nurse', 'Owner', 'Recepcja', 'Pielęgniarka', 'Administrator');
/

-- 2. Pełna karta historii medycznej pacjenta
CREATE OR REPLACE VIEW "v_PatientMedicalHistory" AS
SELECT 
    p."Id" AS "PatientId",
    p."FirstName" AS "PatientFirstName",
    p."LastName" AS "PatientLastName",
    v."Start" AS "VisitDate",
    d."Symptoms",
    d."Illness",
    w."FirstName" AS "DoctorFirstName",
    w."LastName" AS "DoctorLastName"
FROM "Patients" p
JOIN "Visits" v ON p."Id" = v."PatientId"
JOIN "Diagnosis" d ON d."Id" = v."DiagnosisId"
JOIN "Workers" w ON w."Id" = d."WorkerId";
/

-- 3. Widok materiałów wymagających uzupełnienia (niski stan)
CREATE OR REPLACE VIEW "v_LowStockSupplies" AS
SELECT 
    s."Id" AS "SupplyId",
    s."Name" AS "SupplyName",
    s."Quantity",
    r."Id" AS "RoomNumber",
    r."Purpose" AS "RoomName"
FROM "Supplies" s
JOIN "Rooms" r ON s."RoomId" = r."Id"
WHERE s."Quantity" < 10;
/
