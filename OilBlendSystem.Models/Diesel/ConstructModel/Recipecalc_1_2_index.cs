namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Recipecalc_1_2_index
    {
        //场景1
        //优化目标设置表格
        public int index { get; set; }//行的索引
        public string? WeightName { get; set; }//优化目标
        public float Weight { get; set; }//权值

    }
}