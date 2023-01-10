namespace OilBlendSystem.Models.Gas.ConstructModel
{
    public class GasSchemeVerify_1_1
    {
        //场景1
        //成品油质量/体积产量分配
        //质量/体积都用这一个Model
        public string? ComOilName { get; set; }//组分油名称
        public float gas92Product { get; set; }//车柴
        public float gas95Product { get; set; }//出柴
        public float gas98Product { get; set; }//备用成品油1
        public float gasSelfProduct { get; set; }//备用成品油2

    }
}