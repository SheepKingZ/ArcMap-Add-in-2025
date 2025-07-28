using System;
using System.Collections.Generic;
using System.Linq;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// CZCDYDQC�ֶ�ӳ�����ù�����
    /// �����Դ���ݣ�SLZYZC_DLTB��CYZYZC_DLTB��SDZYZC_DLTB����Ŀ��CZCDYDQC���ֶ�ӳ��
    /// </summary>
    public static class CZCDYDFieldMappings
    {
        /// <summary>
        /// �ֶ�ӳ����Ϣ
        /// </summary>
        public class FieldMapping
        {
            /// <summary>
            /// Ŀ���ֶ���
            /// </summary>
            public string TargetField { get; set; }

            /// <summary>
            /// Դ�ֶ���
            /// </summary>
            public string SourceField { get; set; }

            /// <summary>
            /// �Ƿ�Ϊ��������ֶ�
            /// </summary>
            public bool IsSpecialCalculation { get; set; }

            /// <summary>
            /// �����������
            /// </summary>
            public string CalculationType { get; set; }

            /// <summary>
            /// �ֶ�����
            /// </summary>
            public string Description { get; set; }

            public FieldMapping(string targetField, string sourceField, bool isSpecialCalculation = false, 
                              string calculationType = null, string description = "")
            {
                TargetField = targetField;
                SourceField = sourceField;
                IsSpecialCalculation = isSpecialCalculation;
                CalculationType = calculationType;
                Description = description;
            }
        }

        /// <summary>
        /// ��ȡ��׼���ֶ�ӳ������
        /// �����û��ṩ���ֶ�ӳ��Ҫ��
        /// </summary>
        /// <returns>�ֶ�ӳ�������б�</returns>
        public static List<FieldMapping> GetStandardFieldMappings()
        {
            return new List<FieldMapping>
            {
                // ��׼�ֶ�ӳ��
                new FieldMapping("YZCQCBSM", "ZCQCBSM", false, null, "Դ���������"),
                new FieldMapping("YSDM", "YSDM", false, null, "Ҫ�ش���"),
                new FieldMapping("XZQDM", "XZQDM", false, null, "����������"),
                new FieldMapping("XZQMC", "XZQMC", false, null, "����������"),
                new FieldMapping("GTDCTBBSM", "GTDCTBBSM", false, null, "��������ͼ�߱��"),
                new FieldMapping("DLBM", "GTDCDLBM", false, null, "�������"),
                new FieldMapping("DLMC", "GTDCDLMC", false, null, "��������"),
                new FieldMapping("TBDLMJ", "GTDCTBMJ", false, null, "ͼ�ߵ������"),
                new FieldMapping("HSJG", "HSJG", false, null, "��ʵ�۸�"),
                new FieldMapping("TBJJJZ", "JJJZ", false, null, "ͼ�߾��ü�ֵ"),

                // ��������ֶ�
                new FieldMapping("ZCQCBSM", "", true, "COUNTY_CODE_GENERATION", "��Դ��������� = �ش���+9110+12λFID"),
                new FieldMapping("HRCZCMJ", "", true, "AREA_RATIO_CALCULATION", "��ʵ�������� = GTDCTBMJ * area2 / area1"),
                new FieldMapping("HRCZCTKMJ", "0", true, "FIXED_VALUE", "��ʵ������˿���� = 0"),
                new FieldMapping("HRCZCJJJZ", "", true, "VALUE_RATIO_CALCULATION", "��ʵ����徭�ü�ֵ = JJJZ * area2 / area1"),
                new FieldMapping("TKJJJJZ", "", true, "PRICE_CALCULATION", "�˿Ѿ��ü�ֵ = HRCZCMJ * TKJHSJG"),
                new FieldMapping("TKJHSJG", "", true, "COUNTY_PRICE_LOOKUP", "�˿Ѽ۸��ʵ�۸� = �����ش����ѯ��ͼ�")
            };
        }

        /// <summary>
        /// ��ȡCZCDYDQC����ֶζ���
        /// �����������shapefile�������ֶνṹ
        /// </summary>
        /// <returns>����ֶζ����б�</returns>
        public static List<OutputFieldDefinition> GetOutputFieldDefinitions()
        {
            return new List<OutputFieldDefinition>
            {
                new OutputFieldDefinition("ZCQCBSM", "�ַ���", 20, 0, "��Դ���������"),
                new OutputFieldDefinition("YZCQCBSM", "�ַ���", 20, 0, "Դ���������"),
                new OutputFieldDefinition("YSDM", "�ַ���", 10, 0, "Ҫ�ش���"),
                new OutputFieldDefinition("XZQDM", "�ַ���", 6, 0, "����������"),
                new OutputFieldDefinition("XZQMC", "�ַ���", 60, 0, "����������"),
                new OutputFieldDefinition("GTDCTBBSM", "�ַ���", 20, 0, "��������ͼ�߱��"),
                new OutputFieldDefinition("DLBM", "�ַ���", 5, 0, "�������"),
                new OutputFieldDefinition("DLMC", "�ַ���", 60, 0, "��������"),
                new OutputFieldDefinition("TBDLMJ", "˫����", 18, 2, "ͼ�ߵ������"),
                new OutputFieldDefinition("HSJG", "˫����", 12, 2, "��ʵ�۸�"),
                new OutputFieldDefinition("TBJJJZ", "˫����", 18, 2, "ͼ�߾��ü�ֵ"),
                new OutputFieldDefinition("HRCZCMJ", "˫����", 18, 2, "��ʵ��������"),
                new OutputFieldDefinition("HRCZCTKMJ", "˫����", 18, 2, "��ʵ������˿����"),
                new OutputFieldDefinition("HRCZCJJJZ", "˫����", 18, 2, "��ʵ����徭�ü�ֵ"),
                new OutputFieldDefinition("TKJJJJZ", "˫����", 18, 2, "�˿Ѿ��ü�ֵ"),
                new OutputFieldDefinition("TKJHSJG", "˫����", 12, 2, "�˿Ѽ۸��ʵ�۸�")
            };
        }

        /// <summary>
        /// ����ֶζ���
        /// </summary>
        public class OutputFieldDefinition
        {
            public string FieldName { get; set; }
            public string DataType { get; set; }
            public int Length { get; set; }
            public int DecimalPlaces { get; set; }
            public string Description { get; set; }

            public OutputFieldDefinition(string fieldName, string dataType, int length, int decimalPlaces, string description)
            {
                FieldName = fieldName;
                DataType = dataType;
                Length = length;
                DecimalPlaces = decimalPlaces;
                Description = description;
            }

            /// <summary>
            /// ��ȡArcGIS�ֶ�����
            /// </summary>
            /// <returns>��Ӧ��ArcGIS�ֶ�����</returns>
            public ESRI.ArcGIS.Geodatabase.esriFieldType GetArcGISFieldType()
            {
                switch (DataType.ToLower())
                {
                    case "�ַ���":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                    case "����":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger;
                    case "˫����":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble;
                    case "������":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSingle;
                    default:
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                }
            }
        }

        /// <summary>
        /// ��ȡ�ֶ�ӳ���ֵ䣨Ŀ���ֶ��� -> Դ�ֶ�����
        /// </summary>
        /// <returns>�ֶ�ӳ���ֵ�</returns>
        public static Dictionary<string, string> GetFieldMappingDictionary()
        {
            var mappings = GetStandardFieldMappings();
            var result = new Dictionary<string, string>();

            foreach (var mapping in mappings)
            {
                if (!mapping.IsSpecialCalculation)
                {
                    result[mapping.TargetField] = mapping.SourceField;
                }
            }

            return result;
        }

        /// <summary>
        /// ��ȡ��������ֶ��б�
        /// </summary>
        /// <returns>��������ֶ�ӳ���б�</returns>
        public static List<FieldMapping> GetSpecialCalculationFields()
        {
            var mappings = GetStandardFieldMappings();
            var result = new List<FieldMapping>();

            foreach (var mapping in mappings)
            {
                if (mapping.IsSpecialCalculation)
                {
                    result.Add(mapping);
                }
            }

            return result;
        }

        /// <summary>
        /// ��֤�ֶ�ӳ���������
        /// </summary>
        /// <param name="sourceFields">Դ�ֶ��б�</param>
        /// <returns>��֤���</returns>
        public static FieldMappingValidationResult ValidateFieldMappings(List<string> sourceFields)
        {
            var result = new FieldMappingValidationResult();
            var mappings = GetStandardFieldMappings();

            foreach (var mapping in mappings)
            {
                if (!mapping.IsSpecialCalculation)
                {
                    if (sourceFields.Contains(mapping.SourceField))
                    {
                        result.ValidMappings.Add(mapping);
                    }
                    else
                    {
                        result.MissingSourceFields.Add(mapping);
                    }
                }
                else
                {
                    result.SpecialCalculationFields.Add(mapping);
                }
            }

            return result;
        }

        /// <summary>
        /// �ֶ�ӳ����֤���
        /// </summary>
        public class FieldMappingValidationResult
        {
            public List<FieldMapping> ValidMappings { get; set; } = new List<FieldMapping>();
            public List<FieldMapping> MissingSourceFields { get; set; } = new List<FieldMapping>();
            public List<FieldMapping> SpecialCalculationFields { get; set; } = new List<FieldMapping>();

            public bool IsValid => MissingSourceFields.Count == 0;
            public int TotalMappings => ValidMappings.Count + MissingSourceFields.Count + SpecialCalculationFields.Count;
        }

        /// <summary>
        /// ��֤�ش������Ч��
        /// </summary>
        /// <param name="countyCode">�ش���</param>
        /// <returns>�Ƿ���Ч</returns>
        public static bool IsValidCountyCode(string countyCode)
        {
            if (string.IsNullOrEmpty(countyCode))
                return false;
                
            return countyCode.Length == 6 && countyCode.All(char.IsDigit);
        }

        /// <summary>
        /// ������Դ���������
        /// </summary>
        /// <param name="countyCode">�ش���</param>
        /// <param name="fid">Ҫ��ID</param>
        /// <returns>��Դ���������</returns>
        public static string GenerateZCQCBSM(string countyCode, int fid)
        {
            if (!IsValidCountyCode(countyCode))
            {
                throw new ArgumentException("��Ч���ش���", nameof(countyCode));
            }
            
            return $"{countyCode}9110{fid:D12}";
        }

        /// <summary>
        /// �������������ȷ��������1
        /// </summary>
        /// <param name="baseArea">�������</param>
        /// <param name="numeratorArea">�������</param>
        /// <param name="denominatorArea">��ĸ���</param>
        /// <returns>������</returns>
        public static double CalculateAreaRatio(double baseArea, double numeratorArea, double denominatorArea)
        {
            if (denominatorArea <= 0)
                return 0.0;
                
            double ratio = numeratorArea / denominatorArea;
            if (ratio > 1.0)
                ratio = 1.0;
                
            return baseArea * ratio;
        }

        /// <summary>
        /// �����˿Ѿ��ü�ֵ
        /// </summary>
        /// <param name="hrczcmj">��ʵ��������</param>
        /// <param name="tkjhsjg">�˿Ѽ۸��ʵ�۸�</param>
        /// <returns>�˿Ѿ��ü�ֵ</returns>
        public static double CalculateTKJJJZ(double hrczcmj, double tkjhsjg)
        {
            return hrczcmj * tkjhsjg;
        }

        /// <summary>
        /// ��ȡ�ֶ�ӳ����֤����
        /// </summary>
        /// <returns>��֤�����ֵ�</returns>
        public static Dictionary<string, Func<object, bool>> GetFieldValidationRules()
        {
            return new Dictionary<string, Func<object, bool>>
            {
                ["ZCQCBSM"] = value => value != null && value.ToString().Length == 20,
                ["XZQDM"] = value => value != null && IsValidCountyCode(value.ToString()),
                ["TBDLMJ"] = value => value != null && double.TryParse(value.ToString(), out double area) && area >= 0,
                ["HRCZCMJ"] = value => value != null && double.TryParse(value.ToString(), out double area) && area >= 0,
                ["HRCZCTKMJ"] = value => value != null && double.TryParse(value.ToString(), out double area) && area >= 0,
                ["HRCZCJJJZ"] = value => value != null && double.TryParse(value.ToString(), out double value1) && value1 >= 0,
                ["TKJJJJZ"] = value => value != null && double.TryParse(value.ToString(), out double value2) && value2 >= 0,
                ["TKJHSJG"] = value => value != null && double.TryParse(value.ToString(), out double price) && price >= 0
            };
        }

        /// <summary>
        /// ��֤Ҫ�����Ե�������
        /// </summary>
        /// <param name="featureAttributes">Ҫ�������ֵ�</param>
        /// <returns>��֤���</returns>
        public static FieldValidationResult ValidateFeatureAttributes(Dictionary<string, object> featureAttributes)
        {
            var result = new FieldValidationResult();
            var rules = GetFieldValidationRules();
            
            foreach (var rule in rules)
            {
                string fieldName = rule.Key;
                var validator = rule.Value;
                
                if (featureAttributes.ContainsKey(fieldName))
                {
                    if (validator(featureAttributes[fieldName]))
                    {
                        result.ValidFields.Add(fieldName);
                    }
                    else
                    {
                        result.InvalidFields.Add(fieldName);
                        result.ValidationErrors.Add($"�ֶ� {fieldName} ֵ��Ч: {featureAttributes[fieldName]}");
                    }
                }
                else
                {
                    result.MissingFields.Add(fieldName);
                    result.ValidationErrors.Add($"ȱ�ٱ����ֶ�: {fieldName}");
                }
            }
            
            return result;
        }

        /// <summary>
        /// �ֶ���֤���
        /// </summary>
        public class FieldValidationResult
        {
            public List<string> ValidFields { get; set; } = new List<string>();
            public List<string> InvalidFields { get; set; } = new List<string>();
            public List<string> MissingFields { get; set; } = new List<string>();
            public List<string> ValidationErrors { get; set; } = new List<string>();
            
            public bool IsValid => InvalidFields.Count == 0 && MissingFields.Count == 0;
            public int TotalFields => ValidFields.Count + InvalidFields.Count + MissingFields.Count;
        }
    }
}