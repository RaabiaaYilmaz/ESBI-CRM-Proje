using CRMProject.Data.Context;
using CRMProject.Data.Entities;
using CRMProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

using CRMProject.Data.Context;
using CRMProject.Data.Entities;
using CRMProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Services.Implementations
{
    // ICariService'in veritabanı üzerinden çalışan gerçeklemesi
    public class CariService : ICariService
    {
        private readonly AppDbContext _context;

        public CariService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> ToplamSayiAsync()
        {
            return await _context.Cariler.CountAsync(c => c.AktifMi);
        }

        public async Task<List<Cari>> SonEklenenlerAsync(int adet)
        {
            return await _context.Cariler
                .Where(c => c.AktifMi)
                .OrderByDescending(c => c.OlusturmaTarihi)
                .Take(adet)
                .ToListAsync();
        }

        public async Task<List<Cari>> ListeAsync(string aramaMetni)
        {
            var sorgu = _context.Cariler.Where(c => c.AktifMi);

            if (!string.IsNullOrWhiteSpace(aramaMetni))
            {
                sorgu = sorgu.Where(c =>
                    c.Unvan.Contains(aramaMetni) ||
                    c.CariKodu.Contains(aramaMetni));
            }

            return await sorgu.OrderBy(c => c.Unvan).ToListAsync();
        }

        public async Task<Cari?> GetirAsync(int id)
        {
            return await _context.Cariler.FirstOrDefaultAsync(c => c.Id == id && c.AktifMi);
        }

        public async Task<bool> KodMevcutMuAsync(string cariKodu, int? haricTutulanId = null)
        {
            return await _context.Cariler.AnyAsync(c =>
                c.AktifMi && c.CariKodu == cariKodu && c.Id != haricTutulanId);
        }

        public async Task EkleAsync(Cari cari)
        {
            cari.OlusturmaTarihi = DateTime.Now;
            cari.AktifMi = true;
            _context.Cariler.Add(cari);
            await _context.SaveChangesAsync();
        }

        public async Task GuncelleAsync(int id, Cari guncelCari)
        {
            var mevcut = await _context.Cariler.FirstOrDefaultAsync(c => c.Id == id);
            if (mevcut == null) return;

            mevcut.Unvan = guncelCari.Unvan;
            mevcut.CariTipi = guncelCari.CariTipi;
            mevcut.VergiDairesi = guncelCari.VergiDairesi;
            mevcut.VergiNo = guncelCari.VergiNo;
            mevcut.Telefon = guncelCari.Telefon;
            mevcut.Eposta = guncelCari.Eposta;
            mevcut.Adres = guncelCari.Adres;
            mevcut.Il = guncelCari.Il;
            mevcut.Ilce = guncelCari.Ilce;
            mevcut.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task PasifYapAsync(int id)
        {
            var cari = await _context.Cariler.FirstOrDefaultAsync(c => c.Id == id);
            if (cari == null) return;

            cari.AktifMi = false;
            cari.GuncellemeTarihi = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}