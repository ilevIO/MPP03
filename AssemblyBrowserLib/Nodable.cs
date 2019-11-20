using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLib
{
    public class Nodable
    {
        protected String _name;
        protected IList<Nodable> _children;
        public String GetName { get { return _name; } }
        public String GetPresentation { get; }
        public IList<Nodable> GetChildren { get { return _children; } }
    }
}
