using System;
using System.Collections.Generic;
using System.Linq;

namespace ForestResourcePlugin.Utils
{
    /// <summary>
    /// �ش���ӳ���� - �ṩ�������ش����ӳ�书��
    /// </summary>
    public static class CountyCodeMapper
    {
        /// <summary>
        /// �������ش����ӳ���ֵ�
        /// </summary>
        private static readonly Dictionary<string, string> CountyCodeMap = new Dictionary<string, string>
        {
            // ������
            { "������", "440103" },
            { "Խ����", "440104" },
            { "������", "440105" },
            { "�����", "440106" },
            { "������", "440111" },
            { "������", "440112" },
            { "��خ��", "440113" },
            { "������", "440114" },
            { "��ɳ��", "440115" },
            { "�ӻ���", "440117" },
            { "������", "440118" },
            
            // �ع���
            { "�佭��", "440203" },
            { "䥽���", "440204" },
            { "������", "440205" },
            { "ʼ����", "440222" },
            { "�ʻ���", "440224" },
            { "��Դ��", "440229" },
            { "��Դ��", "440232" },
            { "�·���", "440233" },
            { "�ֲ���", "440281" },
            { "������", "440282" },
            
            // ������
            { "�޺���", "440303" },
            { "������", "440304" },
            { "��ɽ��", "440305" },
            { "������", "440306" },
            { "������", "440307" },
            { "������", "440308" },
            { "��������", "440309" },
            { "ƺɽ����", "440310" },
            { "��������", "440311" },
            
            // �麣��
            { "������", "440402" },
            { "������", "440403" },
            { "������", "440404" },
            
            // ��ͷ��
            { "������", "440507" },
            { "��ƽ��", "440511" },
            { "婽���", "440512" },
            { "������", "440513" },
            { "������", "440514" },
            { "�κ���", "440515" },
            { "�ϰ���", "440523" },
            
            // ��ɽ��
            { "������", "440604" },
            { "�Ϻ���", "440605" },
            { "˳����", "440606" },
            { "��ˮ��", "440607" },
            { "������", "440608" },
            
            // ������
            { "���", "440703" },
            { "������", "440704" },
            { "�»���", "440705" },
            { "̨ɽ��", "440781" },
            { "��ƽ��", "440783" },
            { "��ɽ��", "440784" },
            { "��ƽ��", "440785" },
            
            // տ����
            { "�࿲��", "440802" },
            { "ϼɽ��", "440803" },
            { "��ͷ��", "440804" },
            { "������", "440805" },
            { "������", "440811" },
            { "��Ϫ��", "440823" },
            { "������", "440825" },
            { "������", "440881" },
            { "������", "440882" },
            { "�⴨��", "440883" },
            
            // ï����
            { "ï����", "440902" },
            { "ï����", "440903" },
            { "�����", "440904" },
            { "������", "440981" },
            { "������", "440982" },
            
            // ������
            { "������", "441202" },
            { "������", "441203" },
            { "��Ҫ��", "441204" },
            { "������", "441223" },
            { "������", "441224" },
            { "�⿪��", "441225" },
            { "������", "441226" },
            { "�Ļ���", "441284" },
            
            // ������
            { "�ݳ���", "441302" },
            { "������", "441303" },
            { "������", "441322" },
            { "�ݶ���", "441323" },
            { "������", "441324" },
            
            // ÷����
            { "÷����", "441402" },
            { "÷����", "441403" },
            { "������", "441422" },
            { "��˳��", "441423" },
            { "�廪��", "441424" },
            { "ƽԶ��", "441426" },
            { "������", "441427" },
            { "������", "441481" },
            
            // ��β��
            { "����", "441502" },
            { "������", "441521" },
            { "½����", "441523" },
            { "½����", "441581" },
            
            // ��Դ��
            { "Դ����", "441602" },
            { "�Ͻ���", "441621" },
            { "������", "441622" },
            { "��ƽ��", "441623" },
            { "��ƽ��", "441624" },
            { "��Դ��", "441625" },
            
            // ������
            { "������", "441702" },
            { "������", "441704" },
            { "������", "441704" },
            { "������", "441721" },
            { "������", "441781" },
            
            // ��Զ��
            { "�����", "441802" },
            { "������", "441803" },
            { "�����", "441821" },
            { "��ɽ��", "441823" },
            { "��ɽ��", "441825" },
            { "������", "441826" },
            { "Ӣ����", "441881" },
            { "������", "441882" },
            
            // ������
            { "������", "445102" },
            { "������", "445103" },
            { "��ƽ��", "445122" },
            
            // ������
            { "�ų���", "445202" },
            { "�Ҷ���", "445203" },
            { "������", "445222" },
            { "������", "445224" },
            { "������", "445281" },
            
            // �Ƹ���
            { "�Ƴ���", "445302" },
            { "�ư���", "445303" },
            { "������", "445321" },
            { "������", "445322" },
            { "�޶���", "445381" }
        };

        /// <summary>
        /// ����������ȡ�ش���
        /// </summary>
        /// <param name="countyName">����</param>
        /// <returns>�ش��룬����Ҳ����򷵻�"000000"</returns>
        public static string GetCountyCode(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                System.Diagnostics.Debug.WriteLine("GetCountyCode: ����Ϊ��");
                return "000000";
            }

            // ��׼������
            string normalizedName = NormalizeCountyName(countyName);
            System.Diagnostics.Debug.WriteLine($"GetCountyCode: ԭʼ����='{countyName}', ��׼������='{normalizedName}'");

            // ֱ�Ӳ���
            if (CountyCodeMap.TryGetValue(normalizedName, out string code))
            {
                System.Diagnostics.Debug.WriteLine($"GetCountyCode: �ҵ�ƥ�� '{normalizedName}' -> '{code}'");
                return code;
            }

            // ģ��ƥ�� - ����ƥ������ؼ��ֵ�����
            var fuzzyMatch = CountyCodeMap.FirstOrDefault(kvp => 
                kvp.Key.Contains(normalizedName) || normalizedName.Contains(kvp.Key));
            
            if (!fuzzyMatch.Equals(default(KeyValuePair<string, string>)))
            {
                System.Diagnostics.Debug.WriteLine($"GetCountyCode: ģ��ƥ�� '{normalizedName}' -> '{fuzzyMatch.Key}' -> '{fuzzyMatch.Value}'");
                return fuzzyMatch.Value;
            }

            // û���ҵ�ƥ����
            System.Diagnostics.Debug.WriteLine($"GetCountyCode: δ�ҵ����� '{countyName}' �Ĵ���ӳ��");
            return "000000";
        }

        /// <summary>
        /// ���������������ȡ����������
        /// </summary>
        /// <param name="countyCode">����������</param>
        /// <returns>���������ƣ�����Ҳ����򷵻ؿ��ַ���</returns>
        public static string GetCountyNameFromCode(string countyCode)
        {
            if (string.IsNullOrEmpty(countyCode))
            {
                System.Diagnostics.Debug.WriteLine("GetCountyNameFromCode: ����������Ϊ��");
                return string.Empty;
            }

            // ��׼������ - ȡǰ6λ
            string normalizedCode = countyCode.Length > 6 ? countyCode.Substring(0, 6) : countyCode;
            System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: ԭʼ����='{countyCode}', ��׼������='{normalizedCode}'");

            // ֱ�Ӳ���
            var match = CountyCodeMap.FirstOrDefault(kvp => kvp.Value.Equals(normalizedCode, StringComparison.OrdinalIgnoreCase));
            
            if (!match.Equals(default(KeyValuePair<string, string>)))
            {
                System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: �ҵ�ƥ�� '{normalizedCode}' -> '{match.Key}'");
                return match.Key;
            }

            // ģ��ƥ�� - ����ƥ�����ǰ׺
            if (normalizedCode.Length >= 4)
            {
                string codePrefix = normalizedCode.Substring(0, 4);
                var prefixMatch = CountyCodeMap.FirstOrDefault(kvp => kvp.Value.StartsWith(codePrefix));
                
                if (!prefixMatch.Equals(default(KeyValuePair<string, string>)))
                {
                    System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: ǰ׺ƥ�� '{codePrefix}' -> '{prefixMatch.Key}'");
                    return prefixMatch.Key;
                }
            }

            // û���ҵ�ƥ����
            System.Diagnostics.Debug.WriteLine($"GetCountyNameFromCode: δ�ҵ����� '{countyCode}' ������ӳ��");
            return string.Empty;
        }

        /// <summary>
        /// ��׼������ - �Ƴ������ַ���ͳһ��ʽ
        /// </summary>
        /// <param name="countyName">ԭʼ����</param>
        /// <returns>��׼���������</returns>
        private static string NormalizeCountyName(string countyName)
        {
            if (string.IsNullOrEmpty(countyName))
            {
                return string.Empty;
            }

            // �Ƴ����ż�������
            string normalized = System.Text.RegularExpressions.Regex.Replace(countyName, @"[��(].*?[��)]", "");
            
            // �Ƴ��ض����޹��ַ�
            normalized = normalized.Replace("ȫ��������Ȼ��Դ�ʲ�������ݳɹ�", "")
                                 .Replace("���ݳɹ�", "")
                                 .Replace("�ɹ�", "")
                                 .Trim();

            // ���û������������׺�����Դ�ӳ�����ҵ���ƥ���
            if (!normalized.EndsWith("��") && !normalized.EndsWith("��") && 
                !normalized.EndsWith("��") && !normalized.EndsWith("��"))
            {
                // �����ҵ����������Ƶ�������������
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
        /// ����Ƿ����ָ��������ӳ��
        /// </summary>
        /// <param name="countyName">����</param>
        /// <returns>�Ƿ����ӳ��</returns>
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
        /// ��ȡ����֧�ֵ������б�
        /// </summary>
        /// <returns>�����б�</returns>
        public static List<string> GetSupportedCountyNames()
        {
            return CountyCodeMap.Keys.ToList();
        }

        /// <summary>
        /// ��ȡ�ش���ӳ�������
        /// </summary>
        /// <returns>ӳ������</returns>
        public static int GetMappingCount()
        {
            return CountyCodeMap.Count;
        }
    }
}