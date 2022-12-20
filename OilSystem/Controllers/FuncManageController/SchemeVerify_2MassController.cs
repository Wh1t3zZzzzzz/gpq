using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilSystem.ReturnClass;
using Newtonsoft.Json;
using OilBlendSystem.BLL.Implementation;
using OilBlendSystem.BLL.Interface;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class SchemeVerify_2MassController : ControllerBase
{

    private readonly oilblendContext context;

    public SchemeVerify_2MassController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilPercent")]
    //质量
    //方案验证场景2成品油参调百分比表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilPercentList = context.Schemeverify1s.ToList();
        List<SchemeVerify_2_1> ResultList = new List<SchemeVerify_2_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilPercentList.Count; i++){
            SchemeVerify_2_1 result = new SchemeVerify_2_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilPercentList[i].ComOilName;
            result.AutoPercent = ProdOilPercentList[i].AutoFlowPercentMass;
            result.ExpPercent = ProdOilPercentList[i].ExpFlowPercentMass;
            result.Prod1Percent = ProdOilPercentList[i].Prod1FlowPercentMass;
            result.Prod2Percent = ProdOilPercentList[i].Prod2FlowPercentMass;
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
    public ApiModel Put1(SchemeVerify_2_1_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilPercentList = context.Schemeverify1s.ToList();
        var list1 = context.Recipecalc1s.ToList();
        var list2 = context.Compoilconfigs.ToList();

        // if(0 <= obj.AutoPercent && obj.AutoPercent <= 100 
        // && 0 <= obj.ExpPercent && obj.ExpPercent <= 100 
        // && 0 <= obj.Prod1Percent && obj.Prod1Percent <= 100 
        // && 0 <= obj.Prod2Percent && obj.Prod2Percent <= 100){

        ProdOilPercentList[obj.index].ComOilName = obj.ComOilName;
        list1[obj.index].ComOilName = obj.ComOilName;
        list2[obj.index].ComOilName = obj.ComOilName;
        ProdOilPercentList[obj.index].AutoFlowPercentMass = obj.AutoPercent;
        ProdOilPercentList[obj.index].ExpFlowPercentMass = obj.ExpPercent;
        ProdOilPercentList[obj.index].Prod1FlowPercentMass = obj.Prod1Percent;
        ProdOilPercentList[obj.index].Prod2FlowPercentMass = obj.Prod2Percent;

        context.Schemeverify1s.Update(ProdOilPercentList[obj.index]);
        context.Recipecalc1s.Update(list1[obj.index]);
        context.Compoilconfigs.Update(list2[obj.index]);
        context.SaveChanges();

        float sum1 = 0;
        float sum2 = 0;
        float sum3 = 0;
        float sum4 = 0;

        for(int i = 0; i < ProdOilPercentList.Count; i++){
            sum1 += ProdOilPercentList[i].AutoFlowPercentMass;
            sum2 += ProdOilPercentList[i].ExpFlowPercentMass;
            sum3 += ProdOilPercentList[i].Prod1FlowPercentMass;
            sum4 += ProdOilPercentList[i].Prod2FlowPercentMass;
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
        var TotalBlendList = context.Schemeverify2s.ToList();
        List<SchemeVerify_2_2> ResultList = new List<SchemeVerify_2_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            SchemeVerify_2_2 result = new SchemeVerify_2_2();//实体，可以理解为一个对象  
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
    public ApiModel Put2(SchemeVerify_2_2_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2s.ToList();
        var list1 = context.Recipecalc3s.ToList();
        var list2 = context.Prodoilconfigs.ToList();

        // if(0 < obj.ProdTotalBlend && obj.ProdTotalBlend <= 9999999999){
        TotalBlendList[obj.index].ProdOilName = obj.ProdOilName;
        list1[obj.index].ProdOilName = obj.ProdOilName;
        list2[obj.index].ProdOilName = obj.ProdOilName;
        TotalBlendList[obj.index].TotalBlendMass2 = obj.ProdTotalBlend;

        context.Schemeverify2s.Update(TotalBlendList[obj.index]);
        context.Recipecalc3s.Update(list1[obj.index]);
        context.Prodoilconfigs.Update(list2[obj.index]);
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
