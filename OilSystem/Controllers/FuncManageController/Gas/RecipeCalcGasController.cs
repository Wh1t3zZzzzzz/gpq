using Microsoft.AspNetCore.Mvc;
using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class RecipeCalcGasController : ControllerBase
{

    private readonly oilblendContext context;

    public RecipeCalcGasController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdList = context.Prodoilconfig_gases.ToList();
        List<GasRecipecalc> ResultList = new List<GasRecipecalc>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdList.Count; i++){
            GasRecipecalc result = new GasRecipecalc();//实体，可以理解为一个对象  
            result.prodOilName = ProdList[i].ProdOilName;
            result.apply = ProdList[i].Apply;
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

    [HttpPut]
    public ApiModel Put(GasRecipecalc_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list1 = context.Prodoilconfig_gases.ToList();
        var list2 = context.Recipecalc2_gases.ToList();//场景1优化目标
        var list3 = context.Recipecalc2_2_gases.ToList();//场景2优化目标
        var list4 = context.Recipecalc2_3_gases.ToList();//场景3优化目标
        var list5 = context.Recipecalc3_gases.ToList();

        list1[obj.index].Apply = obj.apply;
        list5[obj.index].Apply = obj.apply;
        for(int i = 0; i < 3; i++){
            list2[obj.index + 4 * i].Apply = obj.apply;
            list3[obj.index + 4 * i].Apply = obj.apply;
            list4[obj.index + 4 * i].Apply = obj.apply;            
        }

        context.Prodoilconfig_gases.Update(list1[obj.index]);
        context.Recipecalc2_gases.Update(list2[obj.index]);
        context.Recipecalc2_2_gases.Update(list3[obj.index]);
        context.Recipecalc2_3_gases.Update(list4[obj.index]);
        context.Recipecalc3_gases.Update(list5[obj.index]);
        context.SaveChanges();
        // var list = context.Propertie_gases.Where(m => m.Apply == 1).ToList();//自动表名后加s
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = null,
        msg = "修改成功"
        };
    }
}
