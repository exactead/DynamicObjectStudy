using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicObjectStudy;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetValueTest()
        {
            object data = new { Test1 = 10, Test2 = "20", Test3 = "30" };
            dynamic record = new DynamicDataRecord(data);
            Assert.AreEqual(record.Test1, 10);
            Assert.AreEqual(record[1], "20");
            Assert.AreEqual(record["Test3"], "30");
            System.Console.WriteLine($"{record.Test1},{record[1]},{record["Test3"]}");
        }

        [TestMethod]
        public void SetValueTest()
        {
            object data = new { Test1 = 10, Test2 = "20", Test3 = "30" };
            dynamic record = new DynamicDataRecord(data);
            System.Console.WriteLine($"{record.Test1},{record[1]},{record["Test3"]}");
            record.Test1 = 20;
            record[1] = "30";
            record["Test3"] = "40";
            Assert.AreEqual(record.Test1, 20);
            Assert.AreEqual(record[1], "30");
            Assert.AreEqual(record["Test3"], "40");
            System.Console.WriteLine($"{record.Test1},{record[1]},{record["Test3"]}");
        }
    }
}
