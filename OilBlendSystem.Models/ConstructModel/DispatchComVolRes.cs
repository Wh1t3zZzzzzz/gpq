namespace OilBlendSystem.Models.ConstructModel
{
    public class DispatchComVolRes
    {
        //public int ID { get; set; }//主键
        //public string? action { get; set; }//
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public int Time { get; set; }//周期
        public int ComOilNum { get; set; }//组分油个数
        public float ComOilVol1 { get; set; }//组分油库存第一周期
        public float ComOilVol2 { get; set; }
        public float ComOilVol3 { get; set; }
        public float ComOilVol4 { get; set; }
        public float ComOilVol5 { get; set; }
        public float ComOilVol6 { get; set; }
        public float ComOilVol7 { get; set; }


    }
}