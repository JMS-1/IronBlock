using IronBlock.Blocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IronBlock.Tests
{
  [TestClass]
  public class ExampleTests
  {
    [TestMethod]
    public void Test_Example1_Xml()
    {
      var xml = File.ReadAllText("../../../Examples/example1.xml");

      var parser = Parser.CreateXml();

      parser.AddStandardBlocks();
      parser.AddDebugPrinter();

      parser.Parse(xml).Evaluate();

      Assert.AreEqual("2,4,8,16", string.Join(",", TestExtensions.GetDebugText()));
    }

    [TestMethod]
    public void Test_Example1_Json()
    {
      var json = File.ReadAllText("../../../Examples/example1.json");

      var parser = Parser.CreateJson();

      parser.AddStandardBlocks();
      parser.AddDebugPrinter();

      parser.Parse(json).Evaluate();

      Assert.AreEqual("2,4,8,16", string.Join(",", TestExtensions.GetDebugText()));
    }

    [TestMethod]
    public void Test_Example2_Xml()
    {
      var xml = File.ReadAllText("../../../Examples/example2.xml");
      var parser = Parser.CreateXml();

      parser.AddStandardBlocks();
      parser.AddDebugPrinter();

      parser.Parse(xml).Evaluate();

      Assert.AreEqual("Don't panic", TestExtensions.GetDebugText().First());
    }

    [TestMethod]
    public void Test_Example2_Json()
    {
      var json = File.ReadAllText("../../../Examples/example2.json");

      var parser = Parser.CreateJson();

      parser.AddStandardBlocks();
      parser.AddDebugPrinter();

      parser.Parse(json).Evaluate();

      Assert.AreEqual("Don't panic", TestExtensions.GetDebugText().First());
    }
  }
}
