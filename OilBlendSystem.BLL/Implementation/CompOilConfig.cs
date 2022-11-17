using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.BLL.Interface;
using Microsoft.EntityFrameworkCore;
namespace OilBlendSystem.BLL.Implementation
{
    public partial class CompOilConfig : ICompOilConfig
    {
        private readonly oilblendContext context;//带问号是可以为空

        public CompOilConfig(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }

        public IEnumerable<Compoilconfig> GetAllCompOilConfigList()
        {
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return context.Compoilconfigs.ToList();
        }

        // public void UpdateCompOilConfig(CompOilConfig CompOilConfigList)
        // {
        //     //CompOilConfigList.Update(CompOilConfigList);
        // }
    }
}