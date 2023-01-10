using OilBlendSystem.Models.Gas.DataBaseModel;
using OilBlendSystem.Models.Gas.ConstructModel;
using OilBlendSystem.Models;
using System.Diagnostics;
using OilBlendSystem.BLL.Interface.Gas;

namespace OilBlendSystem.BLL.Implementation.Gas
{
    public partial class Dispatch : IDispatch
    {
        private readonly oilblendContext context;//带问号是可以为空
        public Dispatch(oilblendContext _context)
        {
           context = _context;
        }

        string[] Status;//求解标志
        int[] flow_nums;//组分油参调流量
        int[] ComOil_Inv_nums;//组分油库存
        int[] ProdOil_Inv_nums;//成品油库存
        int[] Prod_LP_nums;//成品油提货量
        int[] Obj_nums;//目标函数值

        public void RunPythonScript()//决策计算里的按钮调用这个接口 求解结果
        {
            string sArgName = @"GasCalc.py";//Gas
            string args = "-u";
            Process p = new Process();
            //py文件存储位置
            string path = @"E:\Python_env\" + sArgName;//(因为我没放debug下，所以直接写的绝对路径,替换掉上面的路径了)
            p.StartInfo.FileName = @"E:\Python_env\venv\Scripts\python.exe";//(注意：用的话需要换成自己的)没有配环境变量的话，可以像我这样写python.exe的绝对路径(用的话需要换成自己的)。如果配了，直接写"python.exe"即可
            string sArguments = path;

            sArguments += " " + args;

            p.StartInfo.Arguments = sArguments;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            // p.StartInfo.RedirectStandardOutput = false;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.CreateNoWindow = true;

            p.Start();

            p.BeginOutputReadLine();

            p.WaitForExit();

        }
        public void RunIOFromtxt()
        {
            string self_FilePath = @"E:\Python_env\gas_txt\";
            string[] flow_strs = File.ReadAllLines(self_FilePath + "Flow_txt.txt");//组分油参调流量
            string[] ComOil_Inv_strs = File.ReadAllLines(self_FilePath + "ComOil_Inv_txt.txt");//组分油库存
            string[] ProdOil_Inv_strs = File.ReadAllLines(self_FilePath + "ProdOil_Inv_txt.txt");//成品油库存
            string[] Prod_LP_strs = File.ReadAllLines(self_FilePath + "Prod_LP_txt.txt");//成品油提货量
            string[] Status_strs = File.ReadAllLines(self_FilePath + "Status_txt.txt");//求解标志
            string[] Obj_strs = File.ReadAllLines(self_FilePath + "Obj_txt.txt");//目标函数值

            string flow_str = string.Join("", flow_strs);//字符串数组转成字符串
            string ComOil_Inv_str = string.Join("", ComOil_Inv_strs);
            string ProdOil_Inv_str = string.Join("", ProdOil_Inv_strs);
            string Prod_LP_str = string.Join("", Prod_LP_strs);
            string Status_str = string.Join("", Status_strs);
            string Obj_str = string.Join("", Obj_strs);

            flow_strs = flow_str.Split(',');//字符串转化为字符串数组
            ComOil_Inv_strs = ComOil_Inv_str.Split(',');
            ProdOil_Inv_strs = ProdOil_Inv_str.Split(',');
            Prod_LP_strs = Prod_LP_str.Split(',');
            Status_strs = Status_str.Split(',');
            Obj_strs = Obj_str.Split(',');

            flow_nums = Array.ConvertAll(flow_strs, int.Parse);
            ComOil_Inv_nums = Array.ConvertAll(ComOil_Inv_strs, int.Parse);
            ProdOil_Inv_nums = Array.ConvertAll(ProdOil_Inv_strs, int.Parse);
            Prod_LP_nums = Array.ConvertAll(Prod_LP_strs, int.Parse);
            Obj_nums = Array.ConvertAll(Obj_strs, int.Parse);
            Status = Status_strs;
        }
        public IEnumerable<GasDispatch_decsCalc> GetDispatch_decsCalc()
        {
            RunPythonScript();
            RunIOFromtxt();
            List<GasDispatch_decsCalc> ResultList = new List<GasDispatch_decsCalc>();//新建一个List用来append的,返回的是list形式
            GasDispatch_decsCalc Result = new GasDispatch_decsCalc();
            Result.status = Status[0];
            Result.objValue = Obj_nums[0];
            ResultList.Add(Result);
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd1()//第一个成品油的组分油参调流量（七天）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 0){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(Period > 0){
                        Result.comFlowT1 = flow_nums[ComOilNum * ProdOilNum * 0 + i];
                    }
                    if(Period > 1){
                        Result.comFlowT2 = flow_nums[ComOilNum * ProdOilNum * 1 + i];
                    }
                    if(Period > 2){
                        Result.comFlowT3 = flow_nums[ComOilNum * ProdOilNum * 2 + i];
                    }
                    if(Period > 3){
                        Result.comFlowT4 = flow_nums[ComOilNum * ProdOilNum * 3 + i];
                    }
                    if(Period > 4){
                        Result.comFlowT5 = flow_nums[ComOilNum * ProdOilNum * 4 + i];
                    }
                    if(Period > 5){
                        Result.comFlowT6 = flow_nums[ComOilNum * ProdOilNum * 5 + i];
                    }
                    if(Period > 6){
                        Result.comFlowT7 = flow_nums[ComOilNum * ProdOilNum * 6 + i];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.comFlowT1 = 0;
                    Result.comFlowT3 = 0;
                    Result.comFlowT4 = 0;
                    Result.comFlowT5 = 0;
                    Result.comFlowT6 = 0;
                    Result.comFlowT7 = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd2()//第二个成品油的组分油参调流量
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 1){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(Period > 0){
                        Result.comFlowT1 = flow_nums[ComOilNum * ProdOilNum * 0 + i + ComOilNum];
                    }
                    if(Period > 1){
                        Result.comFlowT2 = flow_nums[ComOilNum * ProdOilNum * 1 + i + ComOilNum];
                    }
                    if(Period > 2){
                        Result.comFlowT3 = flow_nums[ComOilNum * ProdOilNum * 2 + i + ComOilNum];
                    }
                    if(Period > 3){
                        Result.comFlowT4 = flow_nums[ComOilNum * ProdOilNum * 3 + i + ComOilNum];
                    }
                    if(Period > 4){
                        Result.comFlowT5 = flow_nums[ComOilNum * ProdOilNum * 4 + i + ComOilNum];
                    }
                    if(Period > 5){
                        Result.comFlowT6 = flow_nums[ComOilNum * ProdOilNum * 5 + i + ComOilNum];
                    }
                    if(Period > 6){
                        Result.comFlowT7 = flow_nums[ComOilNum * ProdOilNum * 6 + i + ComOilNum];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.comFlowT1 = 0;
                    Result.comFlowT3 = 0;
                    Result.comFlowT4 = 0;
                    Result.comFlowT5 = 0;
                    Result.comFlowT6 = 0;
                    Result.comFlowT7 = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd3()//第三个成品油的组分油参调流量
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 2){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(Period > 0){
                        Result.comFlowT1 = flow_nums[ComOilNum * ProdOilNum * 0 + i + ComOilNum * 2];
                    }
                    if(Period > 1){
                        Result.comFlowT2 = flow_nums[ComOilNum * ProdOilNum * 1 + i + ComOilNum * 2];
                    }
                    if(Period > 2){
                        Result.comFlowT3 = flow_nums[ComOilNum * ProdOilNum * 2 + i + ComOilNum * 2];
                    }
                    if(Period > 3){
                        Result.comFlowT4 = flow_nums[ComOilNum * ProdOilNum * 3 + i + ComOilNum * 2];
                    }
                    if(Period > 4){
                        Result.comFlowT5 = flow_nums[ComOilNum * ProdOilNum * 4 + i + ComOilNum * 2];
                    }
                    if(Period > 5){
                        Result.comFlowT6 = flow_nums[ComOilNum * ProdOilNum * 5 + i + ComOilNum * 2];
                    }
                    if(Period > 6){
                        Result.comFlowT7 = flow_nums[ComOilNum * ProdOilNum * 6 + i + ComOilNum * 2];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.comFlowT1 = 0;
                    Result.comFlowT3 = 0;
                    Result.comFlowT4 = 0;
                    Result.comFlowT5 = 0;
                    Result.comFlowT6 = 0;
                    Result.comFlowT7 = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd4()//第四个成品油的组分油参调流量
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 3){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(Period > 0){
                        Result.comFlowT1 = flow_nums[ComOilNum * ProdOilNum * 0 + i + ComOilNum * 3];
                    }
                    if(Period > 1){
                        Result.comFlowT2 = flow_nums[ComOilNum * ProdOilNum * 1 + i + ComOilNum * 3];
                    }
                    if(Period > 2){
                        Result.comFlowT3 = flow_nums[ComOilNum * ProdOilNum * 2 + i + ComOilNum * 3];
                    }
                    if(Period > 3){
                        Result.comFlowT4 = flow_nums[ComOilNum * ProdOilNum * 3 + i + ComOilNum * 3];
                    }
                    if(Period > 4){
                        Result.comFlowT5 = flow_nums[ComOilNum * ProdOilNum * 4 + i + ComOilNum * 3];
                    }
                    if(Period > 5){
                        Result.comFlowT6 = flow_nums[ComOilNum * ProdOilNum * 5 + i + ComOilNum * 3];
                    }
                    if(Period > 6){
                        Result.comFlowT7 = flow_nums[ComOilNum * ProdOilNum * 6 + i + ComOilNum * 3];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divProd Result = new GasDispatch_decsScheme_comFlowInfo_divProd();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.comFlowT1 = 0;
                    Result.comFlowT3 = 0;
                    Result.comFlowT4 = 0;
                    Result.comFlowT5 = 0;
                    Result.comFlowT6 = 0;
                    Result.comFlowT7 = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT1()//第一天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divT> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 0){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.gas92ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 0];
                    }
                    if(ProdOilNum > 1){
                        Result.gas95ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 0];
                    }
                    if(ProdOilNum > 2){
                        Result.gas98ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 0];
                    }
                    if(ProdOilNum > 3){
                        Result.gasSelfComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 0];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.gas92ComFlow = 0;
                    Result.gas95ComFlow = 0;
                    Result.gas98ComFlow = 0;
                    Result.gasSelfComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT2()//第二天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divT> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 1){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.gas92ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 1];
                    }
                    if(ProdOilNum > 1){
                        Result.gas95ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 1];
                    }
                    if(ProdOilNum > 2){
                        Result.gas98ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 1];
                    }
                    if(ProdOilNum > 3){
                        Result.gasSelfComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 1];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.gas92ComFlow = 0;
                    Result.gas95ComFlow = 0;
                    Result.gas98ComFlow = 0;
                    Result.gasSelfComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT3()//第三天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divT> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 2){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.gas92ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 2];
                    }
                    if(ProdOilNum > 1){
                        Result.gas95ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 2];
                    }
                    if(ProdOilNum > 2){
                        Result.gas98ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 2];
                    }
                    if(ProdOilNum > 3){
                        Result.gasSelfComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 2];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.gas92ComFlow = 0;
                    Result.gas95ComFlow = 0;
                    Result.gas98ComFlow = 0;
                    Result.gasSelfComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT4()//第四天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divT> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 3){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.gas92ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 3];
                    }
                    if(ProdOilNum > 1){
                        Result.gas95ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 3];
                    }
                    if(ProdOilNum > 2){
                        Result.gas98ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 3];
                    }
                    if(ProdOilNum > 3){
                        Result.gasSelfComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 3];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.gas92ComFlow = 0;
                    Result.gas95ComFlow = 0;
                    Result.gas98ComFlow = 0;
                    Result.gasSelfComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT5()//第五天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divT> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 4){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.gas92ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 4];
                    }
                    if(ProdOilNum > 1){
                        Result.gas95ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 4];
                    }
                    if(ProdOilNum > 2){
                        Result.gas98ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 4];
                    }
                    if(ProdOilNum > 3){
                        Result.gasSelfComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 4];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.gas92ComFlow = 0;
                    Result.gas95ComFlow = 0;
                    Result.gas98ComFlow = 0;
                    Result.gasSelfComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT6()//第六天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divT> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 5){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.gas92ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 5];
                    }
                    if(ProdOilNum > 1){
                        Result.gas95ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 5];
                    }
                    if(ProdOilNum > 2){
                        Result.gas98ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 5];
                    }
                    if(ProdOilNum > 3){
                        Result.gasSelfComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 5];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.gas92ComFlow = 0;
                    Result.gas95ComFlow = 0;
                    Result.gas98ComFlow = 0;
                    Result.gasSelfComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT7()//第七天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_comFlowInfo_divT> ResultList = new List<GasDispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 6){
                for(int i = 0; i < ComOilNum; i++){
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.gas92ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 6];
                    }
                    if(ProdOilNum > 1){
                        Result.gas95ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 6];
                    }
                    if(ProdOilNum > 2){
                        Result.gas98ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 6];
                    }
                    if(ProdOilNum > 3){
                        Result.gasSelfComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 6];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    GasDispatch_decsScheme_comFlowInfo_divT Result = new GasDispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.gas92ComFlow = 0;
                    Result.gas95ComFlow = 0;
                    Result.gas98ComFlow = 0;
                    Result.gasSelfComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_invInfo_comOil> GetDispatch_decsScheme_invInfo_comOil()//组分油库存信息
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_invInfo_comOil> ResultList = new List<GasDispatch_decsScheme_invInfo_comOil>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < ComOilNum; i++){
                GasDispatch_decsScheme_invInfo_comOil Result = new GasDispatch_decsScheme_invInfo_comOil();
                Result.ComOilName = CompOilList[i].ComOilName;
                if(Period > 0){
                    Result.volumeT1 = ComOil_Inv_nums[ComOilNum * 0 + i];
                }
                if(Period > 1){
                    Result.volumeT2 = ComOil_Inv_nums[ComOilNum * 1 + i];
                }
                if(Period > 2){
                    Result.volumeT3 = ComOil_Inv_nums[ComOilNum * 2 + i];
                }
                if(Period > 3){
                    Result.volumeT4 = ComOil_Inv_nums[ComOilNum * 3 + i];
                }
                if(Period > 4){
                    Result.volumeT5 = ComOil_Inv_nums[ComOilNum * 4 + i];
                }
                if(Period > 5){
                    Result.volumeT6 = ComOil_Inv_nums[ComOilNum * 5 + i];
                }
                if(Period > 6){
                    Result.volumeT7 = ComOil_Inv_nums[ComOilNum * 6 + i];
                }
                ResultList.Add(Result);
            }   
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_invInfo_prodOil> GetDispatch_decsScheme_invInfo_prodOil()//成品油库存信息
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<GasDispatch_decsScheme_invInfo_prodOil> ResultList = new List<GasDispatch_decsScheme_invInfo_prodOil>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < ProdOilNum; i++){
                GasDispatch_decsScheme_invInfo_prodOil Result = new GasDispatch_decsScheme_invInfo_prodOil();
                Result.ProdOilName = ProdOilList[i].ProdOilName;
                if(Period > 0){
                    Result.volumeT1 = ComOil_Inv_nums[ProdOilNum * 0 + i];
                }
                if(Period > 1){
                    Result.volumeT2 = ComOil_Inv_nums[ProdOilNum * 1 + i];
                }
                if(Period > 2){
                    Result.volumeT3 = ComOil_Inv_nums[ProdOilNum * 2 + i];
                }
                if(Period > 3){
                    Result.volumeT4 = ComOil_Inv_nums[ProdOilNum * 3 + i];
                }
                if(Period > 4){
                    Result.volumeT5 = ComOil_Inv_nums[ProdOilNum * 4 + i];
                }
                if(Period > 5){
                    Result.volumeT6 = ComOil_Inv_nums[ProdOilNum * 5 + i];
                }
                if(Period > 6){
                    Result.volumeT7 = ComOil_Inv_nums[ProdOilNum * 6 + i];
                }
                ResultList.Add(Result);
            }   
            return ResultList;
        }
        public IEnumerable<GasDispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod1Info()//第一个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd1().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] Prodron = new double[7];
            double[] Prodt50 = new double[7];
            double[] Prodsuf = new double[7];
            double[] Prodden = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT1 * CompOilList[j].ron;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT2 * CompOilList[j].ron;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT3 * CompOilList[j].ron;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT4 * CompOilList[j].ron;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT5 * CompOilList[j].ron;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT6 * CompOilList[j].ron;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT7 * CompOilList[j].ron;                        
                    }
                }
                Prodron[i] = Math.Round(Prodron[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT1 * CompOilList[j].t50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT2 * CompOilList[j].t50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT3 * CompOilList[j].t50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT4 * CompOilList[j].t50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT5 * CompOilList[j].t50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT6 * CompOilList[j].t50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT7 * CompOilList[j].t50;                        
                    }
                }
                Prodt50[i] = Math.Round(Prodt50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT1 * CompOilList[j].suf;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT2 * CompOilList[j].suf;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT3 * CompOilList[j].suf;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT4 * CompOilList[j].suf;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT5 * CompOilList[j].suf;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT6 * CompOilList[j].suf;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT7 * CompOilList[j].suf;                        
                    }
                }
                Prodsuf[i] = Math.Round(Prodsuf[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT1 * CompOilList[j].den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT2 * CompOilList[j].den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT3 * CompOilList[j].den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT4 * CompOilList[j].den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT5 * CompOilList[j].den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT6 * CompOilList[j].den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT7 * CompOilList[j].den;                        
                    }
                }
                Prodden[i] = Math.Round(Prodden[i] / SumFlow, 1);
            }
            #endregion

            List<GasDispatch_decsScheme_prodInfo> ResultList = new List<GasDispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                GasDispatch_decsScheme_prodInfo Result = new GasDispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[0].ronHighLimit;
                    Result.valueLow = ProdOilList[0].ronLowLimit;
                    Result.valueT1 = Prodron[0].ToString("0.0");
                    Result.valueT2 = Prodron[1].ToString("0.0");
                    Result.valueT3 = Prodron[2].ToString("0.0");
                    Result.valueT4 = Prodron[3].ToString("0.0");
                    Result.valueT5 = Prodron[4].ToString("0.0");
                    Result.valueT6 = Prodron[5].ToString("0.0");
                    Result.valueT7 = Prodron[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[0].t50HighLimit;
                    Result.valueLow = ProdOilList[0].t50LowLimit;
                    Result.valueT1 = Prodt50[0].ToString("0");
                    Result.valueT2 = Prodt50[1].ToString("0");
                    Result.valueT3 = Prodt50[2].ToString("0");
                    Result.valueT4 = Prodt50[3].ToString("0");
                    Result.valueT5 = Prodt50[4].ToString("0");
                    Result.valueT6 = Prodt50[5].ToString("0");
                    Result.valueT7 = Prodt50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[0].sufHighLimit;
                    Result.valueLow = ProdOilList[0].sufLowLimit;
                    Result.valueT1 = Prodsuf[0].ToString("0.00");
                    Result.valueT2 = Prodsuf[1].ToString("0.00");
                    Result.valueT3 = Prodsuf[2].ToString("0.00");
                    Result.valueT4 = Prodsuf[3].ToString("0.00");
                    Result.valueT5 = Prodsuf[4].ToString("0.00");
                    Result.valueT6 = Prodsuf[5].ToString("0.00");
                    Result.valueT7 = Prodsuf[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[0].denHighLimit;
                    Result.valueLow = ProdOilList[0].denLowLimit;
                    Result.valueT1 = Prodden[0].ToString("0.0");
                    Result.valueT2 = Prodden[1].ToString("0.0");
                    Result.valueT3 = Prodden[2].ToString("0.0");
                    Result.valueT4 = Prodden[3].ToString("0.0");
                    Result.valueT5 = Prodden[4].ToString("0.0");
                    Result.valueT6 = Prodden[5].ToString("0.0");
                    Result.valueT7 = Prodden[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;   
        }
        public IEnumerable<GasDispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod2Info()//第二个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd2().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] Prodron = new double[7];
            double[] Prodt50 = new double[7];
            double[] Prodsuf = new double[7];
            double[] Prodden = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT1 * CompOilList[j].ron;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT2 * CompOilList[j].ron;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT3 * CompOilList[j].ron;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT4 * CompOilList[j].ron;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT5 * CompOilList[j].ron;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT6 * CompOilList[j].ron;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT7 * CompOilList[j].ron;                        
                    }
                }
                Prodron[i] = Math.Round(Prodron[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT1 * CompOilList[j].t50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT2 * CompOilList[j].t50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT3 * CompOilList[j].t50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT4 * CompOilList[j].t50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT5 * CompOilList[j].t50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT6 * CompOilList[j].t50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT7 * CompOilList[j].t50;                        
                    }
                }
                Prodt50[i] = Math.Round(Prodt50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT1 * CompOilList[j].suf;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT2 * CompOilList[j].suf;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT3 * CompOilList[j].suf;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT4 * CompOilList[j].suf;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT5 * CompOilList[j].suf;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT6 * CompOilList[j].suf;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT7 * CompOilList[j].suf;                        
                    }
                }
                Prodsuf[i] = Math.Round(Prodsuf[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT1 * CompOilList[j].den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT2 * CompOilList[j].den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT3 * CompOilList[j].den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT4 * CompOilList[j].den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT5 * CompOilList[j].den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT6 * CompOilList[j].den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT7 * CompOilList[j].den;                        
                    }
                }
                Prodden[i] = Math.Round(Prodden[i] / SumFlow, 1);
            }
            #endregion

            List<GasDispatch_decsScheme_prodInfo> ResultList = new List<GasDispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                GasDispatch_decsScheme_prodInfo Result = new GasDispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[1].ronHighLimit;
                    Result.valueLow = ProdOilList[1].ronLowLimit;
                    Result.valueT1 = Prodron[0].ToString("0.0");
                    Result.valueT2 = Prodron[1].ToString("0.0");
                    Result.valueT3 = Prodron[2].ToString("0.0");
                    Result.valueT4 = Prodron[3].ToString("0.0");
                    Result.valueT5 = Prodron[4].ToString("0.0");
                    Result.valueT6 = Prodron[5].ToString("0.0");
                    Result.valueT7 = Prodron[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[1].t50HighLimit;
                    Result.valueLow = ProdOilList[1].t50LowLimit;
                    Result.valueT1 = Prodt50[0].ToString("0");
                    Result.valueT2 = Prodt50[1].ToString("0");
                    Result.valueT3 = Prodt50[2].ToString("0");
                    Result.valueT4 = Prodt50[3].ToString("0");
                    Result.valueT5 = Prodt50[4].ToString("0");
                    Result.valueT6 = Prodt50[5].ToString("0");
                    Result.valueT7 = Prodt50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[1].sufHighLimit;
                    Result.valueLow = ProdOilList[1].sufLowLimit;
                    Result.valueT1 = Prodsuf[0].ToString("0.00");
                    Result.valueT2 = Prodsuf[1].ToString("0.00");
                    Result.valueT3 = Prodsuf[2].ToString("0.00");
                    Result.valueT4 = Prodsuf[3].ToString("0.00");
                    Result.valueT5 = Prodsuf[4].ToString("0.00");
                    Result.valueT6 = Prodsuf[5].ToString("0.00");
                    Result.valueT7 = Prodsuf[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[1].denHighLimit;
                    Result.valueLow = ProdOilList[1].denLowLimit;
                    Result.valueT1 = Prodden[0].ToString("0.0");
                    Result.valueT2 = Prodden[1].ToString("0.0");
                    Result.valueT3 = Prodden[2].ToString("0.0");
                    Result.valueT4 = Prodden[3].ToString("0.0");
                    Result.valueT5 = Prodden[4].ToString("0.0");
                    Result.valueT6 = Prodden[5].ToString("0.0");
                    Result.valueT7 = Prodden[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;   
        }
        public IEnumerable<GasDispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod3Info()//第三个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd3().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] Prodron = new double[7];
            double[] Prodt50 = new double[7];
            double[] Prodsuf = new double[7];
            double[] Prodden = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT1 * CompOilList[j].ron;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT2 * CompOilList[j].ron;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT3 * CompOilList[j].ron;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT4 * CompOilList[j].ron;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT5 * CompOilList[j].ron;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT6 * CompOilList[j].ron;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT7 * CompOilList[j].ron;                        
                    }
                }
                Prodron[i] = Math.Round(Prodron[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT1 * CompOilList[j].t50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT2 * CompOilList[j].t50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT3 * CompOilList[j].t50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT4 * CompOilList[j].t50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT5 * CompOilList[j].t50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT6 * CompOilList[j].t50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT7 * CompOilList[j].t50;                        
                    }
                }
                Prodt50[i] = Math.Round(Prodt50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT1 * CompOilList[j].suf;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT2 * CompOilList[j].suf;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT3 * CompOilList[j].suf;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT4 * CompOilList[j].suf;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT5 * CompOilList[j].suf;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT6 * CompOilList[j].suf;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT7 * CompOilList[j].suf;                        
                    }
                }
                Prodsuf[i] = Math.Round(Prodsuf[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT1 * CompOilList[j].den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT2 * CompOilList[j].den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT3 * CompOilList[j].den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT4 * CompOilList[j].den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT5 * CompOilList[j].den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT6 * CompOilList[j].den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT7 * CompOilList[j].den;                        
                    }
                }
                Prodden[i] = Math.Round(Prodden[i] / SumFlow, 1);
            }
            #endregion

            List<GasDispatch_decsScheme_prodInfo> ResultList = new List<GasDispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                GasDispatch_decsScheme_prodInfo Result = new GasDispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[2].ronHighLimit;
                    Result.valueLow = ProdOilList[2].ronLowLimit;
                    Result.valueT1 = Prodron[0].ToString("0.0");
                    Result.valueT2 = Prodron[1].ToString("0.0");
                    Result.valueT3 = Prodron[2].ToString("0.0");
                    Result.valueT4 = Prodron[3].ToString("0.0");
                    Result.valueT5 = Prodron[4].ToString("0.0");
                    Result.valueT6 = Prodron[5].ToString("0.0");
                    Result.valueT7 = Prodron[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[2].t50HighLimit;
                    Result.valueLow = ProdOilList[2].t50LowLimit;
                    Result.valueT1 = Prodt50[0].ToString("0");
                    Result.valueT2 = Prodt50[1].ToString("0");
                    Result.valueT3 = Prodt50[2].ToString("0");
                    Result.valueT4 = Prodt50[3].ToString("0");
                    Result.valueT5 = Prodt50[4].ToString("0");
                    Result.valueT6 = Prodt50[5].ToString("0");
                    Result.valueT7 = Prodt50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[2].sufHighLimit;
                    Result.valueLow = ProdOilList[2].sufLowLimit;
                    Result.valueT1 = Prodsuf[0].ToString("0.00");
                    Result.valueT2 = Prodsuf[1].ToString("0.00");
                    Result.valueT3 = Prodsuf[2].ToString("0.00");
                    Result.valueT4 = Prodsuf[3].ToString("0.00");
                    Result.valueT5 = Prodsuf[4].ToString("0.00");
                    Result.valueT6 = Prodsuf[5].ToString("0.00");
                    Result.valueT7 = Prodsuf[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[3].denHighLimit;
                    Result.valueLow = ProdOilList[3].denLowLimit;
                    Result.valueT1 = Prodden[0].ToString("0.0");
                    Result.valueT2 = Prodden[1].ToString("0.0");
                    Result.valueT3 = Prodden[2].ToString("0.0");
                    Result.valueT4 = Prodden[3].ToString("0.0");
                    Result.valueT5 = Prodden[4].ToString("0.0");
                    Result.valueT6 = Prodden[5].ToString("0.0");
                    Result.valueT7 = Prodden[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;             
        }
        public IEnumerable<GasDispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod4Info()//第四个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd4().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweight_gases.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] Prodron = new double[7];
            double[] Prodt50 = new double[7];
            double[] Prodsuf = new double[7];
            double[] Prodden = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT1 * CompOilList[j].ron;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT2 * CompOilList[j].ron;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT3 * CompOilList[j].ron;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT4 * CompOilList[j].ron;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT5 * CompOilList[j].ron;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT6 * CompOilList[j].ron;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodron[i] = Prodron[i] + FlowList[j].comFlowT7 * CompOilList[j].ron;                        
                    }
                }
                Prodron[i] = Math.Round(Prodron[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT1 * CompOilList[j].t50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT2 * CompOilList[j].t50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT3 * CompOilList[j].t50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT4 * CompOilList[j].t50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT5 * CompOilList[j].t50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT6 * CompOilList[j].t50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodt50[i] = Prodt50[i] + FlowList[j].comFlowT7 * CompOilList[j].t50;                        
                    }
                }
                Prodt50[i] = Math.Round(Prodt50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT1 * CompOilList[j].suf;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT2 * CompOilList[j].suf;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT3 * CompOilList[j].suf;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT4 * CompOilList[j].suf;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT5 * CompOilList[j].suf;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT6 * CompOilList[j].suf;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodsuf[i] = Prodsuf[i] + FlowList[j].comFlowT7 * CompOilList[j].suf;                        
                    }
                }
                Prodsuf[i] = Math.Round(Prodsuf[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT1 * CompOilList[j].den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT2 * CompOilList[j].den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT3 * CompOilList[j].den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT4 * CompOilList[j].den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT5 * CompOilList[j].den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT6 * CompOilList[j].den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        Prodden[i] = Prodden[i] + FlowList[j].comFlowT7 * CompOilList[j].den;                        
                    }
                }
                Prodden[i] = Math.Round(Prodden[i] / SumFlow, 1);
            }
            #endregion

            List<GasDispatch_decsScheme_prodInfo> ResultList = new List<GasDispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                GasDispatch_decsScheme_prodInfo Result = new GasDispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[3].ronHighLimit;
                    Result.valueLow = ProdOilList[3].ronLowLimit;
                    Result.valueT1 = Prodron[0].ToString("0.0");
                    Result.valueT2 = Prodron[1].ToString("0.0");
                    Result.valueT3 = Prodron[2].ToString("0.0");
                    Result.valueT4 = Prodron[3].ToString("0.0");
                    Result.valueT5 = Prodron[4].ToString("0.0");
                    Result.valueT6 = Prodron[5].ToString("0.0");
                    Result.valueT7 = Prodron[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[3].t50HighLimit;
                    Result.valueLow = ProdOilList[3].t50LowLimit;
                    Result.valueT1 = Prodt50[0].ToString("0");
                    Result.valueT2 = Prodt50[1].ToString("0");
                    Result.valueT3 = Prodt50[2].ToString("0");
                    Result.valueT4 = Prodt50[3].ToString("0");
                    Result.valueT5 = Prodt50[4].ToString("0");
                    Result.valueT6 = Prodt50[5].ToString("0");
                    Result.valueT7 = Prodt50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[3].sufHighLimit;
                    Result.valueLow = ProdOilList[3].sufLowLimit;
                    Result.valueT1 = Prodsuf[0].ToString("0.00");
                    Result.valueT2 = Prodsuf[1].ToString("0.00");
                    Result.valueT3 = Prodsuf[2].ToString("0.00");
                    Result.valueT4 = Prodsuf[3].ToString("0.00");
                    Result.valueT5 = Prodsuf[4].ToString("0.00");
                    Result.valueT6 = Prodsuf[5].ToString("0.00");
                    Result.valueT7 = Prodsuf[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[3].denHighLimit;
                    Result.valueLow = ProdOilList[3].denLowLimit;
                    Result.valueT1 = Prodden[0].ToString("0.0");
                    Result.valueT2 = Prodden[1].ToString("0.0");
                    Result.valueT3 = Prodden[2].ToString("0.0");
                    Result.valueT4 = Prodden[3].ToString("0.0");
                    Result.valueT5 = Prodden[4].ToString("0.0");
                    Result.valueT6 = Prodden[5].ToString("0.0");
                    Result.valueT7 = Prodden[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;               
        }
    
    }
}