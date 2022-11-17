using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilBlendSystem.BLL.Interface;
using Microsoft.EntityFrameworkCore;
namespace OilBlendSystem.BLL.Implementation
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