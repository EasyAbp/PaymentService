using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class AddedRefundItemToDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundItem_EasyAbpPaymentServiceRefunds_RefundId",
                table: "RefundItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefundItem",
                table: "RefundItem");

            migrationBuilder.RenameTable(
                name: "RefundItem",
                newName: "EasyAbpPaymentServiceRefundItems");

            migrationBuilder.RenameIndex(
                name: "IX_RefundItem_RefundId",
                table: "EasyAbpPaymentServiceRefundItems",
                newName: "IX_EasyAbpPaymentServiceRefundItems_RefundId");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "EasyAbpPaymentServiceRefundItems",
                type: "decimal(20,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EasyAbpPaymentServiceRefundItems",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EasyAbpPaymentServiceRefundItems",
                table: "EasyAbpPaymentServiceRefundItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EasyAbpPaymentServiceRefundItems_EasyAbpPaymentServiceRefunds_RefundId",
                table: "EasyAbpPaymentServiceRefundItems",
                column: "RefundId",
                principalTable: "EasyAbpPaymentServiceRefunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EasyAbpPaymentServiceRefundItems_EasyAbpPaymentServiceRefunds_RefundId",
                table: "EasyAbpPaymentServiceRefundItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EasyAbpPaymentServiceRefundItems",
                table: "EasyAbpPaymentServiceRefundItems");

            migrationBuilder.RenameTable(
                name: "EasyAbpPaymentServiceRefundItems",
                newName: "RefundItem");

            migrationBuilder.RenameIndex(
                name: "IX_EasyAbpPaymentServiceRefundItems_RefundId",
                table: "RefundItem",
                newName: "IX_RefundItem_RefundId");

            migrationBuilder.AlterColumn<decimal>(
                name: "RefundAmount",
                table: "RefundItem",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,8)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RefundItem",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefundItem",
                table: "RefundItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundItem_EasyAbpPaymentServiceRefunds_RefundId",
                table: "RefundItem",
                column: "RefundId",
                principalTable: "EasyAbpPaymentServiceRefunds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
