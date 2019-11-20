using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AssemblyBrowserLib
{

    public class NodableMethod : Nodable
    {
        String returnType;
        IList<String> genericParameters = new List<String>();
        IList<String> parameters = new List<String>();
        IList<String> attributes = new List<String>();
        public IList<String> GetAttributes()
        {
            return this.attributes;
        }
        public string GetAccessibility()
        {
            throw new NotImplementedException();
        }

        public IList<Nodable> GetChildren
        {
            get
            {
                return null;
            }
        }

        public string GetPresentation
        {
            get
            {
                string presentationString = "";
                for (int i = 0; i < attributes.Count; i++)
                {
                    presentationString += attributes[i] + " ";
                }
                presentationString += this.returnType + " ";
                presentationString += this._name;
                presentationString += "(";
                if (parameters.Count > 0)
                {
                    for (int i = 0; i < this.parameters.Count - 1; i++)
                    {
                        presentationString += parameters[i] + ", ";
                    }
                    presentationString += parameters.Last();
                }
                presentationString += ")";
                /*if (this.genericParameters.Count > 0)
                {
                    presentationString += "<";
                    for (int i = 0; i < this.genericParameters.Count - 1; i++)
                    {
                        presentationString += genericParameters[i] + ", ";
                    }
                    presentationString += genericParameters[genericParameters.Count - 1];
                    presentationString += ">";
                }*/
                return presentationString;
            }
        }
        public NodableMethod(MethodBase methodInfo)
        {
            this.attributes = Common.GetMethodAttributes(methodInfo);
            this.returnType = methodInfo.DeclaringType.Name;
            if (methodInfo.DeclaringType.GenericTypeArguments.Length > 0)
            {
                this.returnType += Common.GetFormattedGenericTypeArtuments(methodInfo.DeclaringType);
            }
            this._name = methodInfo.Name;
            if (methodInfo.IsDefined(typeof(ExtensionAttribute), false))
            {
                this._name = "(extension)" + this._name;
            }
            var methodParameters = methodInfo.GetParameters();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var parameterName = methodParameters[i].Name;
                var parameterType = methodParameters[i].ParameterType.Name;
                if (methodParameters[i].ParameterType.IsGenericParameter)
                {
                    parameterType += Common.GetFormattedGenericTypeArtuments(methodParameters[i].ParameterType);
                }
                //inout
                this.parameters.Add(parameterType + " " + parameterName);
            }
        }
    }
}
