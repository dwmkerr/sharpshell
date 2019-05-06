using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourcesPropertySheet.Loader;

namespace ResourcesPropertySheet.ResourceEditors
{
    interface IResourceEditor
    {
        void Initialise(Win32Resource resource);

        void Release();
    }
}
