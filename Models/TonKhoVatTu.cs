using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[PrimaryKey("MaVatTu", "SoLo")]
[Table("TonKhoVatTu")]
[Index("HanSuDung", Name = "IX_TonKhoVatTu_HanSuDung")]
public partial class TonKhoVatTu
{
    [Key]
    [StringLength(15)]
    [Unicode(false)]
    public string MaVatTu { get; set; } = null!;

    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string SoLo { get; set; } = null!;

    public DateOnly? HanSuDung { get; set; }

    public int? SoLuongTon { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? DonGiaNhap { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? DonGiaXuat { get; set; }

    [StringLength(50)]
    public string? ViTriLuuTru { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayCapNhat { get; set; }

    [ForeignKey("MaVatTu")]
    [InverseProperty("TonKhoVatTus")]
    public virtual DmVatTuYte MaVatTuNavigation { get; set; } = null!;
}
