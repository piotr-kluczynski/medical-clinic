CREATE OR REPLACE PACKAGE PKG_INVENTORY AS

    -- Procedura zmniejszająca stan magazynowy (zużycie podczas zabiegu)
    PROCEDURE ConsumeSupply (
        p_supply_id IN NUMBER,
        p_amount IN NUMBER
    );

    -- Procedura zwiększająca stan magazynowy (dostawa)
    PROCEDURE AddSupplyDelivery (
        p_supply_id IN NUMBER,
        p_amount IN NUMBER
    );

    -- Funkcja wyliczająca łączną wartość sprzętu, pomijając zepsuty
    FUNCTION CalculateAssetsValue RETURN NUMBER;

END PKG_INVENTORY;
/

CREATE OR REPLACE PACKAGE BODY PKG_INVENTORY AS

    PROCEDURE ConsumeSupply (
        p_supply_id IN NUMBER,
        p_amount IN NUMBER
    ) IS
    BEGIN
        -- Zmniejszamy ilość danego materiału o p_amount
        -- UWAGA: Jeśli Quantity spadnie poniżej zera, wyzwolony
        -- zostanie wcześniej przygotowany TRG_LowStockWarning,
        -- który automatycznie zgłosi błąd RAISE_APPLICATION_ERROR
        -- i przerwie tę transakcję.
        UPDATE "Supplies"
        SET "Quantity" = "Quantity" - p_amount
        WHERE "Id" = p_supply_id;

        IF SQL%ROWCOUNT = 0 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Nie znaleziono materiału o podanym identyfikatorze.');
        END IF;

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END ConsumeSupply;


    PROCEDURE AddSupplyDelivery (
        p_supply_id IN NUMBER,
        p_amount IN NUMBER
    ) IS
    BEGIN
        IF p_amount <= 0 THEN
            RAISE_APPLICATION_ERROR(-20006, 'Ilość dostarczanego materiału musi być większa od zera.');
        END IF;

        UPDATE "Supplies"
        SET "Quantity" = "Quantity" + p_amount
        WHERE "Id" = p_supply_id;

        IF SQL%ROWCOUNT = 0 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Nie znaleziono materiału o podanym identyfikatorze.');
        END IF;

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END AddSupplyDelivery;


    FUNCTION CalculateAssetsValue RETURN NUMBER IS
        v_total_value NUMBER := 0;
    BEGIN
        -- Sumowanie wartości wyposażenia (Condition to ciąg znaków, pomijamy te z wartością 'Zepsuty')
        SELECT COALESCE(SUM("Value"), 0)
        INTO v_total_value
        FROM "Equipment"
        WHERE "Condition" != 'Zepsuty';

        RETURN v_total_value;
    END CalculateAssetsValue;

END PKG_INVENTORY;
/
