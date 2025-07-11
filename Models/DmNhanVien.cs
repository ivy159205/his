using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DM_NhanVien")]
public partial class DmNhanVien
{
    [Key]
    [Column("MaNV")]
    [StringLength(10)]
    [Unicode(false)]
    public string MaNv { get; set; } = null!;

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [StringLength(50)]
    public string? ChucVu { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhoa { get; set; }

    [StringLength(100)]
    public string? ChuyenKhoa { get; set; }

    [Column("SDT")]
    [StringLength(15)]
    [Unicode(false)]
    public string? Sdt { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaBacSiNavigation")]
    public virtual ICollection<BenhAnNgoaiTru> BenhAnNgoaiTrus { get; set; } = new List<BenhAnNgoaiTru>();

    [InverseProperty("MaBacSiNavigation")]
    public virtual ICollection<ChiDinhXetNghiem> ChiDinhXetNghiems { get; set; } = new List<ChiDinhXetNghiem>();

    [InverseProperty("MaBacSiNavigation")]
    public virtual ICollection<DangKyKham> DangKyKhams { get; set; } = new List<DangKyKham>();

    [InverseProperty("MaBacSiNavigation")]
    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    [InverseProperty("NguoiThuNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [InverseProperty("NguoiThucHienNavigation")]
    public virtual ICollection<KetQuaXetNghiem> KetQuaXetNghiems { get; set; } = new List<KetQuaXetNghiem>();

    [InverseProperty("MaNvNavigation")]
    public virtual ICollection<LogHoatDong> LogHoatDongs { get; set; } = new List<LogHoatDong>();

    [ForeignKey("MaKhoa")]
    [InverseProperty("DmNhanViens")]
    public virtual DmKhoaPhong? MaKhoaNavigation { get; set; }

    [InverseProperty("MaBacSiNavigation")]
    public virtual ICollection<NhapVien> NhapViens { get; set; } = new List<NhapVien>();

    [InverseProperty("NguoiNhapNavigation")]
    public virtual ICollection<PhieuNhapKho> PhieuNhapKhos { get; set; } = new List<PhieuNhapKho>();

    [InverseProperty("NguoiXuatNavigation")]
    public virtual ICollection<PhieuXuatKho> PhieuXuatKhos { get; set; } = new List<PhieuXuatKho>();
}
