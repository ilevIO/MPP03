using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyBrowserLib
{
    public class NodableNamespace: Nodable
    {
        List<Nodable> classes = new List<Nodable>();
        List<Nodable> interfaces = new List<Nodable>();
        public void AddType(Type newType)
        {
            if (!newType.IsNested) {
                if (newType.IsClass)
                {
                    NodableClass newClass = new NodableClass(newType);
                    this.classes.Add(newClass);
                } else if (newType.IsInterface)
                {
                    NodableInterface newInterface = new NodableInterface(newType);
                    this.interfaces.Add(newInterface);
                } else if (newType.IsEnum)
                {

                }
             }
        }
        public IList<Nodable> GetChildren
        {
            get
            {
                List<Nodable> result = new List<Nodable>();
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
        }

        public string GetPresentation
        {
            get
            {
                return this._name;
            }
        }

        public NodableNamespace(String name)
        {
            this._name = name;
        }
    }
}
