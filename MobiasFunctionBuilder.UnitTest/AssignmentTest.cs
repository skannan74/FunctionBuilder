using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MobiasFunctionBuilder.UnitTest
{
    [TestClass]
    public class AssignmentTest
    {
        [TestMethod]
        public void PropertyAssignShouldAssign()
        {

            const string expected =
            @"public ExpressionBuilder.Test.busPerson Call(ExpressionBuilder.Test.busPerson source, ExpressionBuilder.Test.busPerson target)
            {
              target.FullName = source.FirstName + source.LastName;  
              return target;
            }";


            var newExpression = Function.Create()
                    .InputParameter<busPerson>("source")
                    .InputParameter<busPerson>("target")
                    .Body(BodyLine.Assign("target.FullName", "source.FirstName"))
                    .Returns("target");


            var lambda = newExpression.ToLambda<Func<busPerson, busPerson, busPerson>>();
            Assert.IsNotNull(lambda);

            var expectedResult = new busPerson() {PersonID = 1, FirstName = "Sagi", LastName = "tec", FullName = "Sagitec"};
            var result = lambda(new busPerson() { PersonID = 1, FirstName = "Sagi"}, expectedResult);
            Assert.AreSame(expectedResult, result);
        }
    }
}
