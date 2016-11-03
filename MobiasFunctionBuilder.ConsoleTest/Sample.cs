﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.Operations;

namespace MobiasFunctionBuilder.ConsoleTest
{
    public class Sample
    {

   

        /// <summary>
        /// This method takes dictionary object and fills the keyvalue using indexer
        /// </summary>
        public static void accessdictionaryindexer()
        {
            var dic = new Dictionary<string, object>();// {{"KeyName", 1}
                                                       //};
            var parameterExpression = Expression.Parameter(typeof(IDictionary<string, object>), "d");


            var constant = Expression.Constant("KeyName");
            var propertyGetter = Expression.Property(parameterExpression, "Item", constant);

            var propertyGetter1 = Expression.Property(parameterExpression, "Item", Expression.Constant("KeyName1"));

            BlockExpression block = Expression.Block(
                 Expression.Assign(propertyGetter, Expression.Constant("testitem")),
                 Expression.Assign(propertyGetter1, Expression.Constant("testitem1"))
             );



            //        System.Linq.Expressions.NewExpression newDictionaryExpression =
            //System.Linq.Expressions.Expression.New(typeof(Dictionary<string, object>));

            //var propertyGetter1 = Expression.Property(newDictionaryExpression, "Item", constant);
            // var propertySetter1 = Expression.Assign(parameterExpression, Expression.Constant("testitem"));


            var expr = Expression.Lambda<Func<IDictionary<string, object>, object>>(block, parameterExpression).Compile();


            //   ParameterExpression result = Expression.Parameter(typeof(object), "result");
            //  BlockExpression block = Expression.Block(
            //     new[] { result },               //make the result a variable in scope for the block           
            //   Expression.Assign(result, propertySetter1)
            //  result                          //last value Expression becomes the return of the block 
            //   );


            //  var expr1 = Expression.Lambda<Func<IDictionary<string, object>, object>>(result, //parameterExpression).Compile();

            //var output = expr(dic);
            var output1 = expr(dic);
        }

        /// <summary>
        /// This method creates dictionary object and fills the keyvalue using indexer
        /// </summary>
        public static void CreateAndAccessDictionaryIndexer()
        {
            // var dic = new Dictionary<string, object>();// {{"KeyName", 1}
            //};
            var dictionaryType = typeof(Dictionary<string, object>);
            NewExpression newDictionaryExpression = Expression.New(dictionaryType);
            var newDictionaryInstance = Expression.Variable(typeof(Dictionary<string, object>), "d");


            var propertyGetter1 = Expression.Property(newDictionaryInstance, "Item", Expression.Constant("KeyName1"));

            var propertyGetter2 = Expression.Property(newDictionaryInstance, "Item", Expression.Constant("KeyName2"));

            BlockExpression block = Expression.Block(
                 Expression.Assign(newDictionaryInstance, newDictionaryExpression)//,
                                                                                  // Expression.Assign(propertyGetter1, Expression.Constant("testitem1")),
                                                                                  // Expression.Assign(propertyGetter2, Expression.Constant("testitem2"))
             );



            //        System.Linq.Expressions.NewExpression newDictionaryExpression =
            //System.Linq.Expressions.Expression.New(typeof(Dictionary<string, object>));

            //var propertyGetter1 = Expression.Property(newDictionaryExpression, "Item", constant);
            // var propertySetter1 = Expression.Assign(parameterExpression, Expression.Constant("testitem"));


            var expr = Expression.Lambda<Func<object>>(block).Compile();


            //   ParameterExpression result = Expression.Parameter(typeof(object), "result");
            //  BlockExpression block = Expression.Block(
            //     new[] { result },               //make the result a variable in scope for the block           
            //   Expression.Assign(result, propertySetter1)
            //  result                          //last value Expression becomes the return of the block 
            //   );


            //  var expr1 = Expression.Lambda<Func<IDictionary<string, object>, object>>(result, //parameterExpression).Compile();

            //var output = expr(dic);
            var output1 = expr();
        }

        /// <summary>
        /// This method will create mDictionary object and return it
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<mDictionary>> BuildLambdaAndReturnDictionaryExpression()
        {
            var expectedType = typeof(mDictionary);
            //   var displayValueParam = Expression.Parameter(typeof(bool), "displayValue");
            var ctor = Expression.New(expectedType);
            var local = Expression.Variable(expectedType, "obj"); // or Exprassion.Parameter will work
            //  var displayValueProperty = Expression.Property(local, "DisplayValue");
            var KeyExpressionGetter = Expression.Call(local, expectedType.GetMethod("SetVal"), Expression.Constant("KeyName1"), Expression.Constant("ValName1"));



            var block = Expression.Block(
                new[] { local },
                Expression.Assign(local, ctor),
                KeyExpressionGetter,
                local
                //  returnExpression,
                //  returnLabel
                );
            return Expression.Lambda<Func<mDictionary>>(block);

            //comment the above return to use the return based no label
            /* or */

            var returnTarget = Expression.Label(expectedType);
            var returnExpression = Expression.Return(returnTarget, local, expectedType);
            var returnLabel = Expression.Label(returnTarget, Expression.Default(expectedType));

            var block1 = Expression.Block(
                new[] { local },
                Expression.Assign(local, ctor),
                KeyExpressionGetter,
                 returnExpression,
                 returnLabel
                );
            return
                Expression.Lambda<Func<mDictionary>>(block1);
        }

        public static dynamic PropertySetter()
        {
            var lobjDictionaryType = typeof(mDictionary);
            var newExpression = Function.Create()
                    .InputParameter<busMSSPerson>("source")
                    .Body(
                        BodyLine.CreateVariable(lobjDictionaryType,"lobjDictionaryInstance"),
                        BodyLine.Assign("lobjDictionaryInstance", Operation.CreateInstance(lobjDictionaryType)),
                        Operation.Invoke(Operation.Variable("lobjDictionaryInstance"),"SetVal",new OperationConst("FName"),Operation.Get("source.ibusPersonPrimaryPhone.icdoPersonPhone.phone_number"))
                        
                        )
                    .Returns("lobjDictionaryInstance");

            return newExpression;

        }

        public static dynamic PropertySetterWithLoop()
        {
            var lobjDictionaryType = typeof(mDictionary);
            var newExpression = Function.Create()
                    .InputParameter<busMSSPerson>("source")
                    .Body(
                        BodyLine.CreateVariable(lobjDictionaryType, "lobjDictionaryInstance"),
                        BodyLine.Assign("lobjDictionaryInstance", Operation.CreateInstance(lobjDictionaryType)),
                        Operation.Invoke(Operation.Variable("lobjDictionaryInstance"), "SetVal", new OperationConst("Phone"), Operation.Get("source.ibusPersonPrimaryPhone.icdoPersonPhone.phone_number")),
                        Operation.Invoke(Operation.Variable("lobjDictionaryInstance"), "SetVal", new OperationConst("FName"), Operation.Get("source.FirstName")),
                        BodyLine.CreateWhile()
                        

                        )
                    .Returns("lobjDictionaryInstance");

            return newExpression;

        }

        public static Expression<Func<mDictionary>> NestedProperty(busMSSPerson aobjPerson)
        {
            var expectedType = typeof(mDictionary);
            var ctor = Expression.New(expectedType);
            var local = Expression.Parameter(expectedType, "obj");




            var KeyExpressionGetter = Expression.Call(local, expectedType.GetMethod("SetVal"), Expression.Constant("KeyName1"), Expression.Constant("ValName1"));



            var block = Expression.Block(
                new[] { local },
                Expression.Assign(local, ctor),
                KeyExpressionGetter,
                local
                );
            return Expression.Lambda<Func<mDictionary>>(block);
        }



    }
}
