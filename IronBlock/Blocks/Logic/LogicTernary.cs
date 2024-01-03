using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Logic
{
  public class LogicTernary : IBlock
  {
    public override object Evaluate(Context context)
    {
      var ifValue = (bool)Values.Evaluate("IF", context);

      if (ifValue)
      {
        if (Values.Any(x => x.Name == "THEN"))
        {
          return Values.Evaluate("THEN", context);
        }
      }
      else
      {
        if (Values.Any(x => x.Name == "ELSE"))
        {
          return Values.Generate("ELSE", context);
        }
      }
      return null;
    }
    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("IF", context) is not ExpressionSyntax conditionalExpression)
        throw new ApplicationException($"Unknown expression for conditional statement.");

      if (Values.Generate("THEN", context) is not ExpressionSyntax trueExpression)
        throw new ApplicationException($"Unknown expression for true statement.");

      if (Values.Generate("ELSE", context) is not ExpressionSyntax falseExpression)
        throw new ApplicationException($"Unknown expression for false statement.");

      return ConditionalExpression(
            conditionalExpression,
            trueExpression,
            falseExpression
          );
    }
  }

}