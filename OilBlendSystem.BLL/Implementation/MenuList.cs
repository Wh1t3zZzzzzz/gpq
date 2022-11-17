using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilBlendSystem.BLL.Interface;
using Microsoft.EntityFrameworkCore;

namespace OilBlendSystem.BLL.Implementation
{
    public partial class MenuList : IMenuList
    {
        private readonly oilblendContext context;//带问号是可以为空

        public MenuList(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }
        public List<TreeView> GetTreeViewMenuList()
        {
            List<TreeView> tree = new List<TreeView>();
            var ParentData = context.Menulists.Where(x => x.MenuState == "0").ToList();
            int i = 0;
            foreach (var item in ParentData)
            {
                var ChildrenData = context.Menulists.Where(m => m.ChildID == item.ID.ToString()).ToList();
                // for (var i = 0; i < ParentData.Count; i++)
                // {
                    TreeView treeview = new TreeView()
                    {
                        ID = ParentData[i].ID,
                        MenuName = ParentData[i].MenuName,
                        Icon = ParentData[i].Icon,
                        Path = ParentData[i].Path,
                        Component = ParentData[i].Component,
                        ChildID = ParentData[i].ChildID,
                        ParentID = ParentData[i].ParentID,
                        MenuState = ParentData[i].MenuState,
                        MenuCode = ParentData[i].MenuCode,
                        MenuType = ParentData[i].MenuType,
                        Children = ChildrenData
                    };
                    tree.Add(treeview);
                    i++;
                    //OperationChildData(ParentData, item);//item相当于list[0],list[1].....
                //}
            }     

            return tree; 
        }

    }
}