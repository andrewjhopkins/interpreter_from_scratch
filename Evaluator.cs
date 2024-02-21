using interpreter_from_scratch.Ast;
using interpreter_from_scratch.Evaluation;

namespace interpreter_from_scratch;

public class Evaluator
{
    public InterpreterObject Evaluate(InterpreterProgram program, EnvironmentVariables environmentVariables)
    {
        InterpreterObject result = null;

        for (var i = 0; i < program.Statements.Count(); i++)
        {
            result = Evaluate(program.Statements.ElementAt(i), environmentVariables);
        }

        return result;
    }

    public InterpreterObject Evaluate(Statement statement, EnvironmentVariables environmentVariables)
    {
        switch (statement)
        {
            case ExpressionStatement expressionStatement:
               return Evaluate(expressionStatement.Expression, environmentVariables);
            case Var varStatement:
                var value = Evaluate(varStatement.Value, environmentVariables);
                environmentVariables.Variables.Add(varStatement.Identifier.Value, value);
                break;
            case Return returnStatement:
                var returnValue = Evaluate(returnStatement.Value, environmentVariables);
                return new ReturnObject(returnValue);
            case IfElse ifElseStatement:
                return EvaluateIfElse(ifElseStatement, environmentVariables);
            case Function functionStatement:
                environmentVariables.Variables.Add(functionStatement.Identifier.Value, new FunctionObject(functionStatement.Body, functionStatement.Parameters, environmentVariables));
                break;
            case Block blockStatement:
                return EvaluateBlock(blockStatement, environmentVariables);
            default:
                return null;
        }

        return null;
    }

    public InterpreterObject Evaluate(Expression expression, EnvironmentVariables environmentVariables)
    {
        switch(expression)
        {
            case Integer integer:
                return new IntegerObject(integer.Value);
            case Bool boolean:
                return new BoolObject(boolean.Value);
            case BinaryExpression binaryExpression:
                var left = Evaluate(binaryExpression.Left, environmentVariables);
                var right = Evaluate(binaryExpression.Right, environmentVariables);
                return EvaluateBinaryExpression(left, right, binaryExpression.Operation);
            case FunctionCall functionCall:
                var function = Evaluate(functionCall.Function, environmentVariables);
                var parameters = Evaluate(functionCall.Parameters, environmentVariables);
                if (function is FunctionObject functionObject)
                {
                    return ApplyFunction(functionObject, parameters);
                }

                throw new Exception($"Expected a function. Got {function.Type}");

            case Identifier identifier:
                return EvaluateIdentifier(identifier, environmentVariables);
            default:
                return null;
        }
    }

    public IEnumerable<InterpreterObject> Evaluate(IEnumerable<Expression> expressions, EnvironmentVariables environmentVariables)
    {
        var result = new List<InterpreterObject>();

        foreach (var expression in expressions)
        {
            result.Add(Evaluate(expression, environmentVariables));
        }

        return result;
    }

    private InterpreterObject EvaluateBinaryExpression(InterpreterObject left, InterpreterObject right, TokenType operation)
    {
        if (left is ReturnObject leftReturnObject)
        {
            left = leftReturnObject.Value;
        }

        if (right is ReturnObject rightReturnObject)
        {
            right = rightReturnObject.Value;
        }

        if (left is not IntegerObject || right is not IntegerObject)
        {
            throw new Exception($"Only integers are capable of binary expressions. Got {left.GetType()}, {right.GetType()}");
        }

        var leftIneger = (IntegerObject)left;
        var rightInteger = (IntegerObject)right;

        switch(operation)
        {
            case TokenType.PLUS:
                return new IntegerObject(leftIneger.Value + rightInteger.Value);
            case TokenType.MINUS:
                return new IntegerObject(leftIneger.Value - rightInteger.Value);
            case TokenType.ASTERISK:
                return new IntegerObject(leftIneger.Value * rightInteger.Value);
            case TokenType.SLASH:
                return new IntegerObject(leftIneger.Value / rightInteger.Value);
            case TokenType.GREATERTHAN:
                return new BoolObject(leftIneger.Value > rightInteger.Value);
            case TokenType.LESSTHAN:
                return new BoolObject(leftIneger.Value < rightInteger.Value);
            case TokenType.EQUALS:
                return new BoolObject(leftIneger.Value == rightInteger.Value);
            case TokenType.DOESNOTEQUAL:
                return new BoolObject(leftIneger.Value != rightInteger.Value);
            default:
                return null;
        }
    }

    private InterpreterObject EvaluateIdentifier(Identifier identifier, EnvironmentVariables environmentVariables)
    {
        if (environmentVariables.Variables.ContainsKey(identifier.Value))
        {
            return environmentVariables.Variables[identifier.Value];
        }

        throw new Exception($"Identifier {identifier.Value} not found");
    }

    private InterpreterObject EvaluateIfElse(IfElse ifElse, EnvironmentVariables environmentVariables)
    {
        var condition = Evaluate(ifElse.Condition, environmentVariables);

        if (condition is not BoolObject)
        {
            throw new Exception($"Condition must be a boolean. Got {condition.GetType()}");
        }

        var boolCondition = (BoolObject)condition;

        if (boolCondition.Value)
        {
            return Evaluate(ifElse.Consequence, environmentVariables);
        }
        else if (ifElse.Alternative != null)
        {
            return Evaluate(ifElse.Alternative, environmentVariables);
        }

        return null;
    }

    private InterpreterObject EvaluateBlock(Block block, EnvironmentVariables environmentVariables) 
    {
        InterpreterObject result = null;

        for (var i = 0; i < block.Statements.Count(); i++)
        {
            result = Evaluate(block.Statements.ElementAt(i), environmentVariables);
            if (result != null && result.Type == InterpreterObjectType.RETURNVALUE)
            {
                return result;
            }
        }

        return result;
    }

    private InterpreterObject ApplyFunction(FunctionObject function, IEnumerable<InterpreterObject> parameters)
    {
        if (function.Parameters.Count() != parameters.Count())
        {
            throw new Exception($"Expected {function.Parameters.Count()} parameters. Got {parameters.Count()}");
        }

        var extendedEnvironment = new EnvironmentVariables(function.EnvironmentVariables);

        for (var i = 0; i < function.Parameters.Count(); i++)
        {
            extendedEnvironment.Variables.Add(function.Parameters.ElementAt(i).Value, parameters.ElementAt(i));
        }

        var evaluator = new Evaluator();
        return evaluator.Evaluate(function.Body, extendedEnvironment);
    }
}