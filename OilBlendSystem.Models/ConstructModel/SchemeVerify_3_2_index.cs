namespace OilBlendSystem.Models.ConstructModel
{
    public class SchemeVerify_3_2_index
    {
        //场景3
        //调合总量设置（不含罐底油）
        public int index { get; set; }//行的索引
        public string? ProdOilName { get; set; }//成品油名称
        public float ProdTotalBlend {get; set; }//成品油调合总量   

    }
}