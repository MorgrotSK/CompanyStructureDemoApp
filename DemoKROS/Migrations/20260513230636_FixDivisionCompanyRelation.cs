using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoKROS.Migrations
{
    /// <inheritdoc />
    public partial class FixDivisionCompanyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Companies_CompanyEntityId",
                table: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Divisions_CompanyEntityId",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "CompanyEntityId",
                table: "Divisions");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Companies_CompanyId",
                table: "Divisions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Companies_CompanyId",
                table: "Divisions");

            migrationBuilder.AddColumn<int>(
                name: "CompanyEntityId",
                table: "Divisions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_CompanyEntityId",
                table: "Divisions",
                column: "CompanyEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Companies_CompanyEntityId",
                table: "Divisions",
                column: "CompanyEntityId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
