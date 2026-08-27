using CRMProject.Data.Context;
using CRMProject.Data.Entities;
using CRMProject.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Services.Implementations
{
    public class MalzemeService : IMalzemeService
    {
        private readonly AppDbContext _context;

        public MalzemeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> ToplamSayiAsync()
        {
            return await _context.Malzemeler.CountAsync(m => m.AktifMi);
        }

        public async Task<List<Malzeme>> KritikStoklarAsync()
        {
            return await _context.Malzemeler
                .Where(m => m.AktifMi && m.StokMiktari <= m.KritikStok)
                .OrderBy(m => m.StokMiktari)
                .ToListAsync();
        }

        public async Task<List<Malzeme>> ListeAsync(string aramaMetni, string kategori)
        {
            var sorgu = _context.Malzemeler.Where(m => m.AktifMi);

            if (!string.IsNullOrWhiteSpace(aramaMetni))
            {
                sorgu = sorgu.Where(m =>
                    m.MalzemeAdi.Contains(aramaMetni) ||
                    m.MalzemeKodu.Contains(aramaMetni));
            }

            if (!string.IsNullOrWhiteSpace(kategori))
            {
                sorgu = sorgu.Where(m => m.Kategori == kategori);
            }

            return await sorgu.OrderBy(m => m.MalzemeAdi).ToListAsync();
        }

        public async Task<List<string>> KategoriListesiAsync()
        {
            return await _context.Malzemeler
                .Where(m => m.AktifMi && m.Kategori != null)
                .Select(m => m.Kategori!)
                .Distinct()
                .OrderBy(k => k)
                .ToListAsync();
        }

        public async Task<Malzeme?> GetirAsync(int id)
        {
            return await _context.Malzemeler.FirstOrDefaultAsync(m => m.Id == id && m.AktifMi);
        }

        public async Task<bool> KodMevcutMuAsync(string malzemeKodu, int? haricTutulanId = null)
        {
            return await _context.Malzemeler.AnyAsync(m =>
                m.AktifMi && m.MalzemeKodu == malzemeKodu && m.Id != haricTutulanId);
        }

        public async Task EkleAsync(Malzeme malzeme)
        {
            malzeme.OlusturmaTarihi = DateTime.Now;
            malzeme.AktifMi = true;
            _context.Malzemeler.Add(malzeme);
            await _context.SaveChangesAsync();
        }

        public async Task GuncelleAsync(int id, Malzeme guncelMalzeme)
        {
            var mevcut = await _context.Malzemeler.FirstOrDefaultAsync(m => m.Id == id);
            if (mevcut == null) return;

            mevcut.MalzemeAdi = guncelMalzeme.MalzemeAdi;
            mevcut.Kategori = guncelMalzeme.Kategori;
            mevcut.Birim = guncelMalzeme.Birim;
            mevcut.SatisFiyati = guncelMalzeme.SatisFiyati;
            mevcut.AlisFiyati = guncelMalzeme.AlisFiyati;
            mevcut.StokMiktari = guncelMalzeme.StokMiktari;
            mevcut.KritikStok = guncelMalzeme.KritikStok;
            mevcut.Aciklama = guncelMalzeme.Aciklama;
            mevcut.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task PasifYapAsync(int id)
        {
            var malzeme = await _context.Malzemeler.FirstOrDefaultAsync(m => m.Id == id);
            if (malzeme == null) return;

            malzeme.AktifMi = false;
            malzeme.GuncellemeTarihi = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}