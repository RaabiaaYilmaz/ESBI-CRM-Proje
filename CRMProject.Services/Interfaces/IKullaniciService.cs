using CRMProject.Services.Models;

namespace CRMProject.Services.Interfaces
{
    public interface IKullaniciService
    {
        Task<List<KullaniciListeItem>> ListeAsync();
        Task<(bool basarili, string hata)> EkleAsync(string eposta, string adSoyad, string sifre, string rol);
        Task RolDegistirAsync(string kullaniciId, string yeniRol);
        Task AktifPasifYapAsync(string kullaniciId, bool aktif);
    }
}