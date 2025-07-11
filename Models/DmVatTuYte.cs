using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DM_VatTuYTe")]
public partial class DmVatTuYte
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaVatTu { get; set; } = null!;

    [StringLength(200)]
    public string TenVatTu { get; set; } = null!;

    [StringLength(100)]
    public string? QuyGach { get; set; }

    [StringLength(20)]
    public string? DonViTinh { get; set; }

    [StringLength(100)]
    public string? NhaSanXuat { get; set; }

    [StringLength(50)]
    public string? NuocSanXuat { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiVatTu { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaVatTuNavigation")]
    public virtual ICollection<ChiTietNhapKhoVatTu> ChiTietNhapKhoVatTus { get; set; } = new List<ChiTietNhapKhoVatTu>();

    [InverseProperty("MaVatTuNavigation")]
    public virtual ICollection<TonKhoVatTu> TonKhoVatTus { get; set; } = new List<TonKhoVatTu>();
}
