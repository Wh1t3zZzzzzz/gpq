using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilBlendSystem.BLL.Interface;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace OilBlendSystem.BLL.Implementation
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
            string sArgName = @"dieselCalc.py";//diesel
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
            string self_FilePath = @"E:\Python_env\";
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
        public IEnumerable<Dispatch_decsCalc> GetDispatch_decsCalc()
        {
            RunPythonScript();
            RunIOFromtxt();
            List<Dispatch_decsCalc> ResultList = new List<Dispatch_decsCalc>();//新建一个List用来append的,返回的是list形式
            Dispatch_decsCalc Result = new Dispatch_decsCalc();
            Result.status = Status[0];
            Result.objValue = Obj_nums[0];
            ResultList.Add(Result);
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd1()//第一个成品油的组分油参调流量（七天）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 0){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd2()//第二个成品油的组分油参调流量
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 1){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd3()//第三个成品油的组分油参调流量
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 2){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divProd> GetDispatch_decsScheme_comFlowInfo_divProd4()//第四个成品油的组分油参调流量
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divProd> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divProd>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(ProdOilNum > 3){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
                    Dispatch_decsScheme_comFlowInfo_divProd Result = new Dispatch_decsScheme_comFlowInfo_divProd();
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
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT1()//第一天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divT> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 0){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.prod1ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 0];
                    }
                    if(ProdOilNum > 1){
                        Result.prod2ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 0];
                    }
                    if(ProdOilNum > 2){
                        Result.prod3ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 0];
                    }
                    if(ProdOilNum > 3){
                        Result.prod4ComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 0];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.prod1ComFlow = 0;
                    Result.prod2ComFlow = 0;
                    Result.prod3ComFlow = 0;
                    Result.prod4ComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT2()//第二天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divT> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 1){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.prod1ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 1];
                    }
                    if(ProdOilNum > 1){
                        Result.prod2ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 1];
                    }
                    if(ProdOilNum > 2){
                        Result.prod3ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 1];
                    }
                    if(ProdOilNum > 3){
                        Result.prod4ComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 1];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.prod1ComFlow = 0;
                    Result.prod2ComFlow = 0;
                    Result.prod3ComFlow = 0;
                    Result.prod4ComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT3()//第三天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divT> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 2){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.prod1ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 2];
                    }
                    if(ProdOilNum > 1){
                        Result.prod2ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 2];
                    }
                    if(ProdOilNum > 2){
                        Result.prod3ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 2];
                    }
                    if(ProdOilNum > 3){
                        Result.prod4ComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 2];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.prod1ComFlow = 0;
                    Result.prod2ComFlow = 0;
                    Result.prod3ComFlow = 0;
                    Result.prod4ComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT4()//第四天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divT> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 3){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.prod1ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 3];
                    }
                    if(ProdOilNum > 1){
                        Result.prod2ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 3];
                    }
                    if(ProdOilNum > 2){
                        Result.prod3ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 3];
                    }
                    if(ProdOilNum > 3){
                        Result.prod4ComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 3];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.prod1ComFlow = 0;
                    Result.prod2ComFlow = 0;
                    Result.prod3ComFlow = 0;
                    Result.prod4ComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT5()//第五天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divT> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 4){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.prod1ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 4];
                    }
                    if(ProdOilNum > 1){
                        Result.prod2ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 4];
                    }
                    if(ProdOilNum > 2){
                        Result.prod3ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 4];
                    }
                    if(ProdOilNum > 3){
                        Result.prod4ComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 4];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.prod1ComFlow = 0;
                    Result.prod2ComFlow = 0;
                    Result.prod3ComFlow = 0;
                    Result.prod4ComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT6()//第六天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divT> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 5){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.prod1ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 5];
                    }
                    if(ProdOilNum > 1){
                        Result.prod2ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 5];
                    }
                    if(ProdOilNum > 2){
                        Result.prod3ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 5];
                    }
                    if(ProdOilNum > 3){
                        Result.prod4ComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 5];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.prod1ComFlow = 0;
                    Result.prod2ComFlow = 0;
                    Result.prod3ComFlow = 0;
                    Result.prod4ComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_comFlowInfo_divT> GetDispatch_decsScheme_comFlowInfo_divT7()//第七天的组分油参调流量（四个成品油）
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_comFlowInfo_divT> ResultList = new List<Dispatch_decsScheme_comFlowInfo_divT>();//新建一个List用来append的,返回的是list形式
            //第一个成品油 第一个周期 0-7 第二个周期 24-31 第三个周期 48-55...
            //第二个成品油 第一个周期 8-15 第二个周期 32-39 第三个周期 56-63...
            //第三个成品油 第一个周期 16-23 第三个周期 40 -47 第三个周期 64 -71...
            if(Period > 6){
                for(int i = 0; i < ComOilNum; i++){
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    if(ProdOilNum > 0){
                        Result.prod1ComFlow = flow_nums[ComOilNum * 0 + i + ComOilNum * ProdOilNum * 6];
                    }
                    if(ProdOilNum > 1){
                        Result.prod2ComFlow = flow_nums[ComOilNum * 1 + i + ComOilNum * ProdOilNum * 6];
                    }
                    if(ProdOilNum > 2){
                        Result.prod3ComFlow = flow_nums[ComOilNum * 2 + i + ComOilNum * ProdOilNum * 6];
                    }
                    if(ProdOilNum > 3){
                        Result.prod4ComFlow = flow_nums[ComOilNum * 3 + i + ComOilNum * ProdOilNum * 6];
                    }
                    ResultList.Add(Result);
                }
            }
            else{     
                for(int i = 0; i < ComOilNum; i++){           
                    Dispatch_decsScheme_comFlowInfo_divT Result = new Dispatch_decsScheme_comFlowInfo_divT();
                    Result.ComOilName = CompOilList[i].ComOilName;
                    Result.prod1ComFlow = 0;
                    Result.prod2ComFlow = 0;
                    Result.prod3ComFlow = 0;
                    Result.prod4ComFlow = 0;
                    ResultList.Add(Result);
                }
            }
            return ResultList;
        }
        public IEnumerable<Dispatch_decsScheme_invInfo_comOil> GetDispatch_decsScheme_invInfo_comOil()//组分油库存信息
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_invInfo_comOil> ResultList = new List<Dispatch_decsScheme_invInfo_comOil>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < ComOilNum; i++){
                Dispatch_decsScheme_invInfo_comOil Result = new Dispatch_decsScheme_invInfo_comOil();
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
        public IEnumerable<Dispatch_decsScheme_invInfo_prodOil> GetDispatch_decsScheme_invInfo_prodOil()//成品油库存信息
        {
            RunIOFromtxt();
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);

            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            List<Dispatch_decsScheme_invInfo_prodOil> ResultList = new List<Dispatch_decsScheme_invInfo_prodOil>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < ProdOilNum; i++){
                Dispatch_decsScheme_invInfo_prodOil Result = new Dispatch_decsScheme_invInfo_prodOil();
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
        public IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod1Info()//第一个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd1().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] ProdCET = new double[7];
            double[] ProdD50 = new double[7];
            double[] ProdPOL = new double[7];
            double[] ProdDEN = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT1 * CompOilList[j].Cet;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT2 * CompOilList[j].Cet;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT3 * CompOilList[j].Cet;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT4 * CompOilList[j].Cet;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT5 * CompOilList[j].Cet;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT6 * CompOilList[j].Cet;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT7 * CompOilList[j].Cet;                        
                    }
                }
                ProdCET[i] = Math.Round(ProdCET[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT1 * CompOilList[j].D50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT2 * CompOilList[j].D50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT3 * CompOilList[j].D50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT4 * CompOilList[j].D50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT5 * CompOilList[j].D50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT6 * CompOilList[j].D50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT7 * CompOilList[j].D50;                        
                    }
                }
                ProdD50[i] = Math.Round(ProdD50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT1 * CompOilList[j].Pol;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT2 * CompOilList[j].Pol;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT3 * CompOilList[j].Pol;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT4 * CompOilList[j].Pol;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT5 * CompOilList[j].Pol;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT6 * CompOilList[j].Pol;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT7 * CompOilList[j].Pol;                        
                    }
                }
                ProdPOL[i] = Math.Round(ProdPOL[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT1 * CompOilList[j].Den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT2 * CompOilList[j].Den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT3 * CompOilList[j].Den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT4 * CompOilList[j].Den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT5 * CompOilList[j].Den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT6 * CompOilList[j].Den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT7 * CompOilList[j].Den;                        
                    }
                }
                ProdDEN[i] = Math.Round(ProdDEN[i] / SumFlow, 1);
            }
            #endregion

            List<Dispatch_decsScheme_prodInfo> ResultList = new List<Dispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                Dispatch_decsScheme_prodInfo Result = new Dispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[0].CetHighLimit;
                    Result.valueLow = ProdOilList[0].CetLowLimit;
                    Result.valueT1 = ProdCET[0].ToString("0.0");
                    Result.valueT2 = ProdCET[1].ToString("0.0");
                    Result.valueT3 = ProdCET[2].ToString("0.0");
                    Result.valueT4 = ProdCET[3].ToString("0.0");
                    Result.valueT5 = ProdCET[4].ToString("0.0");
                    Result.valueT6 = ProdCET[5].ToString("0.0");
                    Result.valueT7 = ProdCET[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[0].D50HighLimit;
                    Result.valueLow = ProdOilList[0].D50LowLimit;
                    Result.valueT1 = ProdD50[0].ToString("0");
                    Result.valueT2 = ProdD50[1].ToString("0");
                    Result.valueT3 = ProdD50[2].ToString("0");
                    Result.valueT4 = ProdD50[3].ToString("0");
                    Result.valueT5 = ProdD50[4].ToString("0");
                    Result.valueT6 = ProdD50[5].ToString("0");
                    Result.valueT7 = ProdD50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[0].PolHighLimit;
                    Result.valueLow = ProdOilList[0].PolLowLimit;
                    Result.valueT1 = ProdPOL[0].ToString("0.00");
                    Result.valueT2 = ProdPOL[1].ToString("0.00");
                    Result.valueT3 = ProdPOL[2].ToString("0.00");
                    Result.valueT4 = ProdPOL[3].ToString("0.00");
                    Result.valueT5 = ProdPOL[4].ToString("0.00");
                    Result.valueT6 = ProdPOL[5].ToString("0.00");
                    Result.valueT7 = ProdPOL[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[0].DenHighLimit;
                    Result.valueLow = ProdOilList[0].DenLowLimit;
                    Result.valueT1 = ProdDEN[0].ToString("0.0");
                    Result.valueT2 = ProdDEN[1].ToString("0.0");
                    Result.valueT3 = ProdDEN[2].ToString("0.0");
                    Result.valueT4 = ProdDEN[3].ToString("0.0");
                    Result.valueT5 = ProdDEN[4].ToString("0.0");
                    Result.valueT6 = ProdDEN[5].ToString("0.0");
                    Result.valueT7 = ProdDEN[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;   
        }
        public IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod2Info()//第二个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd2().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] ProdCET = new double[7];
            double[] ProdD50 = new double[7];
            double[] ProdPOL = new double[7];
            double[] ProdDEN = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT1 * CompOilList[j].Cet;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT2 * CompOilList[j].Cet;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT3 * CompOilList[j].Cet;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT4 * CompOilList[j].Cet;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT5 * CompOilList[j].Cet;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT6 * CompOilList[j].Cet;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT7 * CompOilList[j].Cet;                        
                    }
                }
                ProdCET[i] = Math.Round(ProdCET[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT1 * CompOilList[j].D50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT2 * CompOilList[j].D50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT3 * CompOilList[j].D50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT4 * CompOilList[j].D50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT5 * CompOilList[j].D50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT6 * CompOilList[j].D50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT7 * CompOilList[j].D50;                        
                    }
                }
                ProdD50[i] = Math.Round(ProdD50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT1 * CompOilList[j].Pol;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT2 * CompOilList[j].Pol;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT3 * CompOilList[j].Pol;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT4 * CompOilList[j].Pol;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT5 * CompOilList[j].Pol;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT6 * CompOilList[j].Pol;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT7 * CompOilList[j].Pol;                        
                    }
                }
                ProdPOL[i] = Math.Round(ProdPOL[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT1 * CompOilList[j].Den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT2 * CompOilList[j].Den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT3 * CompOilList[j].Den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT4 * CompOilList[j].Den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT5 * CompOilList[j].Den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT6 * CompOilList[j].Den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT7 * CompOilList[j].Den;                        
                    }
                }
                ProdDEN[i] = Math.Round(ProdDEN[i] / SumFlow, 1);
            }
            #endregion

            List<Dispatch_decsScheme_prodInfo> ResultList = new List<Dispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                Dispatch_decsScheme_prodInfo Result = new Dispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[1].CetHighLimit;
                    Result.valueLow = ProdOilList[1].CetLowLimit;
                    Result.valueT1 = ProdCET[0].ToString("0.0");
                    Result.valueT2 = ProdCET[1].ToString("0.0");
                    Result.valueT3 = ProdCET[2].ToString("0.0");
                    Result.valueT4 = ProdCET[3].ToString("0.0");
                    Result.valueT5 = ProdCET[4].ToString("0.0");
                    Result.valueT6 = ProdCET[5].ToString("0.0");
                    Result.valueT7 = ProdCET[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[1].D50HighLimit;
                    Result.valueLow = ProdOilList[1].D50LowLimit;
                    Result.valueT1 = ProdD50[0].ToString("0");
                    Result.valueT2 = ProdD50[1].ToString("0");
                    Result.valueT3 = ProdD50[2].ToString("0");
                    Result.valueT4 = ProdD50[3].ToString("0");
                    Result.valueT5 = ProdD50[4].ToString("0");
                    Result.valueT6 = ProdD50[5].ToString("0");
                    Result.valueT7 = ProdD50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[1].PolHighLimit;
                    Result.valueLow = ProdOilList[1].PolLowLimit;
                    Result.valueT1 = ProdPOL[0].ToString("0.00");
                    Result.valueT2 = ProdPOL[1].ToString("0.00");
                    Result.valueT3 = ProdPOL[2].ToString("0.00");
                    Result.valueT4 = ProdPOL[3].ToString("0.00");
                    Result.valueT5 = ProdPOL[4].ToString("0.00");
                    Result.valueT6 = ProdPOL[5].ToString("0.00");
                    Result.valueT7 = ProdPOL[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[1].DenHighLimit;
                    Result.valueLow = ProdOilList[1].DenLowLimit;
                    Result.valueT1 = ProdDEN[0].ToString("0.0");
                    Result.valueT2 = ProdDEN[1].ToString("0.0");
                    Result.valueT3 = ProdDEN[2].ToString("0.0");
                    Result.valueT4 = ProdDEN[3].ToString("0.0");
                    Result.valueT5 = ProdDEN[4].ToString("0.0");
                    Result.valueT6 = ProdDEN[5].ToString("0.0");
                    Result.valueT7 = ProdDEN[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;   
        }
        public IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod3Info()//第三个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd3().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] ProdCET = new double[7];
            double[] ProdD50 = new double[7];
            double[] ProdPOL = new double[7];
            double[] ProdDEN = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT1 * CompOilList[j].Cet;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT2 * CompOilList[j].Cet;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT3 * CompOilList[j].Cet;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT4 * CompOilList[j].Cet;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT5 * CompOilList[j].Cet;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT6 * CompOilList[j].Cet;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT7 * CompOilList[j].Cet;                        
                    }
                }
                ProdCET[i] = Math.Round(ProdCET[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT1 * CompOilList[j].D50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT2 * CompOilList[j].D50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT3 * CompOilList[j].D50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT4 * CompOilList[j].D50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT5 * CompOilList[j].D50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT6 * CompOilList[j].D50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT7 * CompOilList[j].D50;                        
                    }
                }
                ProdD50[i] = Math.Round(ProdD50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT1 * CompOilList[j].Pol;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT2 * CompOilList[j].Pol;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT3 * CompOilList[j].Pol;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT4 * CompOilList[j].Pol;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT5 * CompOilList[j].Pol;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT6 * CompOilList[j].Pol;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT7 * CompOilList[j].Pol;                        
                    }
                }
                ProdPOL[i] = Math.Round(ProdPOL[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT1 * CompOilList[j].Den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT2 * CompOilList[j].Den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT3 * CompOilList[j].Den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT4 * CompOilList[j].Den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT5 * CompOilList[j].Den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT6 * CompOilList[j].Den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT7 * CompOilList[j].Den;                        
                    }
                }
                ProdDEN[i] = Math.Round(ProdDEN[i] / SumFlow, 1);
            }
            #endregion

            List<Dispatch_decsScheme_prodInfo> ResultList = new List<Dispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                Dispatch_decsScheme_prodInfo Result = new Dispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[2].CetHighLimit;
                    Result.valueLow = ProdOilList[2].CetLowLimit;
                    Result.valueT1 = ProdCET[0].ToString("0.0");
                    Result.valueT2 = ProdCET[1].ToString("0.0");
                    Result.valueT3 = ProdCET[2].ToString("0.0");
                    Result.valueT4 = ProdCET[3].ToString("0.0");
                    Result.valueT5 = ProdCET[4].ToString("0.0");
                    Result.valueT6 = ProdCET[5].ToString("0.0");
                    Result.valueT7 = ProdCET[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[2].D50HighLimit;
                    Result.valueLow = ProdOilList[2].D50LowLimit;
                    Result.valueT1 = ProdD50[0].ToString("0");
                    Result.valueT2 = ProdD50[1].ToString("0");
                    Result.valueT3 = ProdD50[2].ToString("0");
                    Result.valueT4 = ProdD50[3].ToString("0");
                    Result.valueT5 = ProdD50[4].ToString("0");
                    Result.valueT6 = ProdD50[5].ToString("0");
                    Result.valueT7 = ProdD50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[2].PolHighLimit;
                    Result.valueLow = ProdOilList[2].PolLowLimit;
                    Result.valueT1 = ProdPOL[0].ToString("0.00");
                    Result.valueT2 = ProdPOL[1].ToString("0.00");
                    Result.valueT3 = ProdPOL[2].ToString("0.00");
                    Result.valueT4 = ProdPOL[3].ToString("0.00");
                    Result.valueT5 = ProdPOL[4].ToString("0.00");
                    Result.valueT6 = ProdPOL[5].ToString("0.00");
                    Result.valueT7 = ProdPOL[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[3].DenHighLimit;
                    Result.valueLow = ProdOilList[3].DenLowLimit;
                    Result.valueT1 = ProdDEN[0].ToString("0.0");
                    Result.valueT2 = ProdDEN[1].ToString("0.0");
                    Result.valueT3 = ProdDEN[2].ToString("0.0");
                    Result.valueT4 = ProdDEN[3].ToString("0.0");
                    Result.valueT5 = ProdDEN[4].ToString("0.0");
                    Result.valueT6 = ProdDEN[5].ToString("0.0");
                    Result.valueT7 = ProdDEN[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;             
        }
        public IEnumerable<Dispatch_decsScheme_prodInfo> GetDispatch_decsScheme_prod4Info()//第四个成品油的属性信息
        {
            RunIOFromtxt();
            IProdOilConfig _ProdOilConfig = new ProdOilConfig(context);
            ICompOilConfig _CompOilConfig = new CompOilConfig(context);
            IProperty _Property = new PropertyApply(context);

            var FlowList = GetDispatch_decsScheme_comFlowInfo_divProd4().ToList();//第一个成品油的周期组分油参调流量数据
            var CompOilList = _CompOilConfig.GetAllCompOilConfigList().ToList();
            var ProdOilList = _ProdOilConfig.GetAllProdOilConfigList().ToList();
            var PropertyList = _Property.GetAllPropertyList().ToList();
            var Weight = context.Dispatchweights.ToList();

            int ComOilNum = CompOilList.Count;//组分油个数
            int ProdOilNum = (int)Weight[5].Weight;//成品油个数
            int Period = (int)Weight[0].Weight;//调度周期

            double[] ProdCET = new double[7];
            double[] ProdD50 = new double[7];
            double[] ProdPOL = new double[7];
            double[] ProdDEN = new double[7];

            #region 十六烷值
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT1 * CompOilList[j].Cet;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT2 * CompOilList[j].Cet;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT3 * CompOilList[j].Cet;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT4 * CompOilList[j].Cet;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT5 * CompOilList[j].Cet;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT6 * CompOilList[j].Cet;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdCET[i] = ProdCET[i] + FlowList[j].comFlowT7 * CompOilList[j].Cet;                        
                    }
                }
                ProdCET[i] = Math.Round(ProdCET[i] / SumFlow, 1);
            }
            #endregion 

            #region 50%回收温度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT1 * CompOilList[j].D50;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT2 * CompOilList[j].D50;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT3 * CompOilList[j].D50;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT4 * CompOilList[j].D50;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT5 * CompOilList[j].D50;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT6 * CompOilList[j].D50;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdD50[i] = ProdD50[i] + FlowList[j].comFlowT7 * CompOilList[j].D50;                        
                    }
                }
                ProdD50[i] = Math.Round(ProdD50[i] / SumFlow);
            }
            #endregion

            #region 多环芳烃含量
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT1 * CompOilList[j].Pol;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT2 * CompOilList[j].Pol;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT3 * CompOilList[j].Pol;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT4 * CompOilList[j].Pol;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT5 * CompOilList[j].Pol;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT6 * CompOilList[j].Pol;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdPOL[i] = ProdPOL[i] + FlowList[j].comFlowT7 * CompOilList[j].Pol;                        
                    }
                }
                ProdPOL[i] = Math.Round(ProdPOL[i] / SumFlow, 2);
            }
            #endregion

            #region 密度
            for(int i = 0; i < 7; i++){
                double SumFlow = 0.0000000001;//每一个周期的总流量
                for(int j = 0; j < FlowList.Count; j++){
                    if(i == 0){
                        SumFlow = SumFlow + FlowList[j].comFlowT1;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT1 * CompOilList[j].Den;
                    }
                    if(i == 1){
                        SumFlow = SumFlow + FlowList[j].comFlowT2;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT2 * CompOilList[j].Den;                        
                    }
                    if(i == 2){
                        SumFlow = SumFlow + FlowList[j].comFlowT3;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT3 * CompOilList[j].Den;                        
                    }
                    if(i == 3){
                        SumFlow = SumFlow + FlowList[j].comFlowT4;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT4 * CompOilList[j].Den;                        
                    }
                    if(i == 4){
                        SumFlow = SumFlow + FlowList[j].comFlowT5;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT5 * CompOilList[j].Den;                        
                    }
                    if(i == 5){
                        SumFlow = SumFlow + FlowList[j].comFlowT6;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT6 * CompOilList[j].Den;                        
                    }
                    if(i == 6){
                        SumFlow = SumFlow + FlowList[j].comFlowT7;
                        ProdDEN[i] = ProdDEN[i] + FlowList[j].comFlowT7 * CompOilList[j].Den;                        
                    }
                }
                ProdDEN[i] = Math.Round(ProdDEN[i] / SumFlow, 1);
            }
            #endregion

            List<Dispatch_decsScheme_prodInfo> ResultList = new List<Dispatch_decsScheme_prodInfo>();//新建一个List用来append的,返回的是list形式
            for(int i = 0; i < 4; i++){
                Dispatch_decsScheme_prodInfo Result = new Dispatch_decsScheme_prodInfo();
                Result.PropertyName = PropertyList[i].PropertyName;
                if(i == 0){
                    Result.valueHigh = ProdOilList[3].CetHighLimit;
                    Result.valueLow = ProdOilList[3].CetLowLimit;
                    Result.valueT1 = ProdCET[0].ToString("0.0");
                    Result.valueT2 = ProdCET[1].ToString("0.0");
                    Result.valueT3 = ProdCET[2].ToString("0.0");
                    Result.valueT4 = ProdCET[3].ToString("0.0");
                    Result.valueT5 = ProdCET[4].ToString("0.0");
                    Result.valueT6 = ProdCET[5].ToString("0.0");
                    Result.valueT7 = ProdCET[6].ToString("0.0");
                }
                if(i == 1){
                    Result.valueHigh = ProdOilList[3].D50HighLimit;
                    Result.valueLow = ProdOilList[3].D50LowLimit;
                    Result.valueT1 = ProdD50[0].ToString("0");
                    Result.valueT2 = ProdD50[1].ToString("0");
                    Result.valueT3 = ProdD50[2].ToString("0");
                    Result.valueT4 = ProdD50[3].ToString("0");
                    Result.valueT5 = ProdD50[4].ToString("0");
                    Result.valueT6 = ProdD50[5].ToString("0");
                    Result.valueT7 = ProdD50[6].ToString("0");
                }
                if(i == 2){
                    Result.valueHigh = ProdOilList[3].PolHighLimit;
                    Result.valueLow = ProdOilList[3].PolLowLimit;
                    Result.valueT1 = ProdPOL[0].ToString("0.00");
                    Result.valueT2 = ProdPOL[1].ToString("0.00");
                    Result.valueT3 = ProdPOL[2].ToString("0.00");
                    Result.valueT4 = ProdPOL[3].ToString("0.00");
                    Result.valueT5 = ProdPOL[4].ToString("0.00");
                    Result.valueT6 = ProdPOL[5].ToString("0.00");
                    Result.valueT7 = ProdPOL[6].ToString("0.00");
                }
                if(i == 3){
                    Result.valueHigh = ProdOilList[3].DenHighLimit;
                    Result.valueLow = ProdOilList[3].DenLowLimit;
                    Result.valueT1 = ProdDEN[0].ToString("0.0");
                    Result.valueT2 = ProdDEN[1].ToString("0.0");
                    Result.valueT3 = ProdDEN[2].ToString("0.0");
                    Result.valueT4 = ProdDEN[3].ToString("0.0");
                    Result.valueT5 = ProdDEN[4].ToString("0.0");
                    Result.valueT6 = ProdDEN[5].ToString("0.0");
                    Result.valueT7 = ProdDEN[6].ToString("0.0");
                }
                ResultList.Add(Result);
            }
            return ResultList;               
        }
    
    }
}