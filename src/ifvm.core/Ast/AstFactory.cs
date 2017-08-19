using System.Collections.Generic;

namespace IFVM.Ast
{
    public static partial class AstFactory
    {
        public static readonly AstConstantExpression Zero = new AstConstantExpression(0);
        public static readonly AstConstantExpression One = new AstConstantExpression(1);
        public static readonly AstConstantExpression Two = new AstConstantExpression(2);
        public static readonly AstConstantExpression Three = new AstConstantExpression(3);
        public static readonly AstConstantExpression Four = new AstConstantExpression(4);
        public static readonly AstConstantExpression Five = new AstConstantExpression(5);
        public static readonly AstConstantExpression Six = new AstConstantExpression(6);
        public static readonly AstConstantExpression Seven = new AstConstantExpression(7);
        public static readonly AstConstantExpression Eight = new AstConstantExpression(8);

        public static readonly AstConstantExpression MinusOne = new AstConstantExpression(-1);

        public static readonly AstConstantExpression True = new AstConstantExpression(1);
        public static readonly AstConstantExpression False = new AstConstantExpression(0);

        private static readonly Dictionary<int, AstConstantExpression> s_constants = new Dictionary<int, AstConstantExpression>
        {
            { 0, Zero },
            { 1, One },
            { 2, Two },
            { 3, Three },
            { 4, Four },
            { 5, Five },
            { 6, Six },
            { 7, Seven },
            { 8, Eight },
            { -1, MinusOne }
        };

        public static AstConstantExpression ConstantExpression(int value)
        {
            if (!s_constants.TryGetValue(value, out var result))
            {
                result = new AstConstantExpression(value);
                s_constants.Add(value, result);
            }

            return result;
        }

        public static AstConstantExpression ConstantExpression(bool value)
            => value ? True : False;
    }
}
