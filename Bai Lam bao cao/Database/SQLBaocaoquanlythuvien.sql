Create database Baocaoquanlythuvien
use baocaoquanlythuvien


CREATE TABLE Admins (
    MaNhanVien INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) Not null,
    SoDienThoai NVARCHAR(20) not null,
	Chucvu Nvarchar(40) not null,
	TenDangNhap varchar(50) not null,
	Matkhau varchar(50) not null

);

CREATE TABLE KhachHang (
    MaKhachHang INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATE NOT NULL,
    DiaChi NVARCHAR(200) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    SoDienThoai NVARCHAR(20) NOT NULL,
	TenDangNhap varchar(50) NOT NULL,
	Matkhau varchar(50) NOT NULL,
	GioiTinh BIT NOT NULL DEFAULT 1
   
);

CREATE TABLE Sach (
    MaSach INT PRIMARY KEY IDENTITY(1,1),
    TieuDe NVARCHAR(200) NOT NULL,
    TacGia NVARCHAR(100) NOT NULL,
    NhaXuatBan NVARCHAR(100)NOT NULL
    TheLoai NVARCHAR(50) NOT NULL,
    SoLuongTon INT NOT NULL DEFAULT 0,
	SoLuongMuon Int not null Default 0,
	NgayNhap date not null
);
drop table MuonSach ;
CREATE TABLE MuonSach (
    MaMuon INT PRIMARY KEY IDENTITY(1,1),
    MaKhachHang INT NOT NULL,
    MaNhanVien INT NOT NULL,
    NgayMuon DATE NOT NULL DEFAULT GETDATE(),
    HanTra DATE NOT NULL,
    NgayTra DATE NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT N'Đang mượn'
        CHECK (TrangThai IN (N'Đang mượn', N'Đã trả', N'Trễ hạn')),

    FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang),
    FOREIGN KEY (MaNhanVien) REFERENCES Admins(MaNhanVien)
);
CREATE TABLE ChiTietMuonSach (
    MaChiTietMuon INT PRIMARY KEY IDENTITY(1,1),
    MaMuon INT NOT NULL,
    MaSach INT NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    GhiChu NVARCHAR(255) NULL,

    FOREIGN KEY (MaMuon) REFERENCES MuonSach(MaMuon),
    FOREIGN KEY (MaSach) REFERENCES Sach(MaSach)
);

