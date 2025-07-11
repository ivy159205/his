using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[PrimaryKey("MaDonThuoc", "MaThuoc")]
[Table("ChiTietDonThuoc")]
public partial class ChiTietDonThuoc
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaDonThuoc { get; set; } = null!;

    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaThuoc { get; set; } = null!;

    public int SoLuong { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? DonGia { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? ThanhTien { get; set; }

    [StringLength(500)]
    public string? CachDung { get; set; }

    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("MaDonThuoc")]
    [InverseProperty("ChiTietDonThuocs")]
    public virtual DonThuoc MaDonThuocNavigation { get; set; } = null!;

    [ForeignKey("MaThuoc")]
    [InverseProperty("ChiTietDonThuocs")]
    public virtual DmThuoc MaThuocNavigation { get; set; } = null!;
}
