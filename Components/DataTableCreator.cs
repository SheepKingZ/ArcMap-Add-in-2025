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
            fields.AddField(CreateField("AssetInventoryID", "资产清查标识码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("Name", "名称", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("Code", "代码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotID", "图斑标识码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotNumber", "图斑编号", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotArea", "图斑面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandType", "地类", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("ForestryBureau", "林业局", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("ForestFarm", "林场", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("SpotCode", "图斑编码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SubSpotArea", "子图斑面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandTenure", "国土变更调查土地权属", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("TreeOwnership", "林木所有权", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("ForestCategory", "林种", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("DominantSpecies", "优势树种（组）", esriFieldType.esriFieldTypeString, 255));
            fields.AddField(CreateField("Origin", "起源", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("AgeGroup", "龄组", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("CanopyClosureOrCoverage", "郁闭度/覆盖度", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("AverageAge", "平均年龄", esriFieldType.esriFieldTypeInteger));
            fields.AddField(CreateField("AverageHeight", "平均树高", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("AverageDBH", "平均胸径", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("StemsPerHectare", "每公顷株数", esriFieldType.esriFieldTypeInteger));
            fields.AddField(CreateField("SubSpotVolume", "子图斑蓄积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("Remarks", "备注", esriFieldType.esriFieldTypeString, 255));

            featureWorkspace.CreateTable(tableName, fields, null, null, "");
        }

        public void CreateGrasslandTable(IFeatureWorkspace featureWorkspace, string tableName)
        {
            IFieldsEdit fields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
            fields.AddField(CreateField("AssetInventoryID", "资产清查标识码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("Name", "名称", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("Code", "代码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotID", "图斑标识码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotNumber", "图斑编号", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotArea", "图斑面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandType", "地类", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotCode", "图斑编码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("GrasslandCategory", "草原类", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SubSpotArea", "子图斑面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("DominantGrassSpecies", "优势草种", esriFieldType.esriFieldTypeString, 255));
            fields.AddField(CreateField("VegetationCoverage", "植被盖度", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("DryHayYield", "小班干草产量", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("Remarks", "备注", esriFieldType.esriFieldTypeString, 255));

            featureWorkspace.CreateTable(tableName, fields, null, null, "");
        }

        public void CreateWetlandTable(IFeatureWorkspace featureWorkspace, string tableName)
        {
            IFieldsEdit fields = new ESRI.ArcGIS.Geodatabase.FieldsClass();
            fields.AddField(CreateField("AssetInventoryID", "资产清查标识码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("Name", "名称", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("Code", "代码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotID", "图斑标识码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotNumber", "图斑编号", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotArea", "图斑面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("LandType", "地类", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SpotCode", "图斑编码", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("SubSpotArea", "子图斑面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("ManagementLevel", "管理分级", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("ProtectedAreaAttribute", "自然保护地属性", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("UtilizationType", "利用方式", esriFieldType.esriFieldTypeString, 50));
            fields.AddField(CreateField("VegetationType", "植被类型", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("VegetationArea", "植被面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("ThreatStatus", "受威胁状况", esriFieldType.esriFieldTypeString, 100));
            fields.AddField(CreateField("AreaInUrbanDevelopmentBoundary", "划入城镇开发边界面积", esriFieldType.esriFieldTypeDouble));
            fields.AddField(CreateField("Remarks", "备注", esriFieldType.esriFieldTypeString, 255));

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
                field.Precision_2 = 18; // 示例精度
                field.Scale_2 = 8;    // 示例小数位数
            }
            field.IsNullable_2 = true;
            return field;
        }
        
    }
}