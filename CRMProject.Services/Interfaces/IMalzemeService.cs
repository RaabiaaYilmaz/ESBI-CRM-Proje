using CRMProject.Data.Entities;

namespace CRMProject.Services.Interfaces
{
    public interface IMalzemeService
    {
        Task<int> ToplamSayiAsync();
        Task<List<Malzeme>> KritikStoklarAsync();

        Task<List<Malzeme>> ListeAsync(string aramaMetni, string kategori);
        Task<List<string>> KategoriListesiAsync();
        Task<Malzeme?> GetirAsync(int id);
        Task<bool> KodMevcutMuAsync(string malzemeKodu, int? haricTutulanId = null);
        Task EkleAsync(Malzeme malzeme);
        Task GuncelleAsync(int id, Malzeme guncelMalzeme);
        Task PasifYapAsync(int id);
    }
}