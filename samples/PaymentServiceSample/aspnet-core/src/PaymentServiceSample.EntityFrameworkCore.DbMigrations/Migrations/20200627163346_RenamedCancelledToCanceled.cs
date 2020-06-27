using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class RenamedCancelledToCanceled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NonceStr",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "NonceStr",
                table: "PaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.DropColumn(
                name: "Sign",
                table: "PaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.DropColumn(
                name: "SignType",
                table: "PaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.RenameColumn(
                name: "CancelledTime",
                table: "PaymentServiceRefunds",
                newName: "CanceledTime");

            migrationBuilder.RenameColumn(
                name: "CancelledTime",
                table: "PaymentServicePayments",
                newName: "CanceledTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NonceStr",
                table: "PaymentServiceWeChatPayRefundRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NonceStr",
                table: "PaymentServiceWeChatPayPaymentRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sign",
                table: "PaymentServiceWeChatPayPaymentRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignType",
                table: "PaymentServiceWeChatPayPaymentRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "CanceledTime",
                table: "PaymentServiceRefunds",
                newName: "CancelledTime");

            migrationBuilder.RenameColumn(
                name: "CanceledTime",
                table: "PaymentServicePayments",
                newName: "CancelledTime");
        }
    }
}
