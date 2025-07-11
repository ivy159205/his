using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DM_KhoaPhong")]
public partial class DmKhoaPhong
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaKhoa { get; set; } = null!;

    [StringLength(100)]
    public string TenKhoa { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiKhoa { get; set; }

    [StringLength(255)]
    public string? DiaChi { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string? DienThoai { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TruongKhoa { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaKhoaNavigation")]
    public virtual ICollection<DangKyKham> DangKyKhams { get; set; } = new List<DangKyKham>();

    [InverseProperty("MaKhoaNavigation")]
    public virtual ICollection<DmDichVu> DmDichVus { get; set; } = new List<DmDichVu>();

    [InverseProperty("MaKhoaNavigation")]
    public virtual ICollection<DmNhanVien> DmNhanViens { get; set; } = new List<DmNhanVien>();

    [InverseProperty("MaKhoaNavigation")]
    public virtual ICollection<NhapVien> NhapViens { get; set; } = new List<NhapVien>();

    [InverseProperty("MaKhoaNavigation")]
    public virtual ICollection<PhieuXuatKho> PhieuXuatKhos { get; set; } = new List<PhieuXuatKho>();
}
