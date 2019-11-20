using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class ReadableAssembly
    {
        List<Nodable> namespaces;
        private Assembly assemblyFile;

        public IList<Nodable> GetChildren()
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
                if (this.namespaces[i].GetName == name)
                {
                    return true;
                }
            }
            return false;
        }
        private NodableNamespace GetNamespaceWithName(String name)
        {
            for (int i = 0; i < this.namespaces.Count(); i++)
            {
                if (this.namespaces[i].GetName == name)
                {
                    return this.namespaces[i] as NodableNamespace;
                }
            }
            return null;
        }
        private List<MethodBase> _extensionMethods = new List<MethodBase>();
        public ReadableAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();
            this.namespaces = new List<Nodable>();
            foreach (Type type in types)
            {
                NodableNamespace currentNamespace = this.GetNamespaceWithName(type.Namespace);
                if (currentNamespace == null)
                {
                    currentNamespace = new NodableNamespace(type.Namespace);
                    this.namespaces.Add(currentNamespace);
                }
                currentNamespace.AddType(type);
                //extensions check
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Static | BindingFlags.DeclaredOnly;
                var methods = type.GetMethods(bindingFlags);
                foreach (MethodBase method in methods.Where(method=>method.IsDefined(typeof(ExtensionAttribute), false)))
                {
                    this._extensionMethods.Add(method);
                }
            }
            foreach (NodableNamespace nodableNamespace in this.namespaces)
            {
                foreach (NodableClass nodableClass in nodableNamespace.GetChildren.Where(nodable=>nodable is NodableClass))
                {
                    foreach (MethodBase extensionMethod in _extensionMethods
                        .Where(extensionMethod=>extensionMethod.GetParameters()[0].ParameterType.Name == nodableClass.GetName))
                    {
                        nodableClass.AddMethod(extensionMethod);
                    }
                }
            }
            
        }
    }
}
