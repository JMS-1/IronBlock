using System;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Text
{
  public class TextPrompt : IBlock
  {
    public override object Evaluate(Context context)
    {
      var inputType = Mutations.GetValue("type") ?? "TEXT";

      var text = (Values.Evaluate("TEXT", context) ?? "").ToString();

      if (!string.IsNullOrWhiteSpace(text))
      {
        Console.Write($"{text}: ");
      }

      var value = Console.ReadLine();

      if (inputType == "NUMBER")
      {
        return int.Parse(value);
      }

      return value;
    }

    public override SyntaxNode Generate(Context context)
    {
      var inputType = Mutations.GetValue("type") ?? "TEXT";

      if (Values.Generate("TEXT", context) is ExpressionSyntax expression)
      {
        context.Statements.Add(
          ExpressionStatement(
            SyntaxGenerator.MethodInvokeExpression(
              IdentifierName(nameof(Console)),
              nameof(Console.WriteLine),
              expression
            )
          )
        );
      }

      context.Statements.Add(
        LocalDeclarationStatement(
          VariableDeclaration(
            IdentifierName("var")
          )
          .WithVariables(
            SingletonSeparatedList(
              VariableDeclarator(
                Identifier("value")
              )
              .WithInitializer(
                EqualsValueClause(
                  SyntaxGenerator.MethodInvokeExpression(
                    IdentifierName(nameof(Console)),
                    nameof(Console.ReadLine)
                  )
                )
              )
            )
          )
        )
      );

      if (inputType == "NUMBER")
      {
        return
          SyntaxGenerator.MethodInvokeExpression(
            PredefinedType(
              Token(SyntaxKind.IntKeyword)
            ),
            nameof(int.Parse),
            IdentifierName("value")
          );
      }

      return IdentifierName("value");
    }
  }

}