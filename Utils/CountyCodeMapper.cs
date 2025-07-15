using System;
using System.Collections.Generic;
using System.Linq;

namespace ForestResourcePlugin.Utils
{
    /// <summary>
    /// 县代码映射器 - 提供县名到县代码的映射功能
    /// </summary>
    public static class CountyCodeMapper
    {
        /// <summary>
        /// 县名到县代码的映射字典
        /// </summary>
        private static readonly Dictionary<string, string> CountyCodeMap = new Dictionary<string, string>
        {
            // 广州市
            { "荔湾区", "440103" },
            { "越秀区", "440104" },
            { "海珠区", "440105" },
            { "天河区", "440106" },
            { "白云区", "440111" },
            { "黄埔区", "440112" },
            { "番禺区", "440113" },
            { "花都区", "440114" },
            { "南沙区", "440115" },
            { "从化区", "440117" },
            { "增城区", "440118" },
            
            // 韶关市
            { "武江区", "440203" },
            { "浈江区", "440204" },
            { "曲江区", "440205" },
            { "始兴县", "440222" },
            { "仁化县", "440224" },
            { "翁源县", "440229" },
            { "乳源县", "440232" },
            { "新丰县", "440233" },
            { "乐昌市", "440281" },
            { "南雄市", "440282" },
            
            // 深圳市
            { "罗湖区", "440303" },
            { "福田区", "440304" },
            { "南山区", "440305" },
            { "宝安区", "440306" },
            { "龙岗区", "440307" },
            { "盐田区", "440308" },
            { "龙华新区", "440309" },
            { "坪山新区", "440310" },
            { "光明新区", "440311" },
            
            // 珠海市
            { "香洲区", "440402" },
            { "斗门区", "440403" },
            { "金湾区", "440404" },
            
            // 汕头市
            { "龙湖区", "440507" },
            { "金平区", "440511" },
            { "濠江区", "440512" },
            { "潮阳区", "440513" },
            { "潮南区", "440514" },
            { "澄海区", "440515" },
            { "南澳县", "440523" },
            
            // 佛山市
            { "禅城区", "440604" },
            { "南海区", "440605" },
            { "顺德区", "440606" },
            { "三水区", "440607" },
            { "高明区", "440608" },
            
            // 江门市
            { "蓬江区", "440703" },
            { "江海区", "440704" },
            { "新会区", "440705" },
            { "台山市", "440781" },
            { "开平市", "440783" },
            { "鹤山市", "440784" },
            { "恩平市", "440785" },
            
            // 湛江市
            { "赤坎区", "440802" },
            { "霞山区", "440803" },
            { "坡头区", "440804" },
            { "东海区", "440805" },
            { "麻章区", "440811" },
            { "遂溪县", "440823" },
            { "徐闻县", "440825" },
            { "廉江市", "440881" },
            { "雷州市", "440882" },
            { "吴川市", "440883" },
            
            // 茂名市
            { "茂南区", "440902" },
            { "茂港区", "440903" },
            { "电白区", "440904" },
            { "高州市", "440981" },
            { "化州市", "440982" },
            
            // 肇庆市
            { "端州区", "441202" },
            { "鼎湖区", "441203" },
            { "高要区", "441204" },
            { "广宁县", "441223" },
            { "怀集县", "441224" },
            { "封开县", "441225" },
            { "德庆县", "441226" },
            { "四会市", "441284" },
            
            // 惠州市
            { "惠城区", "441302" },
            { "惠阳区", "441303" },
            { "博罗县", "441322" },
            { "惠东县", "441323" },
            { "龙门县", "441324" },
            
            // 梅州市
            { "梅江区", "441402" },
            { "梅县区", "441403" },
            { "大埔县", "441422" },
            { "丰顺县", "441423" },
            { "五华县", "441424" },
            { "平远县", "441426" },
            { "蕉岭县", "441427" },
            { "兴宁市", "441481" },
            
            // 汕尾市
            { "城区", "441502" },
            { "海丰县", "441521" },
            { "陆河县", "441523" },
            { "陆丰市", "441581" },
            
            // 河源市
            { "源城区", "441602" },
            { "紫金县", "441621" },
            { "龙川县", "441622" },
            { "连平县", "441623" },
            { "和平县", "441624" },
            { "东源县", "441625" },
            
            // 阳江市
            { "江城区", "441702" },
            { "阳东区", "441704" },
            { "高新区", "441704" },
            { "阳西县", "441721" },
            { "阳春市", "441781" },
            
            // 清远市
            { "清城区", "441802" },
            { "清新区", "441803" },
            { "佛冈县", "441821" },
            { "阳山县", "441823" },
            { "连山县", "441825" },
            { "连南县", "441826" },
            { "英德市", "441881" },
            { "连州市", "441882" },
            
            // 潮州市
            { "湘桥区", "445102" },
            { "潮安区", "445103" },
            { "饶平县", "445122" },
            
            // 揭阳市
            { "榕城区", "445202" },
            { "揭东区", "445203" },
            { "揭西县", "445222" },
            { "惠来县", "445224" },
            { "普宁市", "445281" },
            
            // 云浮市
            { "云城区", "445302" },
            { "云安县", "445303" },
            { "新兴县", "445321" },
            { "郁南县", "445322" },
            { "罗定市", "445381" }
        };

        /// <summary>
        /// 根据县名获取县代码
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <returns>县代码，如果找不到则返回"000000"</returns>
        public static string GetCountyCode(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                System.Diagnostics.Debug.WriteLine("GetCountyCode: 县名为空");
                return "000000";
            }

            // 标准化县名
            string normalizedName = NormalizeCountyName(countyName);
            System.Diagnostics.Debug.WriteLine($"GetCountyCode: 原始县名='{countyName}', 标准化县名='{normalizedName}'");

            // 直接查找
            if (CountyCodeMap.TryGetValue(normalizedName, out string code))
            {
                System.Diagnostics.Debug.WriteLine($"GetCountyCode: 找到匹配 '{normalizedName}' -> '{code}'");
                return code;
            }

            // 模糊匹配 - 尝试匹配包含关键字的县名
            var fuzzyMatch = CountyCodeMap.FirstOrDefault(kvp => 
                kvp.Key.Contains(normalizedName) || normalizedName.Contains(kvp.Key));
            
            if (!fuzzyMatch.Equals(default(KeyValuePair<string, string>)))
            {
                System.Diagnostics.Debug.WriteLine($"GetCountyCode: 模糊匹配 '{normalizedName}' -> '{fuzzyMatch.Key}' -> '{fuzzyMatch.Value}'");
                return fuzzyMatch.Value;
            }

            // 没有找到匹配项
            System.Diagnostics.Debug.WriteLine($"GetCountyCode: 未找到县名 '{countyName}' 的代码映射");
            return "000000";
        }

        /// <summary>
        /// 根据行政区代码获取行政区名称
        /// </summary>
        /// <param name="countyCode">行政区代码</param>
        /// <returns>行政区名称，如果找不到则返回空字符串</returns>
        public static string GetCountyNameFromCode(string countyCode)
        {
            if (string.IsNullOrEmpty(countyCode))
            {
                System.Diagnostics.Debug.WriteLine("GetCountyNameFromCode: 行政区代码为空");
                return string.Empty;
            }

            // 标准化代码 - 取前6位
            string normalizedCode = countyCode.Length > 6 ? countyCode.Substring(0, 6) : countyCode;
            System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: 原始代码='{countyCode}', 标准化代码='{normalizedCode}'");

            // 直接查找
            var match = CountyCodeMap.FirstOrDefault(kvp => kvp.Value.Equals(normalizedCode, StringComparison.OrdinalIgnoreCase));
            
            if (!match.Equals(default(KeyValuePair<string, string>)))
            {
                System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: 找到匹配 '{normalizedCode}' -> '{match.Key}'");
                return match.Key;
            }

            // 模糊匹配 - 尝试匹配代码前缀
            if (normalizedCode.Length >= 4)
            {
                string codePrefix = normalizedCode.Substring(0, 4);
                var prefixMatch = CountyCodeMap.FirstOrDefault(kvp => kvp.Value.StartsWith(codePrefix));
                
                if (!prefixMatch.Equals(default(KeyValuePair<string, string>)))
                {
                    System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: 前缀匹配 '{codePrefix}' -> '{prefixMatch.Key}'");
                    return prefixMatch.Key;
                }
            }

            // 没有找到匹配项
            System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: 未找到代码 '{countyCode}' 的名称映射");
            return string.Empty;
        }

        /// <summary>
        /// 标准化县名 - 移除多余字符并统一格式
        /// </summary>
        /// <param name="countyName">原始县名</param>
        /// <returns>标准化后的县名</returns>
        private static string NormalizeCountyName(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                return string.Empty;
            }

            // 移除括号及其内容
            string normalized = System.Text.RegularExpressions.Regex.Replace(countyName, @"[（(].*?[）)]", "");
            
            // 移除特定的无关字符
            normalized = normalized.Replace("全民所有自然资源资产清查数据成果", "")
                                 .Replace("数据成果", "")
                                 .Replace("成果", "")
                                 .Trim();

            // 如果没有行政区划后缀，尝试从映射中找到最匹配的
            if (!normalized.EndsWith("区") && !normalized.EndsWith("县") && 
                !normalized.EndsWith("市") && !normalized.EndsWith("旗"))
            {
                // 尝试找到包含该名称的完整行政区名
                var possibleMatch = CountyCodeMap.Keys.FirstOrDefault(key => 
                    key.StartsWith(normalized) || key.Contains(normalized));
                
                if (!string.IsNullOrEmpty(possibleMatch))
                {
                    return possibleMatch;
                }
            }

            return normalized;
        }

        /// <summary>
        /// 检查是否存在指定县名的映射
        /// </summary>
        /// <param name="countyName">县名</param>
        /// <returns>是否存在映射</returns>
        public static bool HasCountyCode(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                return false;
            }

            string normalizedName = NormalizeCountyName(countyName);
            return CountyCodeMap.ContainsKey(normalizedName) ||
                   CountyCodeMap.Keys.Any(key => key.Contains(normalizedName) || normalizedName.Contains(key));
        }

        /// <summary>
        /// 获取所有支持的县名列表
        /// </summary>
        /// <returns>县名列表</returns>
        public static List<string> GetSupportedCountyNames()
        {
            return CountyCodeMap.Keys.ToList();
        }

        /// <summary>
        /// 获取县代码映射的数量
        /// </summary>
        /// <returns>映射数量</returns>
        public static int GetMappingCount()
        {
            return CountyCodeMap.Count;
        }
    }
}