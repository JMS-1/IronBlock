using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Lists
{
  public class ListsCreateWith : IBlock
  {
    public override object Evaluate(Context context)
    {
      var list = new List<object>();
      foreach (var value in Values)
      {
        list.Add(value.Evaluate(context));

      }
      return list;

    }

    public override SyntaxNode Generate(Context context)
    {
      var expressions = new List<ExpressionSyntax>();

      foreach (var value in Values)
      {
        if (value.Generate(context) is not ExpressionSyntax itemExpression)
          throw new ApplicationException($"Unknown expression for item.");

        expressions.Add(itemExpression);
      }

      return
          ObjectCreationExpression(
              GenericName(
                  Identifier(nameof(List))
              )
              .WithTypeArgumentList(
                  TypeArgumentList(
                      SingletonSeparatedList<TypeSyntax>(
                          IdentifierName("dynamic")
                      )
                  )
              )
          )
          .WithInitializer(
              InitializerExpression(
                  SyntaxKind.CollectionInitializerExpression,
                  SeparatedList(expressions)
              )
          );
    }
  }
}