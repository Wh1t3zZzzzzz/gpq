using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;

namespace OilBlendSystem.BLL.Interface
{
    public interface IMenuList
    {
        List<TreeView> GetTreeViewMenuList();
    }
}
