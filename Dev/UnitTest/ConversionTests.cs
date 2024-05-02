using IdcCommon.CommonMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewIDC.Projects;
using System;
using System.IO;
using System.Security.Policy;
using System.Text.Json;
using System.Xml.Serialization;

namespace UnitTestProject1 {
    [TestClass]
    public class ConversionTests {
        [TestMethod]
        public void TestMethod1() {
            ConversionBase money_conversion = new MoneyConversion(new string[] { "Money", "1", "T3", "2","￥", "1", "円", "1", "2", "1" }, 2);
            money_conversion.Convert();
            string jsonString = JsonSerializer.Serialize<object>(money_conversion);
            Assert.IsNotNull(money_conversion);
        }
        [TestMethod]
        public void TestMethod2() {
            ConversionBase money_conversion = new MoneyConversion(new string[] { "Money", "1", "T3", "2", "￥", "1", "円", "1", "2", "1" }, 2);
            ProjectConfig ProjectConfig = new ProjectConfig();
            ProjectConfig.ProjectName = "test";
            ProjectConfig.ConversionList.Add(money_conversion);
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectConfig), new Type[] { typeof(ConversionBase) });
            using (TextWriter writer = new StreamWriter(@"E:\output.ini")) {
                serializer.Serialize(writer, ProjectConfig);
            }
            string xml = File.ReadAllText(@"E:\output.ini");
            using (StringReader reader = new StringReader(xml)) {
                var project = serializer.Deserialize(reader);
            }
        }
        [TestMethod]
        public void TestMethod3() {
            ReadExcel excelReader = new ReadExcel(@"E:\work\2024\NewIdc\MoneyConversion.xlsx", false, "Sheet1", 2, 1, 3, 4, 1);
            var content =  excelReader.Read();
        }

    }
}
