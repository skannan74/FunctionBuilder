﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MobiasFunctionBuilder.Interface;

namespace MobiasFunctionBuilder.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var fn = Sample.BuildLambdaAndReturnDictionaryExpression();
            mDictionary result = fn.Compile().DynamicInvoke() as mDictionary;


            var person = new busMSSPerson()
            {
                FirstName = "Sagitec",
                LastName = "Solutions",
                PersonID = 1,
                ibusPersonPrimaryPhone = new busPersonPrimaryPhone()
                {
                    icdoPersonPhone = new cdoPersonPhone()
                    {
                        phone_number = "12345678"
                    }
                },
                ilstAddresses = new Collection<busPersonAddress>()
                {
                    new busPersonAddress() {Address1 = "Address1Item1", Address2 = "Address2Item1"},
                    new busPersonAddress() {Address1 = "Address1Item2", Address2 = "Address2Item2"},
                    new busPersonAddress() {Address1 = "Address1Item3", Address2 = "Address2Item3"},
                    new busPersonAddress() {Address1 = "Address1Item4", Address2 = "Address2Item4"},
                    new busPersonAddress() {Address1 = "Address1Item5", Address2 = "Address2Item5"}
                }


            };
            
            

            var result1 = Sample.PropertySetterWithLoop() as IExpressionResult;
            var expr = result1.ToExpression();
            var result2 = expr.Compile().DynamicInvoke(person);
            var result3 = result1.ToLambda<Func<busMSSPerson,mDictionary>>()(person);

        }
    }
}
