using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Yaeher.Common
{
   public class AMRFileHelper
    {
        public StringBuilder result = new StringBuilder(); // Store output text of ffmpeg
        /// <summary>        
        ///         
        /// </summary>        
        /// <param name="FromName">需要转换的名字amr</param>        
        public void ConvertMP3(string filedir,string timestamp, string FromName)
        {
            string ExportName = System.IO.Path.ChangeExtension(FromName, ".mp3");
            string FromNamePath = filedir+ timestamp + "\\" + FromName;
            string ExportNamePath = filedir+ timestamp+"\\" + ExportName;
            //如果存在 不转换            
            if (File.Exists(ExportNamePath) == true)
            {
                //File.Create(ExportNamePath);                
                return;
            }
            Process p = new Process();
            //建立外部调用线程            
            //p.StartInfo.FileName = @"D:\ffmpeg.exe";//要调用外部程序的绝对路径            
            p.StartInfo.FileName = filedir+"ffmpeg.exe";
            p.StartInfo.Arguments = "-i  " + FromNamePath + "  " + ExportNamePath;
            //+ "  -i";//参数(这里就是FFMPEG的参数了)            
            p.StartInfo.UseShellExecute = false;
            //不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)            
            p.StartInfo.RedirectStandardError = true;
            //把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的这是我耗费了2个多月得出来的经验mencoder就是用standardOutput来捕获的)            
            p.StartInfo.CreateNoWindow = false;//不创建进程窗口            
            p.ErrorDataReceived += new DataReceivedEventHandler(Output);//外部程序(这里是FFMPEG)输出流时候产生的事件,这里是把流的处理过程转移到下面的方法中,详细请查阅MSDN            
            p.Start();//启动线程            
            p.BeginErrorReadLine();//开始异步读取            
            p.WaitForExit();//阻塞等待进程结束            
            p.Close();//关闭进程            
            p.Dispose();//释放资源        
        }
        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (!String.IsNullOrEmpty(output.Data))
            {
                //处理方法                
                //cc = cc + "\n" + output.Data;//输出结果的提示            
                //Console.WriteLine("提示："+output.Data);
                result.Append(output.Data);
            }
        }
        // Match the 'Duration' section in "ffmpeg -i filepath" output text
        public string MatchDuration(string text)
        {
            string pattern = @"Duration:\s(\d{2}:\d{2}:\d{2}.\d+)";
            Match m = Regex.Match(text, pattern);

            return m.Groups.Count == 2 ? m.Groups[1].ToString() : string.Empty;
        }
    }
}
