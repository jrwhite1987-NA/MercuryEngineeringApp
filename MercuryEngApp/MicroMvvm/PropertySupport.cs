using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MicroMvvm
{
    public static class PropertySupport
    {
        /// <summary>
        /// Extract The property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpresssion"></param>
        /// <returns></returns>
        public static String ExtractPropertyName<T>(Expression<Func<T>> propertyExpresssion)
        {
            //Throw exception if property expression is null
            if (propertyExpresssion == null)
            {
                throw new ArgumentNullException("propertyExpresssion");
            }

            var memberExpression = propertyExpresssion.Body as MemberExpression;
            //throw exception if member expression is null
            if (memberExpression == null)
            {
                throw new ArgumentException("The expression is not a member access expression.", "propertyExpresssion");
            }

            var property = memberExpression.Member as PropertyInfo;
            //throw exception if property expression is null
            if (property == null)
            {
                throw new ArgumentException("The member access expression does not access a property.", "propertyExpresssion");
            }

            var getMethod = property.GetGetMethod(true);
            //throw exception if getmethodexpression is null
            if (getMethod.IsStatic)
            {
                throw new ArgumentException("The referenced property is a static property.", "propertyExpresssion");
            }

            //Return member expression name
            return memberExpression.Member.Name;
        }
    }
}
