using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;

namespace OilBlendSystem.BLL.Interface.Diesel
{
    public interface IMenuList
    {
        List<TreeView> GetTreeViewMenuList();
    }
}
