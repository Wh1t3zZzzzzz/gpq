namespace OilBlendSystem.Models.ConstructModel
{
    public class Comproperty
    {
        public int ID { get; set; }//
        //下面是组分油属性
        public string? action { get; set; }//前端的动作，是增加后的编辑还是普通的编辑
        public string? ComOilName { get; set; }//组分油名称 ComOil = Component Oil
        public float Cet { get; set; }//十六烷值指数
        public float D50 { get; set; }//50%回收温度
        public float Pol { get; set; }//多芳烃含量
        public float Den { get; set; }//密度
        public float Price { get; set; }//价格
    }
}