using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Controls
{
  public class ControlsRepeatExt : IBlock
  {
    public override object Evaluate(Context context)
    {
      var timesValue = (double)Values.Evaluate("TIMES", context);

      if (!Statements.Any(x => x.Name == "DO"))
        return base.Evaluate(context);

      var statement = Statements.Get("DO");

      for (var i = 0; i < timesValue; i++)
      {
        statement.Evaluate(context);

        if (context.EscapeMode == EscapeMode.Break)
        {
          context.EscapeMode = EscapeMode.None;
          break;
        }

        context.EscapeMode = EscapeMode.None;
      }

      context.EscapeMode = EscapeMode.None;

      return base.Evaluate(context);
    }

    public override SyntaxNode Generate(Context context)
    {
      if (Values.Generate("TIMES", context) is not ExpressionSyntax timesExpression)
        throw new ApplicationException($"Unknown expression for times.");

      if (!Statements.Any(x => x.Name == "DO"))
        return base.Generate(context);

      var statement = Statements.Get("DO");

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
              .WithDeclaration(
                VariableDeclaration(
                  PredefinedType(
                    Token(SyntaxKind.IntKeyword)
                  )
                )
                .WithVariables(
                  SingletonSeparatedList<VariableDeclaratorSyntax>(
                    VariableDeclarator(
                      Identifier("count")
                    )
                    .WithInitializer(
                      EqualsValueClause(
                        LiteralExpression(
                          SyntaxKind.NumericLiteralExpression,
                          Literal(0)
                        )
                      )
                    )
                  )
                )
              )
              .WithCondition(
                BinaryExpression(
                  SyntaxKind.LessThanExpression,
                  IdentifierName("count"),
                  timesExpression
                )
              )
              .WithIncrementors(
                SingletonSeparatedList<ExpressionSyntax>(
                  PostfixUnaryExpression(
                    SyntaxKind.PostIncrementExpression,
                    IdentifierName("count")
                  )
                )
              );

      return Statement(forStatement, base.Generate(context), context);
    }
  }

}