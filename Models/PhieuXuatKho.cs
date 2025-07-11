using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("PhieuXuatKho")]
public partial class PhieuXuatKho
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaPhieuXuat { get; set; } = null!;

    public DateOnly NgayXuat { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiPhieu { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhoa { get; set; }

    [StringLength(500)]
    public string? LyDoXuat { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? TongTien { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? NguoiXuat { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? NguoiNhan { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [ForeignKey("MaKhoa")]
    [InverseProperty("PhieuXuatKhos")]
    public virtual DmKhoaPhong? MaKhoaNavigation { get; set; }

    [ForeignKey("NguoiXuat")]
    [InverseProperty("PhieuXuatKhos")]
    public virtual DmNhanVien? NguoiXuatNavigation { get; set; }
}
