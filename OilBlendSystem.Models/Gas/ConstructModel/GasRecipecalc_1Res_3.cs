namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasRecipecalc_1Res_3
    {
        //场景1
        //计算结果：成品油优化配方
        public string? ComOilName { get; set; }//组分油名称
        public float gas92Recipe { get; set; }//车柴优化配方
        public float gas95Recipe { get; set; }//出柴优化配方
        public float gas98Recipe { get; set; }//备用成品油1优化配方
        public float gasSelfRecipe { get; set; }//备用成品油2优化配方

    }
}