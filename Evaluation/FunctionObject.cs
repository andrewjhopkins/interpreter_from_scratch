using interpreter_from_scratch.Ast;

namespace interpreter_from_scratch.Evaluation
{
    public class FunctionObject : InterpreterObject
    {
        public Block Body { get; set; }
        public IEnumerable<Identifier> Parameters { get; set; }
        public EnvironmentVariables EnvironmentVariables { get; set; }

        public FunctionObject(Block body, IEnumerable<Identifier> parameters, EnvironmentVariables environmentVariables)
        {
            Body = body;
            Parameters = parameters;
            EnvironmentVariables = environmentVariables;
            Type = InterpreterObjectType.FUNCTION;
        }
    }
}
