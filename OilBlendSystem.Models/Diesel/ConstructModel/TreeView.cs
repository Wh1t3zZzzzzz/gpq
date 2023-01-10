using OilBlendSystem.Models.Diesel.DataBaseModel;

namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public partial class TreeView
    {
        public int ID { set; get; }
        public string? MenuName { set; get; }
        public string? Icon { set; get; }
        public string? Path { set; get; }
        public string? Component { set; get; }//加问号代表可能为空
        public string? ParentID { set; get; }
        public string? ChildID { set; get; }
        public string? MenuState { set; get; }
        public string? MenuCode { set; get; }
        public string? MenuType { set; get; }
        public List<Menulist>? Children { set; get; }

    }
}
