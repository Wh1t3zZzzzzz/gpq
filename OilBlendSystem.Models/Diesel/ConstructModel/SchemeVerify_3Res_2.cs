namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class SchemeVerify_3Res_2
    {
        //场景3
        //计算结果：成品油质量/体积产量
        public string? ComOilName { get; set; }//组分油名称
        public float AutoProduct { get; set; }//车柴
        public float ExpProduct { get; set; }//出柴
        public float Prod1Product { get; set; }//备用成品油1
        public float Prod2Product { get; set; }//备用成品油2

    }
}