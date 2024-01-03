using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Logic
{
  public class LogicOperation : IBlock
  {
    public override object Evaluate(Context context)
    {
      var a = (bool)(Values.Evaluate("A", context) ?? false);
      var b = (bool)(Values.Evaluate("B", context) ?? false);

      var op = Fields.Get("OP");

      switch (op)
      {
        case "AND": return a && b;
        case "OR": return a || b;
        default: throw new ApplicationException($"Unknown OP {op}");
      }

    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("A", context) is not ExpressionSyntax firstExpression)
        throw new ApplicationException($"Unknown expression for value A.");

      if (Values.Generate("B", context) is not ExpressionSyntax secondExpression)
        throw new ApplicationException($"Unknown expression for value B.");

      var opValue = Fields.Get("OP");

      var binaryOperator = GetBinaryOperator(opValue);
      var expression = BinaryExpression(binaryOperator, firstExpression, secondExpression);

      return ParenthesizedExpression(expression);
    }

    private static SyntaxKind GetBinaryOperator(string opValue)
    {
      switch (opValue)
      {
        case "AND": return SyntaxKind.LogicalAndExpression;
        case "OR": return SyntaxKind.LogicalOrExpression;

        default: throw new ApplicationException($"Unknown OP {opValue}");
      }
    }
  }

}