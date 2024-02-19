
namespace interpreter_from_scratch.Evaluation;

public class BoolObject : InterpreterObject
{
    public bool Value { get; set; }
    public BoolObject(bool value)
    {
        Value = value;
        Type = InterpreterObjectType.BOOLEAN;
    }
}