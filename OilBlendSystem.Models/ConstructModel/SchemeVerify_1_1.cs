namespace OilBlendSystem.Models.ConstructModel
{
    public class SchemeVerify_1_1
    {
        //场景1
        //成品油质量/体积产量分配
        //质量/体积都用这一个Model
        public string? ComOilName { get; set; }//组分油名称
        public float AutoProduct { get; set; }//车柴
        public float ExpProduct { get; set; }//出柴
        public float Prod1Product { get; set; }//备用成品油1
        public float Prod2Product { get; set; }//备用成品油2

    }
}