namespace OilBlendSystem.Models.ConstructModel
{
    public class Recipecalc_3Res_2
    {
        //场景3
        //计算结果：成品油优化配方
        public string? ComOilName { get; set; }//组分油名称
        public float AutoRecipe { get; set; }//车柴优化配方
        public float ExpRecipe { get; set; }//出柴优化配方
        public float Prod1Recipe { get; set; }//备用成品油1优化配方
        public float Prod2Recipe { get; set; }//备用成品油2优化配方


    }
}