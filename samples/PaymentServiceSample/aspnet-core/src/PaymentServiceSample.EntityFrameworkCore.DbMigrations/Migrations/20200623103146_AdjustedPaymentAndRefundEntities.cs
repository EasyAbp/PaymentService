using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentServiceSample.Migrations
{
    public partial class AdjustedPaymentAndRefundEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReqInfo",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.AlterColumn<int>(
                name: "SettlementRefundFee",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CashFee",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CashFeeType",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CashRefundFee",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CouponIds",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CouponRefundCount",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CouponRefundFee",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CouponRefundFees",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CouponTypes",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeeType",
                table: "PaymentServiceWeChatPayRefundRecords",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledTime",
                table: "PaymentServiceRefunds",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedTime",
                table: "PaymentServiceRefunds",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingRefundAmount",
                table: "PaymentServicePayments",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingRefundAmount",
                table: "PaymentServicePaymentItems",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnits",
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
                    ParentId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnitRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    OrganizationUnitId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpOrganizationUnitRoles", x => new { x.OrganizationUnitId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnitRoles_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpOrganizationUnitRoles_AbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbpUserOrganizationUnits",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    OrganizationUnitId = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    TenantId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpUserOrganizationUnits", x => new { x.OrganizationUnitId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AbpUserOrganizationUnits_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbpUserOrganizationUnits_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentServiceWeChatPayRefundRecords_PaymentId",
                table: "PaymentServiceWeChatPayRefundRecords",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentServiceWeChatPayPaymentRecords_PaymentId",
                table: "PaymentServiceWeChatPayPaymentRecords",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnitRoles_RoleId_OrganizationUnitId",
                table: "AbpOrganizationUnitRoles",
                columns: new[] { "RoleId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_Code",
                table: "AbpOrganizationUnits",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ParentId",
                table: "AbpOrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUserOrganizationUnits_UserId_OrganizationUnitId",
                table: "AbpUserOrganizationUnits",
                columns: new[] { "UserId", "OrganizationUnitId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpOrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "AbpUserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_PaymentServiceWeChatPayRefundRecords_PaymentId",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropIndex(
                name: "IX_PaymentServiceWeChatPayPaymentRecords_PaymentId",
                table: "PaymentServiceWeChatPayPaymentRecords");

            migrationBuilder.DropColumn(
                name: "CashFee",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CashFeeType",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CashRefundFee",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CouponIds",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CouponRefundCount",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CouponRefundFee",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CouponRefundFees",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CouponTypes",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "FeeType",
                table: "PaymentServiceWeChatPayRefundRecords");

            migrationBuilder.DropColumn(
                name: "CancelledTime",
                table: "PaymentServiceRefunds");

            migrationBuilder.DropColumn(
                name: "CompletedTime",
                table: "PaymentServiceRefunds");

            migrationBuilder.DropColumn(
                name: "PendingRefundAmount",
                table: "PaymentServicePayments");

            migrationBuilder.DropColumn(
                name: "PendingRefundAmount",
                table: "PaymentServicePaymentItems");

            migrationBuilder.AlterColumn<int>(
                name: "SettlementRefundFee",
                table: "PaymentServiceWeChatPayRefundRecords",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReqInfo",
                table: "PaymentServiceWeChatPayRefundRecords",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
