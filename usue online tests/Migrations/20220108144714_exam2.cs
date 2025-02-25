﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace usue_online_tests.Migrations
{
    public partial class exam2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeEnd",
                table: "Exams",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeStart",
                table: "Exams",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTimeEnd",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "DateTimeStart",
                table: "Exams");
        }
    }
}
