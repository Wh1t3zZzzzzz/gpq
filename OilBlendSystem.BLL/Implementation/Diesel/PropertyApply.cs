using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;
using OilBlendSystem.Models;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.BLL.Interface.Diesel;

namespace OilBlendSystem.BLL.Implementation.Diesel
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
        public IEnumerable<Property> GetAllPropertyList()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Properties.ToList();
        }

    }
}