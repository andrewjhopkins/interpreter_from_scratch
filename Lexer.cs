using System.Diagnostics.Tracing;

namespace interpreter_from_scratch
{
    public class Lexer
    {
        public int Position { get; set; }
        public string Input { get; set; }
        public Token CurrentToken { get; set; }
        public Token PeekToken { get; set; }

        private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
        {
            { "var", TokenType.VAR },
            { "true", TokenType.TRUE },
            { "false", TokenType.FALSE },
            { "if", TokenType.IF },
            { "else", TokenType.ELSE },
            { "function", TokenType.FUNCTION },
            { "return", TokenType.RETURN }
        };

        public Lexer(string input)
        {
            Input = input;
            Position = 0;

            // Load first two tokens
            NextToken();
            NextToken();
        }

        public void NextToken()
        {
            SkipWhitespace();
            Token? token = null;
            CurrentToken = PeekToken;

            if (Position >= Input.Length)
            {
                PeekToken = new Token(TokenType.ENDOFFILE, "");
                return;
            }

            var character = Input[Position];
            switch (character)
            {
                case '=':
                    if (Position < Input.Length - 1 && Input[Position + 1] == '=')
                    {
                        token = new Token(TokenType.EQUALS, "==");
                        Position += 2;
                    }
                    else
                    {
                        token = new Token(TokenType.ASSIGN, "=");
                        Position++;
                    }
                    break;
                case '+':
                    token = new Token(TokenType.PLUS, "+");
                    Position++;
                    break;
                case '-':
                    token = new Token(TokenType.MINUS, "-");
                    Position++;
                    break;
                case '*':
                    token = new Token(TokenType.ASTERISK, "*");
                    Position++;
                    break;
                case '/':
                    token = new Token(TokenType.SLASH, "/");
                    Position++;
                    break;
                case '!':
                    if (Input[Position + 1] == '=')
                    {
                        token = new Token(TokenType.DOESNOTEQUAL, "!=");
                        Position += 2;
                    }
                    break;
                case '>':
                    token = new Token(TokenType.GREATERTHAN, ">");
                    Position++;
                    break;
                case '<':
                    token = new Token(TokenType.LESSTHAN, "<");
                    Position++;
                    break;
                case '(':
                    token = new Token(TokenType.LEFTPAREN, "(");
                    Position++;
                    break;
                case ')':
                    token = new Token(TokenType.RIGHTPAREN, ")");
                    Position++;
                    break;
                case '{':
                    token = new Token(TokenType.LEFTBRACE, "{");
                    Position++;
                    break;
                case '}':
                    token = new Token(TokenType.RIGHTBRACE, "}");
                    Position++;
                    break;
                case ',':
                    token = new Token(TokenType.COMMA, ",");
                    Position++;
                    break;
                case ';':
                    token = new Token(TokenType.SEMICOLON, ";");
                    Position++;
                    break;
                default:
                    if (char.IsNumber(character))
                    {
                        var number = "";
                        while (Position < Input.Length && char.IsNumber(Input[Position]))
                        {
                            number += Input[Position];
                            Position++;
                        }
                        token = new Token(TokenType.INTEGER, number);
                        break;
                    }
                    else if (char.IsLetter(character))
                    {
                        var identifier = "";
                        while (Position < Input.Length && (char.IsLetter(Input[Position]) || char.IsDigit(Input[Position])))
                        {
                            identifier += Input[Position];
                            Position++;
                        }

                        if (_keywords.ContainsKey(identifier))
                        {
                            token = new Token(_keywords[identifier], identifier);
                            break;
                        }

                        token = new Token(TokenType.IDENTIFIER, identifier);
                        break;
                    }

                    throw new Exception($"Unknown token: {Input[Position]} at position {Position}");
            }

            if (token != null)
            {
                PeekToken = token;
            }
        }

        private void SkipWhitespace()
        {
            while (Position < Input.Length && char.IsWhiteSpace(Input[Position]))
            {
                Position++;
            }
        }
    }
}
