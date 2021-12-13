USE master
GO

-- Drop the database if it already exists
IF  EXISTS (
	SELECT name 
		FROM sys.databases 
		WHERE name = N'Nhom4'
)
DROP DATABASE Nhom4
GO

CREATE DATABASE Nhom4
GO

USE Nhom4
GO

create table DanhMucSP(
MaDM varchar(20) not null primary key,
TenDanhMuc nvarchar(50) not null,
)

create table SanPham(
MaSP varchar(10) not null primary key,
TenSP nvarchar(200) not null,
MaDM varchar(20) not null foreign key references DanhMucSP(MaDM) on delete cascade on update cascade,
New bit,
MoTa nvarchar(500),
GiaBan money not null,
)



create table AnhMoTa(
MaAnh int identity primary key,
HinhAnh nvarchar(300),
MaSP varchar(10) foreign key references SanPham(MaSP) on delete cascade on update cascade,
)

--drop table ChiTietAnh
---------------------------------
create table ChiTietSanPham(
MaAnh int foreign key(MaAnh) references AnhMoTa(MaAnh) on delete cascade on update cascade,
KichCo int,
primary key(KichCo,MaAnh),
)

--drop table PickSP

create table TaiKhoanNguoiDung(
MaTK int identity(1,1) not null primary key,
TenDangNhap nvarchar(100) not null,
MatKhau nvarchar(20) not null,
HoTen nvarchar(200) not null,
SDT char(20) not null,
DiaChi nvarchar(500) not null,
Email varchar(50),
TrangThai bit not null,
)

 create table TaiKhoanQuanTri(
 MaTK int identity(1,1) not null primary key,
 TenDangNhap varchar(50) not null,
 MatKhau varchar(50) not null,
 HoTenUser nvarchar(50) not null,
 LoaiTK nvarchar(10) not null,
 TrangThai bit not null,
 )

create table HoaDon(
MaHD int identity(1,1) not null primary key,
MaTK int foreign key references TaiKhoanNguoiDung(MaTK) on delete cascade on update cascade,
HoTenNguoiNhan nvarchar(50),
SDTNguoiNhan varchar(20),
DiaChiNhan nvarchar(50),
EmailNguoiNhan varchar(50),
NgayLap datetime,
TongTien money,
TrangThai nvarchar(50),
GhiChu nvarchar(50),
)

--drop table HoaDon
create table ChiTietHoaDon(
MaHD int foreign key references HoaDon(MaHD) on delete cascade on update cascade,
MaAnh int,
KichCo int,
foreign key (KichCo,MaAnh) references ChiTietSanPham(KichCo,MaAnh) on delete cascade on update cascade,
primary key (MaHD,KichCo,MaAnh),
SoluongMua int not null,
)

Create table TinTuc(
MaTin int identity(1,1) primary key,
TenTin nvarchar(100),
NgayDang datetime,
MaTK int foreign key references TaiKhoanQuanTri(MaTK) on delete cascade on update cascade,
NoiDung nvarchar(1000),
)

------
--drop table ChiTietHoaDon


insert into DanhMucSP(MaDM, TenDanhMuc) values
('DM01', N'Giày Nike'),
('DM02', N'Giày Adias'),
('DM03', N'Giày Vans'),
('DM04', N'Giày Converse'),
('DM05', N'Giày Puma'),
('DM06', N'Giày Balenciaga')


insert into SanPham(MaSP, TenSP, MaDM, MoTa, New, GiaBan) values
('SP001', 'Nike air 1','DM01','This is a description','0','350000'),
('SP002', 'Nike air 1','DM01','This is a description','0','350000'),
('SP003', 'Nike air 1','DM01','This is a description','0','350000'),
('SP004', 'Nike air 1','DM01','This is a description','0','350000'),
('SP005', 'Nike air 1','DM01','This is a description','0','350000'),
('SP006', 'Nike air 1','DM01','This is a description','0','350000'),
('SP007', 'Nike air 1','DM01','This is a description','0','350000'),
('SP008', 'Nike air 1','DM01','This is a description','0','350000'),
('SP009', 'Nike air 1','DM01','This is a description','0','350000'),
('SP010', 'Nike air 1','DM01','This is a description','0','350000'),
('SP011', 'Nike air 1','DM01','This is a description','1','350000'),
('SP012', 'Nike air 1','DM01','This is a description','1','350000'),
('SP013', 'Nike air 1','DM01','This is a description','1','350000'),
('SP014', 'Nike air 1','DM01','This is a description','1','350000'),
('SP015', 'Nike air 1','DM01','This is a description','1','350000'),
('SP016', 'Nike air 1','DM01','This is a description','1','350000'),
('SP017', 'Nike air 1','DM01','This is a description','1','350000'),
('SP018', 'Nike air 1','DM01','This is a description','1','350000')


insert into TaiKhoanNguoiDung(DiaChi, Email, HoTen, SDT, TenDangNhap,MatKhau, TrangThai) values
('Ha Noi', 'NguyenA@gmail.com', N'Nguyen Van A', '098765543','NguyenA','456', '1'),
('Ha Noi', 'NguyenB@gmail.com', N'Nguyen Van B', '098765543','NguyenB','456', '1')


insert into TaiKhoanQuanTri(HoTenUser,TenDangNhap,MatKhau,LoaiTK,TrangThai) values
(N'Quách Phương Thảo', 'QuachThao','123','QuanLy','1'),
(N'Lê Thị Thanh Mỹ', 'ThanhMy','123','QuanLy','1'),
(N'Phạm Thị Thanh', 'PhamThanh','123','QuanLy','1'),
(N'Vũ Ngọc Tâm', 'NgocTam','123','QuanLy','1')

--insert into LienHe(MaLienHe, )
insert into TinTuc (TenTin,MaTK,NgayDang,NoiDung) values
(N'7 cách bảo quản giày thể thao tốt nhất',1,'2/3/2020','This is a content'),
(N'9 kỹ thuật làm đẹp cho u30',1,'2/3/2020','This is a content')



