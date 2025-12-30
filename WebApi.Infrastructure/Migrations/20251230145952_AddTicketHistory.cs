using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assignment_histories");

            migrationBuilder.RenameColumn(
                name: "deadline",
                table: "tickets",
                newName: "start_date");

            migrationBuilder.AddColumn<DateOnly>(
                name: "end_date",
                table: "tickets",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ticket_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    changes = table.Column<string>(type: "jsonb", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ticket_histories", x => new { x.ticket_id, x.id });
                    table.ForeignKey(
                        name: "fk_ticket_histories_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ticket_histories");

            migrationBuilder.DropColumn(
                name: "end_date",
                table: "tickets");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "tickets",
                newName: "deadline");

            migrationBuilder.CreateTable(
                name: "assignment_histories",
                columns: table => new
                {
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    assignee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    change_type = table.Column<int>(type: "integer", nullable: false),
                    changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    previous_assignee_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_by = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assignment_histories", x => new { x.ticket_id, x.id });
                    table.ForeignKey(
                        name: "fk_assignment_histories_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
