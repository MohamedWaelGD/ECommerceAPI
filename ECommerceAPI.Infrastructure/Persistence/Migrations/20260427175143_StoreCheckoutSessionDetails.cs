using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StoreCheckoutSessionDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StripeCheckoutSessionExpiresAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeCheckoutSessionUrl",
                table: "Orders",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCheckoutSessionExpiresAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StripeCheckoutSessionUrl",
                table: "Orders");
        }
    }
}
