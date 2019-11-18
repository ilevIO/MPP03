using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_Assembly_Browser
{
    interface IReadable
    {
        String GetName();
        String GetPresentation();
        IList<IReadable> GetChildren();
    }
    interface IProperty: IReadable
    {

    }
    interface IMethod: IReadable
    {
        Type GetReturnType();
        /*Fix type*/
        String GetAccessibility(); 
    }
    interface IClass: IReadable
    {
        Type GetSuperclass();
        IList<IMethod> GetMethods();
        IList<IProperty> GetProperties();
        /*Fix type*/
        String GetExtensions();
    }
    interface IInterface: IReadable
    {

    }
    class Method : IMethod
    {
        String name;
        String returnType;
        IList<String> genericParameters = new List<String>();
        IList<String> parameters = new List<String>();
        IList<String> attributes = new List<String>();
        public string GetAccessibility()
        {
            throw new NotImplementedException();
        }

        public IList<IReadable> GetChildren()
        {
            return null;
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public Type GetReturnType()
        {
            throw new NotImplementedException();
        }

        public string GetPresentation()
        {
            string presentationString = "";
            for (int i = 0; i < presentationString.Length; i++)
            {
                presentationString += attributes[i] + " ";
            }
            presentationString += this.returnType + " ";
            presentationString += this.name;
            presentationString += "(";
            for (int i = 0; i < this.parameters.Count - 1; i++)
            {
                presentationString += parameters[i] + ", ";
            }
            presentationString += parameters.Last();
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
        String GetFormattedGenericTypeArtuments(Type type)
        {
            String result = "<";
            for (int i = 0; i < type.GenericTypeArguments.Length - 1; i++)
            {
                result += type.GenericTypeArguments[i] + ", ";
            }
            result += type.GenericTypeArguments[type.GenericTypeArguments.Length];
            result += ">";
            return result;
        }
        public Method(MethodBase methodInfo)
        {
            if (methodInfo.IsStatic)
            {
                this.attributes.Add("static");
            }
            if (methodInfo.IsVirtual)
            {
                this.attributes.Add("virtual");
            } else if (methodInfo.IsFinal)
            {
                this.attributes.Add("final");
            } else if (methodInfo.IsAbstract)
            {
                this.attributes.Add("abstract");
            }
            if (methodInfo.IsPublic)
            {
                this.attributes.Add("public");
            } else if (methodInfo.IsPrivate)
            {
                this.attributes.Add("private");
            } else if (methodInfo.IsFamily)
            {
                this.attributes.Add("protected");
            } else if (methodInfo.IsAssembly)
            {
                this.attributes.Add("internal");
            }
            this.returnType = methodInfo.DeclaringType.Name;
            if (methodInfo.DeclaringType.IsGenericType)
            {
                this.returnType += GetFormattedGenericTypeArtuments(methodInfo.DeclaringType);
            }
            this.name = methodInfo.Name;
            var methodParameters = methodInfo.GetParameters();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                var parameterName = methodParameters[i].Name;
                var parameterType = methodParameters[i].ParameterType.Name;
                if (methodParameters[i].ParameterType.IsGenericParameter)
                {
                    parameterType += GetFormattedGenericTypeArtuments(methodParameters[i].ParameterType);
                }
                //inout
                this.parameters.Add(parameterType + " " + parameterName);
            }
        }
    }
    class Class: IClass
    {
        String name;
        IList<String> attributes = new List<String>();
        IList<IReadable> methods;
        IList<IReadable> properties;
        IList<IReadable> constructors;
        IList<IReadable> fields;
        String baseClass;
        IList<String> interfaces;
        IList<String> genericParameters;
        public Class(String name)
        {
            this.name = name;
        }

        public IList<IReadable> GetChildren()
        {
            IList <IReadable> children= new List<IReadable>();
            children = children.Concat(this.methods).Concat(this.constructors).ToList();
            return children;
        }

        public string GetExtensions()
        {
            throw new NotImplementedException();
        }

        public IList<IMethod> GetMethods()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return this.name;
        }

        public IList<IProperty> GetProperties()
        {
            throw new NotImplementedException();
        }

        public Type GetSuperclass()
        {
            throw new NotImplementedException();
        }

        public string GetPresentation()
        {
            String presentationString = "";
            for (int i = 0; i < attributes.Count; i++)
            {
                presentationString += attributes[i] + " ";
            }
            presentationString += this.name;
            if (this.genericParameters.Count > 0)
            {
                presentationString += "<";
                for (int i = 0; i < genericParameters.Count - 1; i++)
                {
                    presentationString += genericParameters[i] + ", ";
                }
                presentationString += genericParameters[genericParameters.Count - 1];
                presentationString += ">";
            }
            bool hasInterfaces = this.interfaces.Count > 0;
            bool isSubclass = this.baseClass != null;
            if (isSubclass || hasInterfaces)
            {
                presentationString += ": ";
                if (isSubclass)
                {
                    presentationString += baseClass;
                    if (hasInterfaces)
                    {
                        presentationString += ", ";
                    }
                }
                if (hasInterfaces)
                { 
                    for (int i = 0; i < interfaces.Count - 1; i++)
                    {
                        presentationString += interfaces[i] + ", ";
                    }
                    presentationString += interfaces[interfaces.Count - 1];
                }
            }
            return presentationString;
        }

        public Class(Type classType)
        {
            this.name = classType.Name;
            var attributes = classType.Attributes;
            var genericAttributes = classType.GenericTypeArguments;
            var constructors = classType.GetConstructors();
            for (int i = 0; i < constructors.Length; i++)
            {
                var constructor = new Method(constructors[i]);
                /*var x = constructors[i].GetParameters();
                bool isStatic = constructors[i].IsStatic;
                bool isPublic = constructors[i].IsPublic;
                bool isPrivate = constructors[i].IsPrivate;
                bool isAbstract = constructors[i].IsAbstract;
                bool isFinal = constructors[i].IsFinal;
                var attributes = constructors[i].Attributes;
                var isVirtual = constructors[i].IsVirtual;
                var genericParams = constructors[i].GetGenericArguments();
                bool isConstructor = true;*/
                this.constructors.Add(constructor);
            }
            var methods = classType.GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                this.methods.Add(new Method(methods[i]));
            }
            var properties = classType.GetProperties();
            var fields = classType.GetFields();
        }
    }
    class Namespace: IReadable
    {
        String name;
        List<IReadable> classes;
        List<IReadable> interfaces;
        public void AddType(Type newType)
        {
            if (newType.IsClass)
            {
                Class newClass = new Class(newType.Name);
                this.classes.Add(newClass);
                var superclass = newType.BaseType;
            }
        }
        public IList<IReadable> GetChildren()
        {
            List<IReadable> result = new List<IReadable>();
            for (int i = 0; i < classes.Count; i++)
            {
                result.Add(classes[i]);
            }
            for (int i = 0; i < interfaces.Count; i++)
            {
                result.Add(interfaces[i]);
            }
            return result;
        }

        public string GetName()
        {
            return this.name;
        }

        public string GetPresentation()
        {
            throw new NotImplementedException();
        }

        public Namespace (String name)
        {
            this.name = name;
        }
    }
    
    class main: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(String propertyName)
        {

        }
        /*TreeViewNodeType*/void AddNode(String title)
        {
            //treeview add node
            //return created node
        }
        /*TreeViewNodeType*/void InsertNode(String title)
        {
            //treeview insertnode
        }
        void Iterate(IReadable nodeElement)
        {
            AddNode(nodeElement.GetPresentation());
            var children = nodeElement.GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                InsertNode(children[i].GetPresentation());
            }
        }
        public void AssemblyLoaded(String filePath) 
        {
            Assembly assemblyFile = Assembly.LoadFrom(filePath);
            var readableAssembly = new ReadableAssembly(assemblyFile);
        }
    }
}
