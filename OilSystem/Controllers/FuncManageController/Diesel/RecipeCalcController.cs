using Microsoft.AspNetCore.Mvc;
using OilBlendSystem.Models.Diesel.DataBaseModel;
using OilBlendSystem.Models.Diesel.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class RecipeCalcController : ControllerBase
{

    private readonly oilblendContext context;

    public RecipeCalcController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var ProdList = context.Prodoilconfigs.ToList();
        List<Recipecalc> ResultList = new List<Recipecalc>();//列表，里面可以添加很多个对象
        for(int i = 0; i < ProdList.Count; i++){
            Recipecalc result = new Recipecalc();//实体，可以理解为一个对象  
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
    public ApiModel Put(Recipecalc_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list1 = context.Prodoilconfigs.ToList();
        var list2 = context.Recipecalc2s.ToList();//场景1优化目标
        var list3 = context.Recipecalc2_2s.ToList();//场景2优化目标
        var list4 = context.Recipecalc2_3s.ToList();//场景3优化目标
        var list5 = context.Recipecalc3s.ToList();

        list1[obj.index].Apply = obj.apply;
        list5[obj.index].Apply = obj.apply;
        for(int i = 0; i < 3; i++){
            list2[obj.index + 4 * i].Apply = obj.apply;
            list3[obj.index + 4 * i].Apply = obj.apply;
            list4[obj.index + 4 * i].Apply = obj.apply;            
        }

        context.Prodoilconfigs.Update(list1[obj.index]);
        context.Recipecalc2s.Update(list2[obj.index]);
        context.Recipecalc2_2s.Update(list3[obj.index]);
        context.Recipecalc2_3s.Update(list4[obj.index]);
        context.Recipecalc3s.Update(list5[obj.index]);
        context.SaveChanges();
        // var list = context.Properties.Where(m => m.Apply == 1).ToList();//自动表名后加s
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = null,
        msg = "修改成功"
        };
    }
}
