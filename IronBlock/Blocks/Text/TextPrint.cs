using System;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Text
{
  public class TextPrint : IBlock
  {
    public override object Evaluate(Context context)
    {
      var text = Values.Evaluate("TEXT", context);

      Console.WriteLine(text);

      return base.Evaluate(context);
    }

    public override SyntaxNode Generate(Context context)
    {
      var syntaxNode = Values.Generate("TEXT", context);
      if (syntaxNode is not ExpressionSyntax expression)
        throw new ApplicationException($"Unknown expression for text.");

      var invocationExpression =
        SyntaxGenerator.MethodInvokeExpression(IdentifierName(nameof(Console)), nameof(Console.WriteLine), expression);

      return Statement(invocationExpression, base.Generate(context), context);
    }
  }

}