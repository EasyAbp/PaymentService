using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class AddedPendingProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PendingTopUpPaymentId",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingWithdrawalAmount",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "PendingWithdrawalRecordId",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PendingTopUpPaymentId",
                table: "EasyAbpPaymentServicePrepaymentAccounts");

            migrationBuilder.DropColumn(
                name: "PendingWithdrawalAmount",
                table: "EasyAbpPaymentServicePrepaymentAccounts");

            migrationBuilder.DropColumn(
                name: "PendingWithdrawalRecordId",
                table: "EasyAbpPaymentServicePrepaymentAccounts");
        }
    }
}
