using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// Shapefile字段模板类
    /// 提供标准的字段定义和映射功能
    /// </summary>
    public class ShapefileFieldTemplate
    {
        /// <summary>
        /// 字段定义类
        /// </summary>
        public class FieldDefinition
        {
            /// <summary>
            /// 字段名称
            /// </summary>
            public string FieldName { get; set; }

            /// <summary>
            /// 中文名称
            /// </summary>
            public string ChineseName { get; set; }

            /// <summary>
            /// 数据类型
            /// </summary>
            public string DataType { get; set; }

            /// <summary>
            /// 字段长度
            /// </summary>
            public int Length { get; set; }

            /// <summary>
            /// 小数位数
            /// </summary>
            public int DecimalPlaces { get; set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="fieldName">字段名称</param>
            /// <param name="chineseName">中文名称</param>
            /// <param name="dataType">数据类型</param>
            /// <param name="length">字段长度</param>
            /// <param name="decimalPlaces">小数位数</param>
            public FieldDefinition(string fieldName, string chineseName, string dataType, int length, int decimalPlaces = 0)
            {
                FieldName = fieldName;
                ChineseName = chineseName;
                DataType = dataType;
                Length = length;
                DecimalPlaces = decimalPlaces;
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
                    case "字符串":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                    case "整型":
                        if (Length <= 2)
                            return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger;
                        else
                            return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger;
                    case "浮点型":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSingle;
                    case "双精度":
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble;
                    default:
                        return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                }
            }

            /// <summary>
            /// 判断是否为数值类型
            /// </summary>
            /// <returns>是否为数值类型</returns>
            public bool IsNumericType()
            {
                return DataType.ToLower() == "整型" || DataType.ToLower() == "浮点型" || DataType.ToLower() == "双精度";
            }

            /// <summary>
            /// 获取Shapefile兼容的字段长度
            /// </summary>
            /// <returns>Shapefile兼容的字段长度</returns>
            public int GetShapefileCompatibleLength()
            {
                if (DataType.ToLower() == "字符型" || DataType.ToLower() == "字符串")
                {
                    // Shapefile字符串字段最大长度为254
                    return Math.Min(Length, 254);
                }
                return Length;
            }
        }

        /// <summary>
        /// 获取所有字段定义
        /// </summary>
        /// <returns>完整的字段定义列表</returns>
        public static List<FieldDefinition> GetAllFieldDefinitions()
        {
            return new List<FieldDefinition>
            {
                new FieldDefinition("bsm", "标识码", "字符型", 18),
                new FieldDefinition("ysdm", "要素代码", "字符型", 10),
                new FieldDefinition("tbybh", "图斑预编号", "字符型", 8),
                new FieldDefinition("tbbh", "图斑编号", "字符型", 8),
                new FieldDefinition("dlbm", "地类编码", "字符型", 5),
                new FieldDefinition("dlmc", "地类名称", "字符型", 60),
                new FieldDefinition("qsxz", "权属性质", "字符型", 2),
                new FieldDefinition("qsdwdm", "权属单位代码", "字符型", 19),
                new FieldDefinition("qsdwmc", "权属单位名称", "字符型", 60),
                new FieldDefinition("zldwdm", "坐落单位代码", "字符型", 19),
                new FieldDefinition("zldwmc", "坐落单位名称", "字符型", 60),
                new FieldDefinition("tbmj", "图斑面积", "双精度", 18, 2),
                new FieldDefinition("kcdlbm", "扣除地类编码", "字符型", 5),
                new FieldDefinition("kcxs", "扣除地类系数", "浮点型", 6, 4),
                new FieldDefinition("kcmj", "扣除地类面积", "双精度", 18, 2),
                new FieldDefinition("tbdlmj", "图斑地类面积", "双精度", 18, 2),
                new FieldDefinition("gdlx", "耕地类型", "字符型", 2),
                new FieldDefinition("gdpdjb", "耕地坡度级别", "字符型", 2),
                new FieldDefinition("xxtbkd", "线性图斑宽度", "浮点型", 5, 1),
                new FieldDefinition("tbxhdm", "图斑细化代码", "字符型", 4),
                new FieldDefinition("tbxhmc", "图斑细化名称", "字符型", 20),
                new FieldDefinition("zzsxdm", "种植属性代码", "字符型", 6),
                new FieldDefinition("zzsxmc", "种植属性名称", "字符型", 10),
                new FieldDefinition("gddb", "耕地等别", "整型", 2),
                new FieldDefinition("frdbs", "飞入地标识", "字符型", 1),
                new FieldDefinition("czcsxm", "城镇村属性码", "字符型", 4),
                new FieldDefinition("sjnf", "数据年份", "整型", 4),
                new FieldDefinition("bz", "国土备注", "字符型", 254),
                new FieldDefinition("sheng", "省（区、市）", "字符型", 2),
                new FieldDefinition("xian", "县（市、旗）", "字符型", 6),
                new FieldDefinition("xiang", "乡", "字符型", 9),
                new FieldDefinition("cun", "村", "字符型", 12),
                new FieldDefinition("lin_ye_ju", "林业局（牧场）", "字符型", 6),
                new FieldDefinition("lin_chang", "林场(分场)", "字符型", 9),
                new FieldDefinition("lin_ban", "林(草)班", "字符型", 4),
                new FieldDefinition("xiao_ban", "小班号", "字符型", 5),
                new FieldDefinition("ly", "流域", "字符型", 3),
                new FieldDefinition("stqw", "生态区位", "字符型", 3),
                new FieldDefinition("stqwmc", "生态区位名称", "字符型", 60),
                new FieldDefinition("di_mao", "地貌", "字符型", 1),
                new FieldDefinition("hai_ba", "海拔", "整型", 5),
                new FieldDefinition("po_xiang", "坡向", "字符型", 1),
                new FieldDefinition("po_wei", "坡位", "字符型", 1),
                new FieldDefinition("po_du", "坡度", "整型", 2),
                new FieldDefinition("tu_rang_lx", "土壤类型", "字符型", 3),
                new FieldDefinition("tu_ceng_hd", "土层厚度", "整型", 3),
                new FieldDefinition("tu_rang_zd", "土壤质地", "字符型", 1),
                new FieldDefinition("ld_qs", "土地所有权属", "字符型", 2),
                new FieldDefinition("tdsyqs", "土地使用权属", "字符型", 2),
                new FieldDefinition("xbmj", "小班面积", "双精度", 18, 2),
                new FieldDefinition("di_lei", "地类", "字符型", 6),
                new FieldDefinition("zbfglx", "植被覆盖类型", "字符型", 5),
                new FieldDefinition("zb_jg", "植被结构", "字符型", 1),
                new FieldDefinition("zbgd", "植被总盖度", "整型", 3),
                new FieldDefinition("yu_bi_du", "郁闭度", "双精度", 6, 2),
                new FieldDefinition("gmgd", "灌木盖度", "整型", 3),
                new FieldDefinition("cbgd", "草本盖度", "整型", 3),
                new FieldDefinition("you_shi_sz", "优势树（灌）种", "字符型", 6),
                new FieldDefinition("qi_yuan", "起源", "字符型", 2),
                new FieldDefinition("pingjun_nl", "平均年龄", "整型", 4),
                new FieldDefinition("ling_zu", "龄组", "字符型", 1),
                new FieldDefinition("pingjun_xj", "平均胸径", "浮点型", 6, 1),
                new FieldDefinition("pingjun_sg", "平均树高", "浮点型", 6, 1),
                new FieldDefinition("mei_gq_zs", "每公顷株数", "整型", 6),
                new FieldDefinition("mei_gq_xj", "每公顷蓄积", "双精度", 12, 2),
                new FieldDefinition("huo_lm_xj", "蓄积量", "整型", 12),
                new FieldDefinition("sheng_wu_l", "生物量", "整型", 12),
                new FieldDefinition("tan_chu_l", "碳储量", "整型", 12),
                new FieldDefinition("lmqs", "林木所有权属", "字符型", 2),
                new FieldDefinition("lmsyqs", "林木使用权属", "字符型", 2),
                new FieldDefinition("lin_zhong", "林种", "字符型", 3),
                new FieldDefinition("shuzhong_zc", "树种组成", "字符型", 16),
                new FieldDefinition("shi_quan_d", "事权等级", "字符型", 2),
                new FieldDefinition("gyl_bhlx", "公益林变化类型", "字符型", 2),
                new FieldDefinition("ldgl_lx", "林地管理类型", "字符型", 2),
                new FieldDefinition("bh_dj", "林地保护等级", "字符型", 1),
                new FieldDefinition("zl_dj", "林地质量等级", "字符型", 1),
                new FieldDefinition("cy_fq", "草原分区", "字符型", 2),
                new FieldDefinition("cd_qy", "草原起源", "字符型", 2),
                new FieldDefinition("cd_l", "草原类", "字符型", 2),
                new FieldDefinition("cd_xing", "草原型", "字符型", 3),
                new FieldDefinition("ys_caoz", "优势草种", "字符型", 20),
                new FieldDefinition("cdgn", "功能类别", "字符型", 2),
                new FieldDefinition("xc_cl", "单位面积鲜草产量", "双精度", 8, 1),
                new FieldDefinition("xb_xccl", "小班鲜草产量", "双精度", 8, 1),
                new FieldDefinition("ksmcbl", "可食牧草比例", "双精度", 8, 1),
                new FieldDefinition("xb_gccl", "小班干草产量", "双精度", 8, 1),
                new FieldDefinition("xb_ksgccl", "小班可食干牧草产量", "双精度", 8, 1),
                new FieldDefinition("xb_ksxccl", "小班可食鲜牧草产量", "双精度", 8, 1),
                new FieldDefinition("cdlyfs", "利用方式", "字符型", 2),
                new FieldDefinition("fm_shch", "放牧时长", "整型", 2),
                new FieldDefinition("cylb", "草原类别", "字符型", 1),
                new FieldDefinition("lbmj_bl", "裸斑面积比例", "整型", 2),
                new FieldDefinition("thlx", "草原退化类型", "字符串", 1),
                new FieldDefinition("thcd", "草原退化程度", "字符串", 1),
                new FieldDefinition("jm_ph", "禁牧与草畜平衡", "字符串", 1),
                new FieldDefinition("cdgl_lx", "草原管理类型", "字符型", 2),
                new FieldDefinition("sd_dj", "湿地管理分级", "字符型", 6),
                new FieldDefinition("zysd_bm", "重要湿地名称", "字符型", 50),
                new FieldDefinition("bhdsx", "湿地保护形式", "字符型", 2),
                new FieldDefinition("bhddm", "自然保护地", "字符型", 50),
                new FieldDefinition("sdlyfs", "湿地利用方式", "字符型", 2),
                new FieldDefinition("sdwxzk", "受威胁状况", "字符型", 4),
                new FieldDefinition("sdgllx", "湿地管理类型", "字符型", 2),
                new FieldDefinition("dcqlx", "荒漠调查区类型", "字符型", 1),
                new FieldDefinition("qhlx", "气候类型", "字符型", 1),
                new FieldDefinition("shlx", "沙化类型", "字符型", 3),
                new FieldDefinition("shcd", "沙化程度", "字符型", 1),
                new FieldDefinition("sssmsd", "所属沙漠沙地", "字符型", 2),
                new FieldDefinition("hmhlx", "荒漠化类型", "字符型", 1),
                new FieldDefinition("hmhcd", "荒漠化程度", "字符型", 1),
                new FieldDefinition("trls", "基岩裸露度/土壤砾石含量", "浮点型", 5, 1),
                new FieldDefinition("fshd", "覆沙厚度", "浮点型", 5, 1),
                new FieldDefinition("sqgd", "沙丘高度", "浮点型", 5, 1),
                new FieldDefinition("qsg", "侵蚀沟面积比例", "浮点型", 5, 1),
                new FieldDefinition("yjb", "盐碱斑占地率", "浮点型", 5, 1),
                new FieldDefinition("zwclxjl", "作物产量下降率", "浮点型", 5, 1),
                new FieldDefinition("zwqml", "作物缺苗率", "浮点型", 5, 1),
                new FieldDefinition("zwcl", "作物产量", "浮点型", 5),
                new FieldDefinition("ggnl", "灌溉能力", "字符型", 1),
                new FieldDefinition("ntlwhl", "农田林网化率", "字符型", 1),
                new FieldDefinition("fszk", "风蚀状况", "字符型", 1),
                new FieldDefinition("trbcjg", "土壤表层结构", "字符型", 1),
                new FieldDefinition("kzld", "荒漠化沙化可治理度", "字符型", 1),
                new FieldDefinition("zlcs", "治理措施", "字符型", 3),
                new FieldDefinition("zlcd", "治理程度", "字符型", 1),
                new FieldDefinition("smhzk", "石漠化状况", "字符型", 1),
                new FieldDefinition("smhcd", "石漠化程度", "字符型", 1),
                new FieldDefinition("smhyblx", "石漠化演变类型", "字符型", 1),
                new FieldDefinition("yrdm", "岩溶地貌", "字符型", 1),
                new FieldDefinition("hsbj", "核实标记", "字符型", 2),
                new FieldDefinition("gfyd", "光伏用地标注", "字符型", 1),
                new FieldDefinition("dc_ry", "调查人员", "字符型", 20),
                new FieldDefinition("dc_rq", "调查日期", "字符型", 8)
            };
        }

        /// <summary>
        /// 获取字段定义映射字典（按字段名索引）
        /// </summary>
        /// <returns>字段名到字段定义的映射</returns>
        public static Dictionary<string, FieldDefinition> GetFieldMappingDictionary()
        {
            var fields = GetAllFieldDefinitions();
            return fields.ToDictionary(f => f.FieldName.ToLower(), f => f);
        }

        /// <summary>
        /// 根据字段名获取字段定义
        /// </summary>
        /// <param name="fieldName">字段名称（不区分大小写）</param>
        /// <returns>字段定义，如果不存在返回null</returns>
        public static FieldDefinition GetFieldDefinition(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;

            var fieldMapping = GetFieldMappingDictionary();
            return fieldMapping.TryGetValue(fieldName.ToLower(), out FieldDefinition field) ? field : null;
        }

        /// <summary>
        /// 获取指定字段的字段定义列表
        /// </summary>
        /// <param name="fieldNames">字段名称列表</param>
        /// <returns>字段定义列表</returns>
        public static List<FieldDefinition> GetFieldDefinitions(IEnumerable<string> fieldNames)
        {
            if (fieldNames == null)
                return new List<FieldDefinition>();

            var fieldMapping = GetFieldMappingDictionary();
            var result = new List<FieldDefinition>();

            foreach (string fieldName in fieldNames)
            {
                if (!string.IsNullOrEmpty(fieldName) && 
                    fieldMapping.TryGetValue(fieldName.ToLower(), out FieldDefinition field))
                {
                    result.Add(field);
                }
            }

            return result;
        }

        /// <summary>
        /// 验证字段列表是否都存在于模板中
        /// </summary>
        /// <param name="fieldNames">要验证的字段名称列表</param>
        /// <returns>验证结果，包含成功/失败字段</returns>
        public static FieldValidationResult ValidateFields(IEnumerable<string> fieldNames)
        {
            var result = new FieldValidationResult();
            
            if (fieldNames == null)
                return result;

            var fieldMapping = GetFieldMappingDictionary();

            foreach (string fieldName in fieldNames)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                    result.InvalidFields.Add(fieldName ?? "null");
                    continue;
                }

                if (fieldMapping.ContainsKey(fieldName.ToLower()))
                {
                    result.ValidFields.Add(fieldName);
                }
                else
                {
                    result.InvalidFields.Add(fieldName);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有字符型字段
        /// </summary>
        /// <returns>字符型字段列表</returns>
        public static List<FieldDefinition> GetStringFields()
        {
            return GetAllFieldDefinitions()
                .Where(f => f.DataType.ToLower() == "字符型" || f.DataType.ToLower() == "字符串")
                .ToList();
        }

        /// <summary>
        /// 获取所有数值型字段
        /// </summary>
        /// <returns>数值型字段列表</returns>
        public static List<FieldDefinition> GetNumericFields()
        {
            return GetAllFieldDefinitions()
                .Where(f => f.IsNumericType())
                .ToList();
        }

        /// <summary>
        /// 根据中文名查找字段定义
        /// </summary>
        /// <param name="chineseName">中文名称</param>
        /// <returns>匹配的字段定义</returns>
        public static FieldDefinition GetFieldByChineseName(string chineseName)
        {
            if (string.IsNullOrEmpty(chineseName))
                return null;

            return GetAllFieldDefinitions()
                .FirstOrDefault(f => f.ChineseName.Equals(chineseName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取适用于Shapefile导出的字段映射
        /// </summary>
        /// <param name="sourceFieldNames">源字段名称列表</param>
        /// <returns>Shapefile兼容的字段映射（源字段名 -> 目标字段定义）</returns>
        public static Dictionary<string, FieldDefinition> GetShapefileExportMapping(IEnumerable<string> sourceFieldNames)
        {
            var result = new Dictionary<string, FieldDefinition>();
            var fieldMapping = GetFieldMappingDictionary();

            foreach (string fieldName in sourceFieldNames ?? new List<string>())
            {
                if (!string.IsNullOrEmpty(fieldName) && 
                    fieldMapping.TryGetValue(fieldName.ToLower(), out FieldDefinition fieldDef))
                {
                    // 创建Shapefile兼容的字段定义副本
                    var shapefileFieldDef = new FieldDefinition(
                        fieldDef.FieldName,
                        fieldDef.ChineseName,
                        fieldDef.DataType,
                        fieldDef.GetShapefileCompatibleLength(),
                        fieldDef.DecimalPlaces
                    );
                    
                    result[fieldName] = shapefileFieldDef;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取字段统计信息
        /// </summary>
        /// <returns>字段统计信息</returns>
        public static FieldStatistics GetFieldStatistics()
        {
            var allFields = GetAllFieldDefinitions();
            
            return new FieldStatistics
            {
                TotalFields = allFields.Count,
                StringFields = allFields.Count(f => f.DataType.ToLower() == "字符型" || f.DataType.ToLower() == "字符串"),
                IntegerFields = allFields.Count(f => f.DataType.ToLower() == "整型"),
                FloatFields = allFields.Count(f => f.DataType.ToLower() == "浮点型"),
                DoubleFields = allFields.Count(f => f.DataType.ToLower() == "双精度"),
                MaxStringLength = allFields.Where(f => f.DataType.ToLower() == "字符型" || f.DataType.ToLower() == "字符串")
                                          .Max(f => f.Length),
                FieldsByType = allFields.GroupBy(f => f.DataType).ToDictionary(g => g.Key, g => g.Count())
            };
        }
    }

    /// <summary>
    /// 字段验证结果
    /// </summary>
    public class FieldValidationResult
    {
        /// <summary>
        /// 有效字段列表
        /// </summary>
        public List<string> ValidFields { get; set; } = new List<string>();

        /// <summary>
        /// 无效字段列表
        /// </summary>
        public List<string> InvalidFields { get; set; } = new List<string>();

        /// <summary>
        /// 是否所有字段都有效
        /// </summary>
        public bool IsAllValid => InvalidFields.Count == 0;

        /// <summary>
        /// 验证成功率
        /// </summary>
        public double SuccessRate
        {
            get
            {
                int total = ValidFields.Count + InvalidFields.Count;
                return total == 0 ? 0 : (double)ValidFields.Count / total;
            }
        }
    }

    /// <summary>
    /// 字段统计信息
    /// </summary>
    public class FieldStatistics
    {
        /// <summary>
        /// 总字段数
        /// </summary>
        public int TotalFields { get; set; }

        /// <summary>
        /// 字符型字段数
        /// </summary>
        public int StringFields { get; set; }

        /// <summary>
        /// 整型字段数
        /// </summary>
        public int IntegerFields { get; set; }

        /// <summary>
        /// 浮点型字段数
        /// </summary>
        public int FloatFields { get; set; }

        /// <summary>
        /// 双精度字段数
        /// </summary>
        public int DoubleFields { get; set; }

        /// <summary>
        /// 字符型字段最大长度
        /// </summary>
        public int MaxStringLength { get; set; }

        /// <summary>
        /// 按类型分组的字段数量
        /// </summary>
        public Dictionary<string, int> FieldsByType { get; set; } = new Dictionary<string, int>();
    }
}
