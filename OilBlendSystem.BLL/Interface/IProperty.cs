using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;

namespace OilBlendSystem.BLL.Interface
{
    public interface IProperty
    {
          IEnumerable<Property> GetAllPropertyList();
    }
}