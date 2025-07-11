using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DM_Thuoc")]
public partial class DmThuoc
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaThuoc { get; set; } = null!;

    [StringLength(200)]
    public string TenThuoc { get; set; } = null!;

    [StringLength(200)]
    public string? TenHoatChat { get; set; }

    [StringLength(50)]
    public string? DangBaoChe { get; set; }

    [StringLength(50)]
    public string? HamLuong { get; set; }

    [StringLength(20)]
    public string? DonViTinh { get; set; }

    [StringLength(100)]
    public string? NhaSanXuat { get; set; }

    [StringLength(50)]
    public string? NuocSanXuat { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? SoDangKy { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiThuoc { get; set; }

    [StringLength(100)]
    public string? DuongDung { get; set; }

    [StringLength(500)]
    public string? ChiDinh { get; set; }

    [StringLength(500)]
    public string? ChongChiDinh { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaThuocNavigation")]
    public virtual ICollection<ChiTietDonThuoc> ChiTietDonThuocs { get; set; } = new List<ChiTietDonThuoc>();

    [InverseProperty("MaThuocNavigation")]
    public virtual ICollection<ChiTietNhapKhoThuoc> ChiTietNhapKhoThuocs { get; set; } = new List<ChiTietNhapKhoThuoc>();

    [InverseProperty("MaThuocNavigation")]
    public virtual ICollection<TonKhoThuoc> TonKhoThuocs { get; set; } = new List<TonKhoThuoc>();
}
