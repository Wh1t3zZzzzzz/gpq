using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Diesel;

namespace OilBlendSystem.BLL.Implementation.Diesel
{
    public partial class ProdOilConfig : IProdOilConfig
    {
        private readonly oilblendContext context;//带问号是可以为空

        public ProdOilConfig(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }
        public IEnumerable<Prodoilconfig> GetAllProdOilConfigList()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Prodoilconfigs.ToList();
        }

        // public void UpdateCompOilConfig(CompOilConfig CompOilConfigList)
        // {
        //     //CompOilConfigList.Update(CompOilConfigList);
        // }
    }
}