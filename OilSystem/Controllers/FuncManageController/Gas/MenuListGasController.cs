using Microsoft.AspNetCore.Mvc;
using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using OilSystem.ReturnClass;
using OilBlendSystem.BLL.Implementation.Gas;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilSystem.Controllers;
//using System.Data.Entity;
[ApiController]
[Route("[controller]")]
public class MenuListGasController : ControllerBase
{
    private readonly oilblendContext context;

    public MenuListGasController(oilblendContext _context)
    {
       context = _context;
    }

    [HttpGet]
    //ID ParentID Name   List<TreeChildren>

    public ApiModel Get()//model里的名字 多个数据用IEnumberable，单个数据不用
    {

        //using oilblendContext context = new();
        IMenuList _MenuList = new MenuList(context);
        var list = _MenuList.GetTreeViewMenuList();
        // var list = context.Menulist_gases.ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }


}
