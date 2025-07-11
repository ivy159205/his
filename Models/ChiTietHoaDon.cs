using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[PrimaryKey("MaHoaDon", "MaDichVu")]
[Table("ChiTietHoaDon")]
public partial class ChiTietHoaDon
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaHoaDon { get; set; } = null!;

    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDichVu { get; set; } = null!;

    public int? SoLuong { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? DonGia { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? ThanhTien { get; set; }

    [Column("TienBHYT", TypeName = "decimal(15, 2)")]
    public decimal? TienBhyt { get; set; }

    [Column("TienBNTra", TypeName = "decimal(15, 2)")]
    public decimal? TienBntra { get; set; }

    [ForeignKey("MaDichVu")]
    [InverseProperty("ChiTietHoaDons")]
    public virtual DmDichVu MaDichVuNavigation { get; set; } = null!;

    [ForeignKey("MaHoaDon")]
    [InverseProperty("ChiTietHoaDons")]
    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;
}
