//-----------------------------------------------------------------------
// <copyright file="ShowIfAttribute.cs" company="Lost Signal LLC">
//     Copyright (c) Lost Signal LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Lost
{
    using System;
    using UnityEngine;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : PropertyAttribute
    {
        private readonly string conditionFieldName;
        private readonly object compareValue;

        public string ConditionFieldName => this.conditionFieldName;

        public object CompareValue => this.compareValue;

        public ShowIfAttribute(string conditionFieldName, object compareValue)
        {
            this.conditionFieldName = conditionFieldName;
            this.compareValue = compareValue;
        }
    }
}
