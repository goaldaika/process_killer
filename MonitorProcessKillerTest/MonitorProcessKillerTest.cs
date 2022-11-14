using MonitorProcessKiller;
using NUnit.Framework;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MonitorProcessKillerTest
{
    public class Tests
    {
        [Test]
        public void ProcessKillerTest_KillProcess()
        {
            //replace the path and the name with the process path and name you wanna test
            Process.Start("C:/Program Files/Sublime Text/sublime_text.exe");
            var name = "sublime_text";
            var maxTime = 0;
            var freq = 0;
            var killer = new ProcessKiller(name, maxTime, freq);
            var res = killer.Monitoring();

            Assert.IsTrue(res);
            
        }

        [Test]
        public void ProcessKillerTest_KillProcess_LifeTimeSmallerThanMaxTime()
        {
            
            Process.Start("C:/Program Files/Sublime Text/sublime_text.exe");
            var name = "sublime_text";
            var maxTime = 999999;
            var freq = 99999;
            var killer = new ProcessKiller(name, maxTime, freq);
            var res = killer.Monitoring();


            Assert.IsFalse(res);
        }
        
        [Test]
        public void ProcessKillerTest_KillProcess_NullProcess()
        {
            
            var maxTime = 0;
            var freq = 0;
            var killer = new ProcessKiller(null, maxTime, freq);
            var res = killer.Monitoring();


            Assert.IsFalse(res);
        }

        [Test]
        public void ProcessKillerTest_WriteLog_LogNotExist_CreateLogFile()
        {
            Process.Start("C:/Program Files/Sublime Text/sublime_text.exe");
            File.Delete("ProcessKilled.xml");
            var name = "sublime_text";
            var maxTime = 0;
            var freq = 0;
            var killer = new ProcessKiller(name, maxTime, freq);

            var res = killer.Monitoring();

            XmlDocument processedData = new XmlDocument();
            processedData.Load("ProcessKilled.xml");

            var checkProcessName = processedData.SelectSingleNode("Root/Process");
            var processName = checkProcessName.Attributes["Name"].Value;

            Assert.AreEqual(name,processName);
            Assert.IsTrue(File.Exists("ProcessKilled.xml"));
            Assert.IsTrue(res); 
        }





    }
}