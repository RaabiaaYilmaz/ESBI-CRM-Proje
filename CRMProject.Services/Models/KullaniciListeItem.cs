namespace CRMProject.Services.Models
{
    // Kullanıcı Yönetimi ekranında listelenecek özet bilgi — Entity değil, sadece görüntüleme amaçlı
    public class KullaniciListeItem
    {
        public string Id { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string Eposta { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public bool Aktif { get; set; }
    }
}