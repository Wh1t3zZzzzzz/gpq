using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;
using OilBlendSystem.Models;
using OilBlendSystem.BLL.Interface.Diesel;

namespace OilBlendSystem.BLL.Implementation.Diesel
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
            TreeView treeview1 = new TreeView()
            {
                ID = ParentData[5].ID,
                MenuName = ParentData[5].MenuName,
                Icon = ParentData[5].Icon,
                Path = ParentData[5].Path,
                Component = ParentData[5].Component,
                ChildID = ParentData[5].ChildID,
                ParentID = ParentData[5].ParentID,
                MenuState = ParentData[5].MenuState,
                MenuCode = ParentData[5].MenuCode,
                MenuType = ParentData[5].MenuType,
            };
            tree.Add(treeview1);
            foreach (var item in ParentData)
            {
                if(item.ID == 21) break;
                var ChildrenData = context.Menulists.Where(m => m.ChildID == item.ID.ToString()).ToList();
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
            }     

            return tree; 
        }

    }
}