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
    }
}
