using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("BenhAnNgoaiTru")]
[Index("NgayKham", Name = "IX_BenhAnNgoaiTru_NgayKham")]
public partial class BenhAnNgoaiTru
{
    [Key]
    [Column("MaBA")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBa { get; set; } = null!;

    [StringLength(15)]
    [Unicode(false)]
    public string MaDangKy { get; set; } = null!;

    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayKham { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaBacSi { get; set; }

    [StringLength(500)]
    public string? LyDoKham { get; set; }

    [StringLength(1000)]
    public string? TienSuBenh { get; set; }

    [StringLength(1000)]
    public string? BenhSu { get; set; }

    public int? Mach { get; set; }

    [Column(TypeName = "decimal(4, 1)")]
    public decimal? NhietDo { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? HuyetAp { get; set; }

    public int? NhipTho { get; set; }

    [Column(TypeName = "decimal(5, 1)")]
    public decimal? CanNang { get; set; }

    [Column(TypeName = "decimal(5, 1)")]
    public decimal? ChieuCao { get; set; }

    [StringLength(1000)]
    public string? ToanThan { get; set; }

    [StringLength(1000)]
    public string? CacBoPhan { get; set; }

    [StringLength(500)]
    public string? ChanDoanSoBo { get; set; }

    [Column("ChanDoanChinhICD")]
    [StringLength(10)]
    [Unicode(false)]
    public string? ChanDoanChinhIcd { get; set; }

    [StringLength(500)]
    public string? ChanDoanChinh { get; set; }

    [StringLength(500)]
    public string? ChanDoanPhu { get; set; }

    [StringLength(1000)]
    public string? HuongDieuTri { get; set; }

    [StringLength(1000)]
    public string? LoiDan { get; set; }

    public DateOnly? NgayTaiKham { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [ForeignKey("MaBacSi")]
    [InverseProperty("BenhAnNgoaiTrus")]
    public virtual DmNhanVien? MaBacSiNavigation { get; set; }

    [ForeignKey("MaBn")]
    [InverseProperty("BenhAnNgoaiTrus")]
    public virtual BenhNhan MaBnNavigation { get; set; } = null!;

    [ForeignKey("MaDangKy")]
    [InverseProperty("BenhAnNgoaiTrus")]
    public virtual DangKyKham MaDangKyNavigation { get; set; } = null!;
}
