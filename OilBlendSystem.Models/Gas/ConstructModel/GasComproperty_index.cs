namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasComproperty_index
    {
        //下面是组分油属性
        public int index { get; set; }//行的索引
        public string? action { get; set; }//前端返回操作变量，是edit还是add
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float ron { get; set; }//十六烷值指数
        public float t50 { get; set; }//50%回收温度
        public float suf { get; set; }//多芳烃含量
        public float den { get; set; }//密度
        public float Price { get; set; }//价格
    }
}