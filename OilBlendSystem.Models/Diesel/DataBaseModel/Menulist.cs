/// <summary>
/// 新系统的菜单
/// </summary>
namespace OilBlendSystem.Models.Diesel.DataBaseModel
{
    public partial class Menulist
    {
        public int ID { get; set; }
        public string? MenuName { get; set; }
        public string? Icon { get; set; }
        public string? Path { get; set; }
        public string? Component { get; set; }
        public string? ChildID { get; set; }
        public string? ParentID { get; set; }
        public string? MenuState { get; set; }
        public string? MenuCode { get; set; }
        public string? MenuType { get; set; }
    }
}
