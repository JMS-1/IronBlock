using System;


namespace IronBlock.Blocks.Math
{
  public class MathRandomInt : IBlock
  {
    private static readonly Random rand = new Random();

    public override object Evaluate(Context context)
    {
      var from = (double)Values.Evaluate("FROM", context);
      var to = (double)Values.Evaluate("TO", context);
      return (double)rand.Next((int)System.Math.Min(from, to), (int)System.Math.Max(from, to));
    }

  }
}