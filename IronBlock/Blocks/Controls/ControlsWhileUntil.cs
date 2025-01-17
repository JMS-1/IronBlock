using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Controls
{
  public class ControlsWhileUntil : IBlock
  {
    public override object Evaluate(Context context)
    {
      var mode = Fields.Get("MODE");
      var value = Values.FirstOrDefault(x => x.Name == "BOOL");

      if (!Statements.Any(x => x.Name == "DO") || null == value)
        return base.Evaluate(context);

      var statement = Statements.Get("DO");

      if (mode == "WHILE")
      {
        while ((bool)value.Evaluate(context))
        {
          if (context.EscapeMode == EscapeMode.Break)
          {
            context.EscapeMode = EscapeMode.None;
            break;
          }
          statement.Evaluate(context);
        }
      }
      else
      {
        while (!(bool)value.Evaluate(context))
        {
          statement.Evaluate(context);
        }
      }

      return base.Evaluate(context);
    }

    public override SyntaxNode Generate(Context context)
    {
      var mode = Fields.Get("MODE");
      var value = Values.FirstOrDefault(x => x.Name == "BOOL");

      if (!Statements.Any(x => x.Name == "DO") || null == value)
        return base.Generate(context);

      var statement = Statements.Get("DO");

      if (value.Generate(context) is not ExpressionSyntax conditionExpression)
        throw new ApplicationException($"Unknown expression for condition.");

      var whileContext = new Context() { Parent = context };
      if (statement?.Block != null)
      {
        var statementSyntax = statement.Block.GenerateStatement(whileContext);
        if (statementSyntax != null)
        {
          whileContext.Statements.Add(statementSyntax);
        }
      }

      if (mode != "WHILE")
      {
        conditionExpression = PrefixUnaryExpression(SyntaxKind.LogicalNotExpression, conditionExpression);
      }

      var whileStatement =
          WhileStatement(
            conditionExpression,
            Block(whileContext.Statements)
          );

      return Statement(whileStatement, base.Generate(context), context);
    }
  }

}