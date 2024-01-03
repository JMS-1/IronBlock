using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IronBlock.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Lists
{
  public class ListsSplit : IBlock
  {
    public override object Evaluate(Context context)
    {
      var mode = Fields.Get("MODE");
      var input = Values.Evaluate("INPUT", context);
      var delim = Values.Evaluate("DELIM", context);

      switch (mode)
      {
        case "SPLIT":
          return input
              .ToString()
              .Split(new string[] { delim.ToString() }, StringSplitOptions.None)
              .Select(x => x as object)
              .ToList();

        case "JOIN":
          return string
              .Join(delim.ToString(), (input as IEnumerable<object>)
              .Select(x => x.ToString()));

        default:
          throw new NotSupportedException($"unknown mode: {mode}");

      }
    }

    public override SyntaxNode Generate(Context context)
    {
      var mode = Fields.Get("MODE");
      if (Values.Generate("INPUT", context) is not ExpressionSyntax inputExpression)
        throw new ApplicationException($"Unknown expression for input.");

      if (Values.Generate("DELIM", context) is not ExpressionSyntax delimExpression)
        throw new ApplicationException($"Unknown expression for delim.");

      switch (mode)
      {
        case "SPLIT":
          return
              SyntaxGenerator.MethodInvokeExpression(
                  SyntaxGenerator.MethodInvokeExpression(
                      inputExpression,
                      nameof(object.ToString),
                      SyntaxGenerator.PropertyAccessExpression(
                          IdentifierName(nameof(CultureInfo)),
                          nameof(CultureInfo.InvariantCulture)
                      )
                  ),
                  nameof(string.Split),
                  delimExpression
              );

        case "JOIN":
          return
              SyntaxGenerator.MethodInvokeExpression(
                  PredefinedType(
                      Token(SyntaxKind.StringKeyword)
                  ),
                  nameof(string.Join),
                  new[] { delimExpression, inputExpression }
              );

        default:
          throw new NotSupportedException($"unknown mode: {mode}");
      }
    }
  }
}