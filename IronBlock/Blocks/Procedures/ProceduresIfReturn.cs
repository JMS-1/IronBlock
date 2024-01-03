using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Text
{
  public class ProceduresIfReturn : IBlock
  {
    public override object Evaluate(Context context)
    {
      var condition = Values.Evaluate("CONDITION", context);
      if ((bool)condition)
      {
        return Values.Evaluate("VALUE", context);
      }

      return base.Evaluate(context);
    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("CONDITION", context) is not ExpressionSyntax condition) throw new ApplicationException($"Unknown expression for condition.");

      var returnStatement = ReturnStatement();

      if (Values.Any(x => x.Name == "VALUE"))
      {
        if (Values.Generate("VALUE", context) is not ExpressionSyntax statement) throw new ApplicationException($"Unknown expression for return statement.");

        returnStatement = ReturnStatement(statement);
      }

      var ifStatement = IfStatement(condition, returnStatement);
      return Statement(ifStatement, base.Generate(context), context);
    }
  }
}