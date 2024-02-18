using interpreter_from_scratch;
using interpreter_from_scratch.Ast;

namespace interpreter_from_scratch_test
{
    public class ParserTests
    {
        [TestCase("var x = 5")]
        [TestCase("var   x    = testing24")]
        [TestCase("return test")]
        [TestCase("return 23")]
        [TestCase("2 * 5")]
        public void TestParseStatementsExpectThrowIfNoSemicolon(string input)
        { 
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);

            Assert.Throws<Exception>(() => parser.ParseProgram());
        }

        [TestCase("var x = 5;", "x", 5)]
        [TestCase("var   y    =    10;", "y", 10)]
        [TestCase("var foobar24 = 13123;", "foobar24", 13123)]
        [TestCase("var TEST = 39;", "TEST", 39)]
        public void TestParseVarStatementWithInteger(string input, string identifier, int value)
        {
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement.Token.Type, Is.EqualTo(TokenType.VAR));
            Assert.That(statement, Is.InstanceOf<Var>());

            var varStatement = (Var)statement;

            Assert.That(varStatement.Identifier.Value, Is.EqualTo(identifier));
            Assert.That(varStatement.Value, Is.InstanceOf<Integer>());

            var integer = (Integer)varStatement.Value;
            Assert.That(integer.Value, Is.EqualTo(value));
        }

        [TestCase("var x = y;", "x", "y")]
        [TestCase("var   y    =    testing24;", "y", "testing24")]
        public void TestParseVarStatementWithIdentifier(string input, string identifier, string value)
        {
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement.Token.Type, Is.EqualTo(TokenType.VAR));
            Assert.That(statement, Is.InstanceOf<Var>());

            var varStatement = (Var)statement;

            Assert.That(varStatement.Identifier.Value, Is.EqualTo(identifier));

            Assert.That(varStatement.Value, Is.InstanceOf<Identifier>());

            var valueIdentifier = (Identifier)varStatement.Value;
            Assert.That(valueIdentifier.Value, Is.EqualTo(value));
        }

        [TestCase("var x = true;", "x", true)]
        [TestCase("var   y    =    false;", "y", false)]
        public void TestParseVarStatementWithIdentifier(string input, string identifier, bool value)
        {
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement.Token.Type, Is.EqualTo(TokenType.VAR));
            Assert.That(statement, Is.InstanceOf<Var>());

            var varStatement = (Var)statement;

            Assert.That(varStatement.Identifier.Value, Is.EqualTo(identifier));

            Assert.That(varStatement.Value, Is.InstanceOf<Bool>());

            var boolean = (Bool)varStatement.Value;
            Assert.That(boolean.Value, Is.EqualTo(value));
        }

        [TestCase("return 5;", 5)]
        [TestCase("return       1234;", 1234)]
        public void TestParseReturnStatementWithInteger(string input, int value)
        { 
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement.Token.Type, Is.EqualTo(TokenType.RETURN));
            Assert.That(statement, Is.InstanceOf<Return>());

            var returnStatement = (Return)statement;

            Assert.That(returnStatement.Value, Is.InstanceOf<Integer>());
            var integer = (Integer)returnStatement.Value;
            Assert.That(integer.Value, Is.EqualTo(value));
        }

        [TestCase("return true;", true)]
        [TestCase("return       false;", false)]
        public void TestParseReturnStatementWithBoolean(string input, bool value)
        { 
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement.Token.Type, Is.EqualTo(TokenType.RETURN));
            Assert.That(statement, Is.InstanceOf<Return>());

            var returnStatement = (Return)statement;

            Assert.That(returnStatement.Value, Is.InstanceOf<Bool>());
            var boolean = (Bool)returnStatement.Value;
            Assert.That(boolean.Value, Is.EqualTo(value));
        }

        [TestCase("5 + 4;", 5, TokenType.PLUS, 4)]
        [TestCase("123 - 321;", 123, TokenType.MINUS, 321)]
        [TestCase("123 / 321;", 123, TokenType.SLASH, 321)]
        [TestCase("123 * 321;", 123, TokenType.ASTERISK, 321)]
        [TestCase("123 == 321;", 123, TokenType.EQUALS, 321)]
        [TestCase("123 != 321;", 123, TokenType.DOESNOTEQUAL, 321)]
        [TestCase("123 > 321;", 123, TokenType.GREATERTHAN, 321)]
        [TestCase("123 < 321;", 123, TokenType.LESSTHAN, 321)]
        public void TestParseBinaryExpression(string input, int left, TokenType operation, int right)
        { 
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement, Is.InstanceOf<ExpressionStatement>());
            var expressionStatement = (ExpressionStatement)statement;

            Assert.That(expressionStatement.Expression, Is.InstanceOf<BinaryExpression>());
            var binaryExpression = (BinaryExpression)expressionStatement.Expression;

            Assert.That(binaryExpression.Left, Is.InstanceOf<Integer>());
            var leftInteger = (Integer)binaryExpression.Left;
            Assert.That(leftInteger.Value, Is.EqualTo(left));

            Assert.That(binaryExpression.Operation, Is.EqualTo(operation));

            Assert.That(binaryExpression.Right, Is.InstanceOf<Integer>());
            var rightInteger = (Integer)binaryExpression.Right;
            Assert.That(rightInteger.Value, Is.EqualTo(right));
        }

        [Test]
        public void TestParseIfElseStatement()
        {
            var input = @"
                if (x < y) {
                    return 5 * 5;
                } else {
                    return 5 * 10;
                }
            ";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement, Is.InstanceOf<IfElse>());
            var ifElseStatement = (IfElse)statement;

            Assert.That(ifElseStatement.Condition, Is.InstanceOf<BinaryExpression>());
            var binaryExpression = (BinaryExpression)ifElseStatement.Condition;
            Assert.That(binaryExpression.Left, Is.InstanceOf<Identifier>());
            var leftIdentifier = (Identifier)binaryExpression.Left;
            Assert.That(leftIdentifier.Value, Is.EqualTo("x"));

            Assert.That(binaryExpression.Operation, Is.EqualTo(TokenType.LESSTHAN));

            Assert.That(binaryExpression.Right, Is.InstanceOf<Identifier>());
            var rightIdentifier = (Identifier)binaryExpression.Right;
            Assert.That(rightIdentifier.Value, Is.EqualTo("y"));

            Assert.That(ifElseStatement.Consequence.Statements.Count(), Is.EqualTo(1));
            var consequenceStatement = ifElseStatement.Consequence.Statements.First();
            Assert.That(consequenceStatement, Is.InstanceOf<Return>());
            var consequenceReturn = (Return)consequenceStatement;
            Assert.That(consequenceReturn.Value, Is.InstanceOf<BinaryExpression>());

            var consequenceBinaryExpression = (BinaryExpression)consequenceReturn.Value;
            Assert.That(consequenceBinaryExpression.Left, Is.InstanceOf<Integer>());
            var consequenceLeftInteger = (Integer)consequenceBinaryExpression.Left;
            Assert.That(consequenceLeftInteger.Value, Is.EqualTo(5));

            Assert.That(consequenceBinaryExpression.Right, Is.InstanceOf<Integer>());
            var consequenceRightInteger = (Integer)consequenceBinaryExpression.Right;
            Assert.That(consequenceRightInteger.Value, Is.EqualTo(5));


            Assert.That(ifElseStatement.Alternative.Statements.Count(), Is.EqualTo(1));
            var alternativeStatement = ifElseStatement.Alternative.Statements.First();
            Assert.That(alternativeStatement, Is.InstanceOf<Return>());
            var alternativeReturn = (Return)alternativeStatement;
            Assert.That(alternativeReturn.Value, Is.InstanceOf<BinaryExpression>());

            var alternativeBinaryExpression = (BinaryExpression)alternativeReturn.Value;
            var alternativeLeftInteger = (Integer)alternativeBinaryExpression.Left;
            Assert.That(alternativeLeftInteger.Value, Is.EqualTo(5));

            Assert.True(alternativeBinaryExpression.Operation == TokenType.ASTERISK);

            var alternativeRightInteger = (Integer)alternativeBinaryExpression.Right;
            Assert.That(alternativeRightInteger.Value, Is.EqualTo(10));
        }

        [Test]
        public void TestParseFunctionStatement()
        {
            var input = @"
                function(x, y) {
                    var z = x + y;
                    return z;
                }
            ";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement, Is.InstanceOf<Function>());
            var functionStatement = (Function)statement;

            Assert.That(functionStatement.Parameters.Count, Is.EqualTo(2));
            var firstParameter = functionStatement.Parameters.First();
            Assert.That(firstParameter.Value, Is.EqualTo("x"));
            var secondParameter = functionStatement.Parameters.Last();
            Assert.That(secondParameter.Value, Is.EqualTo("y"));

            var functionBodyStatements = functionStatement.Body.Statements;

            var firstStatement = functionBodyStatements.First();
            Assert.That(firstStatement, Is.InstanceOf<Var>());

            var secondStatement = functionBodyStatements.Last();
            Assert.That(secondStatement, Is.InstanceOf<Return>());
        }

        [Test]
        public void TestParseFunctionCall()
        {
            var input = "add(x, y, 2, true);";

            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            Assert.That(program.Statements.Count, Is.EqualTo(1));
            var statement = program.Statements.First();

            Assert.That(statement, Is.InstanceOf<ExpressionStatement>());
            var expressionStatement = (ExpressionStatement)statement;

            Assert.That(expressionStatement.Expression, Is.InstanceOf<FunctionCall>());
            var functionCall = (FunctionCall)expressionStatement.Expression;

            Assert.That(functionCall.Function, Is.InstanceOf<Identifier>());
            var functionCallIdentifier = (Identifier)functionCall.Function;
            Assert.That(functionCallIdentifier.Value, Is.EqualTo("add"));

            Assert.That(functionCall.Parameters.Count, Is.EqualTo(4));

            var firstParameter = functionCall.Parameters.ElementAt(0);
            Assert.That(firstParameter, Is.InstanceOf<Identifier>());
            var identifierParameter = (Identifier)firstParameter;
            Assert.That(identifierParameter.Value, Is.EqualTo("x"));

            var secondParameter = functionCall.Parameters.ElementAt(1);
            Assert.That(secondParameter, Is.InstanceOf<Identifier>());
            var identifierParameter2 = (Identifier)secondParameter;
            Assert.That(identifierParameter2.Value, Is.EqualTo("y"));

            var thirdParameter = functionCall.Parameters.ElementAt(2);
            Assert.That(thirdParameter, Is.InstanceOf<Integer>());
            var integerParameter = (Integer)thirdParameter;
            Assert.That(integerParameter.Value, Is.EqualTo(2));

            var fourthParameter = functionCall.Parameters.ElementAt(3);
            Assert.That(fourthParameter, Is.InstanceOf<Bool>());
            var booleanParameter = (Bool)fourthParameter;
            Assert.That(booleanParameter.Value, Is.EqualTo(true));
        }
    }
}
