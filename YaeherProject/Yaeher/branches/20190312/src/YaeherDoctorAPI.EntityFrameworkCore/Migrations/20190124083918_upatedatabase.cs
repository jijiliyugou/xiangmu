using Microsoft.EntityFrameworkCore.Migrations;

namespace YaeherDoctorAPI.Migrations
{
    public partial class upatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "WecharSendMessage",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CompleteMoney",
                table: "ConsultationOrderTotal",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RefundMoney",
                table: "ConsultationOrderTotal",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompleteMoney",
                table: "ConsultationOrderTotal");

            migrationBuilder.DropColumn(
                name: "RefundMoney",
                table: "ConsultationOrderTotal");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "WecharSendMessage",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
