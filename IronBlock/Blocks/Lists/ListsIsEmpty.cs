using System;
using System.Collections.Generic;
using System.Linq;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IronBlock.Blocks.Lists
{
  public class ListsIsEmpty : IBlock
  {
    public override object Evaluate(Context context)
    {
      if (Values.Evaluate("VALUE", context) is not IEnumerable<object> value) return true;

      return !value.Any();
    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("VALUE", context) is not ExpressionSyntax valueExpression) throw new ApplicationException($"Unknown expression for value.");

      return SyntaxGenerator.MethodInvokeExpression(valueExpression, nameof(Enumerable.Any));
    }
  }
}