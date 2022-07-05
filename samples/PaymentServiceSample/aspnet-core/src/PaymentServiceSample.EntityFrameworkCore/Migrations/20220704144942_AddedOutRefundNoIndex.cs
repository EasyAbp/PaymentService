using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentServiceSample.Migrations
{
    public partial class AddedOutRefundNoIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OutRefundNo",
                table: "EasyAbpPaymentServiceWeChatPayRefundRecords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpPaymentServiceWeChatPayRefundRecords_OutRefundNo",
                table: "EasyAbpPaymentServiceWeChatPayRefundRecords",
                column: "OutRefundNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EasyAbpPaymentServiceWeChatPayRefundRecords_OutRefundNo",
                table: "EasyAbpPaymentServiceWeChatPayRefundRecords");

            migrationBuilder.AlterColumn<string>(
                name: "OutRefundNo",
                table: "EasyAbpPaymentServiceWeChatPayRefundRecords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
