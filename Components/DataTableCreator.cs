using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace TestArcMapAddin2.Components
{
    public class DataTableCreator
    {
        public void CreateForestTable(IFeatureWorkspace featureWorkspace, string tableName)
        {
            IFieldsEdit fields = new FieldsClass();
            fields.AddField(CreateField("AssetInventoryID", "�ʲ�����ʶ��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("Name", "����", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("Code", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotID", "ͼ�߱�ʶ��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotNumber", "ͼ�߱��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotArea", "ͼ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandType", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("ForestryBureau", "��ҵ��", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("ForestFarm", "�ֳ�", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("SpotCode", "ͼ�߱���", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SubSpotArea", "��ͼ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandTenure", "���������������Ȩ��", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("TreeOwnership", "��ľ����Ȩ", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("ForestCategory", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("DominantSpecies", "�������֣��飩", esriFieldType.esriFieldTypeString, 255));
            fields.AddField(CreateField("Origin", "��Դ", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("AgeGroup", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("CanopyClosureOrCoverage", "���ն�/���Ƕ�", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("AverageAge", "ƽ������", esriFieldType.esriFieldTypeInteger));
            fields.AddField(CreateField("AverageHeight", "ƽ������", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("AverageDBH", "ƽ���ؾ�", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("StemsPerHectare", "ÿ��������", esriFieldType.esriFieldTypeInteger));
            fields.AddField(CreateField("SubSpotVolume", "��ͼ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("Remarks", "��ע", esriFieldType.esriFieldTypeString, 255));

            featureWorkspace.CreateTable(tableName, fields, null, null, "");
        }

        public void CreateGrasslandTable(IFeatureWorkspace featureWorkspace, string tableName)
        {
            IFieldsEdit fields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
            fields.AddField(CreateField("AssetInventoryID", "�ʲ�����ʶ��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("Name", "����", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("Code", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotID", "ͼ�߱�ʶ��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotNumber", "ͼ�߱��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotArea", "ͼ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandType", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotCode", "ͼ�߱���", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("GrasslandCategory", "��ԭ��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SubSpotArea", "��ͼ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("DominantGrassSpecies", "���Ʋ���", esriFieldType.esriFieldTypeString, 255));
            fields.AddField(CreateField("VegetationCoverage", "ֲ���Ƕ�", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("DryHayYield", "С��ɲݲ���", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("Remarks", "��ע", esriFieldType.esriFieldTypeString, 255));

            featureWorkspace.CreateTable(tableName, fields, null, null, "");
        }

        public void CreateWetlandTable(IFeatureWorkspace featureWorkspace, string tableName)
        {
            IFieldsEdit fields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
            fields.AddField(CreateField("AssetInventoryID", "�ʲ�����ʶ��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("Name", "����", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("Code", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotID", "ͼ�߱�ʶ��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotNumber", "ͼ�߱��", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotArea", "ͼ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandType", "����", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotCode", "ͼ�߱���", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SubSpotArea", "��ͼ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("ManagementLevel", "����ּ�", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("ProtectedAreaAttribute", "��Ȼ����������", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("UtilizationType", "���÷�ʽ", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("VegetationType", "ֲ������", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("VegetationArea", "ֲ�����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("ThreatStatus", "����в״��", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("AreaInUrbanDevelopmentBoundary", "������򿪷��߽����", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("Remarks", "��ע", esriFieldType.esriFieldTypeString, 255));

            featureWorkspace.CreateTable(tableName, fields, null, null, "");
        }

        private IField CreateField(string name, string aliasName, esriFieldType type, int length = 0)
        {
            IFieldEdit field = new ESRI.ArcGIS.Geodatabase.FieldClass();
            field.Name_2 = name;
            field.AliasName_2 = aliasName;
            field.Type_2 = type;
            if (type == esriFieldType.esriFieldTypeString && length > 0)
            {
                field.Length_2 = length;
            }
            if (type == esriFieldType.esriFieldTypeDouble || type == esriFieldType.esriFieldTypeSingle)
            {
                field.Precision_2 = 18; // ʾ������
                field.Scale_2 = 8;    // ʾ��С��λ��
            }
            field.IsNullable_2 = true;
            return field;
        }
        
    }
}