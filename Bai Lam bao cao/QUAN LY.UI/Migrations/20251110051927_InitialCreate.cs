using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QUAN_LY.UI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    MaNhanVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Anhcanhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chucvu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tendangnhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matkhau = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.MaNhanVien);
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tendangnhap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Matkhau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Anhcanhan = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.MaKhachHang);
                });

            migrationBuilder.CreateTable(
                name: "Sach",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TacGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NhaXuatBan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false),
                    SoLuongMuon = table.Column<int>(type: "int", nullable: false),
                    Thoihanmuon = table.Column<int>(type: "int", nullable: false),
                    Mota = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayNhap = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sach", x => x.MaSach);
                });

            migrationBuilder.CreateTable(
                name: "MuonSach",
                columns: table => new
                {
                    MaMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    Soluong = table.Column<int>(type: "int", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayYeuCau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuonSach", x => x.MaMuon);
                    table.ForeignKey(
                        name: "FK_MuonSach_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietMuonSach",
                columns: table => new
                {
                    MaChiTietMuon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaMuon = table.Column<int>(type: "int", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    NgayMuon = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HanTra = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTra = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietMuonSach", x => x.MaChiTietMuon);
                    table.ForeignKey(
                        name: "FK_ChiTietMuonSach_MuonSach_MaMuon",
                        column: x => x.MaMuon,
                        principalTable: "MuonSach",
                        principalColumn: "MaMuon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietMuonSach_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Giohang",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    MaMuon = table.Column<int>(type: "int", nullable: true),
                    SoLuongmuon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giohang", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_Giohang_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Giohang_MuonSach_MaMuon",
                        column: x => x.MaMuon,
                        principalTable: "MuonSach",
                        principalColumn: "MaMuon");
                    table.ForeignKey(
                        name: "FK_Giohang_Sach_MaSach",
                        column: x => x.MaSach,
                        principalTable: "Sach",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HiddenHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    MaChiTietMuon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiddenHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HiddenHistories_ChiTietMuonSach_MaChiTietMuon",
                        column: x => x.MaChiTietMuon,
                        principalTable: "ChiTietMuonSach",
                        principalColumn: "MaChiTietMuon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HiddenHistories_KhachHang_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMuonSach_MaMuon",
                table: "ChiTietMuonSach",
                column: "MaMuon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietMuonSach_MaSach",
                table: "ChiTietMuonSach",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_Giohang_MaKhachHang",
                table: "Giohang",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_Giohang_MaMuon",
                table: "Giohang",
                column: "MaMuon");

            migrationBuilder.CreateIndex(
                name: "IX_Giohang_MaSach",
                table: "Giohang",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_HiddenHistories_MaChiTietMuon",
                table: "HiddenHistories",
                column: "MaChiTietMuon");

            migrationBuilder.CreateIndex(
                name: "IX_HiddenHistories_MaKhachHang",
                table: "HiddenHistories",
                column: "MaKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_MuonSach_MaKhachHang",
                table: "MuonSach",
                column: "MaKhachHang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Giohang");

            migrationBuilder.DropTable(
                name: "HiddenHistories");

            migrationBuilder.DropTable(
                name: "ChiTietMuonSach");

            migrationBuilder.DropTable(
                name: "MuonSach");

            migrationBuilder.DropTable(
                name: "Sach");

            migrationBuilder.DropTable(
                name: "KhachHang");
        }
    }
}
