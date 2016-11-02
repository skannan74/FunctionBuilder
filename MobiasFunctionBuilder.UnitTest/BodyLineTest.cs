using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobiasFunctionBuilder.Enums;

namespace MobiasFunctionBuilder.UnitTest
{

   

    [TestClass]
    public class BodyLineTest
    {
        [TestMethod]
        public void AssignShouldAssign()
        {

            const string expected =
            @"public System.String Call(System.String first, System.String second)
            {
              first = second;
              return first;
            }";

            var newExpression = Function.Create()
                    .InputParameter<string>("first")
                    .InputParameter<string>("second")
                    .Body(
                            BodyLine.Assign("first", "second")
                    )
                    .Returns("first");

           // AssertString.AreEqual(expected, newExpression.ToString());

            var lambda = newExpression.ToLambda<Func<string, string, string>>();
            Assert.IsNotNull(lambda);

            var result = lambda("test", "another");
            Assert.AreEqual("another", result);
        }

        [TestMethod]
        public void SumAssignShouldWorkForStrings()
        {
            const string expected =
            @"public System.String Call(System.String first, System.String second)
            {
              first += second;
              return first;
            }";

            var newExpression = Function.Create()
                    .InputParameter<string>("first")
                    .InputParameter<string>("second")
                    .Body(
                            BodyLine.Assign("first", "second", AssignementOperator.SumAssign)
                    )
                    .Returns("first");

            var lambda = newExpression.ToLambda<Func<string, string, string>>();
            Assert.IsNotNull(lambda);

            var result = lambda("test", "another");
            Assert.AreEqual("testanother", result);
        }


        [TestMethod]
        public void SumAssignShouldWorkForValueTypes()
        {
            const string expected =
            @"public System.Int32 Call(System.Int32 first, System.Int32 second)
            {
              first += second;
              return first;
            }";

            var newExpression = Function.Create()
                    .InputParameter<int>("first")
                    .InputParameter<int>("second")
                    .Body(
                            BodyLine.Assign("first", "second", AssignementOperator.SumAssign)
                    )
                    .Returns("first");


            var lambda = newExpression.ToLambda<Func<int, int, int>>();
            Assert.IsNotNull(lambda);

            var result = lambda(1, 2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void AssignShouldAssignObjects()
        {
            const string expected =
            @"public ExpressionBuilder.Test.busPerson Call(ExpressionBuilder.Test.busPerson first, ExpressionBuilder.Test.busPerson second)
            {
              first = second;
              return first;
            }";

            var newExpression = Function.Create()
                    .InputParameter<busPerson>("first")
                    .InputParameter<busPerson>("second")
                    .Body(
                            BodyLine.Assign("first", "second")
                    )
                    .Returns("first");


            var lambda = newExpression.ToLambda<Func<busPerson, busPerson, busPerson>>();
            Assert.IsNotNull(lambda);

            var expectedResult = new busPerson();
            var result = lambda(new busPerson(), expectedResult);
            Assert.AreSame(expectedResult, result);
        }

        [TestMethod]
        public void AssignShouldAcceptRightableAndLeftable()
        {
            const string expected =
            @"public System.String Call(System.String par)
            {
              par = ""another"";
              return par;
            }";

            var newExpression = Function.Create()
                    .InputParameter<string>("par")
                    .Body(
                            BodyLine.Assign(Operation.Variable("par"), Operation.Constant("another"))
                    )
                    .Returns("par");

            var newSource = newExpression.ToString();

            var lambda = newExpression.ToLambda<Func<string, string>>();
            Assert.IsNotNull(lambda);

            var result = lambda("test");
            Assert.AreEqual("another", result);
        }



        [TestMethod]
        public void AssignShouldWorkForInternalVariables()
        {
            const string expected =
            @"public System.String Call(System.String par)
            {
              System.String var;
              var = ""another"";
              var += par;
              return var;
            }";

            var newExpression = Function.Create()
                .InputParameter<string>("par")
                .Body(
                            BodyLine.CreateVariable<string>("var"),
                            BodyLine.Assign("var", Operation.Constant("another")),
                            BodyLine.Assign(Operation.Variable("var"), Operation.Variable("par"), AssignementOperator.SumAssign)
                    )
                    .Returns("var");


            var lambda = newExpression.ToLambda<Func<string, string>>();
            Assert.IsNotNull(lambda);

            var result = lambda("test");
            Assert.AreEqual("anothertest", result);
        }

//        [TestMethod]
//        public void CreateVariablesShouldWorkInsideWhileIfAndThen()
//        {
//            const string expected =
//    @"public void Call(System.String par)
//{
//  System.Int32 count;
//  count = 1;
//  while(count == 0)
//  {
//   if(True == True)
//   {
//    System.String var;
//    var = par;
//   }   
//   else
//   {
//    System.String var;
//    var = par;
//   };
//  };
//}";

//            var newExpression = Function.Create()
//                .InputParameter<string>("par")
//                .Body(
//                    BodyLine.CreateVariable<int>("count"),
//                    BodyLine.AssignConstant("count", 1),
//                    BodyLine.CreateWhile(Condition.CompareConst("count", 0))
//                        .Do(
//                            BodyLine.CreateIf(Condition.CompareConst(Operation.Constant(true), true))
//                                .Then(
//                                    BodyLine.CreateVariable<string>("var"),
//                                    BodyLine.Assign("var", "par")
//                                )
//                                .ElseIf(Condition.CompareConst(Operation.Constant(true), true))
//                                .Then(
//                                    BodyLine.CreateVariable<string>("var"),
//                                    BodyLine.Assign("var", "par")
//                                )
//                                .Else(
//                                    BodyLine.CreateVariable<string>("var"),
//                                    BodyLine.Assign("var", "par")
//                                )
//                        )
//                    );


//            var lambda = newExpression.ToLambda<Action<string>>();
//            Assert.IsNotNull(lambda);

//            lambda("test");
//        }

        [TestMethod]
        public void MultiplyAssignShouldWorkForValueTypes()
        {
            const string expected =
            @"public System.Int32 Call(System.Int32 first, System.Int32 second)
            {
              first *= second;
              return first;
            }";

            var newExpression = Function.Create()
                    .InputParameter<int>("first")
                    .InputParameter<int>("second")
                    .Body(
                            BodyLine.Assign("first", "second", AssignementOperator.MultiplyAssign)
                    )
                    .Returns("first");


            var lambda = newExpression.ToLambda<Func<int, int, int>>();
            Assert.IsNotNull(lambda);

            var result = lambda(1, 2);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void SubtractAssignShouldWorkForValueTypes()
        {
            const string expected =
            @"public System.Int32 Call(System.Int32 first, System.Int32 second)
            {
              first -= second;
              return first;
            }";

            var newExpression = Function.Create()
                    .InputParameter<int>("first")
                    .InputParameter<int>("second")
                    .Body(
                            BodyLine.Assign("first", "second", AssignementOperator.SubtractAssign)
                    )
                    .Returns("first");


            var lambda = newExpression.ToLambda<Func<int, int, int>>();
            Assert.IsNotNull(lambda);

            var result = lambda(4, 2);
            Assert.AreEqual(2, result);
        }
    }
}
