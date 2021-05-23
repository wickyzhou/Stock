using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Common
{
    public static class ModelTypeHelper
    {
        public static void PropertyMapper<T1, T2>( T1 source, T2 destination) 
        {
            try
            {
                var destinationTypes = destination.GetType();
                var sourceTypes = source.GetType();
                foreach (PropertyInfo sp in sourceTypes.GetProperties())
                {
                    foreach (PropertyInfo dp in destinationTypes.GetProperties())
                    {
                        if (sp.Name == dp.Name)
                        {
                            sp.SetValue(source, dp.GetValue(destination, null), null);//获得d对象属性的值复制给s对象的属性
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T Clone<T>(this T obj) where T : class
        {
            if (obj is string || obj.GetType().IsValueType)
                return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    field.SetValue(retval, Clone(field.GetValue(obj)));
                }
                catch { }
            }
            return (T)retval;
        }

    }



    public static class ObjectDeepCopy<TIn, TOut>
    {

        private static readonly Func<TIn, TOut> cache = GetFunc();
        private static Func<TIn, TOut> GetFunc()
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
            List<MemberBinding> memberBindingList = new List<MemberBinding>();

            foreach (var item in typeof(TOut).GetProperties())
            {
                if (!item.CanWrite) continue;
                MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
                MemberBinding memberBinding = Expression.Bind(item, property);
                memberBindingList.Add(memberBinding);
            }

            MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
            Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

            return lambda.Compile();
        }

        public static TOut Trans(TIn tIn)
        {
            return cache(tIn);
        }
    }

}
