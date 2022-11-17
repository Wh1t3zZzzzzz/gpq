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
        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateTime.Now.AddDays(index),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
        //using oilblendContext context = new();
        //  IRecipeCalc _RecipeCalc = new RecipeCalc(context);
        // var list = _RecipeCalc.GetRecipe1().ToList();
        var list = context.Properties.ToList();
        // var list = context.Properties.Where(m => m.Apply == 1).ToList();//自动表名后加s
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };
        //using oilblendContext context = new();//context上下文紧接着的是model中的名字（数据库名字+s）
        //retun context.Menulists.Where(m=>m.Id == 1).ToList();//自动表名后加s
        //context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;//用到了microsoft.entity包
        //var menulist = context.Menulists.ToList();//firstordefult 如果没有值，则返回默认值
        //return context.Compoilconfigs.ToList();
        //asnotracking关闭跟踪
        //return context.TestTables.Single(m=>m.Id == 1);//只有一个值用single
        //index.Index = number;
        //context.Menulists.Update(menulist);//把赋予的对象放到update里,这句其实可以不写
        // context.TestTables.Add(new TestTable{
        //     PartName = "Leo",
        //     Point = 10,
        // });
        //throw new Exception();
        //context.SaveChanges();//跟踪原理,不需要写update，如果取消更新则需要加上update
        //查询的时候用asnotracking，可以降低内存消耗，减少不必要的跟踪，二分写八分读

       //return menulist;

    }



}
