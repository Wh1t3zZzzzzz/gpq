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
public class SchemeVerify_1MassController : ControllerBase
{

    private readonly oilblendContext context;

    public SchemeVerify_1MassController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Set/ProdOilProduct")]
    //质量
    //方案验证场景1成品油产量分配表格
    public ApiModel Set1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdOilProductList = context.Schemeverify1s.ToList();
        List<SchemeVerify_1_1> ResultList = new List<SchemeVerify_1_1>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdOilProductList.Count; i++){
            SchemeVerify_1_1 result = new SchemeVerify_1_1();//实体，可以理解为一个对象  
            result.ComOilName = ProdOilProductList[i].ComOilName;
            result.AutoProduct = ProdOilProductList[i].AutoQualityProduct;
            result.ExpProduct = ProdOilProductList[i].ExpQualityProduct;
            result.Prod1Product = ProdOilProductList[i].Prod1QualityProduct;
            result.Prod2Product = ProdOilProductList[i].Prod2QualityProduct;
            ResultList.Add(result);
        }
    
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };

    }

    [HttpGet("Set/BottomInfo")]
    //方案验证场景1罐底油信息配置表格
    public ApiModel Set2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        var BottomInfoList = context.Schemeverify2s.ToList();
        List<SchemeVerify_1_2> ResultList = new List<SchemeVerify_1_2>();//列表，里面可以添加很多个对象
        for(int i = 0; i < BottomInfoList.Count; i++){
            SchemeVerify_1_2 result = new SchemeVerify_1_2();//实体，可以理解为一个对象  
            result.ProdOilName = BottomInfoList[i].ProdOilName;
            result.BottomCapacity = BottomInfoList[i].BottomMass;
            result.BottomCET = BottomInfoList[i].CetMass;
            result.BottomD50 = BottomInfoList[i].D50Mass;
            result.BottomPOL = BottomInfoList[i].PolMass;
            result.BottomDEN = BottomInfoList[i].DenMass;
            ResultList.Add(result);
        }
     
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };

    }

    [HttpGet("Set/TotalBlend")]
    //方案验证场景1调合总量设置表格（含罐底油）
    public ApiModel Set3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        var TotalBlendList = context.Schemeverify2s.ToList();
        List<SchemeVerify_1_3> ResultList = new List<SchemeVerify_1_3>();//列表，里面可以添加很多个对象
        for(int i = 0; i < TotalBlendList.Count; i++){
            SchemeVerify_1_3 result = new SchemeVerify_1_3();//实体，可以理解为一个对象  
            result.ProdOilName = TotalBlendList[i].ProdOilName;
            result.ProdTotalBlend = TotalBlendList[i].TotalBlendMass;
            ResultList.Add(result);
        }
     
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = ResultList,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Time")]
    //方案验证场景1计算结果：调合时间
    public ApiModel Res1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult1_mass_Time().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Product")]
    //方案验证场景1计算结果：成品油产量
    public ApiModel Res2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult1_mass_Product().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Property")]
    //方案验证场景1计算结果：成品油属性
    public ApiModel Res3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        ISchemeVerify _SchemeVerify = new SchemeVerify(context);
        var list = _SchemeVerify.GetSchemeverifyResult1_mass_Property().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
