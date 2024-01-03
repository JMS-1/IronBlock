using System;
using System.Collections.Generic;
using System.Linq;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Lists
{
  public class ListsRepeat : IBlock
  {
    public override object Evaluate(Context context)
    {
      var item = Values.Evaluate("ITEM", context);
      var num = (double)Values.Evaluate("NUM", context);

      var list = new List<object>();
      for (var i = 0; i < num; i++)
      {
        list.Add(item);

      }
      return list;

    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("ITEM", context) is not ExpressionSyntax itemExpression) throw new ApplicationException($"Unknown expression for item.");

      if (Values.Generate("NUM", context) is not ExpressionSyntax numExpression) throw new ApplicationException($"Unknown expression for number.");

      return SyntaxGenerator.MethodInvokeExpression(
          SyntaxGenerator.MethodInvokeExpression(
              IdentifierName(nameof(Enumerable)),
              nameof(Enumerable.Repeat),
              new[] { itemExpression, numExpression }
          ),
          nameof(Enumerable.ToList)
      );
    }
  }
}