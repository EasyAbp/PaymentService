using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class RedesignedRefundAggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentItemId",
                table: "EasyAbpPaymentServiceRefunds");

            migrationBuilder.AddColumn<string>(
                name: "DisplayReason",
                table: "EasyAbpPaymentServiceRefunds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefundItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    PaymentItemId = table.Column<Guid>(nullable: false),
                    RefundAmount = table.Column<decimal>(nullable: false),
                    CustomerRemark = table.Column<string>(nullable: true),
                    StaffRemark = table.Column<string>(nullable: true),
                    RefundId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundItem_EasyAbpPaymentServiceRefunds_RefundId",
                        column: x => x.RefundId,
                        principalTable: "EasyAbpPaymentServiceRefunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefundItem_RefundId",
                table: "RefundItem",
                column: "RefundId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefundItem");

            migrationBuilder.DropColumn(
                name: "DisplayReason",
                table: "EasyAbpPaymentServiceRefunds");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentItemId",
                table: "EasyAbpPaymentServiceRefunds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
