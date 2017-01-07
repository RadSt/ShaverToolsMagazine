using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShaverToolsShop.Data.Migrations
{
    public partial class RenamedDeliveryDayFieldsSubscriptionTab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubscriptionSecondDay",
                table: "Subscriptions",
                newName: "SecondDeliveryDay");

            migrationBuilder.RenameColumn(
                name: "SubscriptionFirstDay",
                table: "Subscriptions",
                newName: "FirstDeliveryDay");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondDeliveryDay",
                table: "Subscriptions",
                newName: "SubscriptionSecondDay");

            migrationBuilder.RenameColumn(
                name: "FirstDeliveryDay",
                table: "Subscriptions",
                newName: "SubscriptionFirstDay");
        }
    }
}
