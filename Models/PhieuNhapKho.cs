using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("PhieuNhapKho")]
public partial class PhieuNhapKho
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaPhieuNhap { get; set; } = null!;

    public DateOnly NgayNhap { get; set; }

    [Column("MaNCC")]
    [StringLength(10)]
    [Unicode(false)]
    public string? MaNcc { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiPhieu { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SoHoaDon { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? TongTien { get; set; }

    [StringLength(500)]
    public string? GhiChu { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? NguoiNhap { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaPhieuNhapNavigation")]
    public virtual ICollection<ChiTietNhapKhoThuoc> ChiTietNhapKhoThuocs { get; set; } = new List<ChiTietNhapKhoThuoc>();

    [InverseProperty("MaPhieuNhapNavigation")]
    public virtual ICollection<ChiTietNhapKhoVatTu> ChiTietNhapKhoVatTus { get; set; } = new List<ChiTietNhapKhoVatTu>();

    [ForeignKey("MaNcc")]
    [InverseProperty("PhieuNhapKhos")]
    public virtual DmNhaCungCap? MaNccNavigation { get; set; }

    [ForeignKey("NguoiNhap")]
    [InverseProperty("PhieuNhapKhos")]
    public virtual DmNhanVien? NguoiNhapNavigation { get; set; }
}
