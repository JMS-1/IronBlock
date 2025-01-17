using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
namespace IronBlock.Blocks.Controls
{
  public class ControlsForEach : IBlock
  {
    public override object Evaluate(Context context)
    {
      var variableName = Fields.Get("VAR");
      var list = Values.Evaluate("LIST", context) as IEnumerable<object>;

      var statement = Statements.Where(x => x.Name == "DO").FirstOrDefault();

      if (null == statement)
        return base.Evaluate(context);

      foreach (var item in list)
      {
        if (context.Variables.ContainsKey(variableName))
        {
          context.Variables[variableName] = item;
        }
        else
        {
          context.Variables.Add(variableName, item);
        }
        statement.Evaluate(context);
      }

      return base.Evaluate(context);
    }

    public override SyntaxNode Generate(Context context)
    {
      var variableName = Fields.Get("VAR").CreateValidName();
      if (Values.Generate("LIST", context) is not ExpressionSyntax listExpression)
        throw new ApplicationException($"Unknown expression for list.");

      var statement = Statements.Where(x => x.Name == "DO").FirstOrDefault();

      if (null == statement)
        return base.Generate(context);

      var forEachContext = new Context() { Parent = context };
      if (statement?.Block != null)
      {
        var statementSyntax = statement.Block.GenerateStatement(forEachContext);
        if (statementSyntax != null)
        {
          forEachContext.Statements.Add(statementSyntax);
        }
      }

      var forEachStatement =
          ForEachStatement(
              IdentifierName("var"),
              Identifier(variableName),
              listExpression,
              Block(
                forEachContext.Statements
              )
            );

      return Statement(forEachStatement, base.Generate(context), context);
    }
  }

}