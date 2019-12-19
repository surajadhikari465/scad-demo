using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using DataAccess.Repository;
using DataAccess.UnitOfWork;
using System.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ConvertData
{

    public enum DataTypeEnum
    {
        Text = 1,
        Number = 2,
        Boolean = 3
    }

    public class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<IconRebootData> iconRebootData = getdata();
            createCharacterSets(iconRebootData);
            processData(iconRebootData);

        }

        private static void createCharacterSets(IEnumerable<IconRebootData> iconRebootDataList)
        {
            IUnitOfWork unitOfWork = new UnitOfWork(new iCONDevEntities());
            IRepository<CharacterSet> characterSetRepository = new Repository<CharacterSet>(unitOfWork);
            List<string> characterSets = new List<string>(); ;
            foreach (IconRebootData IconRebootData in iconRebootDataList)
            {
                if (IconRebootData.CHARACTER_SETS.Length > 2)
                {
                    var characterSetList = IconRebootData.CHARACTER_SETS.Replace("[", "");
                    characterSetList = characterSetList.Replace("]", "");
                    string[] characterSetalues = characterSetList.Split(new char[] { ',' });


                    foreach (string pickListValue in characterSetalues)
                    {
                        var currentValue = pickListValue;
                        currentValue = currentValue.Substring(1, currentValue.Length - 2);

                        if (!characterSets.Contains(currentValue) && currentValue != "SPECIAL")
                        {
                            characterSets.Add(currentValue);
                        }
                    }

                }


            }

            foreach (string characterSetNameValue in characterSets)
            {
                CharacterSet characterSet = new CharacterSet();
                characterSet.Name = characterSetNameValue;
                characterSet.RegEx = setRegexForCharacterSet(characterSetNameValue);
                characterSetRepository.Add(characterSet);
            }
            characterSetRepository.UnitOfWork.Commit();
        }

        private static string setRegexForCharacterSet(string characterSetNameValue)
        {
            string regex = "";
            switch (characterSetNameValue)
            {
                case "UPPERCASE":
                    regex = "[A-Z]+";
                    break;
                case "LOWERCASE":
                    regex = "[a-z]+";
                    break;
                case "NUMERIC":
                    regex = "^[0-9]*$";
                    break;
                case "WHITESPACE":
                    regex = "\\s";
                    break;
                case "Default":
                    regex = "";
                    break;
            }
            return regex;
        }

        private static int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }

        private static void processData(IEnumerable<IconRebootData> iconRebootDataList)
        {
            IUnitOfWork unitOfWork = new UnitOfWork(new iCONDevEntities());
            IRepository<Attribute> attributeRepository = new Repository<Attribute>(unitOfWork);
            IRepository<AttributeGroup> attributeTypeRepository = new Repository<AttributeGroup>(unitOfWork);
            IRepository<PickListData> pickListRepository = new Repository<PickListData>(unitOfWork);
            IRepository<CharacterSet> characterSetRepository = new Repository<CharacterSet>(unitOfWork);
            foreach (IconRebootData IconRebootData in iconRebootDataList)
            {
                Attribute attribute = new Attribute();
                attribute.DisplayName = IconRebootData.NAME;
                attribute.AttributeName = Regex.Replace(IconRebootData.NAME, @"\s+", "") ;

                attribute.AttributeGroupId = attributeTypeRepository.GetAll().Where(s => s.AttributeGroupName == IconRebootData.ATTRIBUTE_TYPE).Select(s => s.AttributeGroupId).FirstOrDefault();

                attribute.Description = IconRebootData.DESCRIPTION;

                attribute.DisplayOrder = ToNullableInt(IconRebootData.DISPLAY_ORDER);
                attribute.InitialValue = ToNullableInt(IconRebootData.INITIAL_VALUE);
                attribute.IncrementBy = ToNullableInt(IconRebootData.INCREMENT_BY);
                attribute.InitialMax = ToNullableInt(IconRebootData.INITIAL_MAX);
                attribute.DisplayType = IconRebootData.DISPLAY_TYPE;
                attribute.TraitCode = IconRebootData.EXTERNAL_SYSTEM_ID;

                if (IconRebootData.SPECIAL_CHARACTERS.Length > 2)
                {
                    var specialCharacters = IconRebootData.SPECIAL_CHARACTERS.Replace("[", "");
                    specialCharacters = specialCharacters.Replace("]", "");
                    attribute.SpecialCharactersAllowed = specialCharacters;
                }


                if (IconRebootData.CHARACTER_SETS.Length > 2)
                {
                    var characterSets = IconRebootData.CHARACTER_SETS.Replace("[", "");
                    characterSets = characterSets.Replace("]", "");
                    string[] characterSetalues = characterSets.Split(new char[] { ',' });


                    foreach (string pickListValue in characterSetalues)
                    {
                        var currentValue = pickListValue;
                        currentValue = currentValue.Substring(1, currentValue.Length - 2);
                        if (currentValue != "SPECIAL")
                        {
                            var characterSetId = characterSetRepository.GetAll().Where(c => c.Name == currentValue).Select(s => s.CharacterSetId).FirstOrDefault();
                            attribute.AttributeCharacterSets.Add(new AttributeCharacterSet
                            {
                                CharacterSetId = characterSetId
                            });
                        }
                    }
                }

                if (IconRebootData.DEFAULT_VALUE.Length > 2)
                {
                    var defaultValue = IconRebootData.DEFAULT_VALUE.Replace("[", "");
                    defaultValue = defaultValue.Replace("]", "");
                    if(defaultValue.Contains("\""))
                    attribute.DefaultValue = defaultValue.Substring(1, defaultValue.Length - 2);
                    else
                    {
                        attribute.DefaultValue = defaultValue;
                    }
                }

                attribute.RequiredForPublishing = IconRebootData.REQUIRED_FOR_VALIDATION;
                if (IconRebootData.RULE_TYPE.Contains("UniqueValue"))
                {
                    attribute.HasUniqueValues = true;
                }
                else
                {
                    attribute.HasUniqueValues = false;
                }

                if (IconRebootData.PICKLIST_VALUES.Length > 2)
                {
                    attribute.PickList = true;
                }
                else
                {
                    attribute.PickList = false;
                }
                DataTypeEnum datatypePassed;

                if (Enum.TryParse(IconRebootData.DATA_TYPE, true, out datatypePassed))
                {
                    attribute.DataTypeId = (int)datatypePassed;
                }

                if (IconRebootData.PICKLIST_VALUES.Length > 2)
                {
                    var pickListValues = IconRebootData.PICKLIST_VALUES.Replace("[", "");
                    pickListValues = pickListValues.Replace("]", "");
                    string[] pickList = pickListValues.Split(new char[] { ',' });
                    foreach (string pickListValue in pickList)
                    {
                        var currentValue = pickListValue;
                        currentValue = currentValue.Substring(1, currentValue.Length - 2);
                        attribute.PickListDatas.Add(new PickListData
                        {
                            PickListValue = currentValue
                        });

                    }

                }

                if (IconRebootData.RULE_TYPE.Length > 2)
                {
                    var ruleValueList = IconRebootData.RULE_VALUE.Replace("[", "");
                    ruleValueList = ruleValueList.Replace("]", "");
                    string[] ruleValues = ruleValueList.Split(new char[] { ',' });

                    NumberValueModel numberValueModel = new NumberValueModel();
                    int count = 0;
                    if (IconRebootData.RULE_TYPE.Contains("MaxLength"))
                    {
                        attribute.MaxLengthAllowed = Convert.ToInt32(ruleValues[count]);
                        count = count + 1;
                    }
                    if (IconRebootData.RULE_TYPE.Contains("MinValue"))
                    {
                        numberValueModel.MinValue = ruleValues[count];
                        count = count + 1;
                    }

                    if (IconRebootData.RULE_TYPE.Contains("MaxValue"))
                    {
                        numberValueModel.MaxValue = ruleValues[count];
                        count = count + 1;
                    }

                    if (IconRebootData.RULE_TYPE.Contains("NumberOfDecimals"))
                    {
                        numberValueModel.NumberOfDecimals = Convert.ToInt32(ruleValues[count]);
                        count = count + 1;
                    }
                    if (numberValueModel.MinValue != null || numberValueModel.MaxValue != null || numberValueModel.NumberOfDecimals != null)
                        attribute.NumberValidationRule = Newtonsoft.Json.JsonConvert.SerializeObject(numberValueModel,
                                Newtonsoft.Json.Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore
                                });
                }
                attributeRepository.Add(attribute);
                attributeRepository.UnitOfWork.Commit();
            }
        }

        public static IEnumerable<IconRebootData> getdata()
        {

            var sql = @"
                       SELECT [NAME]
                          ,[INFOR_SYSTEM_ID]
                          ,[EXTERNAL_SYSTEM_ID]
                          ,[ATTRIBUTE_TYPE]
                          ,[DATA_TYPE]
                          ,[DESCRIPTION]
                          ,[DEFAULT_VALUE]
                          ,[RULE_TYPE]
                          ,[RULE_VALUE]
                          ,[PICKLIST_VALUES]
                          ,[CHARACTER_SETS]
                          ,[SPECIAL_CHARACTERS]
                          ,[REQUIRED_FOR_VALIDATION]
                          ,[DISPLAY_ORDER]
                          ,[CREATED_BY]
                          ,[CREATED_DATE]
                          ,[MODIFIED_BY]
                          ,[MODIFIED_ON]
                          ,[INITIAL_VALUE]
                          ,[INCREMENT_BY]
                          ,[INITIAL_MAX]
                          ,[SELECT_LOWEST_VALUE]
                          ,[DISPLAY_TYPE]
                      FROM [dbo].[IconRebootData]";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ToString()))
            {
                conn.Open();
                return conn.Query<IconRebootData>(sql);
            }

        }
    }
}
