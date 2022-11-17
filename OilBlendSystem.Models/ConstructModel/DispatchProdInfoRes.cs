namespace OilBlendSystem.Models.ConstructModel
{
    public class DispatchProdInfoRes
    {
        //public int ID { get; set; }//主键
        //public string? action { get; set; }//
        public string? ProdOilName { get; set; }
        public int Time { get; set; }//周期
        public int ProdOilNum { get; set; }//成品油个数
        public float InfoT1 { get; set; }//第一周期成品油信息
        public float InfoT2 { get; set; }
        public float InfoT3 { get; set; }
        public float InfoT4 { get; set; }
        public float InfoT5 { get; set; }
        public float InfoT6 { get; set; }
        public float InfoT7 { get; set; }


    }
}