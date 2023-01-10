using Microsoft.AspNetCore.Mvc;
using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class PropertyGasController : ControllerBase
{

    private readonly oilblendContext context;

    public PropertyGasController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Get1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list = context.Propertie_gases.ToList();
        List<GasProperty_1> ResultList = new List<GasProperty_1>();//列表，里面可以添加很多个对象  
        for(int i = 0; i < list.Count; i++){
            GasProperty_1 result = new GasProperty_1();//实体，可以理解为一个对象  
            result.propertyName = list[i].PropertyName;
            result.apply = list[i].Apply;
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
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Put1(GasProperty_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list = context.Propertie_gases.ToList();
        list[obj.index].Apply = obj.apply;
        context.Propertie_gases.Update(list[obj.index]);
        context.SaveChanges();
        // var list = context.Propertie_gases.Where(m => m.Apply == 1).ToList();//自动表名后加s
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
    }



}
