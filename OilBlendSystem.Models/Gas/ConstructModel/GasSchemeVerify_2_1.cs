namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasSchemeVerify_2_1
    {
        //场景2
        //成品油质量/体积参调百分比
        //质量/体积都用这一个Model
        public string? ComOilName { get; set; }//组分油名称
        public float gas92Percent { get; set; }//车柴
        public float gas95Percent { get; set; }//出柴
        public float gas98Percent { get; set; }//备用成品油1
        public float gasSelfPercent { get; set; }//备用成品油2

    }
}