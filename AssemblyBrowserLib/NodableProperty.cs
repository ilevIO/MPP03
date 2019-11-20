using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib
{
    class NodableProperty: Nodable
    {
        private IList<String> attributes = new List<String>();
        private IList<String> getterAttributes = new List<String>();
        private IList<String> setterAttributes = new List<String>();
        private String _returnType;
        private bool hasSetter = false;
        private bool hasGetter = false;
        public IList<String> GetAttributes()
        {
            return this.attributes;
        }
        public String GetPresentation
        {
            get
            {
                String presentationString = "";
                foreach (String attribute in this.attributes)
                {
                    presentationString += attribute + " ";
                }
                presentationString += this._returnType + " ";
                presentationString += this._name;
                presentationString += "{";
                if (this.hasGetter)
                {
                    foreach(String attribute in getterAttributes)
                    {
                        presentationString += attribute + " ";
                    }
                    presentationString += "get; ";
                }
                if (this.hasSetter)
                {
                    foreach (String attribute in setterAttributes)
                    {
                        presentationString += attribute + " ";
                    }
                    presentationString += "set; ";
                }
                presentationString += "}";
                return presentationString;
            }
        }
        public NodableProperty(PropertyInfo property)
        {
            this._name = property.Name;
            this._returnType = property.PropertyType.Name;
            this.attributes.Add(Common.GetTypeAccessibility(property.PropertyType));

            var accessors = property.GetAccessors();
            if (property.CanRead)
            {
                this.getterAttributes = Common.GetMethodAttributes(property.GetMethod);
                this.hasGetter = true;
            }
            if (property.CanWrite)
            {
                this.hasSetter = true;
                this.setterAttributes = Common.GetMethodAttributes(property.SetMethod);
            }
            //property.GetAccessors();
        }
    }
}
