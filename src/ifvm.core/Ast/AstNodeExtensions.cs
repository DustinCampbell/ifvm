namespace IFVM.Ast
{
    public static class AstNodeExtensions
    {
        public static AstExpression Plus(this AstExpression expression, AstExpression other)
        {
            return AstFactory.AddExpression(
                left: expression,
                right: other);
        }

        public static AstExpression Plus(this AstExpression expression, int value)
        {
            return expression.Plus(AstFactory.ConstantExpression(value));
        }

        public static AstExpression Minus(this AstExpression expression, AstExpression other)
        {
            return AstFactory.SubtractExpression(
                left: expression,
                right: other);
        }

        public static AstExpression Minus(this AstExpression expression, int value)
        {
            return expression.Minus(AstFactory.ConstantExpression(value));
        }

        public static AstExpression Times(this AstExpression expression, AstExpression other)
        {
            return AstFactory.MultiplyExpression(
                left: expression,
                right: other);
        }

        public static AstExpression Times(this AstExpression expression, int value)
        {
            return expression.Times(AstFactory.ConstantExpression(value));
        }

        public static AstExpression DividedBy(this AstExpression expression, AstExpression other)
        {
            return AstFactory.DivideExpression(
                left: expression,
                right: other);
        }

        public static AstExpression DividedBy(this AstExpression expression, int value)
        {
            return expression.DividedBy(AstFactory.ConstantExpression(value));
        }

        public static AstExpression Modulo(this AstExpression expression, AstExpression other)
        {
            return AstFactory.ModuloExpression(
                left: expression,
                right: other);
        }

        public static AstExpression Modulo(this AstExpression expression, int value)
        {
            return expression.Modulo(AstFactory.ConstantExpression(value));
        }
    }
}
