using interpreter_from_scratch.Ast;

namespace interpreter_from_scratch
{
    public class Parser
    {
        public Lexer Lexer { get; set; }

        private readonly HashSet<TokenType> _binaryOperations = new HashSet<TokenType>
        {
            TokenType.PLUS,
            TokenType.MINUS,
            TokenType.SLASH,
            TokenType.ASTERISK,
            TokenType.EQUALS,
            TokenType.DOESNOTEQUAL,
            TokenType.GREATERTHAN,
            TokenType.LESSTHAN,
        };

        public Parser(Lexer lexer)
        {
            Lexer = lexer;
        }

        public InterpreterProgram ParseProgram()
        { 
            var statements = new List<Statement>();

            while (Lexer.CurrentToken.Type != TokenType.ENDOFFILE)
            {
                var statement = ParseStatement();
                if (statement != null)
                {
                    statements.Add(statement);
                }
                Lexer.NextToken();
            }

            return new InterpreterProgram(statements);
        }

        public Statement ParseStatement()
        {
            switch (Lexer.CurrentToken.Type)
            {
                case TokenType.VAR:
                    return ParseVarStatement();
                case TokenType.RETURN:
                    return ParseReturnStatement();
                default:
                    return ParseExpressionStatement();
            }
        }

        public Var ParseVarStatement()
        {
            var token = Lexer.CurrentToken;
            ExpectNextToken(TokenType.IDENTIFIER);

            var identifier = new Identifier(Lexer.CurrentToken);
            ExpectNextToken(TokenType.ASSIGN);

            Lexer.NextToken();

            var value = ParseExpression();
            ExpectNextToken(TokenType.SEMICOLON);

            return new Var(token, identifier, value);
        }

        public Return ParseReturnStatement()
        { 
            var token = Lexer.CurrentToken;
            Lexer.NextToken();

            var value = ParseExpression();
            ExpectNextToken(TokenType.SEMICOLON);

            return new Return(token, value);
        }

        public ExpressionStatement ParseExpressionStatement()
        {
            var token = Lexer.CurrentToken;
            var value = ParseExpression();
            ExpectNextToken(TokenType.SEMICOLON);

            return new ExpressionStatement(token, value);
        }

        public Expression ParseExpression()
        {
            var token = Lexer.CurrentToken;
            Expression leftExpression;

            switch (token.Type)
            { 
                case TokenType.IDENTIFIER:
                    leftExpression = new Identifier(token);
                    break;
                case TokenType.INTEGER:
                    leftExpression = new Integer(token, int.Parse(token.Literal));
                    break;
                case TokenType.TRUE:
                case TokenType.FALSE:
                    leftExpression =  new Bool(token, token.Type == TokenType.TRUE);
                    break; 
                default:
                    leftExpression = null;
                    break;
            }

            if (_binaryOperations.Contains(Lexer.PeekToken.Type))
            {
                return ParseBinaryExpression(leftExpression);
            }

            return leftExpression;
        }

        public BinaryExpression ParseBinaryExpression(Expression left)
        { 
            Lexer.NextToken();

            var operation = Lexer.CurrentToken;
            Lexer.NextToken();

            var right = ParseExpression();

            return new BinaryExpression(left, operation.Type, right);
        }

        public void ExpectNextToken(TokenType type)
        { 
            if (Lexer.PeekToken.Type != type)
            {
                throw new Exception($"Expected next token to be {type}, got {Lexer.PeekToken.Type} instead.");
            }
            Lexer.NextToken();
        }
    }
}
