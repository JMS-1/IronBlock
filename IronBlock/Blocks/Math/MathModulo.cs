using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Math
{
  public class MathModulo : IBlock
  {
    public override object Evaluate(Context context)
    {
      var dividend = (double)Values.Evaluate("DIVIDEND", context);
      var divisor = (double)Values.Evaluate("DIVISOR", context);

      return dividend % divisor;
    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("DIVIDEND", context) is not ExpressionSyntax dividendExpression)
        throw new ApplicationException($"Unknown expression for dividend.");

      if (Values.Generate("DIVISOR", context) is not ExpressionSyntax divisorExpression)
        throw new ApplicationException($"Unknown expression for divisor.");

      return BinaryExpression(
        SyntaxKind.ModuloExpression,
        dividendExpression,
        divisorExpression
      );
    }
  }
}