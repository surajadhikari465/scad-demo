using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Icon.Common;
using Icon.Common.Models;

namespace Icon.Web.Mvc.Utility
{
    /// <summary>
    /// Class with helper methods relating to attributes.
    /// </summary>
    public class AttributesHelper : IAttributesHelper
    {
        /// <summary>
        /// This regex is a list of characters that have special meaning in a regex expression
        /// and need to be escaped. It is meant to be used to replace the character in the 
        /// specialCharacters in CreateCharacterSetRegexPattern.
        /// </summary>
        private readonly Regex escapeCharactersRegex = new Regex(@"[\.\$\^\{\}\[\]\(\|\)\*\+\?\\-]");

        /// <summary>
        /// This regex matches +, $, *, or ^ and is meant to be used to replace those symbols in
        /// the CharacterSets.
        /// </summary>
        private readonly Regex characterSetConstantsRegex = new Regex($"\\{Constants.SquareBracketOpen}|\\{Constants.SquareBracketClosed}|\\{Constants.PlusSign}|\\{Constants.DollarSign}|\\{Constants.AsteriskSign}|\\{Constants.CaretSign}");

        /// <summary>
        /// Returns a Regex pattern given the character sets and special characters to validate text attributes.
        /// </summary>
        /// <param name="characterSets"></param>
        /// <param name="specialCharacters"></param>
        /// <returns></returns>
        public string CreateCharacterSetRegexPattern(int dataTypeId, List<CharacterSetModel> characterSets, string specialCharacters)
        {
            //Only return regex pattern if data type is Text
            if (dataTypeId == (int)DataType.Text)
            {
                if ((characterSets == null || characterSets.All(cs => !cs.IsSelected)) && string.IsNullOrWhiteSpace(specialCharacters))
                {
                    //Return null if no character sets are selected and no special characters are selected
                    return null;
                }
                else if (characterSets == null)
                {
                    if (specialCharacters == Constants.SpecialCharactersAll)
                    {
                        throw new ArgumentException("Unable to create character set regex pattern when special characters is set to All but no character sets are null.", nameof(characterSets));
                    }
                    else
                    {
                        //Return regex pattern matching only selected special characters
                        return $"^[{escapeCharactersRegex.Replace(specialCharacters, (s) => "\\" + s)}]*$";
                    }
                }
                else
                {
                    if (specialCharacters == Constants.SpecialCharactersAll)
                    {
                        if (characterSets.All(cs => cs.IsSelected))
                        {
                            //Return regex pattern that matches everything if all character sets are selected and all special characters are selected
                            return Constants.SpecialCharactersAllRegexPattern;
                        }
                        else if (characterSets.All(cs => !cs.IsSelected))
                        {
                            //Return regex pattern not matching any character sets
                            var notSelectedCharacterSets = characterSets
                                .Select(cs => characterSetConstantsRegex.Replace(cs.RegEx, ""))
                                .Aggregate((s1, s2) => s1 + s2);
                            return $"^[^{notSelectedCharacterSets}]*$";
                        }
                        else
                        {
                            var selectedCharacterSets = characterSets
                                .Where(cs => cs.IsSelected)
                                .Select(cs => characterSetConstantsRegex.Replace(cs.RegEx, ""))
                                .Aggregate((s1, s2) => s1 + s2);
                            var notSelectedCharacterSets = characterSets
                                .Where(cs => !cs.IsSelected)
                                .Select(cs => characterSetConstantsRegex.Replace(cs.RegEx, ""))
                                .Aggregate((s1, s2) => s1 + s2);

                            //Return regex pattern matching selected character sets but not matching none selected character sets.
                            //Since the negated character set is present this will allow any special character
                            return $"^([{selectedCharacterSets}]|[^{notSelectedCharacterSets}])*$";
                        }
                    }
                    else
                    {
                        if (characterSets.All(cs => !cs.IsSelected))
                        {
                            //Return regex pattern matching only selected special characters
                            return $"^[{escapeCharactersRegex.Replace(specialCharacters, (s) => "\\" + s)}]*$";
                        }
                        var selectedCharacterSets = characterSets
                            .Where(cs => cs.IsSelected)
                            .Select(cs => characterSetConstantsRegex.Replace(cs.RegEx, ""))
                            .Aggregate((s1, s2) => s1 + s2);
                        var characterSetRegexPattern = $"^[{selectedCharacterSets}";//]*$";
                        if(string.IsNullOrWhiteSpace(specialCharacters))
                        {
                            //Set regex pattern to only selected character sets if special characters are not selected
                            characterSetRegexPattern = characterSetRegexPattern + "]*$";
                        }
                        else
                        {
                            //Set regex pattern to selected character sets and special characters
                            characterSetRegexPattern = characterSetRegexPattern + escapeCharactersRegex.Replace(specialCharacters, (s) => "\\" + s) + "]*$";
                        }
                        return characterSetRegexPattern;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public static class AttributesColumnNames
        {
            public const string DisplayName = "DisplayName";
            public const string AttributeName = "AttributeName";
            public const string AttributeGroupName = "AttributeGroupName";
            public const string HasUniqueValues = "HasUniqueValues";
            public const string Description = "Description";
            public const string DefaultValue = "DefaultValue";
            public const string IsRequired = "IsRequired";
            public const string SpecialCharactersAllowed = "SpecialCharactersAllowed";
            public const string TraitCode = "TraitCode";
            public const string DataTypeName = "DataTypeName";
            public const string DisplayOrder = "DisplayOrder";
            public const string InitialValue = "InitialValue";
            public const string IncrementBy = "IncrementBy";
            public const string InitialMax = "InitialMax";
            public const string DisplayType = "DisplayType";
            public const string MaxLengthAllowed = "MaxLengthAllowed";
            public const string MinimumNumber = "MinimumNumber";
            public const string MaximumNumber = "MaximumNumber";
            public const string NumberOfDecimals = "NumberOfDecimals";
            public const string IsPickList = "IsPickList";
            public const string XmlTraitDescription = "XmlTraitDescription";
            public const string PickListData = "PickListData";
            public const string CharacterSet = "CharacterSet";
            public const string GlobalItem = "Global Item";
            public const string ItemCount = "ItemCount";
        }
    }
}