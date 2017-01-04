using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaverToolsShop.Data.Migrations
{
    public partial class AddSubscriptionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    SubscriptionStatus = table.Column<int>(nullable: false),
                    SubscriptionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubscriptionId",
                table: "Products",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Subscriptions_SubscriptionId",
                table: "Products",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Subscriptions_SubscriptionId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubscriptionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Products");
        }
    }
}
