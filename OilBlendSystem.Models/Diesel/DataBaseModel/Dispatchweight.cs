/// <summary>
/// 配方优化中的第一个组分油产量表格
/// </summary>
namespace OilBlendSystem.Models.Diesel.DataBaseModel
{
    public partial class Dispatchweight
    {
        public int Id { get; set; }//数据库主键ID
        public string? WeightName { get; set; }//组分油参调流量
        public float Weight { get; set; }//权值

    }
}
