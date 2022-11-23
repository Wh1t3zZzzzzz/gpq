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
public class DispatchResController : ControllerBase
{
    //智能决策模块设置控制器

    private readonly oilblendContext context;

    public DispatchResController(oilblendContext _context)
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

    [HttpGet("Res/ComFlowInfo_divProd1")]
    public ApiModel GetProd1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd1().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divProd2")]
    public ApiModel GetProd2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd2().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divProd3")]
    public ApiModel GetProd3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd3().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divProd4")]
    public ApiModel GetProd4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divProd4().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT1")]
    public ApiModel GetT1()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT1().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT2")]
    public ApiModel GetT2()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT2().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT3")]
    public ApiModel GetT3()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT3().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT4")]
    public ApiModel GetT4()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT4().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT5")]
    public ApiModel GetT5()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT5().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComFlowInfo_divT6")]
    public ApiModel GetT6()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT6().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }
    
    [HttpGet("Res/ComFlowInfo_divT7")]
    public ApiModel GetT7()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_comFlowInfo_divT7().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ComInv")]
    public ApiModel GetComInv()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_invInfo_comOil().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/ProdInv")]
    public ApiModel GetProdInv()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_invInfo_prodOil().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Prod1Info")]
    public ApiModel GetProd1Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod1Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Prod2Info")]
    public ApiModel GetProd2Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod2Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

    [HttpGet("Res/Prod3Info")]
    public ApiModel GetProd3Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod3Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }
    [HttpGet("Res/Prod4Info")]
    public ApiModel GetProd4Info()//model里的名字 多个数据用IEnumberable，单个数据不用
    {       
        IDispatch _Dispatch = new Dispatch(context);
        // var list = context.Properties.ToList();
        var list = _Dispatch.GetDispatch_decsScheme_prod4Info().ToList();
        return new ApiModel()
        {
        code = 200,
        //data = JsonConvert.SerializeObject(list),
        data = list,
        msg = "查询成功"
        };

    }

}
