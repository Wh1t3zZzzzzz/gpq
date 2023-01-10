using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilBlendSystem.BLL.Implementation.Gas
{
    public partial class PropertyApply : IProperty
    {
        private readonly oilblendContext context;//带问号是可以为空
        //oilblendContext context ;

        public PropertyApply(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }
        public IEnumerable<Property_gas> GetAllPropertyList()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Propertie_gases.ToList();
        }

    }
}