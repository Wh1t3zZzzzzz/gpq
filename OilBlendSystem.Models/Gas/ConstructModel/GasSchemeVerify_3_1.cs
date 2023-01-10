namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasSchemeVerify_3_1
    {
        //场景3
        //成品油质量/体积参调流量
        //质量/体积都用这一个Model
        public string? ComOilName { get; set; }//组分油名称
        public float gas92Flow { get; set; }//车柴
        public float gas95Flow { get; set; }//出柴
        public float gas98Flow { get; set; }//备用成品油1
        public float gasSelfFlow { get; set; }//备用成品油2
    }
}