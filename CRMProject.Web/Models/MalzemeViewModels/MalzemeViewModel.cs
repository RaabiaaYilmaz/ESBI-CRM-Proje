using System.ComponentModel.DataAnnotations;

namespace CRMProject.Web.Models.MalzemeViewModels
{
    // Malzeme ekleme/düzenleme formunun bağlandığı model — Entity doğrudan View'a gönderilmiyor
    public class MalzemeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Malzeme kodu zorunludur.")]
        [StringLength(30)]
        [Display(Name = "Malzeme Kodu")]
        public string MalzemeKodu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Malzeme adı zorunludur.")]
        [StringLength(200)]
        [Display(Name = "Malzeme Adı")]
        public string MalzemeAdi { get; set; } = string.Empty;

        public string? Kategori { get; set; }

        [Required(ErrorMessage = "Birim zorunludur.")]
        public string Birim { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Satış fiyatı negatif olamaz.")]
        [Display(Name = "Satış Fiyatı")]
        public decimal SatisFiyati { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Alış fiyatı negatif olamaz.")]
        [Display(Name = "Alış Fiyatı")]
        public decimal AlisFiyati { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Stok miktarı negatif olamaz.")]
        [Display(Name = "Stok Miktarı")]
        public decimal StokMiktari { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Kritik stok negatif olamaz.")]
        [Display(Name = "Kritik Stok Seviyesi")]
        public decimal KritikStok { get; set; }

        public string? Aciklama { get; set; }
    }
}