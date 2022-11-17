namespace OilBlendSystem.Models.ConstructModel
{
    public class Recipecalc_1Res_2
    {
        //场景1
        //计算结果：成品油质量产量
        public string? ComOilName { get; set; }//组分油名称
        public float AutoProduct { get; set; }//车柴质量产量
        public float ExpProduct { get; set; }//出柴质量产量
        public float Prod1Product { get; set; }//备用成品油1质量产量
        public float Prod2Product { get; set; }//备用成品油2质量产量


    }
}