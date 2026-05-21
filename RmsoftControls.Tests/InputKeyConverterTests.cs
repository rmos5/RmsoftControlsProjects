using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RmsoftControls.TextControls;
using System.Windows.Input;

namespace RmsoftControls.Tests
{
    [TestClass]
    public class InputKeyConverterTests
    {
        private readonly InputKeyConverter converter = new InputKeyConverter();

        [TestMethod]
        public void Convert_WithValidKeyString_ReturnsSameKeyString()
        {
            var result = converter.Convert("Enter", typeof(string), null, CultureInfo.InvariantCulture);

            Assert.AreEqual("Enter", result);
        }

        [TestMethod]
        public void Convert_WithInvalidValue_ReturnsDefaultKeyString()
        {
            var result = converter.Convert("not-a-key", typeof(string), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(Key.Return.ToString(), result);
        }

        [TestMethod]
        public void ConvertBack_WithValidKeyString_ReturnsKeyEnum()
        {
            var result = converter.ConvertBack("Escape", typeof(Key), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(Key.Escape, result);
        }

        [TestMethod]
        public void ConvertBack_WithNull_ReturnsDefaultKey()
        {
            var result = converter.ConvertBack(null, typeof(Key), null, CultureInfo.InvariantCulture);

            Assert.AreEqual(Key.Return, result);
        }
    }
}
