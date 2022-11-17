namespace OilBlendSystem.Models.ConstructModel
{
    public class DispatchComFlowRes
    {
        //public int ID { get; set; }//主键
        //public string? action { get; set; }//
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public int Time { get; set; }//周期
        public int ComOilNum { get; set; }//组分油个数
        public float ComOilFlow1 { get; set; }//组分油参调流量第一周期
        public float ComOilFlow2 { get; set; }//组分油参调流量
        public float ComOilFlow3 { get; set; }//组分油参调流量
        public float ComOilFlow4 { get; set; }//组分油参调流量
        public float ComOilFlow5 { get; set; }//组分油参调流量
        public float ComOilFlow6 { get; set; }//组分油参调流量
        public float ComOilFlow7 { get; set; }//组分油参调流量


    }
}