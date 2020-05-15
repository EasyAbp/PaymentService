using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentServicePaymentService",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false),
                    PaymentMethod = table.Column<string>(nullable: true),
                    PayeeAccount = table.Column<string>(nullable: true),
                    ExternalTradingCode = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    OriginalPaymentAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PaymentDiscount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ActualPaymentAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CompletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentServicePaymentService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentServiceRefunds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExtraProperties = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true),
                    PaymentId = table.Column<Guid>(nullable: false),
                    PaymentItemId = table.Column<Guid>(nullable: false),
                    RefundPaymentMethod = table.Column<string>(nullable: true),
                    ExternalTradingCode = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    CustomerRemark = table.Column<string>(nullable: true),
                    StaffRemark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentServiceRefunds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentServicePaymentItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ItemType = table.Column<string>(nullable: true),
                    ItemKey = table.Column<Guid>(nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    OriginalPaymentAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PaymentDiscount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    ActualPaymentAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    PaymentId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentServicePaymentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentServicePaymentItems_PaymentServicePaymentService_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "PaymentServicePaymentService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentServicePaymentItems_PaymentId",
                table: "PaymentServicePaymentItems",
                column: "PaymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentServicePaymentItems");

            migrationBuilder.DropTable(
                name: "PaymentServiceRefunds");

            migrationBuilder.DropTable(
                name: "PaymentServicePaymentService");
        }
    }
}
