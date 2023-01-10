namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasRecipecalc_1Res_2
    {
        //场景1
        //计算结果：成品油质量产量
        public string? ComOilName { get; set; }//组分油名称
        public float gas92Product { get; set; }//车柴质量产量
        public float gas95Product { get; set; }//出柴质量产量
        public float gas98Product { get; set; }//备用成品油1质量产量
        public float gasSelfProduct { get; set; }//备用成品油2质量产量


    }
}