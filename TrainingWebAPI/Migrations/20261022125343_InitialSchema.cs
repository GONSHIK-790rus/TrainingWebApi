using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingWebAPI.Migrations;

/// <inheritdoc />
public partial class InitialSchema : Migration {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) {
        migrationBuilder.CreateTable(
            name: "users",
            columns: table => new {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                age = table.Column<int>(type: "integer", nullable: false),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
            },
            constraints: table => {
                table.PrimaryKey("users_pkey", x => x.id);
                table.CheckConstraint("users_age_check", "age > 0");
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) {
        migrationBuilder.DropTable(
            name: "users");
    }
}

