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
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestFiles\DllWithResources.dll");
            var resources = ResourceLoader.LoadResources(path);
            var resourceTypeStrings = resources.Select(rt => rt.ResourceType.ToString()).ToArray();

            //  Assert we have the expected set of resource types.
            Assert.Contains("Bitmap", resourceTypeStrings);
            Assert.Contains("Cursor", resourceTypeStrings);
            Assert.Contains("Dialog", resourceTypeStrings);
            Assert.Contains("HTML", resourceTypeStrings);
            Assert.Contains("Group Cursor", resourceTypeStrings);
            Assert.Contains("Group Icon", resourceTypeStrings);
            Assert.Contains("Icon", resourceTypeStrings);
            Assert.Contains("Menu", resourceTypeStrings);
            Assert.Contains("\"PNG\"", resourceTypeStrings);
            Assert.Contains("RT_MANIFEST", resourceTypeStrings);
            Assert.Contains("\"RT_RIBBON_XML\"", resourceTypeStrings);
            Assert.Contains("241", resourceTypeStrings); // toolbars
            Assert.Contains("Version", resourceTypeStrings);

            //  Check we have loaded a bitmap property.
            var bitmaps = resources.Single(rt => rt.ResourceType.IsKnownResourceType(ResType.RT_BITMAP));
            var bitmap103 = bitmaps.Resouces.Single(b => b.ResourceName.IsInt && b.ResourceName.IntValue == 103);
            Assert.AreEqual(bitmap103.ResourceName.IsInt, true);
            Assert.AreEqual(bitmap103.ResourceName.IntValue, 103);
            Assert.AreEqual(bitmap103.ResourceName.ToString(), "103");
            Assert.AreEqual(bitmap103.BitmapData.Width, 48);
            Assert.AreEqual(bitmap103.BitmapData.Height, 48);
        }
    }
}
