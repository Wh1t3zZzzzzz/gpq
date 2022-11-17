namespace OilBlendSystem.Models.ConstructModel
{
    public class DispatchProdVolRes
    {
        //public int ID { get; set; }//主键
        //public string? action { get; set; }//
        public string? ProdOilName { get; set; }
        public int Time { get; set; }//周期
        public int ProdOilNum { get; set; }//成品油个数
        public float ProdOilVol1 { get; set; }//成品油库存第一周期
        public float ProdOilVol2 { get; set; }
        public float ProdOilVol3 { get; set; }
        public float ProdOilVol4 { get; set; }
        public float ProdOilVol5 { get; set; }
        public float ProdOilVol6 { get; set; }
        public float ProdOilVol7 { get; set; }


    }
}