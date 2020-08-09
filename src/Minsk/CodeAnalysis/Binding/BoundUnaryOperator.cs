using System;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator{
        public BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType)  
                            : this(syntaxKind, kind, operandType, operandType)  {}
        public BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operatorType, Type resultType)                            
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperatorType = operatorType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperatorType { get; }
        public Type ResultType { get; }

        private static BoundUnaryOperator[] _operators = {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.TildeToken, BoundUnaryOperatorKind.OnesComplement, typeof(int)),
        };

        public static BoundUnaryOperator Bind(SyntaxKind kind, Type operandType){
            foreach(var op in _operators){
                if(op.SyntaxKind == kind && op.OperatorType == operandType){
                    return op;
                }
            }
            return null;
        }
    }
}