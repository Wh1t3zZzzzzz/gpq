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
public class SchemeVerify_3MassController : ControllerBase
{
    private readonly oilblendContext context;

    public SchemeVerify_3MassController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilFlow")]
    //质量
    //方案验证场景3成品油参调流量表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        var ProdOilFlowList = context.Schemeverify1s.ToList();
        List<SchemeVerify_3_1> ResultList = new List<SchemeVerify_3_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilFlowList.Count; i++){
            SchemeVerify_3_1 result = new SchemeVerify_3_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilFlowList[i].ComOilName;
            result.AutoFlow = ProdOilFlowList[i].AutoFlowMass;
            result.ExpFlow = ProdOilFlowList[i].ExpFlowMass;
            result.Prod1Flow = ProdOilFlowList[i].Prod1FlowMass;
            result.Prod2Flow = ProdOilFlowList[i].Prod2FlowMass;
            ResultList.Add(result);
        }
    
        return new ApiModel()
        {
        code = 200,
        data = ResultList,
        msg = "查询成功"
        };

    }

    [HttpGet("Set/TotalBlend")]
    //方案验证场景3成品油调合总量（不含罐底油）
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2s.ToList();
        List<SchemeVerify_3_2> ResultList = new List<SchemeVerify_3_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            SchemeVerify_3_2 result = new SchemeVerify_3_2();//实体，可以理解为一个对象  
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
