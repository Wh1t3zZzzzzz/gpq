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
        // string CMD_OutPut;
        public Dispatch(oilblendContext _context)
        {
           //oilblendContext _context = new();
           context = _context;
        }


        public void RunPythonScript()//string sArgName, string args = ""
        {
            string sArgName = @"diesel.py";
            string args = "-u";
            Process p = new Process();
            //string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + sArgName;// 获得python文件的绝对路径（将文件放在c#的debug文件夹中可以这样操作）
            //string path = "diesel_combsql.py";//D:\GASHLCY_new\GASHLCY  D:\diesel\
            string path = @"E:\Python_env\" + sArgName;//(因为我没放debug下，所以直接写的绝对路径,替换掉上面的路径了)
            p.StartInfo.FileName = @"E:\Python_env\venv\Scripts\python.exe";//(注意：用的话需要换成自己的)没有配环境变量的话，可以像我这样写python.exe的绝对路径(用的话需要换成自己的)。如果配了，直接写"python.exe"即可
            string sArguments = path;

            sArguments += " " + args;

            p.StartInfo.Arguments = sArguments;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardInput = true;

            // p.StartInfo.RedirectStandardError = true;

            p.StartInfo.CreateNoWindow = true;

            p.Start();

            // p.BeginOutputReadLine();

            // string output = p.StandardOutput.ReadToEnd();

            // // p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);

            // // Console.ReadLine();

            p.WaitForExit();

            // Console.Write(output);//输出

            // CMD_OutPut = output;

            // CMD_OutPut = output.TrimEnd((char[])"\r\n".ToCharArray());//去掉末尾的换行符
            
            // CMD_OutPut = CMD_OutPut.Replace(" ", "");//剔除字符串中的空格

            //textStr = Regex.Replace(textStr, @"[/n/r]", ""); 
                  
            // iNums = Array.ConvertAll<string, int>(CMD_OutPut , int.Parse);

            // p.Close(); 

        }


        public IEnumerable<DispatchComFlowRes> GetDispatchComFlowRes1(){

            RunPythonScript();

            string[] flow_strs = File.ReadAllLines(@"E:\Python_env\Flow_txt.txt");//这个字符串数组只有一行
            Console.WriteLine(flow_strs[0]);
            Console.WriteLine(flow_strs[0]);
            // Console.WriteLine(flow_strs[1]);
            // Console.WriteLine(flow_strs[2]);

            string flow_str = string.Join("", flow_strs);//字符串数组转成字符串

            // FileStream fileStream = new FileStream(filePath, FileMode.Open);
            // StreamReader sr = new StreamReader(fileStream);
            // string line;
            // while((line=sr.ReadLine())!=null)
            // {
            //     Console.WriteLine(line.ToString());
            // }
            flow_strs = flow_str.Split(',');//字符串转化为字符串数组
            // CMD_OutPut.TrimStart();
            // CMD_OutPut.Trim();
            // CMD_OutPut.TrimEnd();
            // string[] CMD_OutPutList = CMD_OutPut.Split(new char[]{',','[',']'});//字符串转化为字符串数组

            // // CMD_OutPutList[0] = Regex.Replace(CMD_OutPutList[0], @"[\r\n]", ""); 
            // // CMD_OutPutList[CMD_OutPutList.Length - 1] = Regex.Replace(CMD_OutPutList[CMD_OutPutList.Length - 1], @"[\[\]]", ""); 

            // // 在这一块换成，将所有字符数组里的空格去掉

            // CMD_OutPutList[0] = CMD_OutPutList[0].Replace("[","");
            // CMD_OutPutList[CMD_OutPutList.Length - 1] = CMD_OutPutList[CMD_OutPutList.Length - 1].Replace("]","");

            // CMD_OutPutList = CMD_OutPutList.Where(s => !string.IsNullOrEmpty(s)).ToArray();
            // textStr = Regex.Replace(textStr, @"[/n/r]", ""); 

            int [] iNums = Array.ConvertAll(flow_strs, int.Parse);

            // Console.Write(iNums);

            List<DispatchComFlowRes> ResultList = new List<DispatchComFlowRes>();//新建一个List用来append的,返回的是list形式
            var comFlowList = context.Dispatchcomflows.ToList();
            var dispatchWeight = context.Dispatchweights.ToList();
            var comList = context.Compoilconfigs.ToList();

            for(int i = 0; i < comFlowList.Count / 2; i++){
                DispatchComFlowRes Result = new DispatchComFlowRes();//
                Result.ComOilNum = comFlowList.Count / 2;
                Result.Time = (int)dispatchWeight[0].Weight;// Convert.ToInt32
                // Result.ComOilName = comList[i].ComOilName;
                // Result.ComOilName = CMD_OutPutList[4];
                Result.ComOilFlow1 = comFlowList[i].ComFlowT1;
                Result.ComOilFlow2 = comFlowList[i].ComFlowT2;
                Result.ComOilFlow3 = comFlowList[i].ComFlowT3;
                Result.ComOilFlow4 = comFlowList[i].ComFlowT4;
                Result.ComOilFlow5 = comFlowList[i].ComFlowT5;
                Result.ComOilFlow6 = comFlowList[i].ComFlowT6;
                Result.ComOilFlow7 = comFlowList[i].ComFlowT7;
                ResultList.Add(Result);
            }

            return ResultList;
        }

        // void DispatchDecision();
        // IEnumerable<DispatchComFlowRes> GetDispatchComFlowRes();
    }
}