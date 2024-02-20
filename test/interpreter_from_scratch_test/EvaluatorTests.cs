using interpreter_from_scratch;
using interpreter_from_scratch.Evaluation;

namespace interpreter_from_scratch_test
{
    public class EvaluatorTests
    {
        [TestCase("5;", 5)]
        [TestCase("10;", 10)]
        [TestCase("24245;", 24245)]
        public void TestEvaluateIntegerExpression(string input, int value)
        {
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program);

            Assert.IsInstanceOf<IntegerObject>(interpreterObject);
            var integerObject = (IntegerObject)interpreterObject;
            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("true;", true)]
        [TestCase("false;", false)]
        public void TestEvaluateBooleanExpression(string input, bool value)
        { 
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program);

            Assert.IsInstanceOf<BoolObject>(interpreterObject);
            var boolObject = (BoolObject)interpreterObject;

            Assert.That(boolObject.Value, Is.EqualTo(value));
        }

        [TestCase("5 + 3;", 8)]
        [TestCase("10 * 2;", 20)]
        [TestCase("10 / 5;", 2)]
        [TestCase("1092 - 53;", 1039)]
        public void TestEvaluateBinaryExpressionReturnsIntegerObject(string input, int value)
        { 
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program);

            Assert.IsInstanceOf<IntegerObject>(interpreterObject);
            var integerObject = (IntegerObject)interpreterObject;

            Assert.That(integerObject.Value, Is.EqualTo(value));
        }

        [TestCase("5 == 5;", true)]
        [TestCase("5 == 123;", false)]
        [TestCase("5 > 123;", false)]
        [TestCase("5 < 123;", true)]
        public void TestEvaluateBinaryExpressionReturnsBooleanObject(string input, bool value)
        { 
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            var interpreterObject = evaluator.Evaluate(program);

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
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            var program = parser.ParseProgram();

            var evaluator = new Evaluator();
            Assert.Throws<Exception>(() => evaluator.Evaluate(program));
        }
    }
}
