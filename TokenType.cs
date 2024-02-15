namespace interpreter_from_scratch
{
    public enum TokenType
    {
        IDENTIFIER,
        INTEGER,
        TRUE,
        FALSE,

        FUNCTION,
        VAR,
        IF,
        ELSE,
        RETURN,

        ASSIGN,
        EQUALS,
        DOESNOTEQUAL,
        GREATERTHAN,
        LESSTHAN,

        PLUS,
        MINUS,
        SLASH,
        ASTERISK,

        LEFTPAREN,
        RIGHTPAREN,
        LEFTBRACE,
        RIGHTBRACE,
        COMMA,

        SEMICOLON,
        ENDOFFILE,
    }
}
