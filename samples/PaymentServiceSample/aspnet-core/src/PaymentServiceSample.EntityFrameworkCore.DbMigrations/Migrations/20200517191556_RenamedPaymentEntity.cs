using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class RenamedPaymentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentServicePaymentItems_PaymentServicePaymentService_PaymentId",
                table: "PaymentServicePaymentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentServicePaymentService",
                table: "PaymentServicePaymentService");

            migrationBuilder.RenameTable(
                name: "PaymentServicePaymentService",
                newName: "PaymentServicePayments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentServicePayments",
                table: "PaymentServicePayments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentServicePaymentItems_PaymentServicePayments_PaymentId",
                table: "PaymentServicePaymentItems",
                column: "PaymentId",
                principalTable: "PaymentServicePayments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentServicePaymentItems_PaymentServicePayments_PaymentId",
                table: "PaymentServicePaymentItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentServicePayments",
                table: "PaymentServicePayments");

            migrationBuilder.RenameTable(
                name: "PaymentServicePayments",
                newName: "PaymentServicePaymentService");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentServicePaymentService",
                table: "PaymentServicePaymentService",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentServicePaymentItems_PaymentServicePaymentService_PaymentId",
                table: "PaymentServicePaymentItems",
                column: "PaymentId",
                principalTable: "PaymentServicePaymentService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
