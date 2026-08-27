using System.ComponentModel.DataAnnotations;

namespace CRMProject.Web.Models.CariViewModels
{
    // Cari ekleme/düzenleme formunun bağlandığı model — Entity doğrudan View'a gönderilmiyor
    public class CariViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cari kodu zorunludur.")]
        [StringLength(20)]
        [Display(Name = "Cari Kodu")]
        public string CariKodu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ünvan zorunludur.")]
        [StringLength(200)]
        public string Unvan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cari tipi zorunludur.")]
        [Display(Name = "Cari Tipi")]
        public string CariTipi { get; set; } = string.Empty;

        [Display(Name = "Vergi Dairesi")]
        public string? VergiDairesi { get; set; }

        [Display(Name = "Vergi No")]
        public string? VergiNo { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string? Telefon { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string? Eposta { get; set; }

        public string? Adres { get; set; }

        [Display(Name = "İl")]
        public string? Il { get; set; }

        [Display(Name = "İlçe")]
        public string? Ilce { get; set; }
    }
}