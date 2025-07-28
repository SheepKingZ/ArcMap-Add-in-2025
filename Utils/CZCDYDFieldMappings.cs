using System;
using System.Collections.Generic;
using System.Linq;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// CZCDYDQC字段映射配置工具类
    /// 定义从源数据（SLZYZC_DLTB、CYZYZC_DLTB、SDZYZC_DLTB）到目标CZCDYDQC的字段映射
    /// </summary>
    public static class CZCDYDFieldMappings
    {
        /// <summary>
        /// 字段映射信息
        /// </summary>
        public class FieldMapping
        {
            /// <summary>
            /// 目标字段名
            /// </summary>
            public string TargetField { get; set; }

            /// <summary>
            /// 源字段名
            /// </summary>
            public string SourceField { get; set; }

            /// <summary>
            /// 是否为特殊计算字段
            /// </summary>
            public bool IsSpecialCalculation { get; set; }

            /// <summary>
            /// 特殊计算类型
            /// </summary>
            public string CalculationType { get; set; }

            /// <summary>
            /// 字段描述
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
        /// 获取标准的字段映射配置
        /// 基于用户提供的字段映射要求
        /// </summary>
        /// <returns>字段映射配置列表</returns>
        public static List<FieldMapping> GetStandardFieldMappings()
        {
            return new List<FieldMapping>
            {
                // 标准字段映射
                new FieldMapping("YZCQCBSM", "ZCQCBSM", false, null, "源调查区编号"),
                new FieldMapping("YSDM", "YSDM", false, null, "要素代码"),
                new FieldMapping("XZQDM", "XZQDM", false, null, "行政区代码"),
                new FieldMapping("XZQMC", "XZQMC", false, null, "行政区名称"),
                new FieldMapping("GTDCTBBSM", "GTDCTBBSM", false, null, "国土调查图斑编号"),
                new FieldMapping("DLBM", "GTDCDLBM", false, null, "地类编码"),
                new FieldMapping("DLMC", "GTDCDLMC", false, null, "地类名称"),
                new FieldMapping("TBDLMJ", "GTDCTBMJ", false, null, "图斑地类面积"),
                new FieldMapping("HSJG", "HSJG", false, null, "核实价格"),
                new FieldMapping("TBJJJZ", "JJJZ", false, null, "图斑经济价值"),

                // 特殊计算字段
                new FieldMapping("ZCQCBSM", "", true, "COUNTY_CODE_GENERATION", "资源调查区编号 = 县代码+9110+12位FID"),
                new FieldMapping("HRCZCMJ", "", true, "AREA_RATIO_CALCULATION", "核实城镇村面积 = GTDCTBMJ * area2 / area1"),
                new FieldMapping("HRCZCTKMJ", "0", true, "FIXED_VALUE", "核实城镇村退垦面积 = 0"),
                new FieldMapping("HRCZCJJJZ", "", true, "VALUE_RATIO_CALCULATION", "核实城镇村经济价值 = JJJZ * area2 / area1"),
                new FieldMapping("TKJJJJZ", "", true, "PRICE_CALCULATION", "退垦经济价值 = HRCZCMJ * TKJHSJG"),
                new FieldMapping("TKJHSJG", "", true, "COUNTY_PRICE_LOOKUP", "退垦价格核实价格 = 根据县代码查询最低价")
            };
        }

        /// <summary>
        /// 获取CZCDYDQC输出字段定义
        /// 定义最终输出shapefile的完整字段结构
        /// </summary>
        /// <returns>输出字段定义列表</returns>
        public static List<OutputFieldDefinition> GetOutputFieldDefinitions()
        {
            return new List<OutputFieldDefinition>
            {
                new OutputFieldDefinition("ZCQCBSM", "字符型", 20, 0, "资源调查区编号"),
                new OutputFieldDefinition("YZCQCBSM", "字符型", 20, 0, "源调查区编号"),
                new OutputFieldDefinition("YSDM", "字符型", 10, 0, "要素代码"),
                new OutputFieldDefinition("XZQDM", "字符型", 6, 0, "行政区代码"),
                new OutputFieldDefinition("XZQMC", "字符型", 60, 0, "行政区名称"),
                new OutputFieldDefinition("GTDCTBBSM", "字符型", 20, 0, "国土调查图斑编号"),
                new OutputFieldDefinition("DLBM", "字符型", 5, 0, "地类编码"),
                new OutputFieldDefinition("DLMC", "字符型", 60, 0, "地类名称"),
                new OutputFieldDefinition("TBDLMJ", "双精度", 18, 2, "图斑地类面积"),
                new OutputFieldDefinition("HSJG", "双精度", 12, 2, "核实价格"),
                new OutputFieldDefinition("TBJJJZ", "双精度", 18, 2, "图斑经济价值"),
                new OutputFieldDefinition("HRCZCMJ", "双精度", 18, 2, "核实城镇村面积"),
                new OutputFieldDefinition("HRCZCTKMJ", "双精度", 18, 2, "核实城镇村退垦面积"),
                new OutputFieldDefinition("HRCZCJJJZ", "双精度", 18, 2, "核实城镇村经济价值"),
                new OutputFieldDefinition("TKJJJJZ", "双精度", 18, 2, "退垦经济价值"),
                new OutputFieldDefinition("TKJHSJG", "双精度", 12, 2, "退垦价格核实价格")
            };
        }

        /// <summary>
        /// 输出字段定义
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
            /// 获取ArcGIS字段类型
            /// </summary>
            /// <returns>对应的ArcGIS字段类型</returns>
            public ESRI.ArcGIS.Geodatabase.esriFieldType GetArcGISFieldType()
            {
                switch (DataType.ToLower())
                {
                    case "字符型":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                    case "整型":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger;
                    case "双精度":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble;
                    case "浮点型":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSingle;
                    default:
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                }
            }
        }

        /// <summary>
        /// 获取字段映射字典（目标字段名 -> 源字段名）
        /// </summary>
        /// <returns>字段映射字典</returns>
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
        /// 获取特殊计算字段列表
        /// </summary>
        /// <returns>特殊计算字段映射列表</returns>
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
        /// 验证字段映射的完整性
        /// </summary>
        /// <param name="sourceFields">源字段列表</param>
        /// <returns>验证结果</returns>
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
        /// 字段映射验证结果
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
        /// 验证县代码的有效性
        /// </summary>
        /// <param name="countyCode">县代码</param>
        /// <returns>是否有效</returns>
        public static bool IsValidCountyCode(string countyCode)
        {
            if (string.IsNullOrEmpty(countyCode))
                return false;
                
            return countyCode.Length == 6 && countyCode.All(char.IsDigit);
        }

        /// <summary>
        /// 生成资源调查区编号
        /// </summary>
        /// <param name="countyCode">县代码</param>
        /// <param name="fid">要素ID</param>
        /// <returns>资源调查区编号</returns>
        public static string GenerateZCQCBSM(string countyCode, int fid)
        {
            if (!IsValidCountyCode(countyCode))
            {
                throw new ArgumentException("无效的县代码", nameof(countyCode));
            }
            
            return $"{countyCode}9110{fid:D12}";
        }

        /// <summary>
        /// 计算面积比例，确保不超过1
        /// </summary>
        /// <param name="baseArea">基础面积</param>
        /// <param name="numeratorArea">分子面积</param>
        /// <param name="denominatorArea">分母面积</param>
        /// <returns>计算结果</returns>
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
        /// 计算退垦经济价值
        /// </summary>
        /// <param name="hrczcmj">核实城镇村面积</param>
        /// <param name="tkjhsjg">退垦价格核实价格</param>
        /// <returns>退垦经济价值</returns>
        public static double CalculateTKJJJZ(double hrczcmj, double tkjhsjg)
        {
            return hrczcmj * tkjhsjg;
        }

        /// <summary>
        /// 获取字段映射验证规则
        /// </summary>
        /// <returns>验证规则字典</returns>
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
        /// 验证要素属性的完整性
        /// </summary>
        /// <param name="featureAttributes">要素属性字典</param>
        /// <returns>验证结果</returns>
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
                        result.ValidationErrors.Add($"字段 {fieldName} 值无效: {featureAttributes[fieldName]}");
                    }
                }
                else
                {
                    result.MissingFields.Add(fieldName);
                    result.ValidationErrors.Add($"缺少必需字段: {fieldName}");
                }
            }
            
            return result;
        }

        /// <summary>
        /// 字段验证结果
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