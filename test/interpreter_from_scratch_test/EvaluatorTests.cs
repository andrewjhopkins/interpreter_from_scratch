using interpreter_from_scratch;
using interpreter_from_scratch.Evaluation;
using System.Security.Cryptography.X509Certificates;

namespace interpreter_from_scratch_test
{
    public class EvaluatorTests
    {
        [TestCase("5;", 5)]
        [TestCase("10;", 10)]
        [TestCase("24245;", 24245)]
        public void TestEvaluateIntegerExpression(string input, int value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<IntegerObject>(interpreterObject);
            var integerObject = (IntegerObject)interpreterObject;
            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("true;", true)]
        [TestCase("false;", false)]
        public void TestEvaluateBooleanExpression(string input, bool value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<BoolObject>(interpreterObject);
            var boolObject = (BoolObject)interpreterObject;

            Assert.That(boolObject.Value, Is.EqualTo(value));
        }

        [TestCase("5 + 3;", 8)]
        [TestCase("10 * 2;", 20)]
        [TestCase("10 / 5;", 2)]
        [TestCase("1092 - 53;", 1039)]
        [TestCase("var test = 1092; var testing = 53; test - testing;", 1039)]
        public void TestEvaluateBinaryExpressionReturnsIntegerObject(string input, int value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<IntegerObject>(interpreterObject);
            var integerObject = (IntegerObject)interpreterObject;

            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("5 == 5;", true)]
        [TestCase("5 == 123;", false)]
        [TestCase("5 > 123;", false)]
        [TestCase("5 < 123;", true)]
        [TestCase("var test = 5; var testing = 123; test < testing;", true)]
        [TestCase("var test = 5; var testing = 5; test == testing;", true)]
        [TestCase("var test = 5; var testing = 5; test != testing;", false)]
        public void TestEvaluateBinaryExpressionReturnsBooleanObject(string input, bool value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<BoolObject>(interpreterObject);
            var boolObject = (BoolObject)interpreterObject;

            Assert.That(boolObject.Value, Is.EqualTo(value));
        }

        [TestCase("5 > true;")]
        [TestCase("true * true;")]
        [TestCase("false - 3;")]
        [TestCase("3 == false;")]
        public void TestEvaluateBinaryExpressionIfBothNotIntegersThrow(string input)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            Assert.Throws<Exception>(() => evaluator.Evaluate(program, environmentVariables));
        }

        [TestCase("var test = 3;", "test", 3)]
        [TestCase("var testing = 192;", "testing", 192)]
        public void TestEvaluateVarWithValueInteger(string input, string identifier, int value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            evaluator.Evaluate(program, environmentVariables);

            Assert.That(environmentVariables.Variables.ContainsKey(identifier), Is.True);

            var integerObject = (IntegerObject)environmentVariables.Variables[identifier];
            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("var foo = true;", "foo", true)]
        [TestCase("var bar = false;", "bar", false)]
        public void TestEvaluateVarWithValueBoolean(string input, string identifier, bool value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            evaluator.Evaluate(program, environmentVariables);

            Assert.That(environmentVariables.Variables.ContainsKey(identifier), Is.True);

            var boolObject = (BoolObject)environmentVariables.Variables[identifier];
            Assert.That(boolObject.Value, Is.EqualTo(value));
        }

        [TestCase("return 5;", 5)]
        [TestCase("return  12315;", 12315)]
        [TestCase("var test = 12315; return  test;", 12315)]
        public void TestEvaluateReturnInteger(string input, int value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<ReturnObject>(interpreterObject);
            var returnObject = (ReturnObject)interpreterObject;

            Assert.IsInstanceOf<IntegerObject>(returnObject.Value);
            var integerObject = (IntegerObject)returnObject.Value;

            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("return true;", true)]
        [TestCase("return false;", false)]
        [TestCase("var test = true; return test;", true)]
        public void TestEvaluateReturnBoolean(string input, bool value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<ReturnObject>(interpreterObject);
            var returnObject = (ReturnObject)interpreterObject;

            Assert.IsInstanceOf<BoolObject>(returnObject.Value);
            var boolObject = (BoolObject)returnObject.Value;

            Assert.That(boolObject.Value, Is.EqualTo(value));
        }

        [TestCase("if (true) { 10; }", 10)]
        [TestCase("if (false) { 10; } else { 15; }", 15)]
        public void TestEvaluateIfElseReturnInteger(string input, int value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<IntegerObject>(interpreterObject);
            var integerObject = (IntegerObject)interpreterObject;

            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("if (2 > 1) { 10; return 15; }", 15)]
        [TestCase("if (1 < 2) { return 15; 10; }", 15)]
        [TestCase("if (1 > 2) { return 15; } else { return 10; }", 10)]
        [TestCase("if (1 > 2) { return 15; } else { return 10; 15; }", 10)]
        public void TestEvaluateIfElseReturnIntegerWithReturn(string input, int value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<ReturnObject>(interpreterObject);
            var returnObject = (ReturnObject)interpreterObject;

            Assert.IsInstanceOf<IntegerObject>(returnObject.Value);
            var integerObject = (IntegerObject)returnObject.Value;

            Assert.That(integerObject.Value, Is.EqualTo(value));
        }


        [TestCase("if (true) { true; }", true)]
        [TestCase("if (false) { true; } else { false; }", false)]
        public void TestEvaluateIfElseReturnBoolean(string input, bool value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<BoolObject>(interpreterObject);
            var boolObject = (BoolObject)interpreterObject;

            Assert.That(boolObject.Value, Is.EqualTo(value));
        }

        [TestCase("if (true) { false; return true;}", true)]
        [TestCase("if (true) { return true; false;}", true)]
        [TestCase("if (false) { return true; } else { return false; }", false)]
        [TestCase("if (false) { return true; } else { true; return false; }", false)]
        public void TestEvaluateIfElseReturnBooleanWithReturnStatement(string input, bool value)
        {
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<ReturnObject>(interpreterObject);
            var returnObject = (ReturnObject)interpreterObject;

            Assert.IsInstanceOf<BoolObject>(returnObject.Value);
            var boolObject = (BoolObject)returnObject.Value;

            Assert.That(boolObject.Value, Is.EqualTo(value));
        }

        [Test]
        public void TestEvaluateFunctionStatement()
        {
            var input = "function test(x, y, z) { return x; }";

            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            evaluator.Evaluate(program, environmentVariables);

            Assert.IsTrue(environmentVariables.Variables.ContainsKey("test"));

            Assert.IsInstanceOf<FunctionObject>(environmentVariables.Variables["test"]);
            var functionObject = (FunctionObject)environmentVariables.Variables["test"];

            Assert.That(functionObject.Parameters.Count(), Is.EqualTo(3));
        }

        [TestCase("function test(x) { return x; } test(5);", 5)]
        [TestCase("function test(x, y) { return x + y; } test(5, 3);", 8)]
        [TestCase("function test(x, y) { var z = x + y; return z; } test(5, 3);", 8)]
        public void TestEvaluateFunctionCallReturnsInteger(string input, int value)
        { 
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<ReturnObject>(interpreterObject);
            var returnObject = (ReturnObject)interpreterObject;

            Assert.IsInstanceOf<IntegerObject>(returnObject.Value);
            var integerObject = (IntegerObject)returnObject.Value;

            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("function test(x) { return true; } test(5);", true)]
        [TestCase("function test(x) { return true; }; test(5);", true)]
        [TestCase("function test(x, y) { return false; } test(5, 3);", false)]
        public void TestEvaluateFunctionCallReturnsBoolean(string input, bool value)
        { 
            var environmentVariables = new EnvironmentVariables();
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program, environmentVariables);

            Assert.IsInstanceOf<ReturnObject>(interpreterObject);
            var returnObject = (ReturnObject)interpreterObject;

            Assert.IsInstanceOf<BoolObject>(returnObject.Value);
            var boolObject = (BoolObject)returnObject.Value;

            Assert.That(boolObject.Value, Is.EqualTo(value));
        }
    }
}
