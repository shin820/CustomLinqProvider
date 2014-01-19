namespace CustomLinqProvider
{
    using System;
    using System.Collections.Generic;

    internal static class TypeSystem
    {
        internal static Type GetElementType(Type type)
        {
            Type ienum = FindIEnumerable(type);
            return ienum == null ? type : ienum.GetGenericArguments()[0];
        }

        private static Type FindIEnumerable(Type type)
        {
            if (type == null || type == typeof(string))
                return null;
            if (type.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(type.GetElementType());

            if (type.IsGenericType)
            {
                foreach (Type argument in type.GetGenericArguments())
                {
                    Type argumentType = typeof(IEnumerable<>).MakeGenericType(argument);
                    if (argumentType.IsAssignableFrom(type))
                    {
                        return argumentType;
                    }
                }
            }

            Type[] interfaces = type.GetInterfaces();
            if (interfaces != null && interfaces.Length > 0)
            {
                foreach (Type iface in interfaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }

            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                return FindIEnumerable(type.BaseType);
            }

            return null;
        }
    }
}