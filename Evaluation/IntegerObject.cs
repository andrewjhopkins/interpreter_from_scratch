namespace interpreter_from_scratch.Evaluation;

public class IntegerObject : InterpreterObject
{
    public int Value { get; set; }
    public IntegerObject(int value)
    {
        Type = InterpreterObjectType.INTEGER;
        Value = value;
    }
}