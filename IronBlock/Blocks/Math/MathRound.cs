using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IronBlock.Blocks.Math
{
  public class MathRound : IBlock
  {
    public override object Evaluate(Context context)
    {
      var op = Fields.Get("OP");
      var number = (double)Values.Evaluate("NUM", context);

      switch (op)
      {
        case "ROUND": return System.Math.Round(number);
        case "ROUNDUP": return System.Math.Ceiling(number);
        case "ROUNDDOWN": return System.Math.Floor(number);
        default: throw new ApplicationException($"Unknown OP {op}");
      }
    }

    public override SyntaxNode Generate(Context context)
    {
      var op = Fields.Get("OP");
      if (Values.Generate("NUM", context) is not ExpressionSyntax numberExpression)
        throw new ApplicationException($"Unknown expression for number.");

      switch (op)
      {
        case "ROUND": return MathSingle.MathFunction(nameof(System.Math.Round), numberExpression);
        case "ROUNDUP": return MathSingle.MathFunction(nameof(System.Math.Ceiling), numberExpression);
        case "ROUNDDOWN": return MathSingle.MathFunction(nameof(System.Math.Floor), numberExpression);
        default: throw new ApplicationException($"Unknown OP {op}");
      }
    }
  }

}