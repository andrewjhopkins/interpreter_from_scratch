namespace interpreter_from_scratch.Ast
{
    public class Integer : Expression
    {
        public int Value { get; set; }

        public Integer(Token token, int value)
        {
            Token = token;
            Value = value;
        }
    }
}
