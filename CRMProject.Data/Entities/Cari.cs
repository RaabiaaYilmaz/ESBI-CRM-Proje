namespace CRMProject.Data.Entities
{
    public class Cari
    {
        public int Id { get; set; }
        public string CariKodu { get; set; } = string.Empty;
        public string Unvan { get; set; } = string.Empty;
        public string CariTipi { get; set; } = string.Empty; // Müşteri | Tedarikçi | Her İkisi

        public string? VergiDairesi { get; set; }
        public string? VergiNo { get; set; }
        public string? Telefon { get; set; }
        public string? Eposta { get; set; }
        public string? Adres { get; set; }
        public string? Il { get; set; }
        public string? Ilce { get; set; }

        public bool AktifMi { get; set; } = true;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}
