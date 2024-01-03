using System;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Math
{
  public class MathConstrain : IBlock
  {
    public override object Evaluate(Context context)
    {
      var value = (double)Values.Evaluate("VALUE", context);
      var low = (double)Values.Evaluate("LOW", context);
      var high = (double)Values.Evaluate("HIGH", context);

      return System.Math.Min(System.Math.Max(value, low), high);
    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("VALUE", context) is not ExpressionSyntax valueExpression)
        throw new ApplicationException($"Unknown expression for value.");

      if (Values.Generate("LOW", context) is not ExpressionSyntax lowExpression)
        throw new ApplicationException($"Unknown expression for low.");

      if (Values.Generate("HIGH", context) is not ExpressionSyntax highExpression)
        throw new ApplicationException($"Unknown expression for high.");

      return
        SyntaxGenerator.MethodInvokeExpression(
          IdentifierName(nameof(System.Math)),
          nameof(System.Math.Min),
          new[]
          {
            SyntaxGenerator.MethodInvokeExpression(
              IdentifierName(nameof(System.Math)),
              nameof(System.Math.Max),
              new [] { valueExpression, lowExpression }
            ),
            highExpression
          }
        );
    }
  }

}