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

        private Statement ParseStatement()
        {
            switch (Lexer.CurrentToken.Type)
            {
                case TokenType.VAR:
                    return ParseVarStatement();
                case TokenType.RETURN:
                    return ParseReturnStatement();
                case TokenType.IF:
                    return ParseIfElseStatement();
                case TokenType.FUNCTION:
                    return ParseFunctionStatement();
                default:
                    return ParseExpressionStatement();
            }
        }

        private Var ParseVarStatement()
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

        private Return ParseReturnStatement()
        { 
            var token = Lexer.CurrentToken;
            Lexer.NextToken();

            var value = ParseExpression();
            ExpectNextToken(TokenType.SEMICOLON);

            return new Return(token, value);
        }

        private IfElse ParseIfElseStatement()
        {
            var token = Lexer.CurrentToken;

            ExpectNextToken(TokenType.LEFTPAREN);
            Lexer.NextToken();

            var condition = ParseExpression();

            ExpectNextToken(TokenType.RIGHTPAREN);

            ExpectNextToken(TokenType.LEFTBRACE);

            var block = ParseBlockStatement();

            if (Lexer.PeekToken.Type == TokenType.ELSE)
            {
                Lexer.NextToken();
                ExpectNextToken(TokenType.LEFTBRACE);

                var elseBlock = ParseBlockStatement();

                return new IfElse(token, condition, block, elseBlock);
            }

            return new IfElse(token, condition, block, null);
        }

        private Function ParseFunctionStatement()
        { 
            var token = Lexer.CurrentToken;
            Lexer.NextToken();

            var identifierToken = Lexer.CurrentToken;
            var functionIdentifier = new Identifier(identifierToken);

            ExpectNextToken(TokenType.LEFTPAREN);

            var parameters = new List<Identifier>();
            while (Lexer.CurrentToken.Type != TokenType.RIGHTPAREN)
            {
                Lexer.NextToken();
                var parameter = new Identifier(Lexer.CurrentToken);
                parameters.Add(parameter);

                if (Lexer.PeekToken.Type != TokenType.RIGHTPAREN)
                {
                    ExpectNextToken(TokenType.COMMA);
                }
                else
                {
                    Lexer.NextToken();
                }
            }

            ExpectNextToken(TokenType.LEFTBRACE);
            var block = ParseBlockStatement();

            // Let the user optionally add a semicolon at the end of the function declaration
            if (Lexer.PeekToken.Type == TokenType.SEMICOLON)
            {
                Lexer.NextToken();
            }

            return new Function(token, functionIdentifier, parameters, block);
        }

        private FunctionCall ParseFunctionCall(Identifier identifier)
        {
            var token = Lexer.CurrentToken;
            return new FunctionCall(token, identifier, ParseFunctionCallParameters());
        }

        private IEnumerable<Expression> ParseFunctionCallParameters()
        {
            var parameters = new List<Expression>();
            Lexer.NextToken();

            while (Lexer.CurrentToken.Type != TokenType.RIGHTPAREN)
            {
                var parameter = ParseExpression();
                parameters.Add(parameter);

                if (Lexer.PeekToken.Type != TokenType.RIGHTPAREN)
                {
                    ExpectNextToken(TokenType.COMMA);
                }
                Lexer.NextToken();
            }

            return parameters;
        }

        private Block ParseBlockStatement()
        {
            var statements = new List<Statement>();

            Lexer.NextToken();

            while (Lexer.CurrentToken.Type != TokenType.RIGHTBRACE)
            {
                var statement = ParseStatement();
                if (statement != null)
                {
                    statements.Add(statement);
                }

                Lexer.NextToken();
            }

            return new Block(statements);
        }

        private ExpressionStatement ParseExpressionStatement()
        {
            var token = Lexer.CurrentToken;
            var value = ParseExpression();
            ExpectNextToken(TokenType.SEMICOLON);

            return new ExpressionStatement(token, value);
        }

        private Expression ParseExpression()
        {
            var token = Lexer.CurrentToken;
            Expression leftExpression;

            switch (token.Type)
            { 
                case TokenType.IDENTIFIER:
                    var identifier = new Identifier(token);

                    if (Lexer.PeekToken.Type == TokenType.LEFTPAREN)
                    {
                        Lexer.NextToken();
                        leftExpression = ParseFunctionCall(identifier);
                    }
                    else
                    {
                        leftExpression = identifier;
                    }

                    break;
                case TokenType.INTEGER:
                    leftExpression = new Integer(token, int.Parse(token.Literal));
                    break;
                case TokenType.TRUE:
                case TokenType.FALSE:
                    leftExpression =  new Bool(token, token.Type == TokenType.TRUE);
                    break; 
                default:
                    return null;
            }

            if (_binaryOperations.Contains(Lexer.PeekToken.Type))
            {
                return ParseBinaryExpression(leftExpression);
            }

            return leftExpression;
        }

        private BinaryExpression ParseBinaryExpression(Expression left)
        { 
            Lexer.NextToken();

            var operation = Lexer.CurrentToken;
            Lexer.NextToken();

            var right = ParseExpression();

            return new BinaryExpression(left, operation.Type, right);
        }

        private void ExpectNextToken(TokenType type)
        { 
            if (Lexer.PeekToken.Type != type)
            {
                throw new Exception($"Expected next token to be {type}, got {Lexer.PeekToken.Type} instead.");
            }
            Lexer.NextToken();
        }
    }
}
