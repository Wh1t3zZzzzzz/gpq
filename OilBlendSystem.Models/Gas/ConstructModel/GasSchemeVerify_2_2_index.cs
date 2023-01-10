namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasSchemeVerify_2_2_index
    {
        //场景2
        //调合总量设置（不含罐底油）
        public int index { get; set; }//行的索引
        public string? ProdOilName { get; set; }//成品油名称
        public float ProdTotalBlend {get; set; }//成品油调合总量   
    }
}