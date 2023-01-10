namespace OilBlendSystem.Models.Diesel.ConstructModel
{
    public class Dispatch_parmSet_obj
    {
        //智能决策——参数设置——权值设置
        //优化目标设置
        public string? weightName { get; set; }//优化目标
        public float weight { get; set; }//优化目标权值

    }
}