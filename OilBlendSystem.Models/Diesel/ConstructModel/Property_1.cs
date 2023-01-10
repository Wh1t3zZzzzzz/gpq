/// <summary>
/// 方案配置中的属性配置
/// </summary>
namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public partial class Property_1
    //为了和databaseModel里的property区分开
    {
        public string? propertyName { get; set; }//属性名称
        public int apply { get; set; }//启用   1代表是   0代表否

    }
}
