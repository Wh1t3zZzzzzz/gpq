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
public class SchemeVerify_3GasMassController : ControllerBase
{
    private readonly oilblendContext context;

    public SchemeVerify_3GasMassController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilFlow")]
    //质量
    //方案验证场景3成品油参调流量表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilFlowList = context.Schemeverify1_gases.ToList();
        List<GasSchemeVerify_3_1> ResultList = new List<GasSchemeVerify_3_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilFlowList.Count; i++){
            GasSchemeVerify_3_1 result = new GasSchemeVerify_3_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilFlowList[i].ComOilName;
            result.gas92Flow = ProdOilFlowList[i].gas92FlowMass;
            result.gas95Flow = ProdOilFlowList[i].gas95FlowMass;
            result.gas98Flow = ProdOilFlowList[i].gas98FlowMass;
            result.gasSelfFlow = ProdOilFlowList[i].gasSelfFlowMass;
            ResultList.Add(result);
        }
    
        return new ApiModel()
        {
        code = 200,
        data = ResultList,
        msg = "查询成功"
        };

    }

    [HttpPut("Put/ProdOilFlow")]
    //质量
    //方案验证场景3成品油参调流量表格——修改保存功能
    public ApiModel Put1(GasSchemeVerify_3_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilFlowList = context.Schemeverify1_gases.ToList();
        var list1 = context.Recipecalc1_gases.ToList();
        var list2 = context.Compoilconfig_gases.ToList();

        float sum1 = 0;
        float sum2 = 0;
        float sum3 = 0;
        float sum4 = 0;

        for(int i = 0; i < ProdOilFlowList.Count; i++){
            sum1 += ProdOilFlowList[i].gas92FlowMass;
            sum2 += ProdOilFlowList[i].gas95FlowMass;
            sum3 += ProdOilFlowList[i].gas98FlowMass;
            sum4 += ProdOilFlowList[i].gasSelfFlowMass;
        }

        sum1 = sum1 - ProdOilFlowList[obj.index].gas92FlowMass + obj.gas92Flow;
        sum2 = sum2 - ProdOilFlowList[obj.index].gas95FlowMass + obj.gas95Flow;
        sum3 = sum3 - ProdOilFlowList[obj.index].gas98FlowMass + obj.gas98Flow;
        sum4 = sum4 - ProdOilFlowList[obj.index].gasSelfFlowMass + obj.gasSelfFlow;

        if(sum1 != 0  && sum2 != 0 && sum3 != 0 && sum4 != 0){
            ProdOilFlowList[obj.index].ComOilName = obj.ComOilName;
            list1[obj.index].ComOilName = obj.ComOilName;
            list2[obj.index].ComOilName = obj.ComOilName;
            ProdOilFlowList[obj.index].gas92FlowMass = obj.gas92Flow;
            ProdOilFlowList[obj.index].gas95FlowMass = obj.gas95Flow;
            ProdOilFlowList[obj.index].gas98FlowMass = obj.gas98Flow;
            ProdOilFlowList[obj.index].gasSelfFlowMass = obj.gasSelfFlow;

            context.Schemeverify1_gases.Update(ProdOilFlowList[obj.index]);
            context.Recipecalc1_gases.Update(list1[obj.index]);
            context.Compoilconfig_gases.Update(list2[obj.index]);
            context.SaveChanges();
        
            return new ApiModel()
            {
            code = 200,
            //data = JsonConvert.SerializeObject(list),
            data = ProdOilFlowList,
            msg = "查询成功"
            };            
        }else{
            return new ApiModel(){
                code = 405,
                //data = JsonConvert.SerializeObject(list),
                data = null,
                msg = @"每列成品油的参调流量之和不允许为0"
            };           
        }

    }

    [HttpGet("Set/TotalBlend")]
    //方案验证场景3成品油调合总量（不含罐底油）
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2_gases.ToList();
        List<GasSchemeVerify_3_2> ResultList = new List<GasSchemeVerify_3_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            GasSchemeVerify_3_2 result = new GasSchemeVerify_3_2();//实体，可以理解为一个对象  
            result.ProdOilName = TotalBlendList[i].ProdOilName;
            result.ProdTotalBlend = TotalBlendList[i].TotalBlendMass3;
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
    //方案验证场景3成品油调合总量（不含罐底油）——修改保存功能
    public ApiModel Put2(GasSchemeVerify_3_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2_gases.ToList();
        var list1 = context.Recipecalc3_gases.ToList();
        var list2 = context.Prodoilconfig_gases.ToList();
        // if(0 < obj.ProdTotalBlend && obj.ProdTotalBlend <= 9999999999){
        TotalBlendList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        TotalBlendList[obj.index].TotalBlendMass3 = obj.ProdTotalBlend;

        context.Schemeverify2_gases.Update(TotalBlendList[obj.index]);
        context.Recipecalc3_gases.Update(list1[obj.index]);
        context.Prodoilconfig_gases.Update(list2[obj.index]);
        context.SaveChanges();
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = TotalBlendList,
        msg = "查询成功"
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

    [HttpGet("Res/Time")]
    //方案验证场景3计算结果：调合时间
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult3_mass_Time().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Product")]
    //方案验证场景3计算结果：成品油产量
    public ApiModel Res2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult3_mass_Product().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Property")]
    //方案验证场景3计算结果：成品油属性
    public ApiModel Res3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult3_mass_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }



}
