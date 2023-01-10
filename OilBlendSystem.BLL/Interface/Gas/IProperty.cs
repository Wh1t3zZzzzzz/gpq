using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;

namespace OilBlendSystem.BLL.Interface.Gas
{
    public interface IProperty
    {
          IEnumerable<Property_gas> GetAllPropertyList();
    }
}