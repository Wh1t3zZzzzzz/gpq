using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilBlendSystem.BLL.Implementation.Gas
{
    public partial class ProdOilConfig : IProdOilConfig
    {
        private readonly oilblendContext context;//带问号是可以为空

        public ProdOilConfig(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }
        public IEnumerable<Prodoilconfig_gas> GetAllProdOilConfigList()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Prodoilconfig_gases.ToList();
        }

        // public void UpdateCompOilConfig(CompOilConfig CompOilConfigList)
        // {
        //     //CompOilConfigList.Update(CompOilConfigList);
        // }
    }
}