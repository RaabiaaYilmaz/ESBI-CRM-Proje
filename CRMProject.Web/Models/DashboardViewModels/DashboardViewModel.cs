using CRMProject.Data.Entities;

namespace CRMProject.Web.Models.DashboardViewModels
{
    // Dashboard ekranındaki özet kartlarının verisini taşır
    public class DashboardViewModel
    {
        public int ToplamCari { get; set; }
        public int ToplamMalzeme { get; set; }
        public List<Malzeme> KritikStoklar { get; set; } = new();
        public List<Cari> SonCariler { get; set; } = new();
    }
}