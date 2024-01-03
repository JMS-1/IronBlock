namespace IronBlock.Tests
{

  internal static class TestExtensions
  {
    internal class DebugPrint : IBlock
    {
      public static List<string> Text { get; set; }

      static DebugPrint()
      {
        Text = [];
      }

      public override object Evaluate(Context context)
      {
        Text.Add((Values.First(x => x.Name == "TEXT").Evaluate(context) ?? "").ToString()!);
        return base.Evaluate(context);
      }
    }

    internal static IList<string> GetDebugText()
    {
      return DebugPrint.Text;
    }

    internal static T AddDebugPrinter<T>(this T parser) where T : Parser<T>
    {
      DebugPrint.Text.Clear();

      parser.AddBlock<DebugPrint>("text_print");
      return parser;
    }

  }
}