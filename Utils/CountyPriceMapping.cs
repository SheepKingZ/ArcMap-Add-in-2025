using System;
using System.Collections.Generic;
using System.Linq;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// 县代码与最低价格映射工具类
    /// 提供根据县代码查询工业用地最低价格的功能
    /// </summary>
    public static class CountyPriceMapping
    {
        /// <summary>
        /// 县价格信息
        /// </summary>
        public class CountyPriceInfo
        {
            public string CountyCode { get; set; }
            public string CountyName { get; set; }
            public string LandTypeCode { get; set; }
            public string LandTypeName { get; set; }
            public decimal MinimumPrice { get; set; }
        }

        /// <summary>
        /// 县代码与最低价格映射表
        /// 数据来源：工业用地最低价格表
        /// </summary>
        private static readonly Dictionary<string, CountyPriceInfo> CountyPriceMap = new Dictionary<string, CountyPriceInfo>
        {
            { "440103", new CountyPriceInfo { CountyCode = "440103", CountyName = "荔湾区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 1353 } },
            { "440104", new CountyPriceInfo { CountyCode = "440104", CountyName = "越秀区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 1353 } },
            { "440105", new CountyPriceInfo { CountyCode = "440105", CountyName = "海珠区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 1002 } },
            { "440106", new CountyPriceInfo { CountyCode = "440106", CountyName = "天河区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 1002 } },
            { "440111", new CountyPriceInfo { CountyCode = "440111", CountyName = "白云区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 673 } },
            { "440112", new CountyPriceInfo { CountyCode = "440112", CountyName = "黄埔区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 483 } },
            { "440113", new CountyPriceInfo { CountyCode = "440113", CountyName = "番禺区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 673 } },
            { "440114", new CountyPriceInfo { CountyCode = "440114", CountyName = "花都区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 339 } },
            { "440115", new CountyPriceInfo { CountyCode = "440115", CountyName = "南沙区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 673 } },
            { "440117", new CountyPriceInfo { CountyCode = "440117", CountyName = "从化区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 339 } },
            { "440118", new CountyPriceInfo { CountyCode = "440118", CountyName = "增城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 339 } },
            { "440203", new CountyPriceInfo { CountyCode = "440203", CountyName = "武江区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 310 } },
            { "440204", new CountyPriceInfo { CountyCode = "440204", CountyName = "浈江区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 312 } },
            { "440205", new CountyPriceInfo { CountyCode = "440205", CountyName = "曲江区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 194 } },
            { "440222", new CountyPriceInfo { CountyCode = "440222", CountyName = "始兴县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 140 } },
            { "440224", new CountyPriceInfo { CountyCode = "440224", CountyName = "仁化县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 131 } },
            { "440229", new CountyPriceInfo { CountyCode = "440229", CountyName = "翁源县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 141 } },
            { "440232", new CountyPriceInfo { CountyCode = "440232", CountyName = "乳源瑶族自治县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 130 } },
            { "440233", new CountyPriceInfo { CountyCode = "440233", CountyName = "新丰县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 156 } },
            { "440281", new CountyPriceInfo { CountyCode = "440281", CountyName = "乐昌市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 180 } },
            { "440282", new CountyPriceInfo { CountyCode = "440282", CountyName = "南雄市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 124 } },
            { "440303", new CountyPriceInfo { CountyCode = "440303", CountyName = "罗湖区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 2661 } },
            { "440304", new CountyPriceInfo { CountyCode = "440304", CountyName = "福田区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 3364 } },
            { "440305", new CountyPriceInfo { CountyCode = "440305", CountyName = "南山区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 2384 } },
            { "440306", new CountyPriceInfo { CountyCode = "440306", CountyName = "宝安区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 878 } },
            { "440307", new CountyPriceInfo { CountyCode = "440307", CountyName = "龙岗区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 682 } },
            { "440308", new CountyPriceInfo { CountyCode = "440308", CountyName = "盐田区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 1981 } },
            { "440309", new CountyPriceInfo { CountyCode = "440309", CountyName = "龙华区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 934 } },
            { "440310", new CountyPriceInfo { CountyCode = "440310", CountyName = "坪山区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 797 } },
            { "440311", new CountyPriceInfo { CountyCode = "440311", CountyName = "光明区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 847 } },
            { "440402", new CountyPriceInfo { CountyCode = "440402", CountyName = "香洲区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 596 } },
            { "440403", new CountyPriceInfo { CountyCode = "440403", CountyName = "斗门区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 346 } },
            { "440404", new CountyPriceInfo { CountyCode = "440404", CountyName = "金湾区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 418 } },
            { "440507", new CountyPriceInfo { CountyCode = "440507", CountyName = "龙湖区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 660 } },
            { "440511", new CountyPriceInfo { CountyCode = "440511", CountyName = "金平区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 601 } },
            { "440512", new CountyPriceInfo { CountyCode = "440512", CountyName = "濠江区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 601 } },
            { "440513", new CountyPriceInfo { CountyCode = "440513", CountyName = "潮阳区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 409 } },
            { "440514", new CountyPriceInfo { CountyCode = "440514", CountyName = "潮南区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 496 } },
            { "440515", new CountyPriceInfo { CountyCode = "440515", CountyName = "澄海区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 565 } },
            { "440523", new CountyPriceInfo { CountyCode = "440523", CountyName = "南澳县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 417 } },
            { "440604", new CountyPriceInfo { CountyCode = "440604", CountyName = "禅城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 951 } },
            { "440605", new CountyPriceInfo { CountyCode = "440605", CountyName = "南海区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 654 } },
            { "440606", new CountyPriceInfo { CountyCode = "440606", CountyName = "顺德区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 1816 } },
            { "440607", new CountyPriceInfo { CountyCode = "440607", CountyName = "三水区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 387 } },
            { "440608", new CountyPriceInfo { CountyCode = "440608", CountyName = "高明区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 354 } },
            { "440703", new CountyPriceInfo { CountyCode = "440703", CountyName = "蓬江区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 515 } },
            { "440704", new CountyPriceInfo { CountyCode = "440704", CountyName = "江海区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 515 } },
            { "440705", new CountyPriceInfo { CountyCode = "440705", CountyName = "新会区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 327 } },
            { "440781", new CountyPriceInfo { CountyCode = "440781", CountyName = "台山市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 291 } },
            { "440783", new CountyPriceInfo { CountyCode = "440783", CountyName = "开平市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 275 } },
            { "440784", new CountyPriceInfo { CountyCode = "440784", CountyName = "鹤山市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 284 } },
            { "440785", new CountyPriceInfo { CountyCode = "440785", CountyName = "恩平市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 236 } },
            { "440802", new CountyPriceInfo { CountyCode = "440802", CountyName = "赤坎区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 395 } },
            { "440803", new CountyPriceInfo { CountyCode = "440803", CountyName = "霞山区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 357 } },
            { "440804", new CountyPriceInfo { CountyCode = "440804", CountyName = "坡头区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 311 } },
            { "440811", new CountyPriceInfo { CountyCode = "440811", CountyName = "麻章区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 357 } },
            { "440823", new CountyPriceInfo { CountyCode = "440823", CountyName = "遂溪县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 240 } },
            { "440825", new CountyPriceInfo { CountyCode = "440825", CountyName = "徐闻县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 217 } },
            { "440881", new CountyPriceInfo { CountyCode = "440881", CountyName = "廉江市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 241 } },
            { "440882", new CountyPriceInfo { CountyCode = "440882", CountyName = "雷州市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 210 } },
            { "440883", new CountyPriceInfo { CountyCode = "440883", CountyName = "吴川市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 251 } },
            { "440902", new CountyPriceInfo { CountyCode = "440902", CountyName = "茂南区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 289 } },
            { "440904", new CountyPriceInfo { CountyCode = "440904", CountyName = "电白区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 312 } },
            { "440981", new CountyPriceInfo { CountyCode = "440981", CountyName = "高州市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 184 } },
            { "440982", new CountyPriceInfo { CountyCode = "440982", CountyName = "化州市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 199 } },
            { "440983", new CountyPriceInfo { CountyCode = "440983", CountyName = "信宜市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 199 } },
            { "441202", new CountyPriceInfo { CountyCode = "441202", CountyName = "端州区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 859 } },
            { "441203", new CountyPriceInfo { CountyCode = "441203", CountyName = "鼎湖区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 392 } },
            { "441204", new CountyPriceInfo { CountyCode = "441204", CountyName = "高要区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 236 } },
            { "441223", new CountyPriceInfo { CountyCode = "441223", CountyName = "广宁县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 209 } },
            { "441224", new CountyPriceInfo { CountyCode = "441224", CountyName = "怀集县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 233 } },
            { "441225", new CountyPriceInfo { CountyCode = "441225", CountyName = "封开县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 232 } },
            { "441226", new CountyPriceInfo { CountyCode = "441226", CountyName = "德庆县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 232 } },
            { "441284", new CountyPriceInfo { CountyCode = "441284", CountyName = "四会市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 249 } },
            { "441302", new CountyPriceInfo { CountyCode = "441302", CountyName = "惠城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 620 } },
            { "441303", new CountyPriceInfo { CountyCode = "441303", CountyName = "惠阳区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 701 } },
            { "441322", new CountyPriceInfo { CountyCode = "441322", CountyName = "博罗县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 449 } },
            { "441323", new CountyPriceInfo { CountyCode = "441323", CountyName = "惠东县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 443 } },
            { "441324", new CountyPriceInfo { CountyCode = "441324", CountyName = "龙门县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 286 } },
            { "441402", new CountyPriceInfo { CountyCode = "441402", CountyName = "梅江区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 300 } },
            { "441403", new CountyPriceInfo { CountyCode = "441403", CountyName = "梅县区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 285 } },
            { "441422", new CountyPriceInfo { CountyCode = "441422", CountyName = "大埔县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 194 } },
            { "441423", new CountyPriceInfo { CountyCode = "441423", CountyName = "丰顺县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 155 } },
            { "441424", new CountyPriceInfo { CountyCode = "441424", CountyName = "五华县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 180 } },
            { "441426", new CountyPriceInfo { CountyCode = "441426", CountyName = "平远县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 132 } },
            { "441427", new CountyPriceInfo { CountyCode = "441427", CountyName = "蕉岭县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 159 } },
            { "441481", new CountyPriceInfo { CountyCode = "441481", CountyName = "兴宁市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 155 } },
            { "441502", new CountyPriceInfo { CountyCode = "441502", CountyName = "城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 349 } },
            { "441521", new CountyPriceInfo { CountyCode = "441521", CountyName = "海丰县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 172 } },
            { "441523", new CountyPriceInfo { CountyCode = "441523", CountyName = "陆河县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 180 } },
            { "441581", new CountyPriceInfo { CountyCode = "441581", CountyName = "陆丰市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 175 } },
            { "441602", new CountyPriceInfo { CountyCode = "441602", CountyName = "源城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 285 } },
            { "441621", new CountyPriceInfo { CountyCode = "441621", CountyName = "紫金县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 142 } },
            { "441622", new CountyPriceInfo { CountyCode = "441622", CountyName = "龙川县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 130 } },
            { "441623", new CountyPriceInfo { CountyCode = "441623", CountyName = "连平县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 145 } },
            { "441624", new CountyPriceInfo { CountyCode = "441624", CountyName = "和平县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 151 } },
            { "441625", new CountyPriceInfo { CountyCode = "441625", CountyName = "东源县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 131 } },
            { "441702", new CountyPriceInfo { CountyCode = "441702", CountyName = "江城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 297 } },
            { "441704", new CountyPriceInfo { CountyCode = "441704", CountyName = "阳东区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 289 } },
            { "441721", new CountyPriceInfo { CountyCode = "441721", CountyName = "阳西县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 230 } },
            { "441781", new CountyPriceInfo { CountyCode = "441781", CountyName = "阳春市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 185 } },
            { "441802", new CountyPriceInfo { CountyCode = "441802", CountyName = "清城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 446 } },
            { "441803", new CountyPriceInfo { CountyCode = "441803", CountyName = "清新区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 446 } },
            { "441821", new CountyPriceInfo { CountyCode = "441821", CountyName = "佛冈县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 287 } },
            { "441823", new CountyPriceInfo { CountyCode = "441823", CountyName = "阳山县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 223 } },
            { "441825", new CountyPriceInfo { CountyCode = "441825", CountyName = "连山壮族瑶族自治县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 223 } },
            { "441826", new CountyPriceInfo { CountyCode = "441826", CountyName = "连南瑶族自治县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 192 } },
            { "441881", new CountyPriceInfo { CountyCode = "441881", CountyName = "英德市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 242 } },
            { "441882", new CountyPriceInfo { CountyCode = "441882", CountyName = "连州市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 222 } },
            { "441900", new CountyPriceInfo { CountyCode = "441900", CountyName = "东莞市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 584 } },
            { "442000", new CountyPriceInfo { CountyCode = "442000", CountyName = "中山市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 454 } },
            { "445102", new CountyPriceInfo { CountyCode = "445102", CountyName = "湘桥区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 375 } },
            { "445103", new CountyPriceInfo { CountyCode = "445103", CountyName = "潮安区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 341 } },
            { "445122", new CountyPriceInfo { CountyCode = "445122", CountyName = "饶平县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 216 } },
            { "445202", new CountyPriceInfo { CountyCode = "445202", CountyName = "榕城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 335 } },
            { "445203", new CountyPriceInfo { CountyCode = "445203", CountyName = "揭东区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 320 } },
            { "445222", new CountyPriceInfo { CountyCode = "445222", CountyName = "揭西县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 230 } },
            { "445224", new CountyPriceInfo { CountyCode = "445224", CountyName = "惠来县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 310 } },
            { "445281", new CountyPriceInfo { CountyCode = "445281", CountyName = "普宁市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 260 } },
            { "445302", new CountyPriceInfo { CountyCode = "445302", CountyName = "云城区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 254 } },
            { "445303", new CountyPriceInfo { CountyCode = "445303", CountyName = "云安区", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 146 } },
            { "445321", new CountyPriceInfo { CountyCode = "445321", CountyName = "新兴县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 154 } },
            { "445322", new CountyPriceInfo { CountyCode = "445322", CountyName = "郁南县", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 180 } },
            { "445381", new CountyPriceInfo { CountyCode = "445381", CountyName = "罗定市", LandTypeCode = "0601", LandTypeName = "工业用地", MinimumPrice = 196 } }
        };

        /// <summary>
        /// 根据县代码获取最低价格
        /// </summary>
        /// <param name="countyCode">六位县代码</param>
        /// <returns>工业用地最低价格（元/平方米），如果未找到返回0</returns>
        public static decimal GetMinimumPrice(string countyCode)
        {
            if (string.IsNullOrEmpty(countyCode))
            {
                return 0;
            }

            if (CountyPriceMap.TryGetValue(countyCode, out CountyPriceInfo priceInfo))
            {
                return priceInfo.MinimumPrice;
            }

            System.Diagnostics.Debug.WriteLine($"警告: 未找到县代码 {countyCode} 的价格信息");
            return 0;
        }

        /// <summary>
        /// 根据县代码获取完整的价格信息
        /// </summary>
        /// <param name="countyCode">六位县代码</param>
        /// <returns>县价格信息，如果未找到返回null</returns>
        public static CountyPriceInfo GetPriceInfo(string countyCode)
        {
            if (string.IsNullOrEmpty(countyCode))
            {
                return null;
            }

            CountyPriceMap.TryGetValue(countyCode, out CountyPriceInfo priceInfo);
            return priceInfo;
        }

        /// <summary>
        /// 获取所有支持的县代码列表
        /// </summary>
        /// <returns>县代码列表</returns>
        public static List<string> GetSupportedCountyCodes()
        {
            return CountyPriceMap.Keys.ToList();
        }

        /// <summary>
        /// 检查是否支持指定的县代码
        /// </summary>
        /// <param name="countyCode">六位县代码</param>
        /// <returns>是否支持</returns>
        public static bool IsCountySupported(string countyCode)
        {
            return !string.IsNullOrEmpty(countyCode) && CountyPriceMap.ContainsKey(countyCode);
        }

        /// <summary>
        /// 获取所有县的价格信息
        /// </summary>
        /// <returns>所有县的价格信息列表</returns>
        public static List<CountyPriceInfo> GetAllPriceInfo()
        {
            return CountyPriceMap.Values.ToList();
        }
    }
}