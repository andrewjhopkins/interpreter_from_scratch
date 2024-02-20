namespace interpreter_from_scratch.Evaluation
{
    public class ReturnObject : InterpreterObject
    {
        public InterpreterObject Value { get; set; }
        public ReturnObject(InterpreterObject value)
        {
            Value = value;
            Type = InterpreterObjectType.RETURNVALUE;
        }
    }
}
