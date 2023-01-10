using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;

namespace OilBlendSystem.BLL.Interface.Gas
{
    public interface IProdOilConfig
    {
        IEnumerable<Prodoilconfig_gas> GetAllProdOilConfigList();
    }
}
