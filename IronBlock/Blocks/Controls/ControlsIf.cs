using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Controls
{
  public class ControlsIf : IBlock
  {
    public override object Evaluate(Context context)
    {

      var ifCount = 1;
      if (null != Mutations.GetValue("elseif"))
      {
        var elseIf = Mutations.GetValue("elseif");
        ifCount = int.Parse(elseIf) + 1;
      }

      var done = false;
      for (var i = 0; i < ifCount; i++)
      {
        if ((bool)Values.Evaluate($"IF{i}", context))
        {
          var statement = Statements.Get($"DO{i}");
          statement.Evaluate(context);
          done = true;
          break;
        }
      }

      if (!done)
      {
        if (null != Mutations.GetValue("else"))
        {
          var elseExists = Mutations.GetValue("else");
          if (elseExists == "1")
          {
            var statement = Statements.Get("ELSE");
            statement.Evaluate(context);
          }
        }
      }

      return base.Evaluate(context);
    }

    public override SyntaxNode Generate(Context context)
    {
      var ifCount = 1;
      var elseifMutation = Mutations.GetValue("elseif");
      if (!string.IsNullOrEmpty(elseifMutation))
      {
        var elseIf = elseifMutation;
        ifCount = int.Parse(elseIf) + 1;
      }

      var ifStatements = new List<IfStatementSyntax>();

      for (var i = 0; i < ifCount; i++)
      {
        if (Values.Generate($"IF{i}", context) is not ExpressionSyntax conditional)
          throw new ApplicationException($"Unknown expression for condition.");

        var statement = Statements.Get($"DO{i}");

        var ifContext = new Context() { Parent = context };
        if (statement?.Block != null)
        {
          var statementSyntax = statement.Block.GenerateStatement(ifContext);
          if (statementSyntax != null)
          {
            ifContext.Statements.Add(statementSyntax);
          }
        }

        var newIfStatement = IfStatement(conditional, Block(ifContext.Statements));
        ifStatements.Add(newIfStatement);
      }

      var elseMutation = Mutations.GetValue("else");
      if (elseMutation == "1")
      {
        var statement = Statements.Get("ELSE");

        var elseContext = new Context() { Parent = context };
        if (statement?.Block != null)
        {
          var statementSyntax = statement.Block.GenerateStatement(elseContext);
          if (statementSyntax != null)
          {
            elseContext.Statements.Add(statementSyntax);
          }
        }

        var lastIndex = ifStatements.Count - 1;
        if (lastIndex >= 0)
        {
          var lastIfStatement = ifStatements[lastIndex];
          ifStatements[lastIndex] = lastIfStatement.WithElse(ElseClause(Block(elseContext.Statements)));
        }
      }

      for (var index = ifStatements.Count - 1; index >= 0; index--)
      {
        var currentIfStatement = ifStatements[index];
        if (index > 0)
        {
          var previousIfStatement = ifStatements[index - 1];
          ifStatements[index - 1] = previousIfStatement.WithElse(ElseClause(currentIfStatement));
        }
      }

      var ifStatement = ifStatements.FirstOrDefault();

      return Statement(ifStatement, base.Generate(context), context);
    }
  }
}