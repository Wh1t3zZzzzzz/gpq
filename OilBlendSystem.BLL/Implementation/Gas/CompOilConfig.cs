using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilBlendSystem.BLL.Implementation.Gas
{
    public partial class CompOilConfig : ICompOilConfig
    {
        private readonly oilblendContext context;//带问号是可以为空

        public CompOilConfig(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }

        public IEnumerable<Compoilconfig_gas> GetAllCompOilConfigList()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Compoilconfig_gases.ToList();
        }

    }
}