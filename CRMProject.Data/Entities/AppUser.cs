using Microsoft.AspNetCore.Identity;

namespace CRMProject.Data.Entities
{
    // Uygulama kullanıcısı — Identity'nin varsayılan IdentityUser sınıfını genişletiyoruz
    public class AppUser : IdentityUser
    {
        public string AdSoyad { get; set; } = string.Empty;
        public bool Aktif { get; set; } = true;
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
}