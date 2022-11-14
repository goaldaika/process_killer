using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonitorProcessKiller
{
    public class ProcessKiller
    {
        public string ProcessName { get; set; }
        public int MaximumLifeTime { get; set; }
        public int Frequency { get; set; }
        private TimeSpan LifeTime { get; set; }
        public ProcessKiller(string name, int maximumLifeTime, int frequency)
        {
            this.ProcessName = name;
            this.MaximumLifeTime = maximumLifeTime;
            this.Frequency = frequency;
        }
        public bool Monitoring()
        {
            return KillProcess();
        }
        private bool KillProcess()
        {
            if (!string.IsNullOrWhiteSpace(ProcessName))
            {
                foreach (var process in Process.GetProcessesByName(ProcessName))
                {
                    LifeTime = DateTime.Now - process.StartTime;
                    if (LifeTime.TotalMinutes >= MaximumLifeTime)
                    {

                        WriteLog();
                        process.Kill();
                        Console.WriteLine("Process " + process.ProcessName + " has run for " + LifeTime.TotalMinutes.ToString("0.00") +
                            " minutes, which is exceeded the allowed run time is "
                            + MaximumLifeTime + " minutes, process terminated !");
                        return true;
                    }
                }
            }
            return false;
        }
        public void WriteLog()
        {
            if (File.Exists("ProcessKilled.xml"))
            {
                XmlDocument processedData = new XmlDocument();
                processedData.Load("ProcessKilled.xml");

                XmlNode root = processedData.SelectSingleNode("/Root");

                XmlNode processNode = processedData.CreateElement("Process");
                root.AppendChild(processNode);

                XmlAttribute nameAttr = processedData.CreateAttribute("Name");
                nameAttr.Value = ProcessName;
                processNode.Attributes.Append(nameAttr);
                
                foreach(var process in Process.GetProcessesByName(ProcessName))
                {
                    LifeTime = DateTime.Now - process.StartTime;

                    XmlNode startTimeNode = processedData.CreateElement("StartTime");
                    XmlNode runTime = processedData.CreateElement("RunTime");
                    
                    startTimeNode.AppendChild(processedData.CreateTextNode(process.StartTime.ToString()));
                    runTime.AppendChild(processedData.CreateTextNode(LifeTime.ToString()));
                    processNode.AppendChild(startTimeNode);
                    processNode.AppendChild(runTime);
                }
                
                XmlNode killedTimeNode = processedData.CreateElement("KilledTime");
                killedTimeNode.AppendChild(processedData.CreateTextNode(DateTime.Now.ToString()));
                processNode.AppendChild(killedTimeNode);

                

                processedData.Save("ProcessKilled.xml");

            }
            else
            {
                XmlDocument processedData = new XmlDocument();
                processedData.AppendChild(processedData.CreateXmlDeclaration("1.0", "UTF-8", "no"));

                XmlNode root = processedData.CreateElement("Root");
                processedData.AppendChild(root);

                XmlNode processNode = processedData.CreateElement("Process");
                root.AppendChild(processNode);

                XmlAttribute nameAttr = processedData.CreateAttribute("Name");
                nameAttr.Value = ProcessName;
                processNode.Attributes.Append(nameAttr);

                
                foreach (var process in Process.GetProcessesByName(ProcessName))
                {
                    LifeTime = DateTime.Now - process.StartTime;

                    XmlNode startTimeNode = processedData.CreateElement("StartTime");
                    XmlNode runTime = processedData.CreateElement("RunTime");

                    startTimeNode.AppendChild(processedData.CreateTextNode(process.StartTime.ToString()));
                    runTime.AppendChild(processedData.CreateTextNode(LifeTime.ToString()));
                    processNode.AppendChild(startTimeNode);
                    processNode.AppendChild(runTime);
                }

                XmlNode killedTimeNode = processedData.CreateElement("KilledTime");
                killedTimeNode.AppendChild(processedData.CreateTextNode(DateTime.Now.ToString()));
                processNode.AppendChild(killedTimeNode);

                processedData.Save("ProcessKilled.xml");

            }
            
        }
    }
}
