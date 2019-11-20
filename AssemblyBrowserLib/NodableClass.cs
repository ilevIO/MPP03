using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AssemblyBrowserLib
{
    public class NodableClass: Nodable
    {
        IList<String> attributes = new List<String>();
        private IList<Nodable> methods = new List<Nodable>();
        private IList<Nodable> properties = new List<Nodable>();
        private IList<Nodable> constructors = new List<Nodable>();
        private IList<Nodable> fields = new List<Nodable>();
        private IList<Nodable> nestedTypes = new List<Nodable>();
        String baseClass;
        IList<String> interfaces = new List<String>();
        IList<String> genericParameters = new List<String>();
        public IList<String> GetAttributes()
        {
            return this.attributes;
        }
        public IList<Nodable> GetChildren
        {
            get
            {
                IList<Nodable> children = new List<Nodable>();
                children = children.Concat(this.nestedTypes).Concat(this.fields).Concat(this.methods).Concat(this.properties).Concat(this.constructors).ToList();
                return children;
            }
        }
        public string GetPresentation
        {
            get
            {
                String presentationString = "";
                for (int i = 0; i < attributes.Count; i++)
                {
                    presentationString += attributes[i] + " ";
                }
                presentationString += "class ";
                presentationString += this._name;
                
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
        }
        public void AddMethod(MethodBase methodInfo)
        {
            this.methods.Add(new NodableMethod(methodInfo));
        }
        public NodableClass(Type classType)
        {
            this._name = classType.Name;
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Static | BindingFlags.DeclaredOnly;
            var constructors = classType.GetConstructors(bindingFlags);

            if (classType.IsPublic)
            {
                this.attributes.Add("public");
            }
            if (classType.IsSealed)
            {
                this.attributes.Add("sealed");
            }
            if (classType.IsAbstract)
            {
                this.attributes.Add("abstract");
            }
            var interfaces = classType.GetInterfaces();
            foreach (Type classInterface in interfaces)
            {
                this.interfaces.Add(classInterface.Name);
            }
            //Yes I don't remember how to determine that a type is not declared
            if (classType.BaseType != null && classType.BaseType.Name != "Object")
            {
                this.baseClass = classType.BaseType.Name;
            }
            for (int i = 0; i < constructors.Length; i++)
            {
                var constructor = new NodableMethod(constructors[i]);
                this.constructors.Add(constructor);
            }
            var nestedTypes = classType.GetNestedTypes(bindingFlags);
            foreach (Type newType in nestedTypes)
            {
                if (newType.IsClass)
                {
                    NodableClass newClass = new NodableClass(newType);
                    this.nestedTypes.Add(newClass);
                }
                else if (newType.IsInterface)
                {
                    NodableInterface newInterface = new NodableInterface(newType);
                    this.nestedTypes.Add(newInterface);
                }
            }
            var methods = classType.GetMethods(bindingFlags);
            foreach(MethodBase method in methods.Where(method=>!method.IsDefined(typeof(ExtensionAttribute), false)))
            {
                this.methods.Add(new NodableMethod(method));
            }
            var properties = classType.GetProperties(bindingFlags);
            foreach (PropertyInfo property in properties.Where(property => !property.IsDefined(typeof(ExtensionAttribute), false))) {
                this.properties.Add(new NodableProperty(property));
            }
            var fields = classType.GetFields(bindingFlags);
            foreach (FieldInfo fieldInfo in fields)
            {
                this.fields.Add(new NodableField(fieldInfo));
            }
        }
    }
}
