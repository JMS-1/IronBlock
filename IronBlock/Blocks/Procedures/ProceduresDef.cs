using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace IronBlock.Blocks.Text
{
  public class ProceduresDef : IBlock
  {
    public override object Evaluate(Context context)
    {
      var name = Fields.Get("NAME");
      var statement = Statements.FirstOrDefault(x => x.Name == "STACK");

      if (string.IsNullOrWhiteSpace(name))
        return null;

      // if the statement is missing, create a stub one
      if (null == statement)
      {
        statement = new Statement
        {
          Block = null,
          Name = "STACK"
        };
      }

      // tack the return value on as a block at the end of the statement
      if (Values.Any(x => x.Name == "RETURN"))
      {
        var valueBlock = new ValueBlock(Values.First(x => x.Name == "RETURN"));
        if (statement.Block == null)
        {
          statement.Block = valueBlock;
        }
        else
        {
          FindEndOfChain(statement.Block).Next = valueBlock;
        }
      }

      if (context.Functions.ContainsKey(name))
      {
        context.Functions[name] = statement;
      }
      else
      {
        context.Functions.Add(name, statement);
      }

      return null;
    }

    public override SyntaxNode Generate(Context context)
    {
      var name = Fields.Get("NAME").CreateValidName();
      var statement = Statements.FirstOrDefault(x => x.Name == "STACK");

      if (string.IsNullOrWhiteSpace(name))
        return null;

      // if the statement is missing, create a stub one
      if (null == statement)
      {
        statement = new Statement
        {
          Block = null,
          Name = "STACK"
        };
      }

      ReturnStatementSyntax returnStatement = null;

      // tack the return value on as a block at the end of the statement
      if (Values.Any(x => x.Name == "RETURN"))
      {
        var returnValue = Values.First(x => x.Name == "RETURN");
        if (returnValue.Generate(context) is not ExpressionSyntax returnExpression)
          throw new ApplicationException($"Unknown expression for return statement.");

        returnStatement = ReturnStatement(returnExpression);
      }

      var parameters = new List<ParameterSyntax>();

      var procedureContext = new ProcedureContext() { Parent = context };

      foreach (var mutation in Mutations.Where(x => x.Domain == "arg" && x.Name == "name"))
      {
        var parameterName = mutation.Value.CreateValidName();

        var parameter = Parameter(
          Identifier(parameterName)
        )
        .WithType(
          IdentifierName("dynamic")
        );

        parameters.Add(parameter);
        procedureContext.Parameters[parameterName] = parameter;
      }

      if (statement?.Block != null)
      {
        var statementSyntax = statement.Block.GenerateStatement(procedureContext);
        if (statementSyntax != null)
        {
          procedureContext.Statements.Add(statementSyntax);
        }
      }

      if (returnStatement != null)
      {
        procedureContext.Statements.Add(returnStatement);
      }

      LocalFunctionStatementSyntax methodDeclaration = null;

      var returnType = (returnStatement == null) ? PredefinedType(Token(SyntaxKind.VoidKeyword)) : (TypeSyntax)IdentifierName("dynamic");

      methodDeclaration =
          LocalFunctionStatement(
            returnType,
            Identifier(name)
          )
          .WithBody(
            Block(procedureContext.Statements)
          );

      if (parameters.Any())
      {
        var syntaxList = SeparatedList(parameters);

        methodDeclaration =
          methodDeclaration
            .WithParameterList(
              ParameterList(syntaxList)
            );
      }

      context.Functions[name] = methodDeclaration;

      return base.Generate(context);
    }

    private static IBlock FindEndOfChain(IBlock block)
    {
      if (null == block.Next)
        return block;
      return FindEndOfChain(block.Next);
    }

    private class ValueBlock(Value value) : IBlock
    {
      private readonly Value value = value;

      public override object Evaluate(Context context)
      {
        return value.Evaluate(context);
      }
    }

  }


}