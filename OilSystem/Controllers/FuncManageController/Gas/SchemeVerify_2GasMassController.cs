using Microsoft.AspNetCore.Mvc;
using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;
using OilBlendSystem.BLL.Implementation.Gas;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class SchemeVerify_2GasMassController : ControllerBase
{

    private readonly oilblendContext context;

    public SchemeVerify_2GasMassController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilPercent")]
    //质量
    //方案验证场景2成品油参调百分比表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilPercentList = context.Schemeverify1_gases.ToList();
        List<GasSchemeVerify_2_1> ResultList = new List<GasSchemeVerify_2_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilPercentList.Count; i++){
            GasSchemeVerify_2_1 result = new GasSchemeVerify_2_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilPercentList[i].ComOilName;
            result.gas92Percent = ProdOilPercentList[i].gas92FlowPercentMass;
            result.gas95Percent = ProdOilPercentList[i].gas95FlowPercentMass;
            result.gas98Percent = ProdOilPercentList[i].gas98FlowPercentMass;
            result.gasSelfPercent = ProdOilPercentList[i].gasSelfFlowPercentMass;
            ResultList.Add(result);
        }
    
        return new ApiModel()
        {
        code = 200,
        data = ResultList,
        msg = "查询成功"
        };

    }

    [HttpPut("Put/ProdOilPercent")]
    //质量
    //方案验证场景2成品油参调百分比表格——修改保存功能
    public ApiModel Put1(GasSchemeVerify_2_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilPercentList = context.Schemeverify1_gases.ToList();
        var list1 = context.Recipecalc1_gases.ToList();
        var list2 = context.Compoilconfig_gases.ToList();

        // if(0 <= obj.gas92Percent && obj.gas92Percent <= 100 
        // && 0 <= obj.gas95Percent && obj.gas95Percent <= 100 
        // && 0 <= obj.gas98Percent && obj.gas98Percent <= 100 
        // && 0 <= obj.gasSelfPercent && obj.gasSelfPercent <= 100){

        ProdOilPercentList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ProdOilPercentList[obj.index].gas92FlowPercentMass = obj.gas92Percent;
        ProdOilPercentList[obj.index].gas95FlowPercentMass = obj.gas95Percent;
        ProdOilPercentList[obj.index].gas98FlowPercentMass = obj.gas98Percent;
        ProdOilPercentList[obj.index].gasSelfFlowPercentMass = obj.gasSelfPercent;

        context.Schemeverify1_gases.Update(ProdOilPercentList[obj.index]);
        context.Recipecalc1_gases.Update(list1[obj.index]);
        context.Compoilconfig_gases.Update(list2[obj.index]);
        context.SaveChanges();

        float sum1 = 0;
        float sum2 = 0;
        float sum3 = 0;
        float sum4 = 0;

        for(int i = 0; i < ProdOilPercentList.Count; i++){
            sum1 += ProdOilPercentList[i].gas92FlowPercentMass;
            sum2 += ProdOilPercentList[i].gas95FlowPercentMass;
            sum3 += ProdOilPercentList[i].gas98FlowPercentMass;
            sum4 += ProdOilPercentList[i].gasSelfFlowPercentMass;
        }
                    
        if(sum1 == 100 && sum2 == 100 && sum3 == 100 && sum4 == 100){
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = null,
            msg = "修改成功"
            }; 
        }else{
            return new ApiModel(){
                code = 403,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = @"提示: 当前成品油参调比例之和不为100%，请检查"
            };         
        }  
        // }else{
        //     return new ApiModel(){
        //         code = 501,
        //         //data = JsonConvert.SerializeObject(list),
        //         data = null,
        //         msg = @"参调比例范围应为[0,100%]"
        //     };         
        // }  
    }

    [HttpGet("Set/TotalBlend")]
    //方案验证场景2成品油调合总量（不含罐底油）
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2_gases.ToList();
        List<GasSchemeVerify_2_2> ResultList = new List<GasSchemeVerify_2_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            GasSchemeVerify_2_2 result = new GasSchemeVerify_2_2();//实体，可以理解为一个对象  
            result.ProdOilName = TotalBlendList[i].ProdOilName;
            result.ProdTotalBlend = TotalBlendList[i].TotalBlendMass2;
            ResultList.Add(result);
        }
     
        return new ApiModel()
        {
        code = 200,
        data = ResultList,
        msg = "查询成功"
        };

    }


    [HttpPut("Put/TotalBlend")]
    //质量
    //方案验证场景2成品油调合总量（不含罐底油）——修改保存功能
    public ApiModel Put2(GasSchemeVerify_2_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2_gases.ToList();
        var list1 = context.Recipecalc3_gases.ToList();
        var list2 = context.Prodoilconfig_gases.ToList();

        // if(0 < obj.ProdTotalBlend && obj.ProdTotalBlend <= 9999999999){
        TotalBlendList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        TotalBlendList[obj.index].TotalBlendMass2 = obj.ProdTotalBlend;

        context.Schemeverify2_gases.Update(TotalBlendList[obj.index]);
        context.Recipecalc3_gases.Update(list1[obj.index]);
        context.Prodoilconfig_gases.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = TotalBlendList,
        msg = "修改成功"
        };            
        // }else{
        //     return new ApiModel(){
        //         code = 500,
        //         //data = JsonConvert.SerializeObject(list),
        //         data = null,
        //         msg = @"成品油调合总量超出限制: (0,9999999999]"
        //     };  
        // }
    }


    [HttpGet("Res/Product")]
    //方案验证场景2计算结果：成品油产量
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult2_mass_Product().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Property")]
    //方案验证场景2计算结果：成品油属性
    public ApiModel Res2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult2_mass_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
