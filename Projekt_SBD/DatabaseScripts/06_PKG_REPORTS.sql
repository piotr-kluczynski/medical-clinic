CREATE OR REPLACE PACKAGE PKG_REPORTS AS
    -- Definicja typu zwracanego dla wyników raportów
    TYPE t_report_cursor IS REF CURSOR;

    -- Funkcja generująca zestawienie kosztów operacyjnych na dany miesiąc
    FUNCTION MonthlyCosts(
        p_year IN NUMBER, 
        p_month IN NUMBER
    ) RETURN t_report_cursor;

    -- Funkcja zwracająca listę najczęściej zużywanych materiałów
    FUNCTION SuppliesUsageReport(
        p_start_date IN DATE, 
        p_end_date IN DATE
    ) RETURN t_report_cursor;

END PKG_REPORTS;
/

CREATE OR REPLACE PACKAGE BODY PKG_REPORTS AS

    FUNCTION MonthlyCosts(
        p_year IN NUMBER, 
        p_month IN NUMBER
    ) RETURN t_report_cursor IS
        v_cursor t_report_cursor;
    BEGIN
        -- Zgodnie z założeniami kosztem miesięcznym jest suma pensji pracowników.
        -- Parametry p_year i p_month pozwalają w przyszłości na rozszerzenie logiki 
        -- o uwzględnianie historii zatrudnienia lub premii zależnych od czasu.
        OPEN v_cursor FOR
            SELECT 
                p_year AS "Year",
                p_month AS "Month",
                SUM("Salary") AS "TotalSalariesCost"
            FROM "Workers";
            
        RETURN v_cursor;
    END MonthlyCosts;


    FUNCTION SuppliesUsageReport(
        p_start_date IN DATE, 
        p_end_date IN DATE
    ) RETURN t_report_cursor IS
        v_cursor t_report_cursor;
    BEGIN
        -- Logika oblicza sumę zużytych materiałów na podstawie wyliczonego
        -- ubytku ("UsedAmount") bezpośrednio w tabeli historycznej.
        OPEN v_cursor FOR
            SELECT 
                "Name", 
                SUM("UsedAmount") AS "TotalUsed"
            FROM "Supplies_HIST"
            WHERE "ActionType" = 'UPDATE'
              AND "UsedAmount" > 0 
              AND "ArchiveDate" >= p_start_date 
              AND "ArchiveDate" <= p_end_date
            GROUP BY "Name"
            ORDER BY "TotalUsed" DESC;
            
        RETURN v_cursor;
    END SuppliesUsageReport;

END PKG_REPORTS;
/
