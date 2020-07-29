using Microsoft.EntityFrameworkCore.Migrations;

namespace YaeherDoctorAPI.Migrations
{
    public partial class upatedatabaseYaherDoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReceiptState",
                table: "YaeherDoctor",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ServiceState",
                table: "YaeherDoctor",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ActualNumber",
                table: "ServiceMoneyList",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptState",
                table: "YaeherDoctor");

            migrationBuilder.DropColumn(
                name: "ServiceState",
                table: "YaeherDoctor");

            migrationBuilder.DropColumn(
                name: "ActualNumber",
                table: "ServiceMoneyList");
        }
    }
}
