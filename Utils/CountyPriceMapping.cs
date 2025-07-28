using System;
using System.Collections.Generic;
using System.Linq;

namespace TestArcMapAddin2.Utils
{
    /// <summary>
    /// �ش�������ͼ۸�ӳ�乤����
    /// �ṩ�����ش����ѯ��ҵ�õ���ͼ۸�Ĺ���
    /// </summary>
    public static class CountyPriceMapping
    {
        /// <summary>
        /// �ؼ۸���Ϣ
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
        /// �ش�������ͼ۸�ӳ���
        /// ������Դ����ҵ�õ���ͼ۸��
        /// </summary>
        private static readonly Dictionary<string, CountyPriceInfo> CountyPriceMap = new Dictionary<string, CountyPriceInfo>
        {
            { "440103", new CountyPriceInfo { CountyCode = "440103", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 1353 } },
            { "440104", new CountyPriceInfo { CountyCode = "440104", CountyName = "Խ����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 1353 } },
            { "440105", new CountyPriceInfo { CountyCode = "440105", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 1002 } },
            { "440106", new CountyPriceInfo { CountyCode = "440106", CountyName = "�����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 1002 } },
            { "440111", new CountyPriceInfo { CountyCode = "440111", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 673 } },
            { "440112", new CountyPriceInfo { CountyCode = "440112", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 483 } },
            { "440113", new CountyPriceInfo { CountyCode = "440113", CountyName = "��خ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 673 } },
            { "440114", new CountyPriceInfo { CountyCode = "440114", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 339 } },
            { "440115", new CountyPriceInfo { CountyCode = "440115", CountyName = "��ɳ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 673 } },
            { "440117", new CountyPriceInfo { CountyCode = "440117", CountyName = "�ӻ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 339 } },
            { "440118", new CountyPriceInfo { CountyCode = "440118", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 339 } },
            { "440203", new CountyPriceInfo { CountyCode = "440203", CountyName = "�佭��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 310 } },
            { "440204", new CountyPriceInfo { CountyCode = "440204", CountyName = "䥽���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 312 } },
            { "440205", new CountyPriceInfo { CountyCode = "440205", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 194 } },
            { "440222", new CountyPriceInfo { CountyCode = "440222", CountyName = "ʼ����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 140 } },
            { "440224", new CountyPriceInfo { CountyCode = "440224", CountyName = "�ʻ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 131 } },
            { "440229", new CountyPriceInfo { CountyCode = "440229", CountyName = "��Դ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 141 } },
            { "440232", new CountyPriceInfo { CountyCode = "440232", CountyName = "��Դ����������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 130 } },
            { "440233", new CountyPriceInfo { CountyCode = "440233", CountyName = "�·���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 156 } },
            { "440281", new CountyPriceInfo { CountyCode = "440281", CountyName = "�ֲ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 180 } },
            { "440282", new CountyPriceInfo { CountyCode = "440282", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 124 } },
            { "440303", new CountyPriceInfo { CountyCode = "440303", CountyName = "�޺���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 2661 } },
            { "440304", new CountyPriceInfo { CountyCode = "440304", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 3364 } },
            { "440305", new CountyPriceInfo { CountyCode = "440305", CountyName = "��ɽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 2384 } },
            { "440306", new CountyPriceInfo { CountyCode = "440306", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 878 } },
            { "440307", new CountyPriceInfo { CountyCode = "440307", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 682 } },
            { "440308", new CountyPriceInfo { CountyCode = "440308", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 1981 } },
            { "440309", new CountyPriceInfo { CountyCode = "440309", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 934 } },
            { "440310", new CountyPriceInfo { CountyCode = "440310", CountyName = "ƺɽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 797 } },
            { "440311", new CountyPriceInfo { CountyCode = "440311", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 847 } },
            { "440402", new CountyPriceInfo { CountyCode = "440402", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 596 } },
            { "440403", new CountyPriceInfo { CountyCode = "440403", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 346 } },
            { "440404", new CountyPriceInfo { CountyCode = "440404", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 418 } },
            { "440507", new CountyPriceInfo { CountyCode = "440507", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 660 } },
            { "440511", new CountyPriceInfo { CountyCode = "440511", CountyName = "��ƽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 601 } },
            { "440512", new CountyPriceInfo { CountyCode = "440512", CountyName = "婽���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 601 } },
            { "440513", new CountyPriceInfo { CountyCode = "440513", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 409 } },
            { "440514", new CountyPriceInfo { CountyCode = "440514", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 496 } },
            { "440515", new CountyPriceInfo { CountyCode = "440515", CountyName = "�κ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 565 } },
            { "440523", new CountyPriceInfo { CountyCode = "440523", CountyName = "�ϰ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 417 } },
            { "440604", new CountyPriceInfo { CountyCode = "440604", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 951 } },
            { "440605", new CountyPriceInfo { CountyCode = "440605", CountyName = "�Ϻ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 654 } },
            { "440606", new CountyPriceInfo { CountyCode = "440606", CountyName = "˳����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 1816 } },
            { "440607", new CountyPriceInfo { CountyCode = "440607", CountyName = "��ˮ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 387 } },
            { "440608", new CountyPriceInfo { CountyCode = "440608", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 354 } },
            { "440703", new CountyPriceInfo { CountyCode = "440703", CountyName = "���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 515 } },
            { "440704", new CountyPriceInfo { CountyCode = "440704", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 515 } },
            { "440705", new CountyPriceInfo { CountyCode = "440705", CountyName = "�»���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 327 } },
            { "440781", new CountyPriceInfo { CountyCode = "440781", CountyName = "̨ɽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 291 } },
            { "440783", new CountyPriceInfo { CountyCode = "440783", CountyName = "��ƽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 275 } },
            { "440784", new CountyPriceInfo { CountyCode = "440784", CountyName = "��ɽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 284 } },
            { "440785", new CountyPriceInfo { CountyCode = "440785", CountyName = "��ƽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 236 } },
            { "440802", new CountyPriceInfo { CountyCode = "440802", CountyName = "�࿲��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 395 } },
            { "440803", new CountyPriceInfo { CountyCode = "440803", CountyName = "ϼɽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 357 } },
            { "440804", new CountyPriceInfo { CountyCode = "440804", CountyName = "��ͷ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 311 } },
            { "440811", new CountyPriceInfo { CountyCode = "440811", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 357 } },
            { "440823", new CountyPriceInfo { CountyCode = "440823", CountyName = "��Ϫ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 240 } },
            { "440825", new CountyPriceInfo { CountyCode = "440825", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 217 } },
            { "440881", new CountyPriceInfo { CountyCode = "440881", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 241 } },
            { "440882", new CountyPriceInfo { CountyCode = "440882", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 210 } },
            { "440883", new CountyPriceInfo { CountyCode = "440883", CountyName = "�⴨��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 251 } },
            { "440902", new CountyPriceInfo { CountyCode = "440902", CountyName = "ï����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 289 } },
            { "440904", new CountyPriceInfo { CountyCode = "440904", CountyName = "�����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 312 } },
            { "440981", new CountyPriceInfo { CountyCode = "440981", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 184 } },
            { "440982", new CountyPriceInfo { CountyCode = "440982", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 199 } },
            { "440983", new CountyPriceInfo { CountyCode = "440983", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 199 } },
            { "441202", new CountyPriceInfo { CountyCode = "441202", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 859 } },
            { "441203", new CountyPriceInfo { CountyCode = "441203", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 392 } },
            { "441204", new CountyPriceInfo { CountyCode = "441204", CountyName = "��Ҫ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 236 } },
            { "441223", new CountyPriceInfo { CountyCode = "441223", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 209 } },
            { "441224", new CountyPriceInfo { CountyCode = "441224", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 233 } },
            { "441225", new CountyPriceInfo { CountyCode = "441225", CountyName = "�⿪��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 232 } },
            { "441226", new CountyPriceInfo { CountyCode = "441226", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 232 } },
            { "441284", new CountyPriceInfo { CountyCode = "441284", CountyName = "�Ļ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 249 } },
            { "441302", new CountyPriceInfo { CountyCode = "441302", CountyName = "�ݳ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 620 } },
            { "441303", new CountyPriceInfo { CountyCode = "441303", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 701 } },
            { "441322", new CountyPriceInfo { CountyCode = "441322", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 449 } },
            { "441323", new CountyPriceInfo { CountyCode = "441323", CountyName = "�ݶ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 443 } },
            { "441324", new CountyPriceInfo { CountyCode = "441324", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 286 } },
            { "441402", new CountyPriceInfo { CountyCode = "441402", CountyName = "÷����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 300 } },
            { "441403", new CountyPriceInfo { CountyCode = "441403", CountyName = "÷����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 285 } },
            { "441422", new CountyPriceInfo { CountyCode = "441422", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 194 } },
            { "441423", new CountyPriceInfo { CountyCode = "441423", CountyName = "��˳��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 155 } },
            { "441424", new CountyPriceInfo { CountyCode = "441424", CountyName = "�廪��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 180 } },
            { "441426", new CountyPriceInfo { CountyCode = "441426", CountyName = "ƽԶ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 132 } },
            { "441427", new CountyPriceInfo { CountyCode = "441427", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 159 } },
            { "441481", new CountyPriceInfo { CountyCode = "441481", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 155 } },
            { "441502", new CountyPriceInfo { CountyCode = "441502", CountyName = "����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 349 } },
            { "441521", new CountyPriceInfo { CountyCode = "441521", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 172 } },
            { "441523", new CountyPriceInfo { CountyCode = "441523", CountyName = "½����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 180 } },
            { "441581", new CountyPriceInfo { CountyCode = "441581", CountyName = "½����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 175 } },
            { "441602", new CountyPriceInfo { CountyCode = "441602", CountyName = "Դ����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 285 } },
            { "441621", new CountyPriceInfo { CountyCode = "441621", CountyName = "�Ͻ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 142 } },
            { "441622", new CountyPriceInfo { CountyCode = "441622", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 130 } },
            { "441623", new CountyPriceInfo { CountyCode = "441623", CountyName = "��ƽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 145 } },
            { "441624", new CountyPriceInfo { CountyCode = "441624", CountyName = "��ƽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 151 } },
            { "441625", new CountyPriceInfo { CountyCode = "441625", CountyName = "��Դ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 131 } },
            { "441702", new CountyPriceInfo { CountyCode = "441702", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 297 } },
            { "441704", new CountyPriceInfo { CountyCode = "441704", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 289 } },
            { "441721", new CountyPriceInfo { CountyCode = "441721", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 230 } },
            { "441781", new CountyPriceInfo { CountyCode = "441781", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 185 } },
            { "441802", new CountyPriceInfo { CountyCode = "441802", CountyName = "�����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 446 } },
            { "441803", new CountyPriceInfo { CountyCode = "441803", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 446 } },
            { "441821", new CountyPriceInfo { CountyCode = "441821", CountyName = "�����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 287 } },
            { "441823", new CountyPriceInfo { CountyCode = "441823", CountyName = "��ɽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 223 } },
            { "441825", new CountyPriceInfo { CountyCode = "441825", CountyName = "��ɽ׳������������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 223 } },
            { "441826", new CountyPriceInfo { CountyCode = "441826", CountyName = "��������������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 192 } },
            { "441881", new CountyPriceInfo { CountyCode = "441881", CountyName = "Ӣ����", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 242 } },
            { "441882", new CountyPriceInfo { CountyCode = "441882", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 222 } },
            { "441900", new CountyPriceInfo { CountyCode = "441900", CountyName = "��ݸ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 584 } },
            { "442000", new CountyPriceInfo { CountyCode = "442000", CountyName = "��ɽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 454 } },
            { "445102", new CountyPriceInfo { CountyCode = "445102", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 375 } },
            { "445103", new CountyPriceInfo { CountyCode = "445103", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 341 } },
            { "445122", new CountyPriceInfo { CountyCode = "445122", CountyName = "��ƽ��", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 216 } },
            { "445202", new CountyPriceInfo { CountyCode = "445202", CountyName = "�ų���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 335 } },
            { "445203", new CountyPriceInfo { CountyCode = "445203", CountyName = "�Ҷ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 320 } },
            { "445222", new CountyPriceInfo { CountyCode = "445222", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 230 } },
            { "445224", new CountyPriceInfo { CountyCode = "445224", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 310 } },
            { "445281", new CountyPriceInfo { CountyCode = "445281", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 260 } },
            { "445302", new CountyPriceInfo { CountyCode = "445302", CountyName = "�Ƴ���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 254 } },
            { "445303", new CountyPriceInfo { CountyCode = "445303", CountyName = "�ư���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 146 } },
            { "445321", new CountyPriceInfo { CountyCode = "445321", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 154 } },
            { "445322", new CountyPriceInfo { CountyCode = "445322", CountyName = "������", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 180 } },
            { "445381", new CountyPriceInfo { CountyCode = "445381", CountyName = "�޶���", LandTypeCode = "0601", LandTypeName = "��ҵ�õ�", MinimumPrice = 196 } }
        };

        /// <summary>
        /// �����ش����ȡ��ͼ۸�
        /// </summary>
        /// <param name="countyCode">��λ�ش���</param>
        /// <returns>��ҵ�õ���ͼ۸�Ԫ/ƽ���ף������δ�ҵ�����0</returns>
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

            System.Diagnostics.Debug.WriteLine($"����: δ�ҵ��ش��� {countyCode} �ļ۸���Ϣ");
            return 0;
        }

        /// <summary>
        /// �����ش����ȡ�����ļ۸���Ϣ
        /// </summary>
        /// <param name="countyCode">��λ�ش���</param>
        /// <returns>�ؼ۸���Ϣ�����δ�ҵ�����null</returns>
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
        /// ��ȡ����֧�ֵ��ش����б�
        /// </summary>
        /// <returns>�ش����б�</returns>
        public static List<string> GetSupportedCountyCodes()
        {
            return CountyPriceMap.Keys.ToList();
        }

        /// <summary>
        /// ����Ƿ�֧��ָ�����ش���
        /// </summary>
        /// <param name="countyCode">��λ�ش���</param>
        /// <returns>�Ƿ�֧��</returns>
        public static bool IsCountySupported(string countyCode)
        {
            return !string.IsNullOrEmpty(countyCode) && CountyPriceMap.ContainsKey(countyCode);
        }

        /// <summary>
        /// ��ȡ�����صļ۸���Ϣ
        /// </summary>
        /// <returns>�����صļ۸���Ϣ�б�</returns>
        public static List<CountyPriceInfo> GetAllPriceInfo()
        {
            return CountyPriceMap.Values.ToList();
        }
    }
}