using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib
{
    class NodableInterface: Nodable
    {
        private List<String> attributes = new List<String>();
        private List<NodableMethod> methods = new List<NodableMethod>();
        private List<NodableProperty> properties = new List<NodableProperty>();
        private List<String> interfaces = new List<String>();
        public IList<String> GetAttributes()
        {
            return this.attributes;
        }
        public IList<Nodable> GetChildren
        {
            get {
                IList<Nodable> children = new List<Nodable>();
                children = children.Concat(this.methods).Concat(this.properties).ToList();
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
                presentationString += "interface ";
                presentationString += this._name;
                
                bool hasInterfaces = this.interfaces.Count > 0;
                
                if (hasInterfaces)
                {
                    presentationString += ": ";
                    for (int i = 0; i < interfaces.Count - 1; i++)
                    {
                            presentationString += interfaces[i] + ", ";
                    }
                    presentationString += interfaces[interfaces.Count - 1];
                }
                return presentationString;
            }
        }
        public NodableInterface(Type interfaceInfo)
        {
            this._name = interfaceInfo.Name;
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Static | BindingFlags.DeclaredOnly;
            this.attributes.Add(Common.GetTypeAccessibility(interfaceInfo));
            var methods = interfaceInfo.GetMethods(bindingFlags);
            foreach (MethodBase method in methods)
            {
                this.methods.Add(new NodableMethod(method));
            }
            var properties = interfaceInfo.GetProperties(bindingFlags);
            foreach (PropertyInfo property in properties)
            {
                this.properties.Add(new NodableProperty(property));
            }
        }
    }
}
