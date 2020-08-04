using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class ChangedDecimalColumnTypeTo20_8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EasyAbpPaymentServicePrepaymentTransactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "PaymentServiceRefunds",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "PaymentServicePayments",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingRefundAmount",
                table: "PaymentServicePayments",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentDiscount",
                table: "PaymentServicePayments",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalPaymentAmount",
                table: "PaymentServicePayments",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPaymentAmount",
                table: "PaymentServicePayments",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingRefundAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentDiscount",
                table: "PaymentServicePaymentItems",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalPaymentAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPaymentAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalBalance",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChangedBalance",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountUserId",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<decimal>(
                name: "LockedBalance",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountUserId",
                table: "EasyAbpPaymentServicePrepaymentTransactions");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "PaymentServiceRefunds",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "PaymentServicePayments",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingRefundAmount",
                table: "PaymentServicePayments",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentDiscount",
                table: "PaymentServicePayments",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalPaymentAmount",
                table: "PaymentServicePayments",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPaymentAmount",
                table: "PaymentServicePayments",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PendingRefundAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PaymentDiscount",
                table: "PaymentServicePaymentItems",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalPaymentAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPaymentAmount",
                table: "PaymentServicePaymentItems",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OriginalBalance",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChangedBalance",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<decimal>(
                name: "LockedBalance",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");
        }
    }
}
