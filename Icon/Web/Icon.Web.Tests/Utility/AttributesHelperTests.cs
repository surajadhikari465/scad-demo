using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Icon.Common;
using Icon.Common.Models;

namespace Icon.Web.Tests.Unit.Utility
{
    [TestClass]
    public class AttributesHelperTests
    {
        private AttributesHelper attributesHelper;
        private List<CharacterSetModel> characterSetModels;
        private string specialCharacters;
        private int dataTypeId;

        [TestInitialize]
        public void Initialize()
        {
            attributesHelper = new AttributesHelper();
            characterSetModels = new List<CharacterSetModel>();
            dataTypeId = (int)DataType.Text;
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_DataTypeIsNotText_ReturnsNull()
        {
            //Given
            dataTypeId = (int)DataType.Boolean;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.IsNull(characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsRegExContainsOpenBracket_ResultingRegexWrapsCharacterSetRegexWithOpenBracketRemoved()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a", IsSelected = true });

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsRegExContainsClosingBracket_ResultingRegexWrapsCharacterSetRegexWithClosingBracketRemoved()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "a]", IsSelected = true });

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsRegExContainsAsterisk_ResultingRegexWrapsCharacterSetRegexWithAsteriskRemoved()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "a*", IsSelected = true });

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsRegExContainsPlusSign_ResultingRegexWrapsCharacterSetRegexWithPlusSignRemoved()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "a+", IsSelected = true });

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsRegExContainsDollarSign_ResultingRegexWrapsCharacterSetRegexWithDollarSignRemoved()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "a$", IsSelected = true });

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsRegExContainsCaretSign_ResultingRegexWrapsCharacterSetRegexWithCaretSignRemoved()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "^a", IsSelected = true });

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_MultipleCharacterSetsArePassedAndSpecialCharactersSelectedIsPassed_BuildsRegexPatternFromCharacterSets()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = @"!\{]";

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a-zA-Z0-9\s!\\\{\]]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_SpecialCharactersIsAllAndAllCharacterSetsAreSelected_ReturnsSpecialCharactersAllConstant()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = Constants.SpecialCharactersAll;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(Constants.SpecialCharactersAllRegexPattern, characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_SpecialCharactersIsNull_OnlyCharacterSetsArePresentInRegexPattern()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = null;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a-zA-Z0-9\s]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_SpecialCharactersIsEmpty_OnlyCharacterSetsArePresentInRegexPattern()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = string.Empty;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a-zA-Z0-9\s]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_SpecialCharactersIsWhitespace_OnlyCharacterSetsArePresentInRegexPattern()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = "    ";

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a-zA-Z0-9\s]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsHasNoSelectedAndSpecialCharactersIsNull_ShouldReturnNull()
        {
            //Given
            characterSetModels = new List<CharacterSetModel> { new CharacterSetModel { RegEx = "a" } };
            specialCharacters = null;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.IsNull(characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsHasNotSelectedCharacterSetsAndSpecialCharactersAreSet_OnlySelectedCharacterSetAndSpecialCharacterssAreInRegexPattern()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = false });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = false });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = "@!";

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual("^[a-z\\s@!]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsHasNotSelectedCharacterSetsAndSpecialCharactersAreNotSet_OnlySelectedCharacterSetsAreInRegexPattern()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = false });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = false });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = null;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual("^[a-z\\s]*$", characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsHasNotSelectedCharacterSetsAndSpecialCharactersIsAll_NotSelectedCharacterSetsAreInANegatedCharacterSetAndSelectedCharacterSetsAreInACharacterSet()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = false });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = false });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = Constants.SpecialCharactersAll;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual("^([a-z\\s]|[^A-Z0-9])*$", characterSetRegex);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateCharacterSetRegexPattern_CharacterSetsIsNullAndSpecialCharactersIsAll_ThrowsExceptionBecauseUnableToCreateRegexPattern()
        {
            //Given
            characterSetModels = null;
            specialCharacters = Constants.SpecialCharactersAll;

            //When
            attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_CharacterSetsIsNullAndSpecialCharactersIsNull_ShouldReturnNull()
        {
            //Given
            characterSetModels = null;
            specialCharacters = null;

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.IsNull(characterSetRegex);
        }

        [TestMethod]
        public void CreateCharacterSetRegexPattern_SpecialCharactersContainsHyphen_ShouldReturnRegexThatHasHyphenEscapted()
        {
            //Given
            characterSetModels.Add(new CharacterSetModel { RegEx = "[a-z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[A-Z]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = "[0-9]*", IsSelected = true });
            characterSetModels.Add(new CharacterSetModel { RegEx = @"\s*", IsSelected = true });
            specialCharacters = "!,#,$,%,&,',(,),*,,,-,.,/,:,;,<,=,>,?,@";

            //When
            var characterSetRegex = attributesHelper.CreateCharacterSetRegexPattern(dataTypeId, characterSetModels, specialCharacters);

            //Then
            Assert.AreEqual(@"^[a-zA-Z0-9\s!,#,\$,%,&,',\(,\),\*,,,\-,\.,/,:,;,<,=,>,\?,@]*$", characterSetRegex);
        }
    }
}
