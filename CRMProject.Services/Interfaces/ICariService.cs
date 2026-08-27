using CRMProject.Data.Entities;

namespace CRMProject.Services.Interfaces
{
    // Cari işlemlerinin sözleşmesi — Controller bu arayüze bağımlı olacak, somut sınıfa değil
    public interface ICariService
    {
        Task<int> ToplamSayiAsync();
        Task<List<Cari>> SonEklenenlerAsync(int adet);

        Task<List<Cari>> ListeAsync(string aramaMetni);
        Task<Cari?> GetirAsync(int id);
        Task<bool> KodMevcutMuAsync(string cariKodu, int? haricTutulanId = null);
        Task EkleAsync(Cari cari);
        Task GuncelleAsync(int id, Cari guncelCari);
        Task PasifYapAsync(int id);
    }
}