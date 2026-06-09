-- Tworzenie triggera ostrzegającego o niskim stanie i archiwizującego
CREATE OR REPLACE TRIGGER TRG_LowStockWarning
AFTER UPDATE OR DELETE ON "Supplies"
FOR EACH ROW
DECLARE
    v_action VARCHAR2(10);
    v_user VARCHAR2(100);
BEGIN
    IF UPDATING THEN
        v_action := 'UPDATE';
        -- Blokada jeśli stan po aktualizacji schodzi poniżej 0
        IF :NEW."Quantity" < 0 THEN
            RAISE_APPLICATION_ERROR(-20004, 'Niewystarczająca ilość materiałów. Stan zapasów zablokowałby się poniżej zera.');
        END IF;
    ELSIF DELETING THEN
        v_action := 'DELETE';
    END IF;

    SELECT USER INTO v_user FROM DUAL;

    INSERT INTO "Supplies_HIST" (
        "ActionType", 
        "ArchiveDate", 
        "ArchiveUser", 
        "OriginalSupplyId", 
        "Name", 
        "Description", 
        "Quantity",
        "RoomId"
    ) VALUES (
        v_action,
        CURRENT_TIMESTAMP,
        v_user,
        :OLD."Id",
        :OLD."Name",
        :OLD."Description",
        :OLD."Quantity",
        :OLD."RoomId"
    );
END;
/
