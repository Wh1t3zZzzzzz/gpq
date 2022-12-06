using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilBlendSystem.BLL.Implementation;
using OilBlendSystem.BLL.Interface;
using OilSystem.ReturnClass;
using Newtonsoft.Json;
namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class PropertyController : ControllerBase
{

    private readonly oilblendContext context;

    public PropertyController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Get1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list = context.Properties.ToList();
        List<Property_1> ResultList = new List<Property_1>();//列表，里面可以添加很多个对象  
        for(int i = 0; i < list.Count; i++){
            Property_1 result = new Property_1();//实体，可以理解为一个对象  
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
    public ApiModel Put1(Property_index obj)//model里的名字 多个数据用IEnumberable，单个数据不用
    {
        var list = context.Properties.ToList();
        list[obj.index].Apply = obj.apply;
        context.Properties.Update(list[obj.index]);
        context.SaveChanges();
        // var list = context.Properties.Where(m => m.Apply == 1).ToList();//自动表名后加s
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
    }



}
