using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilSystem.ReturnClass;
using Newtonsoft.Json;
namespace OilSystem.Controllers;
using OilBlendSystem.BLL.Implementation;
using OilBlendSystem.BLL.Interface;

//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class DispatchController : ControllerBase
{

    private readonly oilblendContext context;

    public DispatchController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    //public IEnumerable<TestTable> Get()//model里的名字
    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatchComFlowRes1().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };


    }


}
