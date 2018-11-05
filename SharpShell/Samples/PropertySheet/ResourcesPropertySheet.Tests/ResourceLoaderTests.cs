using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ResourcesPropertySheet.Loader;

namespace ResourcesPropertySheet.Tests
{
    /// <summary>
    /// These tests are functional tests rather than unit tests, but they do the trick and provide
    /// a pretty reasonable degree of confidence that this is all working.
    /// </summary>
    [TestFixture]
    public class ResourceLoaderTests
    {
        [Test]
        public void CanLoadResourceTypes()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestFiles\Dbgview.exe");
            var resourceTypes = ResourceLoader.LoadResources(path);
            var resourceTypeStrings = resourceTypes.Select(rt => rt.ResourceType.ToString()).ToArray();

            //  Assert we have the expected set of resource types.
            Assert.Contains("Accelerator", resourceTypeStrings);
            Assert.Contains("\"BINRES\"", resourceTypeStrings);
            Assert.Contains("Bitmap", resourceTypeStrings);
            Assert.Contains("Cursor", resourceTypeStrings);
            Assert.Contains("Dialog", resourceTypeStrings);
            Assert.Contains("Icon", resourceTypeStrings);
            Assert.Contains("Menu", resourceTypeStrings);
            Assert.Contains("RT_MANIFEST", resourceTypeStrings);
            Assert.Contains("String Table", resourceTypeStrings);
            Assert.Contains("Version", resourceTypeStrings);

            //  Spot check the accelerator resources.
            var accelerators = resourceTypes.Single(rt => rt.ResourceType.IsKnownResourceType(ResType.RT_ACCELERATOR)).ResourceNames.Select(rn => rn.ToString()).ToArray();
            Assert.Contains("\"ACCELERATORS\"", accelerators);

            //  Spot check the bitmap resources.
            var bitmaps = resourceTypes.Single(rt => rt.ResourceType.IsKnownResourceType(ResType.RT_BITMAP)).ResourceNames.Select(rn => rn.ToString()).ToArray();
            Assert.Contains("400", bitmaps);
            Assert.Contains("\"IDB_DISCONN\"", bitmaps);
            Assert.Contains("\"IDB_SELOFF\"", bitmaps);
            Assert.Contains("\"IDB_SELON\"", bitmaps);
            Assert.Contains("\"IDB_UNSELOFF\"", bitmaps);
            Assert.Contains("\"IDB_UNSELON\"", bitmaps);
        }
    }
}
