using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class AddedIndexesRemovedOppositePartAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OppositePartAccount",
                table: "EasyAbpPaymentServicePrepaymentTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpPaymentServicePrepaymentTransactions_AccountId",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpPaymentServicePrepaymentTransactions_AccountUserId",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                column: "AccountUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EasyAbpPaymentServicePrepaymentAccounts_UserId",
                table: "EasyAbpPaymentServicePrepaymentAccounts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EasyAbpPaymentServicePrepaymentTransactions_AccountId",
                table: "EasyAbpPaymentServicePrepaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_EasyAbpPaymentServicePrepaymentTransactions_AccountUserId",
                table: "EasyAbpPaymentServicePrepaymentTransactions");

            migrationBuilder.DropIndex(
                name: "IX_EasyAbpPaymentServicePrepaymentAccounts_UserId",
                table: "EasyAbpPaymentServicePrepaymentAccounts");

            migrationBuilder.AddColumn<string>(
                name: "OppositePartAccount",
                table: "EasyAbpPaymentServicePrepaymentTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
