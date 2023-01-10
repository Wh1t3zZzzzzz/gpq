namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasSchemeVerify_1_3_index
    {
        //场景1
        //调合总量设置（含罐底油）
        public int index { get; set; }//行的索引
        public string? ProdOilName { get; set; }//成品油名称
        public float ProdTotalBlend {get; set; }//成品油调合总量   


    }
}