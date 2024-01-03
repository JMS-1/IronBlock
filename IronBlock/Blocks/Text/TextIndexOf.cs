using System;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IronBlock.Blocks.Text
{
  public class TextIndexOf : IBlock
  {
    public override object Evaluate(Context context)
    {
      var mode = Fields.Get("END");

      var text = (Values.Evaluate("VALUE", context) ?? "").ToString();
      var term = (Values.Evaluate("FIND", context) ?? "").ToString();

      switch (mode)
      {
        case "FIRST": return (double)text.IndexOf(term) + 1;
        case "LAST": return (double)text.LastIndexOf(term) + 1;
        default: throw new ApplicationException("unknown mode");
      }
    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("VALUE", context) is not ExpressionSyntax textExpression) throw new ApplicationException($"Unknown expression for value.");

      if (Values.Generate("FIND", context) is not ExpressionSyntax findExpression) throw new ApplicationException($"Unknown expression for find.");

      var mode = Fields.Get("END");
      switch (mode)
      {
        case "FIRST": return SyntaxGenerator.MethodInvokeExpression(textExpression, nameof(string.IndexOf), findExpression);
        case "LAST": return SyntaxGenerator.MethodInvokeExpression(textExpression, nameof(string.LastIndexOf), findExpression);
        default: throw new NotSupportedException("unknown mode");
      }
    }
  }
}