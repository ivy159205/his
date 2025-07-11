using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DonThuoc")]
public partial class DonThuoc
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaDonThuoc { get; set; } = null!;

    [Column("MaBA")]
    [StringLength(15)]
    [Unicode(false)]
    public string? MaBa { get; set; }

    [Column("MaBN")]
    [StringLength(15)]
    [Unicode(false)]
    public string MaBn { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayKeDon { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaBacSi { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiDon { get; set; }

    [StringLength(500)]
    public string? ChanDoan { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? TongTien { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaDonThuocNavigation")]
    public virtual ICollection<ChiTietDonThuoc> ChiTietDonThuocs { get; set; } = new List<ChiTietDonThuoc>();

    [ForeignKey("MaBacSi")]
    [InverseProperty("DonThuocs")]
    public virtual DmNhanVien? MaBacSiNavigation { get; set; }

    [ForeignKey("MaBn")]
    [InverseProperty("DonThuocs")]
    public virtual BenhNhan MaBnNavigation { get; set; } = null!;
}
