namespace interpreter_from_scratch.Evaluation;

public class EnvironmentVariables
{
    public Dictionary<string, InterpreterObject> Variables = new Dictionary<string, InterpreterObject>();
    public EnvironmentVariables OuterEnvironment = null;

    public EnvironmentVariables()
    {
        Variables = new Dictionary<string, InterpreterObject>();
    }

    public EnvironmentVariables(EnvironmentVariables outer)
    {
        Variables = new Dictionary<string, InterpreterObject>();
        OuterEnvironment = outer;
    }

    public InterpreterObject Get(string key)
    {
        if (Variables.ContainsKey(key)) 
        {
            return Variables[key];
        }

        if (OuterEnvironment != null) 
        { 
            return OuterEnvironment.Get(key);
        }

        return null;
    }
}
