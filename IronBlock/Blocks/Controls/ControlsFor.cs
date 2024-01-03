using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Controls
{
  public class ControlsFor : IBlock
  {
    public override object Evaluate(Context context)
    {
      var variableName = Fields.Get("VAR");

      var fromValue = (double)Values.Evaluate("FROM", context);
      var toValue = (double)Values.Evaluate("TO", context);
      var byValue = (double)Values.Evaluate("BY", context);

      var statement = Statements.FirstOrDefault();


      if (context.Variables.ContainsKey(variableName))
      {
        context.Variables[variableName] = fromValue;
      }
      else
      {
        context.Variables.Add(variableName, fromValue);
      }


      while ((double)context.Variables[variableName] <= toValue)
      {
        statement.Evaluate(context);
        context.Variables[variableName] = (double)context.Variables[variableName] + byValue;
      }

      return base.Evaluate(context);
    }

    public override SyntaxNode Generate(Context context)
    {
      var variableName = Fields.Get("VAR").CreateValidName();

      if (Values.Generate("FROM", context) is not ExpressionSyntax fromValueExpression) throw new ApplicationException($"Unknown expression for from value.");

      if (Values.Generate("TO", context) is not ExpressionSyntax toValueExpression) throw new ApplicationException($"Unknown expression for to value.");

      if (Values.Generate("BY", context) is not ExpressionSyntax byValueExpression) throw new ApplicationException($"Unknown expression for by value.");

      var statement = Statements.FirstOrDefault();

      var rootContext = context.GetRootContext();
      if (!rootContext.Variables.ContainsKey(variableName))
      {
        rootContext.Variables[variableName] = null;
      }

      var forContext = new Context() { Parent = context };
      if (statement?.Block != null)
      {
        var statementSyntax = statement.Block.GenerateStatement(forContext);
        if (statementSyntax != null)
        {
          forContext.Statements.Add(statementSyntax);
        }
      }

      var forStatement =
          ForStatement(
                Block(forContext.Statements)
              )
              .WithInitializers(
                SingletonSeparatedList<ExpressionSyntax>(
                  AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    IdentifierName(variableName),
                    fromValueExpression
                  )
                )
              )
              .WithCondition(
                BinaryExpression(
                  SyntaxKind.LessThanOrEqualExpression,
                  IdentifierName(variableName),
                  toValueExpression
                )
              )
              .WithIncrementors(
                SingletonSeparatedList<ExpressionSyntax>(
                  AssignmentExpression(
                    SyntaxKind.AddAssignmentExpression,
                    IdentifierName(variableName),
                    byValueExpression
                  )
                )
              );

      return Statement(forStatement, base.Generate(context), context);
    }
  }

}