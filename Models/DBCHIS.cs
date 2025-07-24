using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class DBCHIS : DbContext
{
    public DBCHIS()
    {
    }

    public DBCHIS(DbContextOptions<DBCHIS> options)
        : base(options)
    {
    }

    public virtual DbSet<BenhAnNgoaiTru> BenhAnNgoaiTrus { get; set; }

    public virtual DbSet<BenhAnNoiTru> BenhAnNoiTrus { get; set; }

    public virtual DbSet<BenhNhan> BenhNhans { get; set; }

    public virtual DbSet<ChiDinhXetNghiem> ChiDinhXetNghiems { get; set; }

    public virtual DbSet<ChiTietDonThuoc> ChiTietDonThuocs { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietNhapKhoThuoc> ChiTietNhapKhoThuocs { get; set; }

    public virtual DbSet<ChiTietNhapKhoVatTu> ChiTietNhapKhoVatTus { get; set; }

    public virtual DbSet<DangKyKham> DangKyKhams { get; set; }

    public virtual DbSet<DmDichVu> DmDichVus { get; set; }

    public virtual DbSet<DmDoiTuongBhyt> DmDoiTuongBhyts { get; set; }

    public virtual DbSet<DmKhoaPhong> DmKhoaPhongs { get; set; }

    public virtual DbSet<DmNhaCungCap> DmNhaCungCaps { get; set; }

    public virtual DbSet<DmNhanVien> DmNhanViens { get; set; }

    public virtual DbSet<DmThuoc> DmThuocs { get; set; }

    public virtual DbSet<DmVatTuYte> DmVatTuYtes { get; set; }

    public virtual DbSet<DonThuoc> DonThuocs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<KetQuaXetNghiem> KetQuaXetNghiems { get; set; }

    public virtual DbSet<LogHoatDong> LogHoatDongs { get; set; }

    public virtual DbSet<NhapVien> NhapViens { get; set; }

    public virtual DbSet<PhieuNhapKho> PhieuNhapKhos { get; set; }

    public virtual DbSet<PhieuXuatKho> PhieuXuatKhos { get; set; }

    public virtual DbSet<TonKhoThuoc> TonKhoThuocs { get; set; }

    public virtual DbSet<TonKhoVatTu> TonKhoVatTus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=host.docker.internal,1437;Initial Catalog=HIS_Database;Persist Security Info=True;User ID=sa;Password=Test!@#1234;encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BenhAnNgoaiTru>(entity =>
        {
            entity.HasKey(e => e.MaBa).HasName("PK__BenhAnNg__272475B8A5D1CAE4");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("DANG_XU_LY");

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.BenhAnNgoaiTrus).HasConstraintName("FK__BenhAnNgo__MaBac__6477ECF3");

            entity.HasOne(d => d.MaBnNavigation).WithMany(p => p.BenhAnNgoaiTrus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhAnNgoa__MaBN__6383C8BA");

            entity.HasOne(d => d.MaDangKyNavigation).WithMany(p => p.BenhAnNgoaiTrus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhAnNgo__MaDan__628FA481");
        });

        modelBuilder.Entity<BenhAnNoiTru>(entity =>
        {
            entity.HasKey(e => e.MaBanoiTru).HasName("PK__BenhAnNo__324B6E62C3D40B36");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("DANG_DIEU_TRI");

            entity.HasOne(d => d.MaBnNavigation).WithMany(p => p.BenhAnNoiTrus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhAnNoiT__MaBN__70DDC3D8");

            entity.HasOne(d => d.MaNhapVienNavigation).WithMany(p => p.BenhAnNoiTrus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BenhAnNoi__MaNha__6FE99F9F");
        });

        modelBuilder.Entity<BenhNhan>(entity =>
        {
            entity.HasKey(e => e.MaBn).HasName("PK__BenhNhan__272475AD6C9BBF19");

            entity.ToTable("BenhNhan", tb => tb.HasTrigger("TR_AutoMaBN"));

            entity.Property(e => e.GioiTinh).IsFixedLength();
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaDoiTuongBhytNavigation).WithMany(p => p.BenhNhans).HasConstraintName("FK__BenhNhan__MaDoiT__571DF1D5");
        });

        modelBuilder.Entity<ChiDinhXetNghiem>(entity =>
        {
            entity.HasKey(e => e.MaChiDinh).HasName("PK__ChiDinhX__A125E7908D8308B3");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("CHO_THUC_HIEN");

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.ChiDinhXetNghiems).HasConstraintName("FK__ChiDinhXe__MaBac__29221CFB");

            entity.HasOne(d => d.MaBnNavigation).WithMany(p => p.ChiDinhXetNghiems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiDinhXet__MaBN__282DF8C2");

            entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.ChiDinhXetNghiems).HasConstraintName("FK__ChiDinhXe__MaDic__2A164134");
        });

        modelBuilder.Entity<ChiTietDonThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaDonThuoc, e.MaThuoc }).HasName("PK__ChiTietD__2A4281830B598D40");

            entity.HasOne(d => d.MaDonThuocNavigation).WithMany(p => p.ChiTietDonThuocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__MaDon__17F790F9");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietDonThuocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__MaThu__18EBB532");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDon, e.MaDichVu }).HasName("PK__ChiTietH__6F50BCD37A52869E");

            entity.Property(e => e.SoLuong).HasDefaultValue(1);

            entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.ChiTietHoaDons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaDic__236943A5");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaHoa__22751F6C");
        });

        modelBuilder.Entity<ChiTietNhapKhoThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuNhap, e.MaThuoc, e.SoLo }).HasName("PK__ChiTietN__EC77CC927345B098");

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.ChiTietNhapKhoThuocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietNh__MaPhi__04E4BC85");

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.ChiTietNhapKhoThuocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietNh__MaThu__05D8E0BE");
        });

        modelBuilder.Entity<ChiTietNhapKhoVatTu>(entity =>
        {
            entity.HasKey(e => new { e.MaPhieuNhap, e.MaVatTu, e.SoLo }).HasName("PK__ChiTietN__4871F44685B565BF");

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.ChiTietNhapKhoVatTus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietNh__MaPhi__08B54D69");

            entity.HasOne(d => d.MaVatTuNavigation).WithMany(p => p.ChiTietNhapKhoVatTus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietNh__MaVat__09A971A2");
        });

        modelBuilder.Entity<DangKyKham>(entity =>
        {
            entity.HasKey(e => e.MaDangKy).HasName("PK__DangKyKh__BA90F02D9DF9D69E");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("CHO_KHAM");

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.DangKyKhams).HasConstraintName("FK__DangKyKha__MaBac__5DCAEF64");

            entity.HasOne(d => d.MaBnNavigation).WithMany(p => p.DangKyKhams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DangKyKham__MaBN__5BE2A6F2");

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.DangKyKhams).HasConstraintName("FK__DangKyKha__MaKho__5CD6CB2B");
        });

        modelBuilder.Entity<DmDichVu>(entity =>
        {
            entity.HasKey(e => e.MaDichVu).HasName("PK__DM_DichV__C0E6DE8FE84D0669");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.DmDichVus).HasConstraintName("FK__DM_DichVu__MaKho__45F365D3");
        });

        modelBuilder.Entity<DmDoiTuongBhyt>(entity =>
        {
            entity.HasKey(e => e.MaDoiTuong).HasName("PK__DM_DoiTu__291408A1DB6028D6");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<DmKhoaPhong>(entity =>
        {
            entity.HasKey(e => e.MaKhoa).HasName("PK__DM_KhoaP__65390405D197C428");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<DmNhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__DM_NhaCu__3A185DEB1C942BC0");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<DmNhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__DM_NhanV__2725D70A8D3BDF5C");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.DmNhanViens).HasConstraintName("FK__DM_NhanVi__MaKho__412EB0B6");
        });

        modelBuilder.Entity<DmThuoc>(entity =>
        {
            entity.HasKey(e => e.MaThuoc).HasName("PK__DM_Thuoc__4BB1F620BAF379FC");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<DmVatTuYte>(entity =>
        {
            entity.HasKey(e => e.MaVatTu).HasName("PK__DM_VatTu__0BD27B6A92308C97");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<DonThuoc>(entity =>
        {
            entity.HasKey(e => e.MaDonThuoc).HasName("PK__DonThuoc__3EF99EE19A70B2BA");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("CHO_CAP_PHAT");

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.DonThuocs).HasConstraintName("FK__DonThuoc__MaBacS__151B244E");

            entity.HasOne(d => d.MaBnNavigation).WithMany(p => p.DonThuocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonThuoc__MaBN__14270015");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13BAB9F2E6C");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("CHUA_THANH_TOAN");

            entity.HasOne(d => d.MaBnNavigation).WithMany(p => p.HoaDons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaBN__1DB06A4F");

            entity.HasOne(d => d.NguoiThuNavigation).WithMany(p => p.HoaDons).HasConstraintName("FK__HoaDon__NguoiThu__1EA48E88");
        });

        modelBuilder.Entity<KetQuaXetNghiem>(entity =>
        {
            entity.HasKey(e => e.MaKetQua).HasName("PK__KetQuaXe__D5B3102A22FD490F");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("CHO_DUYET");

            entity.HasOne(d => d.MaChiDinhNavigation).WithMany(p => p.KetQuaXetNghiems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KetQuaXet__MaChi__2EDAF651");

            entity.HasOne(d => d.NguoiThucHienNavigation).WithMany(p => p.KetQuaXetNghiems).HasConstraintName("FK__KetQuaXet__Nguoi__2FCF1A8A");
        });

        modelBuilder.Entity<LogHoatDong>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LogHoatD__3214EC2780ADE16D");

            entity.Property(e => e.NgayGio).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.LogHoatDongs).HasConstraintName("FK__LogHoatDon__MaNV__339FAB6E");
        });

        modelBuilder.Entity<NhapVien>(entity =>
        {
            entity.HasKey(e => e.MaNhapVien).HasName("PK__NhapVien__D3641966851C6BAB");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("DANG_DIEU_TRI");

            entity.HasOne(d => d.MaBacSiNavigation).WithMany(p => p.NhapViens).HasConstraintName("FK__NhapVien__MaBacS__6B24EA82");

            entity.HasOne(d => d.MaBnNavigation).WithMany(p => p.NhapViens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NhapVien__MaBN__693CA210");

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.NhapViens).HasConstraintName("FK__NhapVien__MaKhoa__6A30C649");
        });

        modelBuilder.Entity<PhieuNhapKho>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhap).HasName("PK__PhieuNha__1470EF3B3BB8B1BE");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("NHAP_KHO");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.PhieuNhapKhos).HasConstraintName("FK__PhieuNhap__MaNCC__01142BA1");

            entity.HasOne(d => d.NguoiNhapNavigation).WithMany(p => p.PhieuNhapKhos).HasConstraintName("FK__PhieuNhap__Nguoi__02084FDA");
        });

        modelBuilder.Entity<PhieuXuatKho>(entity =>
        {
            entity.HasKey(e => e.MaPhieuXuat).HasName("PK__PhieuXua__26C4B5A27F7A6FC7");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("XUAT_KHO");

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.PhieuXuatKhos).HasConstraintName("FK__PhieuXuat__MaKho__0E6E26BF");

            entity.HasOne(d => d.NguoiXuatNavigation).WithMany(p => p.PhieuXuatKhos).HasConstraintName("FK__PhieuXuat__Nguoi__0F624AF8");
        });

        modelBuilder.Entity<TonKhoThuoc>(entity =>
        {
            entity.HasKey(e => new { e.MaThuoc, e.SoLo }).HasName("PK__TonKhoTh__80723A9E0210510F");

            entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaThuocNavigation).WithMany(p => p.TonKhoThuocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TonKhoThu__MaThu__76969D2E");
        });

        modelBuilder.Entity<TonKhoVatTu>(entity =>
        {
            entity.HasKey(e => new { e.MaVatTu, e.SoLo }).HasName("PK__TonKhoVa__C011B7D44F99B707");

            entity.Property(e => e.NgayCapNhat).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoLuongTon).HasDefaultValue(0);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaVatTuNavigation).WithMany(p => p.TonKhoVatTus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TonKhoVat__MaVat__7C4F7684");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
