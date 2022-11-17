using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;

namespace OilBlendSystem.BLL.Interface
{
    public interface IProdOilConfig
    {
        IEnumerable<Prodoilconfig> GetAllProdOilConfigList();
    }
}
