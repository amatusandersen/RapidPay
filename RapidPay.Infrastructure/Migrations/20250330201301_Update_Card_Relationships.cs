using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidPay.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Card_Relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipientCardId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SenderCardId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "ManualCardUpdates",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientCardId",
                table: "Transactions",
                column: "RecipientCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderCardId",
                table: "Transactions",
                column: "SenderCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cards_RecipientCardId",
                table: "Transactions",
                column: "RecipientCardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cards_SenderCardId",
                table: "Transactions",
                column: "SenderCardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cards_RecipientCardId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cards_SenderCardId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecipientCardId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SenderCardId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecipientCardId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderCardId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "ManualCardUpdates");
        }
    }
}
