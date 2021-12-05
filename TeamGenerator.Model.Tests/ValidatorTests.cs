using NUnit.Framework;
using System;
using TeamGenerator.Model.Validators;

namespace TeamGenerator.Model.Tests
{
    [TestFixture]
    public class ValidatorTests
    {
        [Test]
        public void ValidateString_ThrowsException_WhenStringIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Validator.ValidateString(null));
        }

        [Test]
        public void ValidateString_ThrowsException_WhenNickIsEmptyString()
        {
            Assert.Throws<ArgumentException>(() => Validator.ValidateString(""));
        }

        [Test]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void ValidateString_ThrowsException_WhenNickIsWhiteSpaceOnly(string whiteSpaceString)
        {
            Assert.Throws<ArgumentException>(() => Validator.ValidateString(whiteSpaceString));
        }

        [Test]
        public void ValidateDouble_ThrowsException_WhenValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Validator.ValidateDouble(null));
        }
    }
}
