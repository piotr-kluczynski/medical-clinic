-- Tworzenie triggera archiwizującego dla wizyt
CREATE OR REPLACE TRIGGER TRG_ArchiveVisit
AFTER UPDATE OR DELETE ON "Visits"
FOR EACH ROW
DECLARE
    v_action VARCHAR2(10);
    v_user VARCHAR2(100);
BEGIN
    IF UPDATING THEN
        v_action := 'UPDATE';
    ELSIF DELETING THEN
        v_action := 'DELETE';
    END IF;

    SELECT USER INTO v_user FROM DUAL;

    INSERT INTO "Visits_HIST" (
        "ActionType", 
        "ArchiveDate", 
        "ArchiveUser", 
        "OriginalVisitId", 
        "Purpose", 
        "Start", 
        "End", 
        "Cost", 
        "PatientId", 
        "RoomId", 
        "DiagnosisId", 
        "WorkerId"
    ) VALUES (
        v_action,
        CURRENT_TIMESTAMP,
        v_user,
        :OLD."Id",
        :OLD."Purpose",
        :OLD."Start",
        :OLD."End",
        :OLD."Cost",
        :OLD."PatientId",
        :OLD."RoomId",
        :OLD."DiagnosisId",
        :OLD."WorkerId"
    );
END;
/
