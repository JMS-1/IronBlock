using System;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Text
{
  public class TextIsEmpty : IBlock
  {
    public override object Evaluate(Context context)
    {
      var text = (Values.Evaluate("VALUE", context) ?? "").ToString();

      return string.IsNullOrEmpty(text);
    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("VALUE", context) is not ExpressionSyntax textExpression) throw new ApplicationException($"Unknown expression for text.");
      return SyntaxGenerator.MethodInvokeExpression(PredefinedType(Token(SyntaxKind.StringKeyword)), nameof(string.IsNullOrEmpty), textExpression);
    }
  }
}