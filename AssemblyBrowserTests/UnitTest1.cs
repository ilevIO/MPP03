using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyBrowserTests
{
    [TestClass]
    public class UnitTest1
    {
        AssemblyBrowserLib.ReadableAssembly validAssembly;
        IList<AssemblyBrowserLib.Nodable> nodableStructure;
        String validAssemblyPath = "C:\\Users\\ilyayelagov\\source\\repos\\Lab3PuppetAssembly\\Lab3PuppetAssembly\\bin\\Debug\\netstandard2.0\\Lab3PuppetAssembly.dll";
        String invalidAssemblyPath = "C:\\Users\\ilyayelagov\\source\\repos\\Lab3PuppetAssembly\\Lab3PuppetAssembly\\bin\\Debug\\netstandard2.0\\Lab3PuppetAssemblyTrash.dll";
        [TestInitialize] 
        public void Init()
        {
            Assembly assemblyFile = Assembly.LoadFrom(validAssemblyPath);
            validAssembly = new AssemblyBrowserLib.ReadableAssembly(assemblyFile);
            nodableStructure = validAssembly.GetChildren();
        }
        [TestMethod] 
        public void TestNamespaces()
        {
            IList<AssemblyBrowserLib.Nodable> nodables = nodableStructure;
            Assert.IsTrue(nodables.Count == 2);
            AssemblyBrowserLib.Nodable nodableNamespace1 = nodableStructure.Where(nodable => nodable.GetName == "Lab3PuppetAssembly").ToList()[0];
            Assert.IsNotNull(nodableNamespace1);
            AssemblyBrowserLib.Nodable nodableNamespace2 = nodableStructure.Where(nodable => nodable.GetName == "Lab3PuppetAssemblySegundo").ToList()[0];
            Assert.IsNotNull(nodableNamespace2);
        }
        [TestMethod]
        public void TestPublicAbstractClass()
        {
            AssemblyBrowserLib.NodableNamespace nodableNamespace = nodableStructure.Where(nodable => nodable.GetName == "Lab3PuppetAssembly").ToList()[0] as AssemblyBrowserLib.NodableNamespace;
            var children = nodableNamespace.GetChildren;
            IList<AssemblyBrowserLib.Nodable> nodables = children.Where(nodable => nodable.GetName == "PublicAbstractClass").ToList();
            Assert.IsTrue(nodables.Count == 1);
            var nodableClass = nodables.First() as AssemblyBrowserLib.NodableClass;
            var attributes = nodableClass.GetAttributes();
            Assert.IsTrue(attributes.Contains("public"));
            Assert.IsTrue(attributes.Contains("abstract"));
            var classChildren = nodableClass.GetChildren;
            Assert.IsTrue(classChildren.Count == 2);
        }
        public void TestPublicStaticClass()
        {

        }
        [TestMethod]
        public void TestDamagedAssembly()
        {
            bool errorHandled = false;
            try
            {

                Assembly assemblyFile = Assembly.LoadFrom(invalidAssemblyPath);
                AssemblyBrowserLib.ReadableAssembly readableAssembly = new AssemblyBrowserLib.ReadableAssembly(assemblyFile);
                var nodes = readableAssembly.GetChildren();
            } catch (Exception e)
            {
                errorHandled = true;
            }
            Assert.IsTrue(errorHandled);
        }
    }
}
