namespace CRMProject.Data.Entities
{
    public class Malzeme
    {
        public int Id { get; set; }
        public string MalzemeKodu { get; set; } = string.Empty;
        public string MalzemeAdi { get; set; } = string.Empty;
        public string? Kategori { get; set; }
        public string Birim { get; set; } = string.Empty; // Adet, Kg, Lt, Mt...

        public decimal SatisFiyati { get; set; }
        public decimal AlisFiyati { get; set; }
        public decimal StokMiktari { get; set; }
        public decimal KritikStok { get; set; }
        public string? Aciklama { get; set; }

        public bool AktifMi { get; set; } = true;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
        public DateTime? GuncellemeTarihi { get; set; }
    }
}