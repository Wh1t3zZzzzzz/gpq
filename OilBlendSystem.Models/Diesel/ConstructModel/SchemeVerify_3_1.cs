namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class SchemeVerify_3_1
    {
        //场景3
        //成品油质量/体积参调流量
        //质量/体积都用这一个Model
        public string? ComOilName { get; set; }//组分油名称
        public float AutoFlow { get; set; }//车柴
        public float ExpFlow { get; set; }//出柴
        public float Prod1Flow { get; set; }//备用成品油1
        public float Prod2Flow { get; set; }//备用成品油2
    }
}