using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class ChangedItemKeyToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "EasyAbpPaymentServicePaymentItems");

            migrationBuilder.AlterColumn<string>(
                name: "ItemKey",
                table: "EasyAbpPaymentServicePaymentItems",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ItemKey",
                table: "EasyAbpPaymentServicePaymentItems",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "EasyAbpPaymentServicePaymentItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
