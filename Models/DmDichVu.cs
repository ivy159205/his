using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

[Table("DM_DichVu")]
public partial class DmDichVu
{
    [Key]
    [StringLength(10)]
    [Unicode(false)]
    public string MaDichVu { get; set; } = null!;

    [StringLength(200)]
    public string TenDichVu { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? LoaiDichVu { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal DonGia { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? MaKhoa { get; set; }

    public bool? TrangThai { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaDichVuNavigation")]
    public virtual ICollection<ChiDinhXetNghiem> ChiDinhXetNghiems { get; set; } = new List<ChiDinhXetNghiem>();

    [InverseProperty("MaDichVuNavigation")]
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    [ForeignKey("MaKhoa")]
    [InverseProperty("DmDichVus")]
    public virtual DmKhoaPhong? MaKhoaNavigation { get; set; }
}
