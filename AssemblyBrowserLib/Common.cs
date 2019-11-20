using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib
{
    class Common
    {
        public static String GetFormattedGenericTypeArtuments(Type type)
        {
            String result = "<";
            for (int i = 0; i < type.GenericTypeArguments.Length - 1; i++)
            {
                result += type.GenericTypeArguments[i] + ", ";
            }
            result += type.GenericTypeArguments[type.GenericTypeArguments.Length-1];
            result += ">";
            return result;
        }
        public static string GetAccessibility(FieldInfo fieldInfo)
        {
            if (fieldInfo.IsPublic)
            {
                return "public";
            } else if (fieldInfo.IsFamily)
            {
                return "protected";
            }  else if (fieldInfo.IsAssembly)
            {
                return "internal";
            }
            return "private";
        }
        public static List<String> GetMethodAttributes(MethodBase methodInfo)
        {
            var attributes = new List<String>();
            if (methodInfo.IsStatic)
            {
                attributes.Add("static");
            }
            if (methodInfo.IsVirtual)
            {
                attributes.Add("virtual");
            }
            else if (methodInfo.IsFinal)
            {
                attributes.Add("final");
            }
            else if (methodInfo.IsAbstract)
            {
                attributes.Add("abstract");
            }
            if (methodInfo.IsPublic)
            {
                attributes.Add("public");
            }
            else if (methodInfo.IsPrivate)
            {
                attributes.Add("private");
            }
            else if (methodInfo.IsFamily)
            {
                attributes.Add("protected");
            }
            else if (methodInfo.IsAssembly)
            {
                attributes.Add("internal");
            }
            return attributes;
        }
        public static String GetTypeAccessibility(Type type)
        {
            if (type.IsPublic)
            {
                return "public";
            } else if (type.IsNestedFamily)
            {
                return "protected";
            } else if (type.IsNestedAssembly)
            {
                return "assembly";
            }
            return "private";
        }
    }
}
