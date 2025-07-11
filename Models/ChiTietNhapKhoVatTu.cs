using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[PrimaryKey("MaPhieuNhap", "MaVatTu", "SoLo")]
[Table("ChiTietNhapKhoVatTu")]
public partial class ChiTietNhapKhoVatTu
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaPhieuNhap { get; set; } = null!;

    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaVatTu { get; set; } = null!;

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
    [InverseProperty("ChiTietNhapKhoVatTus")]
    public virtual PhieuNhapKho MaPhieuNhapNavigation { get; set; } = null!;

    [ForeignKey("MaVatTu")]
    [InverseProperty("ChiTietNhapKhoVatTus")]
    public virtual DmVatTuYte MaVatTuNavigation { get; set; } = null!;
}
