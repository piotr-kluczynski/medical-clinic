using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt_SBD.Migrations
{
    /// <inheritdoc />
    public partial class TableReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Workers",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiagnosisId",
                table: "Visits",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Visits",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Visits",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Supplies",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Schedules",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Rooms",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RelatedPatientId",
                table: "Patients",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Equipment",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Diagnosis",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Diagnosis",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_RoomId",
                table: "Workers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_DiagnosisId",
                table: "Visits",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_PatientId",
                table: "Visits",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Visits_RoomId",
                table: "Visits",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_RoomId",
                table: "Supplies",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_WorkerId",
                table: "Schedules",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DepartmentId",
                table: "Rooms",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_RelatedPatientId",
                table: "Patients",
                column: "RelatedPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_RoomId",
                table: "Equipment",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosis_PatientId",
                table: "Diagnosis",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosis_WorkerId",
                table: "Diagnosis",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnosis_Patients_PatientId",
                table: "Diagnosis",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnosis_Workers_WorkerId",
                table: "Diagnosis",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_Rooms_RoomId",
                table: "Equipment",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Patients_RelatedPatientId",
                table: "Patients",
                column: "RelatedPatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Departments_DepartmentId",
                table: "Rooms",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Workers_WorkerId",
                table: "Schedules",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Rooms_RoomId",
                table: "Supplies",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Diagnosis_DiagnosisId",
                table: "Visits",
                column: "DiagnosisId",
                principalTable: "Diagnosis",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Patients_PatientId",
                table: "Visits",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Rooms_RoomId",
                table: "Visits",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Rooms_RoomId",
                table: "Workers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnosis_Patients_PatientId",
                table: "Diagnosis");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnosis_Workers_WorkerId",
                table: "Diagnosis");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_Rooms_RoomId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Patients_RelatedPatientId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Departments_DepartmentId",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Workers_WorkerId",
                table: "Schedules");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Rooms_RoomId",
                table: "Supplies");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Diagnosis_DiagnosisId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Patients_PatientId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Rooms_RoomId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Rooms_RoomId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Workers_RoomId",
                table: "Workers");

            migrationBuilder.DropIndex(
                name: "IX_Visits_DiagnosisId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_PatientId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_RoomId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Supplies_RoomId",
                table: "Supplies");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_WorkerId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_DepartmentId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Patients_RelatedPatientId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Equipment_RoomId",
                table: "Equipment");

            migrationBuilder.DropIndex(
                name: "IX_Diagnosis_PatientId",
                table: "Diagnosis");

            migrationBuilder.DropIndex(
                name: "IX_Diagnosis_WorkerId",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "DiagnosisId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RelatedPatientId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Diagnosis");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Diagnosis");
        }
    }
}
