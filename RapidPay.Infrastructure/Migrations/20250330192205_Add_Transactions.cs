using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidPay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Fee",
                table: "Fee");

            migrationBuilder.RenameTable(
                name: "Fee",
                newName: "Fees");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fees",
                table: "Fees",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fees",
                table: "Fees");

            migrationBuilder.RenameTable(
                name: "Fees",
                newName: "Fee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fee",
                table: "Fee",
                column: "Id");
        }
    }
}
