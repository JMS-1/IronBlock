using IronBlock.Blocks;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IronBlock.Tests.Roslyn
{
  [TestClass]
  public class ExampleTests
  {
    [TestMethod]
    public void Test_Example1_Xml()
    {
      var xml = File.ReadAllText("../../../Examples/example1.xml");

      var output = Parser.CreateXml()
          .AddStandardBlocks()
          .Parse(xml)
          .Generate();

      var code = output.NormalizeWhitespace(string.Empty, " ").ToFullString();

      Assert.IsTrue(code.Contains("dynamic n; n = 1; for (int count = 0; count < 4; count++) { n = (n * 2); Console.WriteLine(n); }"));
    }

    [TestMethod]
    public void Test_Example1_Json()
    {
      var json = File.ReadAllText("../../../Examples/example1.json");

      var output = Parser.CreateJson()
          .AddStandardBlocks()
          .Parse(json)
          .Generate();

      var code = output.NormalizeWhitespace(string.Empty, " ").ToFullString();

      Assert.IsTrue(code.Contains("dynamic n; n = 1; for (int count = 0; count < 4; count++) { n = (n * 2); Console.WriteLine(n); }"));
    }

    [TestMethod]
    public void Test_Example2_Xml()
    {
      var xml = File.ReadAllText("../../../Examples/example2.xml");

      var output = Parser.CreateXml()
          .AddStandardBlocks()
          .Parse(xml)
          .Generate();

      var code = output.NormalizeWhitespace(string.Empty, " ").ToFullString();

      Assert.IsTrue(code.Contains("if (((6 * 7) == 42)) { Console.WriteLine(\"Don't panic\"); } else { Console.WriteLine(\"Panic\"); }"));
    }

    [TestMethod]
    public void Test_Example2_Json()
    {
      var json = File.ReadAllText("../../../Examples/example2.json");

      var output = Parser.CreateJson()
          .AddStandardBlocks()
          .Parse(json)
          .Generate();

      var code = output.NormalizeWhitespace(string.Empty, " ").ToFullString();

      Assert.IsTrue(code.Contains("if (((6 * 7) == 42)) { Console.WriteLine(\"Don't panic\"); } else { Console.WriteLine(\"Panic\"); }"));
    }

    [TestMethod]
    public void Test_Example3_Xml()
    {
      var xml = File.ReadAllText("../../../Examples/example3.xml");

      var output = Parser.CreateXml()
          .AddStandardBlocks()
          .Parse(xml)
          .Generate();

      var code = output.NormalizeWhitespace(string.Empty, " ").ToFullString();

      Assert.IsTrue(code.Contains("dynamic a; dynamic b; dynamic i; void test(dynamic x) { a = 2; b = (a * b); b = (b / 2); if ((b > 6)) { for (i = 1; i <= 10; i += 2) { a = (a * i); b = (1 + x); } }  a = (b * x); }  test(11);"));
    }

    [TestMethod]
    public void Test_Example3_Json()
    {
      var json = File.ReadAllText("../../../Examples/example3.json");

      var output = Parser.CreateJson()
          .AddStandardBlocks()
          .Parse(json)
          .Generate();

      var code = output.NormalizeWhitespace(string.Empty, " ").ToFullString();

      Assert.IsTrue(code.Contains("dynamic a; dynamic b; dynamic i; void test(dynamic x) { a = 2; b = (a * b); b = (b / 2); if ((b > 6)) { for (i = 1; i <= 10; i += 2) { a = (a * i); b = (1 + x); } }  a = (b * x); }  test(11);"));
    }

    [TestMethod]
    public void Test_Example4_Xml()
    {
      var xml = File.ReadAllText("../../../Examples/example4.xml");

      Parser.CreateXml()
          .AddStandardBlocks()
          .Parse(xml)
          .Evaluate();
    }

    [TestMethod]
    public void Test_Example4_Json()
    {
      var json = File.ReadAllText("../../../Examples/example4.json");

      Parser.CreateJson()
          .AddStandardBlocks()
          .Parse(json)
          .Evaluate();
    }


    [TestMethod]
    public void Test_Example5_Json()
    {
      var json = File.ReadAllText("../../../Examples/example5.json");

      Parser.CreateJson()
          .AddStandardBlocks()
          .Parse(json)
          .Generate();
    }

    [TestMethod]
    public void Test_Example6_Json()
    {
      var json = File.ReadAllText("../../../Examples/example6.json");

      Parser.CreateJson()
          .AddStandardBlocks()
          .Parse(json)
          .Generate();
    }

    [TestMethod]
    public void Test_Example7_Json()
    {
      var json = File.ReadAllText("../../../Examples/example7.json");

      Parser.CreateJson()
          .AddStandardBlocks()
          .Parse(json)
          .Generate();
    }

    [TestMethod]
    public void Test_Example8_Xml()
    {
      var json = File.ReadAllText("../../../Examples/example8.xml");

      var output = Parser.CreateXml()
          .AddStandardBlocks()
          .Parse(json)
          .Generate();

      var code = output.NormalizeWhitespace(string.Empty, " ").ToFullString();

      Assert.AreEqual(code, "{ dynamic X12; void bad_guy() { if ((X12 < 12)) { bad_guy(); } else if ((X12 < 13)) { bad_guy(); } else if ((X12 < 14)) { bad_guy(); } else { X12 = 2999.11263; } }  X12 = 9999; if ((X12 == 33)) { bad_guy(); } }");
    }
  }
}

