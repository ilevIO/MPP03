using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3_Assembly_Browser
{
    class ReadableAssembly
    {
        List<IReadable> namespaces;
        private Assembly assemblyFile;

        public IList<IReadable> GetChildren()
        {
            return this.namespaces;
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }
        private bool NamespaceExistsWithName(String name)
        {
            for (int i = 0; i < this.namespaces.Count(); i++)
            {
                if (this.namespaces[i].GetName() == name)
                {
                    return true;
                }
            }
            return false;
        }
        private Namespace GetNamespaceWithName(String name)
        {
            for (int i = 0; i < this.namespaces.Count(); i++)
            {
                if (this.namespaces[i].GetName() == name)
                {
                    return this.namespaces[i] as Namespace;
                }
            }
            return null;
        }
        public ReadableAssembly(Assembly assembly)
        {

            var types = assembly.GetTypes();

            this.namespaces = new List<IReadable>();
            for (int i = 0; i < types.Count(); i++)
            {
                Namespace currentNamespace = this.GetNamespaceWithName(types[i].Namespace);
                if (currentNamespace == null)
                {
                    currentNamespace = new Namespace(types[i].Namespace);
                    this.namespaces.Add(currentNamespace);
                }
                currentNamespace.AddType(types[i]);
            }
        }
    }
}
