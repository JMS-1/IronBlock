using System;

namespace IronBlock.Blocks.Math
{
  public class MathRandomFloat : IBlock
  {
    private static Random rand = new Random();

    public override object Evaluate(Context context)
    {
      return rand.NextDouble();
    }

  }
}