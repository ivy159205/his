using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[PrimaryKey("MaPhieuNhap", "MaThuoc", "SoLo")]
[Table("ChiTietNhapKhoThuoc")]
public partial class ChiTietNhapKhoThuoc
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaPhieuNhap { get; set; } = null!;

    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaThuoc { get; set; } = null!;

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string SoLo { get; set; } = null!;

    public DateOnly? HanSuDung { get; set; }

    public int SoLuongNhap { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? DonGiaNhap { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? ThanhTien { get; set; }

    [ForeignKey("MaPhieuNhap")]
    [InverseProperty("ChiTietNhapKhoThuocs")]
    public virtual PhieuNhapKho MaPhieuNhapNavigation { get; set; } = null!;

    [ForeignKey("MaThuoc")]
    [InverseProperty("ChiTietNhapKhoThuocs")]
    public virtual DmThuoc MaThuocNavigation { get; set; } = null!;
}
