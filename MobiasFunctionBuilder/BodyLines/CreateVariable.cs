﻿using MobiasFunctionBuilder.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.Parser;
using System.Linq.Expressions;
using MobiasFunctionBuilder.Utils;

namespace MobiasFunctionBuilder.BodyLines
{
    public class CreateVariable : IBodyLine
    {
        internal Variable VariableDeclaration;

        public CreateVariable(Variable variable)
        {
            VariableDeclaration = variable;
        }

        public Type ParsedType { get; private set; }

        public Expression ToExpression(ParseContext context)
        {
            context.Current.AddVariable(VariableDeclaration);
            VariableDeclaration.Expression = Expression.Variable(VariableDeclaration.DataType, VariableDeclaration.Name);
            return VariableDeclaration.Expression;
        }

        public string ToString(ParseContext context)
        {
            return ReflectionUtil.TypeToString(VariableDeclaration.DataType) + " " + VariableDeclaration.Name;
        }

        public void PreParseExpression(ParseContext context)
        {
            context.Current.AddVariable(VariableDeclaration);
            ParsedType = VariableDeclaration.DataType;
        }
        internal Expression DefaultInitialize(ParseContext context)
        {
            if (VariableDeclaration.Expression == null)
            {
                VariableDeclaration.Expression = ToExpression(context);
            }
            return Expression.Assign(VariableDeclaration.Expression, Expression.Default(VariableDeclaration.DataType));
        }
    }
}
