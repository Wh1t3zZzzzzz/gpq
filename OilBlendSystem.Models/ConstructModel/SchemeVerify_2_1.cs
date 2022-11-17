namespace OilBlendSystem.Models.ConstructModel
{
    public class SchemeVerify_2_1
    {
        //场景2
        //成品油质量/体积参调百分比
        //质量/体积都用这一个Model
        public string? ComOilName { get; set; }//组分油名称
        public float AutoPercent { get; set; }//车柴
        public float ExpPercent { get; set; }//出柴
        public float Prod1Percent { get; set; }//备用成品油1
        public float Prod2Percent { get; set; }//备用成品油2

    }
}