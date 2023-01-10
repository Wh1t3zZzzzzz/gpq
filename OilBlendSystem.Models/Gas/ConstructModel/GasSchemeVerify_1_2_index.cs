namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasSchemeVerify_1_2_index
    {
        //场景1
        //罐底油信息配置
        public int index { get; set; }//行的索引
        public string? ProdOilName { get; set; }//成品油名称
        public float BottomCapacity {get; set; }//罐底油质量/体积
        public float Bottomron {get; set; }//罐底油十六烷值    
        public float Bottomt50 {get; set; }//罐底油50%回收温度
        public float Bottomsuf {get; set; }//罐底油多芳烃含量
        public float Bottomden {get; set; }//罐底油密度

    }
}