using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MobiasFunctionBuilder.ConsoleTest
{
    public class Sample
    {

        private void meth()
        {
            ParameterExpression dictExpr = Expression.Parameter(typeof(Dictionary<string, int>));
            ParameterExpression keyExpr = Expression.Parameter(typeof(string));
            ParameterExpression valueExpr = Expression.Parameter(typeof(int));

            // Simple and direct. Should normally be enough
            // PropertyInfo indexer = dictExpr.Type.GetProperty("Item");

            // Alternative, note that we could even look for the type of parameters, if there are indexer overloads.
            PropertyInfo indexer = (dictExpr.Type.GetDefaultMembers()
                .OfType<PropertyInfo>()
                .Where(p => p.PropertyType == typeof(int))
                .Select(p => new { p, q = p.GetIndexParameters() })
                .Where(t => t.q.Length == 1 && t.q[0].ParameterType == typeof(string))
                .Select(t => t.p)).Single();

            PropertyInfo indexer1 = (from p in dictExpr.Type.GetDefaultMembers().OfType<PropertyInfo>()
                                         // This check is probably useless. You can't overload on return value in C#.
                                     where p.PropertyType == typeof(int)
                                     let q = p.GetIndexParameters()
                                     // Here we can search for the exact overload. Length is the number of "parameters" of the indexer, and then we can check for their type.
                                     where q.Length == 1 && q[0].ParameterType == typeof(string)
                                     select p).Single();



            IndexExpression indexExpr = Expression.Property(dictExpr, indexer, keyExpr);

            BinaryExpression assign = Expression.Assign(indexExpr, valueExpr);

            var lambdaSetter = Expression.Lambda<Action<Dictionary<string, int>, string, int>>(assign, dictExpr, keyExpr, valueExpr);
            var lambdaGetter = Expression.Lambda<Func<Dictionary<string, int>, string, int>>(indexExpr, dictExpr, keyExpr);
            var setter = lambdaSetter.Compile();
            var getter = lambdaGetter.Compile();

            var dict = new Dictionary<string, int>();
            setter(dict, "MyKey", 2);
            var value = getter(dict, "MyKey");
        }

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

        public static Expression<Func<mDictionary>> BuildLambda()
        {
            var expectedType = typeof(mDictionary);
            //   var displayValueParam = Expression.Parameter(typeof(bool), "displayValue");
            var ctor = Expression.New(expectedType);
            var local = Expression.Parameter(expectedType, "obj");
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



        public class mDictionary : Dictionary<string, object>
        {
            public void SetVal(string key, object val)
            {
                this[key] = val;
            }
        }

        public class MyObject
        {
            public bool DisplayValue { get; set; }
        }
        Expression<Func<bool, MyObject>> BuildLambda1()
        {
            var createdType = typeof(MyObject);
            var displayValueParam = Expression.Parameter(typeof(bool), "displayValue");
            var ctor = Expression.New(createdType);
            var displayValueProperty = createdType.GetProperty("DisplayValue");
            var displayValueAssignment = Expression.Bind(
                displayValueProperty, displayValueParam);
            var memberInit = Expression.MemberInit(ctor, displayValueAssignment);

            return
                Expression.Lambda<Func<bool, MyObject>>(memberInit, displayValueParam);
        }

        public static Func<bool, dynamic> Creator;

        public static Func<bool, dynamic> BuildLambda2()
        {
            var expectedType = typeof(MyObject);
            var displayValueParam = Expression.Parameter(typeof(bool), "displayValue");
            var ctor = Expression.New(expectedType);
            var local = Expression.Parameter(expectedType, "obj");
            var displayValueProperty = Expression.Property(local, "DisplayValue");

            var returnTarget = Expression.Label(expectedType);
            var returnExpression = Expression.Return(returnTarget, local, expectedType);
            var returnLabel = Expression.Label(returnTarget, Expression.Default(expectedType));

            var block = Expression.Block(
                new[] { local },
                Expression.Assign(local, ctor),
                Expression.Assign(displayValueProperty, displayValueParam),
                /* I forgot to remove this line:
                 * Expression.Return(Expression.Label(expectedType), local, expectedType), 
                 * and now it works.
                 * */
                returnExpression,
                returnLabel
                );
            return
                Expression.Lambda<Func<bool, dynamic>>(block, displayValueParam)
                    .Compile();
        }
    }
}
