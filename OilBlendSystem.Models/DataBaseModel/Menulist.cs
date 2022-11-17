using System;
using System.Collections.Generic;
/// <summary>
/// 这个是新系统的菜单，宝贝不用管
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
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
