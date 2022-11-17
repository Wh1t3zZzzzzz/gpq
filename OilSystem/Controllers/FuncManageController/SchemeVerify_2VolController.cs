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
public class SchemeVerify_2VolController : ControllerBase
{

    private readonly oilblendContext context;

    public SchemeVerify_2VolController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilPercent")]
    //体积
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

    [HttpGet("Set/TotalBlend")]
    //方案验证场景2成品油调合总量（不含罐底油）
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var TotalBlendList = context.Schemeverify2s.ToList();
        List<SchemeVerify_2_2> ResultList = new List<SchemeVerify_2_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            SchemeVerify_2_2 result = new SchemeVerify_2_2();//实体，可以理解为一个对象  
            result.ProdOilName = TotalBlendList[i].ProdOilName;
            result.ProdTotalBlend = TotalBlendList[i].TotalBlendVol2;
            ResultList.Add(result);
        }
     
        return new ApiModel()
        {
        code = 200,
        data = ResultList,
        msg = "查询成功"
        };

    }


    [HttpGet("Res/Product")]
    //方案验证场景2计算结果：成品油产量
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult2_vol_Product().ToList();
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
        var list = _SchemeVerify.GetSchemeverifyResult2_vol_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
