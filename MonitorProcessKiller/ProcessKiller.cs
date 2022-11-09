using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MonitorProcessKiller
{
    internal class ProcessKiller
    {
        public string ProcessName { get; set; }
        public int MaximumLifeTime { get; set; }
        public int Frequency { get; set; }
        public ProcessKiller(string name, int maximumLifeTime, int frequency)
        {
            this.ProcessName = name;
            this.MaximumLifeTime = maximumLifeTime;
            this.Frequency = frequency;
        }
        public bool KillProcess()
        {
            TimeSpan lifeTime = default;

            foreach (var process in Process.GetProcessesByName(ProcessName))
            {
                lifeTime = DateTime.Now - process.StartTime;
                if (lifeTime.TotalMinutes >= MaximumLifeTime)
                {
                    process.Kill();
                    WriteLog();
                }
            }

            return true;
        }
        public bool WriteLog()
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
                    XmlNode startTimeNode = processedData.CreateElement("StartTime");
                    startTimeNode.AppendChild(processedData.CreateTextNode(process.StartTime.ToString()));
                    processNode.AppendChild(startTimeNode);
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
                    XmlNode startTimeNode = processedData.CreateElement("StartTime");
                    startTimeNode.AppendChild(processedData.CreateTextNode(process.StartTime.ToString()));
                    processNode.AppendChild(startTimeNode);
                }


                XmlNode killedTimeNode = processedData.CreateElement("KilledTime");
                killedTimeNode.AppendChild(processedData.CreateTextNode(DateTime.Now.ToString()));
                processNode.AppendChild(killedTimeNode);

                processedData.Save("ProcessKilled.xml");


            }
            return true;
        }
    }
}
