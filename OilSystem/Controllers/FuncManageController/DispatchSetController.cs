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
public class DispatchSetController : ControllerBase
{
    //智能决策模块求解控制器

    private readonly oilblendContext context;

    public DispatchSetController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet("Res/Obj")]
    public ApiModel GetObj()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsCalc().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

}
