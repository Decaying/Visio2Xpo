using System;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cvo.buyshans.Visio2Xpo.VisioReaderTest
{
    [TestClass]
    public class NameValidatorFixture
    {
        private readonly IValidator<String> _NameValidator = new NameValidator("Test");
            
        [TestMethod]
        public void NameCannotBeEmpty()
        {
            const Boolean expected = false;
            var actual =_NameValidator.Validate("");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameCannotStartWithANumber()
        {
            const Boolean expected = false;
            var actual = _NameValidator.Validate("1CustTable");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameCannotContainSpaces()
        {
            const Boolean expected = false;
            var actual = _NameValidator.Validate("A B");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameMaxLength()
        {
            const Boolean expected = false;
            var actual = _NameValidator.Validate("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NameSpecialCharacters()
        {
            const Boolean expected = false;

            var actual1 = _NameValidator.Validate("$");
            var actual2 = _NameValidator.Validate("^");
            var actual3 = _NameValidator.Validate(")");
            var actual4 = _NameValidator.Validate("&");
            var actual5 = _NameValidator.Validate("@");

            Assert.AreEqual(expected, actual1);
            Assert.AreEqual(expected, actual2);
            Assert.AreEqual(expected, actual3);
            Assert.AreEqual(expected, actual4);
            Assert.AreEqual(expected, actual5);
        }

        [TestMethod]
        public void NameCustTable1IsOk()
        {
            const Boolean expected = true;

            var actual = _NameValidator.Validate("CustTable1");

            Assert.AreEqual(expected, actual);
        }
    }
}