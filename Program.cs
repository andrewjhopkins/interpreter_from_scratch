using interpreter_from_scratch.Evaluation;
namespace interpreter_from_scratch;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Count() > 0)
        {
            var text = File.ReadAllText(args[0]);
            var lexer = new Lexer(text);
            var parser = new Parser(lexer);
            var evaluator = new Evaluator();

            var program = parser.ParseProgram();
            var response = evaluator.Evaluate(program, new EnvironmentVariables());
            PrintInterpreterObject(response);
        }
        else
        {
            Console.WriteLine("Staring REPL...");
            var environment = new EnvironmentVariables();

            while (true)
            {
                Console.Write(">> ");
                var input = Console.ReadLine();
                var lexer = new Lexer(input);
                var parser = new Parser(lexer);
                var evaluator = new Evaluator();

                var program = parser.ParseProgram();
                var response = evaluator.Evaluate(program, environment);
                PrintInterpreterObject(response);

            }
        }
    }

    private static void PrintInterpreterObject(InterpreterObject interpreterObject)
    {
        if (interpreterObject == null)
        {
            return;
        }

        switch (interpreterObject.Type)
        {
            case InterpreterObjectType.INTEGER:
                Console.WriteLine(((IntegerObject)interpreterObject).Value);
                break;
            case InterpreterObjectType.BOOLEAN:
                Console.WriteLine(((BoolObject)interpreterObject).Value);
                break;
            case InterpreterObjectType.RETURNVALUE:
                var returnObject = (ReturnObject)interpreterObject;
                PrintInterpreterObject(returnObject.Value);
                break;
            default:
                break;
        }
    }
}
