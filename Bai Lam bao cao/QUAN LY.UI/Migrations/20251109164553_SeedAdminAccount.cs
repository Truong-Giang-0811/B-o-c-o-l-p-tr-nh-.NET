using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QUAN_LY.UI.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GioiTinh",
                table: "KhachHang",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Anhcanhan",
                table: "KhachHang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "Admins",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "GioiTinh",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Anhcanhan",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "MaNhanVien", "Anhcanhan", "Chucvu", "DiaChi", "Email", "GioiTinh", "HoTen", "Matkhau", "NgaySinh", "SoDienThoai", "Tendangnhap" },
                values: new object[] { -1, null, "Admin", null, null, null, "Trường Giang", "$2a$11$bBw6KOAmlVKN9ibkK95KGex8ctfk0SRNiN3fXwL8m6iFPQW.TuF8i", null, null, "Admin0811" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "MaNhanVien",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "Anhcanhan",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "Anhcanhan",
                table: "Admins");

            migrationBuilder.AlterColumn<bool>(
                name: "GioiTinh",
                table: "KhachHang",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "SoDienThoai",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "Admins",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "GioiTinh",
                table: "Admins",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
