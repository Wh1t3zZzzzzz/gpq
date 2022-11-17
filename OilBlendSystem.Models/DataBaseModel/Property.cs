using System;
using System.Collections.Generic;
/// <summary>
/// 方案配置中的属性配置
/// </summary>
namespace OilBlendSystem.Models.DataBaseModel
{
    public partial class Property
    {
        public int Id { get; set; }//数据库主键ID
        public string? PropertyName { get; set; }//属性名称
        public int Apply { get; set; }//启用   1代表是   0代表否

    }
}
