using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using Projekt_SBD.Data;

#nullable disable

namespace Projekt_SBD.Migrations
{
    [DbContext(typeof(PrzychodniaContext))]
    [Migration("20260531231753_UpdateLoginsAndVisits")]
    partial class UpdateLoginsAndVisits
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "10.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Projekt_SBD.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Room")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Diagnosis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DiagnosisTime")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Illness")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("PatientId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Symptoms")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("WorkerId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("WorkerId");

                    b.ToTable("Diagnosis");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("LastInspection")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("RoomId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("Value")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int?>("RelatedPatientId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("RelatedPatientId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("Floor")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Purpose")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("EndHour")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("StartHour")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("WorkerId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("WorkerId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Supply", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Quantity")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("RoomId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Supplies");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Visit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Cost")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int?>("DiagnosisId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime>("End")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("PatientId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Purpose")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("RoomId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("WorkerId")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("DiagnosisId");

                    b.HasIndex("PatientId");

                    b.HasIndex("RoomId");

                    b.HasIndex("WorkerId");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int?>("RoomId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("Salary")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Diagnosis", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Patient", "Patient")
                        .WithMany("Diagnoses")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projekt_SBD.Models.Worker", "Worker")
                        .WithMany("Diagnoses")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Equipment", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Room", "Room")
                        .WithMany("Equipments")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Patient", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Patient", "RelatedPatient")
                        .WithMany()
                        .HasForeignKey("RelatedPatientId");

                    b.Navigation("RelatedPatient");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Room", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Department", "Department")
                        .WithMany("Rooms")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Schedule", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Worker", "Worker")
                        .WithMany("Schedules")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Supply", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Room", "Room")
                        .WithMany("Supplies")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Visit", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Diagnosis", "Diagnosis")
                        .WithMany()
                        .HasForeignKey("DiagnosisId");

                    b.HasOne("Projekt_SBD.Models.Patient", "Patient")
                        .WithMany("Visits")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projekt_SBD.Models.Room", "Room")
                        .WithMany("Visits")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Projekt_SBD.Models.Worker", "Worker")
                        .WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Diagnosis");

                    b.Navigation("Patient");

                    b.Navigation("Room");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Worker", b =>
                {
                    b.HasOne("Projekt_SBD.Models.Room", "Room")
                        .WithMany("Workers")
                        .HasForeignKey("RoomId");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Department", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Patient", b =>
                {
                    b.Navigation("Diagnoses");

                    b.Navigation("Visits");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Room", b =>
                {
                    b.Navigation("Equipments");

                    b.Navigation("Supplies");

                    b.Navigation("Visits");

                    b.Navigation("Workers");
                });

            modelBuilder.Entity("Projekt_SBD.Models.Worker", b =>
                {
                    b.Navigation("Diagnoses");

                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
