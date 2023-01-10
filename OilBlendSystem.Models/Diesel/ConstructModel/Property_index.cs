/// <summary>
/// 方案配置中的属性配置
/// </summary>
namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public partial class Property_index
    {
        public int index { get; set; }//前端返回的操作行数   
        public string? propertyName { get; set; }//属性名称
        public int apply { get; set; }//启用   1代表是   0代表否

    }
}
