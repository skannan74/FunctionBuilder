using MobiasFunctionBuilder.BodyLines;
using MobiasFunctionBuilder.Enums;
using MobiasFunctionBuilder.Interface;
using System;

namespace MobiasFunctionBuilder
{
    public static class BodyLine
    {
        public static IBodyLine CreateVariable(Type dataType, string variableName)
        {
            return new CreateVariable(new Variable(dataType, variableName));
        }

        public static IBodyLine CreateVariable<TData>(string variableName)
        {
            return CreateVariable(typeof(TData), variableName);
        }

        public static IBodyLine AssignConstant(string lVariableName, object rConst, AssignementOperator assignType = AssignementOperator.Assign)
        {
            return Assign(Operation.Variable(lVariableName), Operation.Constant(rConst), assignType);
        }

        public static IBodyLine AssignConstant(ILeftable lValue, object rConst, AssignementOperator assignType = AssignementOperator.Assign)
        {
            return Assign(lValue, Operation.Constant(rConst), assignType);
        }

        public static IBodyLine Assign(ILeftable lValue, IRightable rValue, AssignementOperator assignType = AssignementOperator.Assign)
        {
            return new Assign(lValue, rValue, assignType);
        }

        public static IBodyLine Assign(string lVariableName, string rVariableName, AssignementOperator assignType = AssignementOperator.Assign)
        {
            return Assign(Operation.Variable(lVariableName), Operation.Variable(rVariableName), assignType);
        }

        public static IBodyLine Assign(ILeftable lValue, string rVariableName, AssignementOperator assignType = AssignementOperator.Assign)
        {
            return Assign(lValue, Operation.Variable(rVariableName), assignType);
        }

        public static IBodyLine Assign(string lVariableName, IRightable rValue, AssignementOperator assignType = AssignementOperator.Assign)
        {
            return Assign(Operation.Variable(lVariableName), rValue, assignType);
        }

        //public static IIf CreateIf(Condition condition)
        //{
        //    return new If(condition);
        //}


        public static IWhile CreateWhile(Condition condition)
        {
            return new While(condition);
        }

        public static IBodyLine Return()
        {
            return new CreateReturn();
        }

        //public static IBodyLine Nop
        //{
        //    get
        //    {
        //        return new Nop();
        //    }

        //}

    }
}
