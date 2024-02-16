namespace interpreter_from_scratch.Ast
{
    public class Bool : Expression
    {
        public bool Value { get; set; }
        
        public Bool(Token token, bool value)
        {
            Token = token;
            Value = value;
        }
    }
}
