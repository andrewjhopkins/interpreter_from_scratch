using interpreter_from_scratch;

namespace interpreter_from_scratch_test
{
    public class LexerTests 
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("+", TokenType.PLUS)]
        [TestCase("-", TokenType.MINUS)]
        [TestCase("*", TokenType.ASTERISK)]
        [TestCase("/", TokenType.SLASH)]
        [TestCase("=", TokenType.ASSIGN)]
        [TestCase("==", TokenType.EQUALS)]
        [TestCase("!=", TokenType.DOESNOTEQUAL)]
        [TestCase(">", TokenType.GREATERTHAN)]
        [TestCase("<", TokenType.LESSTHAN)]
        [TestCase("(", TokenType.LEFTPAREN)]
        [TestCase(")", TokenType.RIGHTPAREN)]
        [TestCase("{", TokenType.LEFTBRACE)]
        [TestCase("}", TokenType.RIGHTBRACE)]
        [TestCase(",", TokenType.COMMA)]
        [TestCase(";", TokenType.SEMICOLON)]
        public void TestLexingSymbols(string input, TokenType expectedTokenType)
        {
            var lexer = new Lexer(input);
            Assert.That(lexer.CurrentToken.Type, Is.EqualTo(expectedTokenType));
            Assert.That(lexer.CurrentToken.Literal, Is.EqualTo(input));
            lexer.NextToken();
        }

        [TestCase("var", TokenType.VAR)]
        [TestCase("  var ", TokenType.VAR)]
        [TestCase("true", TokenType.TRUE)]
        [TestCase("     true", TokenType.TRUE)]
        [TestCase("false", TokenType.FALSE)]
        [TestCase("false ", TokenType.FALSE)]
        [TestCase("if", TokenType.IF)]
        [TestCase("else", TokenType.ELSE)]
        [TestCase("return", TokenType.RETURN)]
        [TestCase("function", TokenType.FUNCTION)]
        [TestCase("foobar", TokenType.IDENTIFIER)]
        [TestCase("foo24bar", TokenType.IDENTIFIER)]
        public void TestLexingKeywords(string input, TokenType expectedTokenType)
        { 
            var lexer = new Lexer(input);
            Assert.That(lexer.CurrentToken.Type, Is.EqualTo(expectedTokenType));
            Assert.That(lexer.CurrentToken.Literal, Is.EqualTo(input.Replace(" ", "")));
            lexer.NextToken();
        }

        [TestCase("5", TokenType.INTEGER)]
        [TestCase("42 ", TokenType.INTEGER)]
        [TestCase(" 423421  ", TokenType.INTEGER)]
        [TestCase("    4234213234", TokenType.INTEGER)]
        public void TestLexingIntegers(string input, TokenType expectedTokenType)
        {
            var lexer = new Lexer(input);
            Assert.That(lexer.CurrentToken.Type, Is.EqualTo(expectedTokenType));
            Assert.That(lexer.CurrentToken.Literal, Is.EqualTo(input.Replace(" ", "")));
            lexer.NextToken();
        }

        [Test]
        public void TestLexingInput()
        { 
            var input = @"
                var five = 5; var ten = 10;
                var add = function(x, y) { x + y; }; 
                var result = add(five, ten); 
                if (result != 10) {
                    return true;
                }
                else {
                    return false;
                }
            ";

            var expectedTokens = new[]
            {
                new Token(TokenType.VAR, "var"),
                new Token(TokenType.IDENTIFIER, "five"),
                new Token(TokenType.ASSIGN, "="),
                new Token(TokenType.INTEGER, "5"),
                new Token(TokenType.SEMICOLON, ";"),
                new Token(TokenType.VAR, "var"),
                new Token(TokenType.IDENTIFIER, "ten"),
                new Token(TokenType.ASSIGN, "="),
                new Token(TokenType.INTEGER, "10"),
                new Token(TokenType.SEMICOLON, ";"),
                new Token(TokenType.VAR, "var"),
                new Token(TokenType.IDENTIFIER, "add"),
                new Token(TokenType.ASSIGN, "="),
                new Token(TokenType.FUNCTION, "function"),
                new Token(TokenType.LEFTPAREN, "("),
                new Token(TokenType.IDENTIFIER, "x"),
                new Token(TokenType.COMMA, ","),
                new Token(TokenType.IDENTIFIER, "y"),
                new Token(TokenType.RIGHTPAREN, ")"),
                new Token(TokenType.LEFTBRACE, "{"),
                new Token(TokenType.IDENTIFIER, "x"),
                new Token(TokenType.PLUS, "+"),
                new Token(TokenType.IDENTIFIER, "y"),
                new Token(TokenType.SEMICOLON, ";"),
                new Token(TokenType.RIGHTBRACE, "}"),
                new Token(TokenType.SEMICOLON, ";"),
                new Token(TokenType.VAR, "var"),
                new Token(TokenType.IDENTIFIER, "result"),
                new Token(TokenType.ASSIGN, "="),
                new Token(TokenType.IDENTIFIER, "add"),
                new Token(TokenType.LEFTPAREN, "("),
                new Token(TokenType.IDENTIFIER, "five"),
                new Token(TokenType.COMMA, ","),
                new Token(TokenType.IDENTIFIER, "ten"),
                new Token(TokenType.RIGHTPAREN, ")"),
                new Token(TokenType.SEMICOLON, ";"),
                new Token(TokenType.IF, "if"),
                new Token(TokenType.LEFTPAREN, "("),
                new Token(TokenType.IDENTIFIER, "result"),
                new Token(TokenType.DOESNOTEQUAL, "!="),
                new Token(TokenType.INTEGER, "10"),
                new Token(TokenType.RIGHTPAREN, ")"),
                new Token(TokenType.LEFTBRACE, "{"),
                new Token(TokenType.RETURN, "return"),
                new Token(TokenType.TRUE, "true"),
                new Token(TokenType.SEMICOLON, ";"),
                new Token(TokenType.RIGHTBRACE, "}"),
                new Token(TokenType.ELSE, "else"),
                new Token(TokenType.LEFTBRACE, "{"),
                new Token(TokenType.RETURN, "return"),
                new Token(TokenType.FALSE, "false"),
                new Token(TokenType.SEMICOLON, ";"),
                new Token(TokenType.RIGHTBRACE, "}"),
                new Token(TokenType.ENDOFFILE, ""),
            };

            var lexer = new Lexer(input);

            for (var i = 0; i < expectedTokens.Length; i++)
            { 
                Assert.That(lexer.CurrentToken.Type, Is.EqualTo(expectedTokens[i].Type));
                Assert.That(lexer.CurrentToken.Literal, Is.EqualTo(expectedTokens[i].Literal));
                lexer.NextToken();
            }
        }
    }
}