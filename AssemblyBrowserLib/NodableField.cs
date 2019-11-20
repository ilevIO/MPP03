using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLib
{
    class NodableField: Nodable
    {
        private String _fieldType;
        private IList<String> attributes = new List<String>();
        public IList<String> GetAttributes()
        {
            return this.attributes;
        }
        public String GetPresentation
        {
            get
            {
                String presentationString = "";
                foreach(String attribute in this.attributes)
                {
                    presentationString += attribute + " ";
                }
                presentationString += this._fieldType + " ";
                presentationString += this._name;
                return presentationString;
            }
        }
        public NodableField(FieldInfo fieldInfo)
        {
            this.attributes.Add(Common.GetAccessibility(fieldInfo));
            if (fieldInfo.IsStatic)
            {
                this.attributes.Add("static");
            }
            this._name = fieldInfo.Name;
            this._fieldType = fieldInfo.FieldType.Name;
        }
    }
}
