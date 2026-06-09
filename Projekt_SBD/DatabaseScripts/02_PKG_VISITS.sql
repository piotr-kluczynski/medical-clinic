CREATE OR REPLACE PACKAGE PKG_VISITS AS

    TYPE t_availability_cursor IS REF CURSOR;

    PROCEDURE ScheduleVisit (
        p_patient_id IN NUMBER,
        p_worker_id IN NUMBER,
        p_room_id IN NUMBER,
        p_start_time IN TIMESTAMP,
        p_end_time IN TIMESTAMP,
        p_purpose IN VARCHAR2,
        p_cost IN NUMBER
    );

    PROCEDURE CancelVisit (
        p_visit_id IN NUMBER
    );

    FUNCTION CheckDoctorAvailability (
        p_worker_id IN NUMBER,
        p_date IN DATE
    ) RETURN t_availability_cursor;

END PKG_VISITS;
/

CREATE OR REPLACE PACKAGE BODY PKG_VISITS AS

    PROCEDURE ScheduleVisit (
        p_patient_id IN NUMBER,
        p_worker_id IN NUMBER,
        p_room_id IN NUMBER,
        p_start_time IN TIMESTAMP,
        p_end_time IN TIMESTAMP,
        p_purpose IN VARCHAR2,
        p_cost IN NUMBER
    ) IS
        v_room_count NUMBER;
        v_worker_count NUMBER;
    BEGIN
        SELECT COUNT(*)
        INTO v_room_count
        FROM "Visits"
        WHERE "RoomId" = p_room_id
          AND (p_start_time < "End" AND p_end_time > "Start");

        IF v_room_count > 0 THEN
            RAISE_APPLICATION_ERROR(-20001, 'Wybrany pokój jest już zajęty w tym czasie.');
        END IF;

        SELECT COUNT(*)
        INTO v_worker_count
        FROM "Visits"
        WHERE "WorkerId" = p_worker_id
          AND (p_start_time < "End" AND p_end_time > "Start");

        IF v_worker_count > 0 THEN
            RAISE_APPLICATION_ERROR(-20002, 'Lekarz ma już zaplanowaną wizytę w tym czasie.');
        END IF;

        INSERT INTO "Visits" ("Purpose", "Start", "End", "Cost", "PatientId", "RoomId", "WorkerId")
        VALUES (p_purpose, p_start_time, p_end_time, p_cost, p_patient_id, p_room_id, p_worker_id);

        UPDATE "Rooms"
        SET "Status" = 'Zajęty'
        WHERE "Id" = p_room_id;

        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END ScheduleVisit;


    PROCEDURE CancelVisit (
        p_visit_id IN NUMBER
    ) IS
        v_room_id NUMBER;
    BEGIN
        SELECT "RoomId" INTO v_room_id
        FROM "Visits"
        WHERE "Id" = p_visit_id;

        DELETE FROM "Visits"
        WHERE "Id" = p_visit_id;

        UPDATE "Rooms"
        SET "Status" = 'Wolny'
        WHERE "Id" = v_room_id;

        COMMIT;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(-20003, 'Wizyta o podanym ID nie istnieje.');
        WHEN OTHERS THEN
            ROLLBACK;
            RAISE;
    END CancelVisit;


    FUNCTION CheckDoctorAvailability (
        p_worker_id IN NUMBER,
        p_date IN DATE
    ) RETURN t_availability_cursor IS
        v_cursor t_availability_cursor;
    BEGIN
        OPEN v_cursor FOR
            SELECT "Start", "End"
            FROM "Visits"
            WHERE "WorkerId" = p_worker_id
              AND TRUNC("Start") = TRUNC(p_date)
            ORDER BY "Start";

        RETURN v_cursor;
    END CheckDoctorAvailability;

END PKG_VISITS;
/
