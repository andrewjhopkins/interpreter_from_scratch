namespace interpreter_from_scratch.Ast
{
    public class IfElse : Statement
    {
        public Expression Condition { get; set; }
        public Block Consequence { get; set; }
        public Block Alternative { get; set; }

        public IfElse(Token token, Expression condition, Block consequence, Block alternative)
        {
            Token = token;
            Condition = condition;
            Consequence = consequence;
            Alternative = alternative;
        }
    }
}
