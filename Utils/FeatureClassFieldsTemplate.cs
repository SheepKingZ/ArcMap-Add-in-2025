using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;

namespace TestArcMapAddin2
{
    public class FeatureClassFieldsTemplate
    {
        /// <summary>
        /// 为Shapefile生成LCXZGX字段（字段名限制在10个字符以内）
        /// </summary>
        /// <param name="fields">字段集合编辑器</param>
        public static void GenerateLcxzgxFields(IFieldsEdit fields)
        {
            if (fields == null)
            {
                System.Windows.Forms.MessageBox.Show("字段集合对象不存在");
                return;
            }

            //标识码字段
            IField field01 = new FieldClass();
            IFieldEdit fieldEdit01 = (IFieldEdit)field01;
            fieldEdit01.Name_2 = "bsm";
            fieldEdit01.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit01.IsNullable_2 = true;
            fieldEdit01.AliasName_2 = "标识码";
            fieldEdit01.DefaultValue_2 = "";
            fieldEdit01.Editable_2 = true;
            fieldEdit01.Length_2 = 18;
            fields.AddField(field01);

            //要素代码
            IField field02 = new FieldClass();
            IFieldEdit fieldEdit02 = (IFieldEdit)field02;
            fieldEdit02.Name_2 = "ysdm";
            fieldEdit02.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit02.IsNullable_2 = true;
            fieldEdit02.AliasName_2 = "要素代码";
            fieldEdit02.DefaultValue_2 = "";
            fieldEdit02.Editable_2 = true;
            fieldEdit02.Length_2 = 10;
            fields.AddField(field02);

            //图斑预编号字段
            IField field03 = new FieldClass();
            IFieldEdit fieldEdit03 = (IFieldEdit)field03;
            fieldEdit03.Name_2 = "tbybh";
            fieldEdit03.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit03.IsNullable_2 = true;
            fieldEdit03.AliasName_2 = "图斑预编号";
            fieldEdit03.DefaultValue_2 = "";
            fieldEdit03.Editable_2 = true;
            fieldEdit03.Length_2 = 8;
            fields.AddField(field03);

            //图斑编号字段
            IField field04 = new FieldClass();
            IFieldEdit fieldEdit04 = (IFieldEdit)field04;
            fieldEdit04.Name_2 = "tbbh";
            fieldEdit04.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit04.IsNullable_2 = true;
            fieldEdit04.AliasName_2 = "图斑编号";
            fieldEdit04.DefaultValue_2 = "";
            fieldEdit04.Editable_2 = true;
            fieldEdit04.Length_2 = 8;
            fields.AddField(field04);

            //地类编码字段
            IField field05 = new FieldClass();
            IFieldEdit fieldEdit05 = (IFieldEdit)field05;
            fieldEdit05.Name_2 = "dlbm";
            fieldEdit05.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit05.IsNullable_2 = true;
            fieldEdit05.AliasName_2 = "地类编码";
            fieldEdit05.DefaultValue_2 = "";
            fieldEdit05.Editable_2 = true;
            fieldEdit05.Length_2 = 5;
            fields.AddField(field05);

            //地类名称字段
            IField field06 = new FieldClass();
            IFieldEdit fieldEdit06 = (IFieldEdit)field06;
            fieldEdit06.Name_2 = "dlmc";
            fieldEdit06.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit06.IsNullable_2 = true;
            fieldEdit06.AliasName_2 = "地类名称";
            fieldEdit06.DefaultValue_2 = "";
            fieldEdit06.Editable_2 = true;
            fieldEdit06.Length_2 = 60;
            fields.AddField(field06);

            //权属性质字段
            IField field07 = new FieldClass();
            IFieldEdit fieldEdit07 = (IFieldEdit)field07;
            fieldEdit07.Name_2 = "qsxz";
            fieldEdit07.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit07.IsNullable_2 = true;
            fieldEdit07.AliasName_2 = "权属性质";
            fieldEdit07.DefaultValue_2 = "";
            fieldEdit07.Editable_2 = true;
            fieldEdit07.Length_2 = 2;
            fields.AddField(field07);

            //权属单位代码字段
            IField field08 = new FieldClass();
            IFieldEdit fieldEdit08 = (IFieldEdit)field08;
            fieldEdit08.Name_2 = "qsdwdm";
            fieldEdit08.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit08.IsNullable_2 = true;
            fieldEdit08.AliasName_2 = "权属单位代码";
            fieldEdit08.DefaultValue_2 = "";
            fieldEdit08.Editable_2 = true;
            fieldEdit08.Length_2 = 19;
            fields.AddField(field08);

            //权属单位名称字段
            IField field09 = new FieldClass();
            IFieldEdit fieldEdit09 = (IFieldEdit)field09;
            fieldEdit09.Name_2 = "qsdwmc";
            fieldEdit09.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit09.IsNullable_2 = true;
            fieldEdit09.AliasName_2 = "权属单位名称";
            fieldEdit09.DefaultValue_2 = "";
            fieldEdit09.Editable_2 = true;
            fieldEdit09.Length_2 = 60;
            fields.AddField(field09);

            //坐落单位代码字段
            IField field10 = new FieldClass();
            IFieldEdit fieldEdit10 = (IFieldEdit)field10;
            fieldEdit10.Name_2 = "zldwdm";
            fieldEdit10.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit10.IsNullable_2 = true;
            fieldEdit10.AliasName_2 = "坐落单位代码";
            fieldEdit10.DefaultValue_2 = "";
            fieldEdit10.Editable_2 = true;
            fieldEdit10.Length_2 = 19;
            fields.AddField(field10);

            //坐落单位名称字段
            IField field11 = new FieldClass();
            IFieldEdit fieldEdit11 = (IFieldEdit)field11;
            fieldEdit11.Name_2 = "zldwmc";
            fieldEdit11.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit11.IsNullable_2 = true;
            fieldEdit11.AliasName_2 = "坐落单位名称";
            fieldEdit11.DefaultValue_2 = "";
            fieldEdit11.Editable_2 = true;
            fieldEdit11.Length_2 = 60;
            fields.AddField(field11);

            //图斑面积字段
            IField field12 = new FieldClass();
            IFieldEdit fieldEdit12 = (IFieldEdit)field12;
            fieldEdit12.Name_2 = "tbmj";
            fieldEdit12.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit12.IsNullable_2 = true;
            fieldEdit12.AliasName_2 = "图斑面积";
            fieldEdit12.DefaultValue_2 = 0;
            fieldEdit12.Editable_2 = true;
            fieldEdit12.Precision_2 = 18;
            fieldEdit12.Scale_2 = 2;
            fields.AddField(field12);

            //扣除地类编码字段
            IField field13 = new FieldClass();
            IFieldEdit fieldEdit13 = (IFieldEdit)field13;
            fieldEdit13.Name_2 = "kcdlbm";
            fieldEdit13.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit13.IsNullable_2 = true;
            fieldEdit13.AliasName_2 = "扣除地类编码";
            fieldEdit13.DefaultValue_2 = "";
            fieldEdit13.Editable_2 = true;
            fieldEdit13.Length_2 = 5;
            fields.AddField(field13);

            //扣除地类系数字段
            IField field14 = new FieldClass();
            IFieldEdit fieldEdit14 = (IFieldEdit)field14;
            fieldEdit14.Name_2 = "kcxs";
            fieldEdit14.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit14.IsNullable_2 = true;
            fieldEdit14.AliasName_2 = "扣除地类系数";
            fieldEdit14.DefaultValue_2 = 0;
            fieldEdit14.Editable_2 = true;
            fieldEdit14.Precision_2 = 6;
            fieldEdit14.Scale_2 = 4;
            fields.AddField(field14);

            //扣除地类面积字段
            IField field15 = new FieldClass();
            IFieldEdit fieldEdit15 = (IFieldEdit)field15;
            fieldEdit15.Name_2 = "kcmj";
            fieldEdit15.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit15.IsNullable_2 = true;
            fieldEdit15.AliasName_2 = "扣除地类面积";
            fieldEdit15.DefaultValue_2 = 0;
            fieldEdit15.Editable_2 = true;
            fieldEdit15.Precision_2 = 18;
            fieldEdit15.Scale_2 = 2;
            fields.AddField(field15);

            //图斑地类面积字段
            IField field16 = new FieldClass();
            IFieldEdit fieldEdit16 = (IFieldEdit)field16;
            fieldEdit16.Name_2 = "tbdlmj";
            fieldEdit16.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit16.IsNullable_2 = true;
            fieldEdit16.AliasName_2 = "图斑地类面积";
            fieldEdit16.DefaultValue_2 = 0;
            fieldEdit16.Editable_2 = true;
            fieldEdit16.Precision_2 = 18;
            fieldEdit16.Scale_2 = 2;
            fields.AddField(field16);

            //耕地类型字段
            IField field17 = new FieldClass();
            IFieldEdit fieldEdit17 = (IFieldEdit)field17;
            fieldEdit17.Name_2 = "gdlx";
            fieldEdit17.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit17.IsNullable_2 = true;
            fieldEdit17.AliasName_2 = "耕地类型编码";
            fieldEdit17.DefaultValue_2 = "";
            fieldEdit17.Editable_2 = true;
            fieldEdit17.Length_2 = 2;
            fields.AddField(field17);

            //耕地坡度级别字段
            IField field18 = new FieldClass();
            IFieldEdit fieldEdit18 = (IFieldEdit)field18;
            fieldEdit18.Name_2 = "gdpdjb";
            fieldEdit18.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit18.IsNullable_2 = true;
            fieldEdit18.AliasName_2 = "耕地坡度级别";
            fieldEdit18.DefaultValue_2 = "";
            fieldEdit18.Editable_2 = true;
            fieldEdit18.Length_2 = 2;
            fields.AddField(field18);

            //线性图斑宽度字段
            IField field19 = new FieldClass();
            IFieldEdit fieldEdit19 = (IFieldEdit)field19;
            fieldEdit19.Name_2 = "xxtbkd";
            fieldEdit19.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit19.IsNullable_2 = true;
            fieldEdit19.AliasName_2 = "线性图斑宽度";
            fieldEdit19.DefaultValue_2 = 0;
            fieldEdit19.Editable_2 = true;
            fieldEdit19.Precision_2 = 5;
            fieldEdit19.Scale_2 = 1;
            fields.AddField(field19);

            //图斑细化代码字段
            IField field20 = new FieldClass();
            IFieldEdit fieldEdit20 = (IFieldEdit)field20;
            fieldEdit20.Name_2 = "tbxhdm";
            fieldEdit20.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit20.IsNullable_2 = true;
            fieldEdit20.AliasName_2 = "图斑细化代码";
            fieldEdit20.DefaultValue_2 = "";
            fieldEdit20.Editable_2 = true;
            fieldEdit20.Length_2 = 4;
            fields.AddField(field20);

            //图斑细化名称字段
            IField field21 = new FieldClass();
            IFieldEdit fieldEdit21 = (IFieldEdit)field21;
            fieldEdit21.Name_2 = "tbxhmc";
            fieldEdit21.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit21.IsNullable_2 = true;
            fieldEdit21.AliasName_2 = "图斑细化名称";
            fieldEdit21.DefaultValue_2 = "";
            fieldEdit21.Editable_2 = true;
            fieldEdit21.Length_2 = 20;
            fields.AddField(field21);

            //种植属性代码字段
            IField field22 = new FieldClass();
            IFieldEdit fieldEdit22 = (IFieldEdit)field22;
            fieldEdit22.Name_2 = "zzsxdm";
            fieldEdit22.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit22.IsNullable_2 = true;
            fieldEdit22.AliasName_2 = "种植属性代码";
            fieldEdit22.DefaultValue_2 = "";
            fieldEdit22.Editable_2 = true;
            fieldEdit22.Length_2 = 6;
            fields.AddField(field22);

            //种植属性名称字段
            IField field23 = new FieldClass();
            IFieldEdit fieldEdit23 = (IFieldEdit)field23;
            fieldEdit23.Name_2 = "zzsxmc";
            fieldEdit23.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit23.IsNullable_2 = true;
            fieldEdit23.AliasName_2 = "种植属性名称";
            fieldEdit23.DefaultValue_2 = "";
            fieldEdit23.Editable_2 = true;
            fieldEdit23.Length_2 = 10;
            fields.AddField(field23);

            //耕地等别字段
            IField field24 = new FieldClass();
            IFieldEdit fieldEdit24 = (IFieldEdit)field24;
            fieldEdit24.Name_2 = "gddb";
            fieldEdit24.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit24.IsNullable_2 = true;
            fieldEdit24.AliasName_2 = "耕地等别";
            fieldEdit24.DefaultValue_2 = 0;
            fieldEdit24.Editable_2 = true;
            fieldEdit24.Length_2 = 2;
            fields.AddField(field24);

            //飞入地标识字段
            IField field25 = new FieldClass();
            IFieldEdit fieldEdit25 = (IFieldEdit)field25;
            fieldEdit25.Name_2 = "frdbs";
            fieldEdit25.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit25.IsNullable_2 = true;
            fieldEdit25.AliasName_2 = "飞入地标识";
            fieldEdit25.DefaultValue_2 = "";
            fieldEdit25.Editable_2 = true;
            fieldEdit25.Length_2 = 1;
            fields.AddField(field25);

            //城镇村属性码字段
            IField field26 = new FieldClass();
            IFieldEdit fieldEdit26 = (IFieldEdit)field26;
            fieldEdit26.Name_2 = "czcsxm";
            fieldEdit26.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit26.IsNullable_2 = true;
            fieldEdit26.AliasName_2 = "城镇村属性码";
            fieldEdit26.DefaultValue_2 = "";
            fieldEdit26.Editable_2 = true;
            fieldEdit26.Length_2 = 4;
            fields.AddField(field26);

            //数据年份字段
            IField field27 = new FieldClass();
            IFieldEdit fieldEdit27 = (IFieldEdit)field27;
            fieldEdit27.Name_2 = "sjnf";
            fieldEdit27.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit27.IsNullable_2 = true;
            fieldEdit27.AliasName_2 = "数据年份";
            fieldEdit27.DefaultValue_2 = 0;
            fieldEdit27.Editable_2 = true;
            fieldEdit27.Length_2 = 4;
            fields.AddField(field27);

            //国土备注字段
            IField field28 = new FieldClass();
            IFieldEdit fieldEdit28 = (IFieldEdit)field28;
            fieldEdit28.Name_2 = "bz";
            fieldEdit28.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit28.IsNullable_2 = true;
            fieldEdit28.AliasName_2 = "国土备注";
            fieldEdit28.DefaultValue_2 = "";
            fieldEdit28.Editable_2 = true;
            fieldEdit28.Length_2 = 254;
            fields.AddField(field28);

            //省(区/市）字段
            IField field29 = new FieldClass();
            IFieldEdit fieldEdit29 = (IFieldEdit)field29;
            fieldEdit29.Name_2 = "sheng";
            fieldEdit29.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit29.IsNullable_2 = true;
            fieldEdit29.AliasName_2 = "省(区/市)";
            fieldEdit29.DefaultValue_2 = "";
            fieldEdit29.Editable_2 = true;
            fieldEdit29.Length_2 = 2;
            fields.AddField(field29);

            //县(市/旗)字段
            IField field30 = new FieldClass();
            IFieldEdit fieldEdit30 = (IFieldEdit)field30;
            fieldEdit30.Name_2 = "xian";
            fieldEdit30.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit30.IsNullable_2 = true;
            fieldEdit30.AliasName_2 = "县(市/旗)";
            fieldEdit30.DefaultValue_2 = "";
            fieldEdit30.Editable_2 = true;
            fieldEdit30.Length_2 = 6;
            fields.AddField(field30);

            //乡字段
            IField field31 = new FieldClass();
            IFieldEdit fieldEdit31 = (IFieldEdit)field31;
            fieldEdit31.Name_2 = "xiang";
            fieldEdit31.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit31.IsNullable_2 = true;
            fieldEdit31.AliasName_2 = "乡";
            fieldEdit31.DefaultValue_2 = "";
            fieldEdit31.Editable_2 = true;
            fieldEdit31.Length_2 = 9;
            fields.AddField(field31);

            //村字段
            IField field32 = new FieldClass();
            IFieldEdit fieldEdit32 = (IFieldEdit)field32;
            fieldEdit32.Name_2 = "cun";
            fieldEdit32.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit32.IsNullable_2 = true;
            fieldEdit32.AliasName_2 = "村";
            fieldEdit32.DefaultValue_2 = "";
            fieldEdit32.Editable_2 = true;
            fieldEdit32.Length_2 = 12;
            fields.AddField(field32);

            //林业局(牧场)字段
            IField field33 = new FieldClass();
            IFieldEdit fieldEdit33 = (IFieldEdit)field33;
            fieldEdit33.Name_2 = "lin_ye_ju";
            fieldEdit33.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit33.IsNullable_2 = true;
            fieldEdit33.AliasName_2 = "林业局(牧场)";
            fieldEdit33.DefaultValue_2 = "";
            fieldEdit33.Editable_2 = true;
            fieldEdit33.Length_2 = 6;
            fields.AddField(field33);

            //林场字段
            IField field34 = new FieldClass();
            IFieldEdit fieldEdit34 = (IFieldEdit)field34;
            fieldEdit34.Name_2 = "lin_chang";
            fieldEdit34.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit34.IsNullable_2 = true;
            fieldEdit34.AliasName_2 = "林场";
            fieldEdit34.DefaultValue_2 = "";
            fieldEdit34.Editable_2 = true;
            fieldEdit34.Length_2 = 9;
            fields.AddField(field34);

            //林(草）班字段
            IField field35 = new FieldClass();
            IFieldEdit fieldEdit35 = (IFieldEdit)field35;
            fieldEdit35.Name_2 = "lin_ban";
            fieldEdit35.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit35.IsNullable_2 = true;
            fieldEdit35.AliasName_2 = "林(草)班";
            fieldEdit35.DefaultValue_2 = "";
            fieldEdit35.Editable_2 = true;
            fieldEdit35.Length_2 = 4;
            fields.AddField(field35);

            //小班号字段
            IField field36 = new FieldClass();
            IFieldEdit fieldEdit36 = (IFieldEdit)field36;
            fieldEdit36.Name_2 = "xiao_ban";
            fieldEdit36.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit36.IsNullable_2 = true;
            fieldEdit36.AliasName_2 = "小班号";
            fieldEdit36.DefaultValue_2 = "";
            fieldEdit36.Editable_2 = true;
            fieldEdit36.Length_2 = 5;
            fields.AddField(field36);

            //流域字段
            IField field37 = new FieldClass();
            IFieldEdit fieldEdit37 = (IFieldEdit)field37;
            fieldEdit37.Name_2 = "ly";
            fieldEdit37.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit37.IsNullable_2 = true;
            fieldEdit37.AliasName_2 = "流域";
            fieldEdit37.DefaultValue_2 = "";
            fieldEdit37.Editable_2 = true;
            fieldEdit37.Length_2 = 3;
            fields.AddField(field37);

            //生态区位字段
            IField field38 = new FieldClass();
            IFieldEdit fieldEdit38 = (IFieldEdit)field38;
            fieldEdit38.Name_2 = "stqw";
            fieldEdit38.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit38.IsNullable_2 = true;
            fieldEdit38.AliasName_2 = "生态区位";
            fieldEdit38.DefaultValue_2 = "";
            fieldEdit38.Editable_2 = true;
            fieldEdit38.Length_2 = 3;
            fields.AddField(field38);

            //生态区位名称字段
            IField field39 = new FieldClass();
            IFieldEdit fieldEdit39 = (IFieldEdit)field39;
            fieldEdit39.Name_2 = "stqwmc";
            fieldEdit39.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit39.IsNullable_2 = true;
            fieldEdit39.AliasName_2 = "生态区位名称";
            fieldEdit39.DefaultValue_2 = " ";
            fieldEdit39.Editable_2 = true;
            fieldEdit39.Length_2 = 60;
            fields.AddField(field39);

            //地貌字段
            IField field40 = new FieldClass();
            IFieldEdit fieldEdit40 = (IFieldEdit)field40;
            fieldEdit40.Name_2 = "di_mao";
            fieldEdit40.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit40.IsNullable_2 = true;
            fieldEdit40.AliasName_2 = "地貌";
            fieldEdit40.DefaultValue_2 = "";
            fieldEdit40.Editable_2 = true;
            fieldEdit40.Length_2 = 1;
            fields.AddField(field40);

            //海拔字段
            IField field41 = new FieldClass();
            IFieldEdit fieldEdit41 = (IFieldEdit)field41;
            fieldEdit41.Name_2 = "hai_ba";
            fieldEdit41.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit41.IsNullable_2 = true;
            fieldEdit41.AliasName_2 = "海拔";
            fieldEdit41.DefaultValue_2 = 0;
            fieldEdit41.Editable_2 = true;
            fieldEdit41.Length_2 = 5;
            fields.AddField(field41);

            //坡向字段
            IField field42 = new FieldClass();
            IFieldEdit fieldEdit42 = (IFieldEdit)field42;
            fieldEdit42.Name_2 = "po_xiang";
            fieldEdit42.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit42.IsNullable_2 = true;
            fieldEdit42.AliasName_2 = "坡向";
            fieldEdit42.DefaultValue_2 = "";
            fieldEdit42.Editable_2 = true;
            fieldEdit42.Length_2 = 1;
            fields.AddField(field42);

            //坡位字段
            IField field43 = new FieldClass();
            IFieldEdit fieldEdit43 = (IFieldEdit)field43;
            fieldEdit43.Name_2 = "po_wei";
            fieldEdit43.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit43.IsNullable_2 = true;
            fieldEdit43.AliasName_2 = "坡位";
            fieldEdit43.DefaultValue_2 = "";
            fieldEdit43.Editable_2 = true;
            fieldEdit43.Length_2 = 1;
            fields.AddField(field43);

            //坡度字段
            IField field44 = new FieldClass();
            IFieldEdit fieldEdit44 = (IFieldEdit)field44;
            fieldEdit44.Name_2 = "po_du";
            fieldEdit44.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit44.IsNullable_2 = true;
            fieldEdit44.AliasName_2 = "坡度";
            fieldEdit44.DefaultValue_2 = 0;
            fieldEdit44.Editable_2 = true;
            fieldEdit44.Length_2 = 2;
            fields.AddField(field44);

            //土壤类型字段
            IField field45 = new FieldClass();
            IFieldEdit fieldEdit45 = (IFieldEdit)field45;
            fieldEdit45.Name_2 = "tu_rang_lx";
            fieldEdit45.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit45.IsNullable_2 = true;
            fieldEdit45.AliasName_2 = "土壤类型";
            fieldEdit45.DefaultValue_2 = "";
            fieldEdit45.Editable_2 = true;
            fieldEdit45.Length_2 = 3;
            fields.AddField(field45);

            //图层厚度字段
            IField field46 = new FieldClass();
            IFieldEdit fieldEdit46 = (IFieldEdit)field46;
            fieldEdit46.Name_2 = "tu_ceng_hd";
            fieldEdit46.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit46.IsNullable_2 = true;
            fieldEdit46.AliasName_2 = "图层厚度";
            fieldEdit46.DefaultValue_2 = 0;
            fieldEdit46.Editable_2 = true;
            fieldEdit46.Length_2 = 3;
            fields.AddField(field46);

            //土壤质地字段
            IField field47 = new FieldClass();
            IFieldEdit fieldEdit47 = (IFieldEdit)field47;
            fieldEdit47.Name_2 = "tu_rang_zd";
            fieldEdit47.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit47.IsNullable_2 = true;
            fieldEdit47.AliasName_2 = "土壤质地";
            fieldEdit47.DefaultValue_2 = "";
            fieldEdit47.Editable_2 = true;
            fieldEdit47.Length_2 = 3;
            fields.AddField(field47);

            //土地所有权属字段
            IField field48 = new FieldClass();
            IFieldEdit fieldEdit48 = (IFieldEdit)field48;
            fieldEdit48.Name_2 = "ld_qs";
            fieldEdit48.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit48.IsNullable_2 = true;
            fieldEdit48.AliasName_2 = "土地所有权属";
            fieldEdit48.DefaultValue_2 = "";
            fieldEdit48.Editable_2 = true;
            fieldEdit48.Length_2 = 2;
            fields.AddField(field48);

            //土地使用权属字段
            IField field49 = new FieldClass();
            IFieldEdit fieldEdit49 = (IFieldEdit)field49;
            fieldEdit49.Name_2 = "tdsyqs";
            fieldEdit49.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit49.IsNullable_2 = true;
            fieldEdit49.AliasName_2 = "土地使用权属";
            fieldEdit49.DefaultValue_2 = "";
            fieldEdit49.Editable_2 = true;
            fieldEdit49.Length_2 = 2;
            fields.AddField(field49);

            //小班面积字段
            IField field50 = new FieldClass();
            IFieldEdit fieldEdit50 = (IFieldEdit)field50;
            fieldEdit50.Name_2 = "xbmj";
            fieldEdit50.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit50.IsNullable_2 = true;
            fieldEdit50.AliasName_2 = "小班面积";
            fieldEdit50.DefaultValue_2 = 0;
            fieldEdit50.Editable_2 = true;
            fieldEdit50.Precision_2 = 18;
            fieldEdit50.Scale_2 = 2;
            fields.AddField(field50);

            //地类字段
            IField field51 = new FieldClass();
            IFieldEdit fieldEdit51 = (IFieldEdit)field51;
            fieldEdit51.Name_2 = "di_lei";
            fieldEdit51.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit51.IsNullable_2 = true;
            fieldEdit51.AliasName_2 = "地类";
            fieldEdit51.DefaultValue_2 = "";
            fieldEdit51.Editable_2 = true;
            fieldEdit51.Length_2 = 6;
            fields.AddField(field51);

            //植被覆盖类型字段
            IField field52 = new FieldClass();
            IFieldEdit fieldEdit52 = (IFieldEdit)field52;
            fieldEdit52.Name_2 = "zbfglx";
            fieldEdit52.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit52.IsNullable_2 = true;
            fieldEdit52.AliasName_2 = "植被覆盖类型";
            fieldEdit52.DefaultValue_2 = "";
            fieldEdit52.Editable_2 = true;
            fieldEdit52.Length_2 = 5;
            fields.AddField(field52);

            //植被结构字段
            IField field53 = new FieldClass();
            IFieldEdit fieldEdit53 = (IFieldEdit)field53;
            fieldEdit53.Name_2 = "zb_jg";
            fieldEdit53.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit53.IsNullable_2 = true;
            fieldEdit53.AliasName_2 = "植被结构";
            fieldEdit53.DefaultValue_2 = "";
            fieldEdit53.Editable_2 = true;
            fieldEdit53.Length_2 = 1;
            fields.AddField(field53);

            //植被总盖度字段
            IField field54 = new FieldClass();
            IFieldEdit fieldEdit54 = (IFieldEdit)field54;
            fieldEdit54.Name_2 = "zbgd";
            fieldEdit54.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit54.IsNullable_2 = true;
            fieldEdit54.AliasName_2 = "植被总盖度";
            fieldEdit54.DefaultValue_2 = 0;
            fieldEdit54.Editable_2 = true;
            fieldEdit54.Length_2 = 3;
            fields.AddField(field54);

            //郁闭度字段
            IField field55 = new FieldClass();
            IFieldEdit fieldEdit55 = (IFieldEdit)field55;
            fieldEdit55.Name_2 = "yu_bi_du";
            fieldEdit55.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit55.IsNullable_2 = true;
            fieldEdit55.AliasName_2 = "郁闭度";
            fieldEdit55.DefaultValue_2 = 0;
            fieldEdit55.Editable_2 = true;
            fieldEdit55.Precision_2 = 6;
            fieldEdit55.Scale_2 = 2;
            fields.AddField(field55);

            //灌木盖度字段
            IField field56 = new FieldClass();
            IFieldEdit fieldEdit56 = (IFieldEdit)field56;
            fieldEdit56.Name_2 = "gmgd";
            fieldEdit56.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit56.IsNullable_2 = true;
            fieldEdit56.AliasName_2 = "灌木盖度";
            fieldEdit56.DefaultValue_2 = 0;
            fieldEdit56.Editable_2 = true;
            fieldEdit56.Length_2 = 3;
            fields.AddField(field56);

            //草本盖度字段
            IField field57 = new FieldClass();
            IFieldEdit fieldEdit57 = (IFieldEdit)field57;
            fieldEdit57.Name_2 = "cbgd";
            fieldEdit57.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit57.IsNullable_2 = true;
            fieldEdit57.AliasName_2 = "草本盖度";
            fieldEdit57.DefaultValue_2 = 0;
            fieldEdit57.Editable_2 = true;
            fieldEdit57.Length_2 = 3;
            fields.AddField(field57);

            //优势树(灌)种字段
            IField field58 = new FieldClass();
            IFieldEdit fieldEdit58 = (IFieldEdit)field58;
            fieldEdit58.Name_2 = "you_shi_sz";
            fieldEdit58.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit58.IsNullable_2 = true;
            fieldEdit58.AliasName_2 = "优势树(灌)种";
            fieldEdit58.DefaultValue_2 = "";
            fieldEdit58.Editable_2 = true;
            fieldEdit58.Length_2 = 6;
            fields.AddField(field58);

            //起源字段
            IField field59 = new FieldClass();
            IFieldEdit fieldEdit59 = (IFieldEdit)field59;
            fieldEdit59.Name_2 = "qi_yuan";
            fieldEdit59.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit59.IsNullable_2 = true;
            fieldEdit59.AliasName_2 = "起源";
            fieldEdit59.DefaultValue_2 = "";
            fieldEdit59.Editable_2 = true;
            fieldEdit59.Length_2 = 2;
            fields.AddField(field59);

            //平均年龄字段
            IField field60 = new FieldClass();
            IFieldEdit fieldEdit60 = (IFieldEdit)field60;
            fieldEdit60.Name_2 = "pingjun_nl";
            fieldEdit60.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit60.IsNullable_2 = true;
            fieldEdit60.AliasName_2 = "平均年龄";
            fieldEdit60.DefaultValue_2 = 0;
            fieldEdit60.Editable_2 = true;
            fieldEdit60.Length_2 = 4;
            fields.AddField(field60);

            //龄组字段
            IField field61 = new FieldClass();
            IFieldEdit fieldEdit61 = (IFieldEdit)field61;
            fieldEdit61.Name_2 = "ling_zu";
            fieldEdit61.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit61.IsNullable_2 = true;
            fieldEdit61.AliasName_2 = "龄组";
            fieldEdit61.DefaultValue_2 = "";
            fieldEdit61.Editable_2 = true;
            fieldEdit61.Length_2 = 1;
            fields.AddField(field61);

            //平均胸径字段
            IField field62 = new FieldClass();
            IFieldEdit fieldEdit62 = (IFieldEdit)field62;
            fieldEdit62.Name_2 = "pingjun_xj";
            fieldEdit62.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit62.IsNullable_2 = true;
            fieldEdit62.AliasName_2 = "平均胸径";
            fieldEdit62.DefaultValue_2 = 0;
            fieldEdit62.Editable_2 = true;
            fieldEdit62.Precision_2 = 6;
            fieldEdit62.Scale_2 = 1;
            fields.AddField(field62);

            //平均树高字段
            IField field63 = new FieldClass();
            IFieldEdit fieldEdit63 = (IFieldEdit)field63;
            fieldEdit63.Name_2 = "pingjun_sg";
            fieldEdit63.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit63.IsNullable_2 = true;
            fieldEdit63.AliasName_2 = "平均树高";
            fieldEdit63.DefaultValue_2 = 0;
            fieldEdit63.Editable_2 = true;
            fieldEdit63.Precision_2 = 6;
            fieldEdit63.Scale_2 = 1;
            fields.AddField(field63);

            //每公顷株树字段
            IField field64 = new FieldClass();
            IFieldEdit fieldEdit64 = (IFieldEdit)field64;
            fieldEdit64.Name_2 = "mei_gq_zs";
            fieldEdit64.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit64.IsNullable_2 = true;
            fieldEdit64.AliasName_2 = "每公顷株树";
            fieldEdit64.DefaultValue_2 = 0;
            fieldEdit64.Editable_2 = true;
            fieldEdit64.Length_2 = 6;
            fields.AddField(field64);

            //每公顷蓄积字段
            IField field65 = new FieldClass();
            IFieldEdit fieldEdit65 = (IFieldEdit)field65;
            fieldEdit65.Name_2 = "mei_gq_xj";
            fieldEdit65.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit65.IsNullable_2 = true;
            fieldEdit65.AliasName_2 = "每公顷蓄积";
            fieldEdit65.DefaultValue_2 = 0;
            fieldEdit65.Editable_2 = true;
            fieldEdit65.Precision_2 =12;
            fieldEdit65.Scale_2 = 2;
            fields.AddField(field65);

            //蓄积量字段
            IField field66 = new FieldClass();
            IFieldEdit fieldEdit66 = (IFieldEdit)field66;
            fieldEdit66.Name_2 = "huo_lm_xj";
            fieldEdit66.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit66.IsNullable_2 = true;
            fieldEdit66.AliasName_2 = "蓄积量";
            fieldEdit66.DefaultValue_2 = 0;
            fieldEdit66.Editable_2 = true;
            fieldEdit66.Length_2 = 12;
            fields.AddField(field66);

            //生物量字段
            IField field67 = new FieldClass();
            IFieldEdit fieldEdit67 = (IFieldEdit)field67;
            fieldEdit67.Name_2 = "sheng_wu_l";
            fieldEdit67.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit67.IsNullable_2 = true;
            fieldEdit67.AliasName_2 = "生物量";
            fieldEdit67.DefaultValue_2 = 0;
            fieldEdit67.Editable_2 = true;
            fieldEdit67.Length_2 = 12;
            fields.AddField(field67);

            //碳储量字段
            IField field68 = new FieldClass();
            IFieldEdit fieldEdit68 = (IFieldEdit)field68;
            fieldEdit68.Name_2 = "tan_chu_l";
            fieldEdit68.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit68.IsNullable_2 = true;
            fieldEdit68.AliasName_2 = "碳储量";
            fieldEdit68.DefaultValue_2 = 0;
            fieldEdit68.Editable_2 = true;
            fieldEdit68.Length_2 = 12;
            fields.AddField(field68);

            //林木所有权属字段
            IField field69 = new FieldClass();
            IFieldEdit fieldEdit69 = (IFieldEdit)field69;
            fieldEdit69.Name_2 = "lmqs";
            fieldEdit69.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit69.IsNullable_2 = true;
            fieldEdit69.AliasName_2 = "林木所有权属";
            fieldEdit69.DefaultValue_2 = "";
            fieldEdit69.Editable_2 = true;
            fieldEdit69.Length_2 = 2;
            fields.AddField(field69);

            //林木使用权属字段
            IField field70 = new FieldClass();
            IFieldEdit fieldEdit70 = (IFieldEdit)field70;
            fieldEdit70.Name_2 = "lmsyqs";
            fieldEdit70.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit70.IsNullable_2 = true;
            fieldEdit70.AliasName_2 = "林木使用权属";
            fieldEdit70.DefaultValue_2 = "";
            fieldEdit70.Editable_2 = true;
            fieldEdit70.Length_2 = 2;
            fields.AddField(field70);

            //林种字段
            IField field71 = new FieldClass();
            IFieldEdit fieldEdit71 = (IFieldEdit)field71;
            fieldEdit71.Name_2 = "lin_zhong";
            fieldEdit71.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit71.IsNullable_2 = true;
            fieldEdit71.AliasName_2 = "林种";
            fieldEdit71.DefaultValue_2 = "";
            fieldEdit71.Editable_2 = true;
            fieldEdit71.Length_2 = 3;
            fields.AddField(field71);

            //树种组成字段
            IField field72 = new FieldClass();
            IFieldEdit fieldEdit72 = (IFieldEdit)field72;
            fieldEdit72.Name_2 = "zhuzhong_zc";
            fieldEdit72.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit72.IsNullable_2 = true;
            fieldEdit72.AliasName_2 = "树种组成";
            fieldEdit72.DefaultValue_2 = "";
            fieldEdit72.Editable_2 = true;
            fieldEdit72.Length_2 = 16;
            fields.AddField(field72);

            //事权等级字段
            IField field73 = new FieldClass();
            IFieldEdit fieldEdit73 = (IFieldEdit)field73;
            fieldEdit73.Name_2 = "shi_quan_d";
            fieldEdit73.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit73.IsNullable_2 = true;
            fieldEdit73.AliasName_2 = "事权等级";
            fieldEdit73.DefaultValue_2 = "";
            fieldEdit73.Editable_2 = true;
            fieldEdit73.Length_2 = 2;
            fields.AddField(field73);

            //公益林变化类型字段
            IField field74 = new FieldClass();
            IFieldEdit fieldEdit74 = (IFieldEdit)field74;
            fieldEdit74.Name_2 = "gyl_bhlx";
            fieldEdit74.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit74.IsNullable_2 = true;
            fieldEdit74.AliasName_2 = "公益林变化类型";
            fieldEdit74.DefaultValue_2 = "";
            fieldEdit74.Editable_2 = true;
            fieldEdit74.Length_2 = 2;
            fields.AddField(field74);

            //林地管理类型字段
            IField field75 = new FieldClass();
            IFieldEdit fieldEdit75 = (IFieldEdit)field75;
            fieldEdit75.Name_2 = "ldgl_lx";
            fieldEdit75.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit75.IsNullable_2 = true;
            fieldEdit75.AliasName_2 = "林地管理类型";
            fieldEdit75.DefaultValue_2 = "";
            fieldEdit75.Editable_2 = true;
            fieldEdit75.Length_2 = 2;
            fields.AddField(field75);

            //林地保护等级字段
            IField field76 = new FieldClass();
            IFieldEdit fieldEdit76 = (IFieldEdit)field76;
            fieldEdit76.Name_2 = "bh_dj";
            fieldEdit76.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit76.IsNullable_2 = true;
            fieldEdit76.AliasName_2 = "林地保护等级";
            fieldEdit76.DefaultValue_2 = "";
            fieldEdit76.Editable_2 = true;
            fieldEdit76.Length_2 = 1;
            fields.AddField(field76);

            //林地质量等级字段
            IField field77 = new FieldClass();
            IFieldEdit fieldEdit77 = (IFieldEdit)field77;
            fieldEdit77.Name_2 = "zl_dj";
            fieldEdit77.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit77.IsNullable_2 = true;
            fieldEdit77.AliasName_2 = "林地质量等级";
            fieldEdit77.DefaultValue_2 = "";
            fieldEdit77.Editable_2 = true;
            fieldEdit77.Length_2 = 1;
            fields.AddField(field77);

            //草原分区字段
            IField field78 = new FieldClass();
            IFieldEdit fieldEdit78 = (IFieldEdit)field78;
            fieldEdit78.Name_2 = "cy_fq";
            fieldEdit78.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit78.IsNullable_2 = true;
            fieldEdit78.AliasName_2 = "草原分区";
            fieldEdit78.DefaultValue_2 = "";
            fieldEdit78.Editable_2 = true;
            fieldEdit78.Length_2 = 2;
            fields.AddField(field78);

            //草原起源字段
            IField field79 = new FieldClass();
            IFieldEdit fieldEdit79 = (IFieldEdit)field79;
            fieldEdit79.Name_2 = "cd_qy";
            fieldEdit79.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit79.IsNullable_2 = true;
            fieldEdit79.AliasName_2 = "草原起源";
            fieldEdit79.DefaultValue_2 = "";
            fieldEdit79.Editable_2 = true;
            fieldEdit79.Length_2 = 2;
            fields.AddField(field79);

            //草原类字段
            IField field80 = new FieldClass();
            IFieldEdit fieldEdit80 = (IFieldEdit)field80;
            fieldEdit80.Name_2 = "cd_l";
            fieldEdit80.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit80.IsNullable_2 = true;
            fieldEdit80.AliasName_2 = "草原类";
            fieldEdit80.DefaultValue_2 = "";
            fieldEdit80.Editable_2 = true;
            fieldEdit80.Length_2 = 2;
            fields.AddField(field80);

            //草原型字段
            IField field81 = new FieldClass();
            IFieldEdit fieldEdit81 = (IFieldEdit)field81;
            fieldEdit81.Name_2 = "cd_xing";
            fieldEdit81.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit81.IsNullable_2 = true;
            fieldEdit81.AliasName_2 = "草原型";
            fieldEdit81.DefaultValue_2 = "";
            fieldEdit81.Editable_2 = true;
            fieldEdit81.Length_2 = 3;
            fields.AddField(field81);

            //优势草种字段
            IField field82 = new FieldClass();
            IFieldEdit fieldEdit82 = (IFieldEdit)field82;
            fieldEdit82.Name_2 = "ys_caoz";
            fieldEdit82.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit82.IsNullable_2 = true;
            fieldEdit82.AliasName_2 = "优势草种";
            fieldEdit82.DefaultValue_2 = "";
            fieldEdit82.Editable_2 = true;
            fieldEdit82.Length_2 = 20;
            fields.AddField(field82);

            //功能类别字段
            IField field83 = new FieldClass();
            IFieldEdit fieldEdit83 = (IFieldEdit)field83;
            fieldEdit83.Name_2 = "cdgn";
            fieldEdit83.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit83.IsNullable_2 = true;
            fieldEdit83.AliasName_2 = "功能类别";
            fieldEdit83.DefaultValue_2 = "";
            fieldEdit83.Editable_2 = true;
            fieldEdit83.Length_2 = 2;
            fields.AddField(field83);

            //单位面积鲜草产量字段
            IField field84 = new FieldClass();
            IFieldEdit fieldEdit84 = (IFieldEdit)field84;
            fieldEdit84.Name_2 = "xc_cl";
            fieldEdit84.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit84.IsNullable_2 = true;
            fieldEdit84.AliasName_2 = "单位面积鲜草产量";
            fieldEdit84.DefaultValue_2 = 0;
            fieldEdit84.Editable_2 = true;
            fieldEdit84.Precision_2 = 8;
            fieldEdit84.Scale_2 = 1;
            fields.AddField(field84);

            //小班鲜草产量字段
            IField field85 = new FieldClass();
            IFieldEdit fieldEdit85 = (IFieldEdit)field85;
            fieldEdit85.Name_2 = "xb_xccl";
            fieldEdit85.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit85.IsNullable_2 = true;
            fieldEdit85.AliasName_2 = "小班鲜草产量";
            fieldEdit85.DefaultValue_2 = 0;
            fieldEdit85.Editable_2 = true;
            fieldEdit85.Precision_2 = 8;
            fieldEdit85.Scale_2 = 1;
            fields.AddField(field85);

            //可食牧草比例字段
            IField field86 = new FieldClass();
            IFieldEdit fieldEdit86 = (IFieldEdit)field86;
            fieldEdit86.Name_2 = "ksmcbl";
            fieldEdit86.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit86.IsNullable_2 = true;
            fieldEdit86.AliasName_2 = "可食牧草比例";
            fieldEdit86.DefaultValue_2 = 0;
            fieldEdit86.Editable_2 = true;
            fieldEdit86.Precision_2 = 8;
            fieldEdit86.Scale_2 = 1;
            fields.AddField(field86);

            //小班干草产量字段
            IField field87 = new FieldClass();
            IFieldEdit fieldEdit87 = (IFieldEdit)field87;
            fieldEdit87.Name_2 = "xb_gccl";
            fieldEdit87.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit87.IsNullable_2 = true;
            fieldEdit87.AliasName_2 = "小班干草产量";
            fieldEdit87.DefaultValue_2 = 0;
            fieldEdit87.Editable_2 = true;
            fieldEdit87.Precision_2 = 8;
            fieldEdit87.Scale_2 = 1;
            fields.AddField(field87);

            //小班可食干牧草产量字段
            IField field88 = new FieldClass();
            IFieldEdit fieldEdit88 = (IFieldEdit)field88;
            fieldEdit88.Name_2 = "xb_ksgccl";
            fieldEdit88.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit88.IsNullable_2 = true;
            fieldEdit88.AliasName_2 = "小班可食干牧草产量";
            fieldEdit88.DefaultValue_2 = 0;
            fieldEdit88.Editable_2 = true;
            fieldEdit88.Precision_2 = 8;
            fieldEdit88.Scale_2 = 1;
            fields.AddField(field88);

            //小班可食鲜牧草产量字段
            IField field89 = new FieldClass();
            IFieldEdit fieldEdit89 = (IFieldEdit)field89;
            fieldEdit89.Name_2 = "xb_ksxccl";
            fieldEdit89.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit89.IsNullable_2 = true;
            fieldEdit89.AliasName_2 = "小班可食鲜牧草产量";
            fieldEdit89.DefaultValue_2 = 0;
            fieldEdit89.Editable_2 = true;
            fieldEdit89.Precision_2 = 8;
            fieldEdit89.Scale_2 = 1;
            fields.AddField(field89);

            //利用方式字段
            IField field90 = new FieldClass();
            IFieldEdit fieldEdit90 = (IFieldEdit)field90;
            fieldEdit90.Name_2 = "cdlyfs";
            fieldEdit90.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit90.IsNullable_2 = true;
            fieldEdit90.AliasName_2 = "利用方式";
            fieldEdit90.DefaultValue_2 = "";
            fieldEdit90.Editable_2 = true;
            fieldEdit90.Length_2 = 2;
            fields.AddField(field90);

            //放牧时长字段
            IField field91 = new FieldClass();
            IFieldEdit fieldEdit91 = (IFieldEdit)field91;
            fieldEdit91.Name_2 = "fm_shch";
            fieldEdit91.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit91.IsNullable_2 = true;
            fieldEdit91.AliasName_2 = "放牧时长";
            fieldEdit91.DefaultValue_2 = 0;
            fieldEdit91.Editable_2 = true;
            fieldEdit91.Length_2 = 2;
            fields.AddField(field91);

            //草原类别字段
            IField field92 = new FieldClass();
            IFieldEdit fieldEdit92 = (IFieldEdit)field92;
            fieldEdit92.Name_2 = "cylb";
            fieldEdit92.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit92.IsNullable_2 = true;
            fieldEdit92.AliasName_2 = "草原类别";
            fieldEdit92.DefaultValue_2 = "";
            fieldEdit92.Editable_2 = true;
            fieldEdit92.Length_2 = 1;
            fields.AddField(field92);

            //裸斑面积比例字段
            IField field93 = new FieldClass();
            IFieldEdit fieldEdit93 = (IFieldEdit)field93;
            fieldEdit93.Name_2 = "lbmj_bl";
            fieldEdit93.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit93.IsNullable_2 = true;
            fieldEdit93.AliasName_2 = "裸斑面积比例";
            fieldEdit93.DefaultValue_2 = 0;
            fieldEdit93.Editable_2 = true;
            fieldEdit93.Length_2 = 2;
            fields.AddField(field93);

            //草原退化类型字段
            IField field94 = new FieldClass();
            IFieldEdit fieldEdit94 = (IFieldEdit)field94;
            fieldEdit94.Name_2 = "thlx";
            fieldEdit94.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit94.IsNullable_2 = true;
            fieldEdit94.AliasName_2 = "草原退化类型";
            fieldEdit94.DefaultValue_2 = "";
            fieldEdit94.Editable_2 = true;
            fieldEdit94.Length_2 = 1;
            fields.AddField(field94);

            //草原退化程度字段
            IField field95 = new FieldClass();
            IFieldEdit fieldEdit95 = (IFieldEdit)field95;
            fieldEdit95.Name_2 = "thcd";
            fieldEdit95.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit95.IsNullable_2 = true;
            fieldEdit95.AliasName_2 = "草原退化程度";
            fieldEdit95.DefaultValue_2 = "";
            fieldEdit95.Editable_2 = true;
            fieldEdit95.Length_2 = 1;
            fields.AddField(field95);

            //禁牧与草蓄平衡字段
            IField field96 = new FieldClass();
            IFieldEdit fieldEdit96 = (IFieldEdit)field96;
            fieldEdit96.Name_2 = "jm_ph";
            fieldEdit96.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit96.IsNullable_2 = true;
            fieldEdit96.AliasName_2 = "禁牧与草蓄平衡";
            fieldEdit96.DefaultValue_2 = "";
            fieldEdit96.Editable_2 = true;
            fieldEdit96.Length_2 = 1;
            fields.AddField(field96);

            //草原管理类型字段
            IField field97 = new FieldClass();
            IFieldEdit fieldEdit97 = (IFieldEdit)field97;
            fieldEdit97.Name_2 = "cdgl_lx";
            fieldEdit97.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit97.IsNullable_2 = true;
            fieldEdit97.AliasName_2 = "草原管理类型";
            fieldEdit97.DefaultValue_2 = "";
            fieldEdit97.Editable_2 = true;
            fieldEdit97.Length_2 = 2;
            fields.AddField(field97);

            //湿地管理分级字段
            IField field98 = new FieldClass();
            IFieldEdit fieldEdit98 = (IFieldEdit)field98;
            fieldEdit98.Name_2 = "sd_dj";
            fieldEdit98.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit98.IsNullable_2 = true;
            fieldEdit98.AliasName_2 = "湿地管理分级";
            fieldEdit98.DefaultValue_2 = "";
            fieldEdit98.Editable_2 = true;
            fieldEdit98.Length_2 = 6;
            fields.AddField(field98);

            //重要湿地名称字段
            IField field99 = new FieldClass();
            IFieldEdit fieldEdit99 = (IFieldEdit)field99;
            fieldEdit99.Name_2 = "sysd_bm";
            fieldEdit99.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit99.IsNullable_2 = true;
            fieldEdit99.AliasName_2 = "重要湿地名称";
            fieldEdit99.DefaultValue_2 = "";
            fieldEdit99.Editable_2 = true;
            fieldEdit99.Length_2 = 50;
            fields.AddField(field99);

            //湿地保护形式字段
            IField field100 = new FieldClass();
            IFieldEdit fieldEdit100 = (IFieldEdit)field100;
            fieldEdit100.Name_2 = "bhdsx";
            fieldEdit100.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit100.IsNullable_2 = true;
            fieldEdit100.AliasName_2 = "湿地保护形式";
            fieldEdit100.DefaultValue_2 = "";
            fieldEdit100.Editable_2 = true;
            fieldEdit100.Length_2 = 2;
            fields.AddField(field100);

            //自然保护地字段
            IField field101 = new FieldClass();
            IFieldEdit fieldEdit101 = (IFieldEdit)field101;
            fieldEdit101.Name_2 = "bhddm";
            fieldEdit101.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit101.IsNullable_2 = true;
            fieldEdit101.AliasName_2 = "自然保护地";
            fieldEdit101.DefaultValue_2 = "";
            fieldEdit101.Editable_2 = true;
            fieldEdit101.Length_2 = 50;
            fields.AddField(field101);

            //湿地利用方式字段
            IField field102 = new FieldClass();
            IFieldEdit fieldEdit102 = (IFieldEdit)field102;
            fieldEdit102.Name_2 = "sdlyfs";
            fieldEdit102.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit102.IsNullable_2 = true;
            fieldEdit102.AliasName_2 = "湿地利用方式";
            fieldEdit102.DefaultValue_2 = "";
            fieldEdit102.Editable_2 = true;
            fieldEdit102.Length_2 = 2;
            fields.AddField(field102);

            //受威胁状况字段
            IField field103 = new FieldClass();
            IFieldEdit fieldEdit103 = (IFieldEdit)field103;
            fieldEdit103.Name_2 = "sdwxzk";
            fieldEdit103.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit103.IsNullable_2 = true;
            fieldEdit103.AliasName_2 = "受威胁状况";
            fieldEdit103.DefaultValue_2 = "";
            fieldEdit103.Editable_2 = true;
            fieldEdit103.Length_2 = 4;
            fields.AddField(field103);

            //湿地管理类型字段
            IField field104 = new FieldClass();
            IFieldEdit fieldEdit104 = (IFieldEdit)field104;
            fieldEdit104.Name_2 = "sdgllx";
            fieldEdit104.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit104.IsNullable_2 = true;
            fieldEdit104.AliasName_2 = "湿地管理类型";
            fieldEdit104.DefaultValue_2 = "";
            fieldEdit104.Editable_2 = true;
            fieldEdit104.Length_2 = 2;
            fields.AddField(field104);

            //荒漠调查区类型字段
            IField field105 = new FieldClass();
            IFieldEdit fieldEdit105 = (IFieldEdit)field105;
            fieldEdit105.Name_2 = "dcqlx";
            fieldEdit105.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit105.IsNullable_2 = true;
            fieldEdit105.AliasName_2 = "荒漠调查区类型";
            fieldEdit105.DefaultValue_2 = "";
            fieldEdit105.Editable_2 = true;
            fieldEdit105.Length_2 = 1;
            fields.AddField(field105);

            //气候类型字段
            IField field106 = new FieldClass();
            IFieldEdit fieldEdit106 = (IFieldEdit)field106;
            fieldEdit106.Name_2 = "qhlx";
            fieldEdit106.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit106.IsNullable_2 = true;
            fieldEdit106.AliasName_2 = "气候类型";
            fieldEdit106.DefaultValue_2 = "";
            fieldEdit106.Editable_2 = true;
            fieldEdit106.Length_2 = 1;
            fields.AddField(field106);

            //沙化类型字段
            IField field107 = new FieldClass();
            IFieldEdit fieldEdit107 = (IFieldEdit)field107;
            fieldEdit107.Name_2 = "shlx";
            fieldEdit107.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit107.IsNullable_2 = true;
            fieldEdit107.AliasName_2 = "沙化类型";
            fieldEdit107.DefaultValue_2 = "";
            fieldEdit107.Editable_2 = true;
            fieldEdit107.Length_2 = 3;
            fields.AddField(field107);

            //沙化程度字段
            IField field108 = new FieldClass();
            IFieldEdit fieldEdit108 = (IFieldEdit)field108;
            fieldEdit108.Name_2 = "shcd";
            fieldEdit108.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit108.IsNullable_2 = true;
            fieldEdit108.AliasName_2 = "沙化程度";
            fieldEdit108.DefaultValue_2 = "";
            fieldEdit108.Editable_2 = true;
            fieldEdit108.Length_2 = 1;
            fields.AddField(field108);

            //所属沙漠沙地字段
            IField field109 = new FieldClass();
            IFieldEdit fieldEdit109 = (IFieldEdit)field109;
            fieldEdit109.Name_2 = "sssmsd";
            fieldEdit109.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit109.IsNullable_2 = true;
            fieldEdit109.AliasName_2 = "所属沙漠沙地";
            fieldEdit109.DefaultValue_2 = "";
            fieldEdit109.Editable_2 = true;
            fieldEdit109.Length_2 = 2;
            fields.AddField(field109);

            //荒漠化类型字段
            IField field110 = new FieldClass();
            IFieldEdit fieldEdit110 = (IFieldEdit)field110;
            fieldEdit110.Name_2 = "hmhlx";
            fieldEdit110.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit110.IsNullable_2 = true;
            fieldEdit110.AliasName_2 = "荒漠化类型";
            fieldEdit110.DefaultValue_2 = "";
            fieldEdit110.Editable_2 = true;
            fieldEdit110.Length_2 = 1;
            fields.AddField(field110);

            //荒漠化程度字段
            IField field111 = new FieldClass();
            IFieldEdit fieldEdit111 = (IFieldEdit)field111;
            fieldEdit111.Name_2 = "hmhcd";
            fieldEdit111.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit111.IsNullable_2 = true;
            fieldEdit111.AliasName_2 = "荒漠化程度";
            fieldEdit111.DefaultValue_2 = "";
            fieldEdit111.Editable_2 = true;
            fieldEdit111.Length_2 = 1;
            fields.AddField(field111);

            //基岩裸露度/土壤砾石含量字段
            IField field112 = new FieldClass();
            IFieldEdit fieldEdit112 = (IFieldEdit)field112;
            fieldEdit112.Name_2 = "trls";
            fieldEdit112.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit112.IsNullable_2 = true;
            fieldEdit112.AliasName_2 = "基岩裸露度/土壤砾石含量";
            fieldEdit112.DefaultValue_2 = 0;
            fieldEdit112.Editable_2 = true;
            fieldEdit112.Precision_2 = 5;
            fieldEdit112.Scale_2 = 1;
            fields.AddField(field112);

            //覆沙厚度字段
            IField field113 = new FieldClass();
            IFieldEdit fieldEdit113 = (IFieldEdit)field113;
            fieldEdit113.Name_2 = "fshd";
            fieldEdit113.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit113.IsNullable_2 = true;
            fieldEdit113.AliasName_2 = "覆沙厚度";
            fieldEdit113.DefaultValue_2 = 0;
            fieldEdit113.Editable_2 = true;
            fieldEdit113.Precision_2 = 5;
            fieldEdit113.Scale_2 = 1;
            fields.AddField(field113);

            //沙丘高度字段
            IField field114 = new FieldClass();
            IFieldEdit fieldEdit114 = (IFieldEdit)field114;
            fieldEdit114.Name_2 = "sqgd";
            fieldEdit114.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit114.IsNullable_2 = true;
            fieldEdit114.AliasName_2 = "沙丘高度";
            fieldEdit114.DefaultValue_2 = 0;
            fieldEdit114.Editable_2 = true;
            fieldEdit114.Precision_2 = 5;
            fieldEdit114.Scale_2 = 1;
            fields.AddField(field114);

            //侵蚀沟面积比例字段
            IField field115 = new FieldClass();
            IFieldEdit fieldEdit115 = (IFieldEdit)field115;
            fieldEdit115.Name_2 = "qsg";
            fieldEdit115.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit115.IsNullable_2 = true;
            fieldEdit115.AliasName_2 = "侵蚀沟面积比例";
            fieldEdit115.DefaultValue_2 = 0;
            fieldEdit115.Editable_2 = true;
            fieldEdit115.Precision_2 = 5;
            fieldEdit115.Scale_2 = 1;
            fields.AddField(field115);

            //盐碱斑占地率字段
            IField field116 = new FieldClass();
            IFieldEdit fieldEdit116 = (IFieldEdit)field116;
            fieldEdit116.Name_2 = "yjb";
            fieldEdit116.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit116.IsNullable_2 = true;
            fieldEdit116.AliasName_2 = "盐碱斑占地率";
            fieldEdit116.DefaultValue_2 = 0;
            fieldEdit116.Editable_2 = true;
            fieldEdit116.Precision_2 = 5;
            fieldEdit116.Scale_2 = 1;
            fields.AddField(field116);

            //作物产量下降率字段
            IField field117 = new FieldClass();
            IFieldEdit fieldEdit117 = (IFieldEdit)field117;
            fieldEdit117.Name_2 = "zwclxjl";
            fieldEdit117.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit117.IsNullable_2 = true;
            fieldEdit117.AliasName_2 = "作物产量下降率";
            fieldEdit117.DefaultValue_2 = 0;
            fieldEdit117.Editable_2 = true;
            fieldEdit117.Precision_2 = 5;
            fieldEdit117.Scale_2 = 1;
            fields.AddField(field117);

            //作物缺苗率字段
            IField field118 = new FieldClass();
            IFieldEdit fieldEdit118 = (IFieldEdit)field118;
            fieldEdit118.Name_2 = "zwqml";
            fieldEdit118.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit118.IsNullable_2 = true;
            fieldEdit118.AliasName_2 = "作物缺苗率";
            fieldEdit118.DefaultValue_2 = 0;
            fieldEdit118.Editable_2 = true;
            fieldEdit118.Precision_2 = 5;
            fieldEdit118.Scale_2 = 1;
            fields.AddField(field118);

            //作物产量字段
            IField field119 = new FieldClass();
            IFieldEdit fieldEdit119 = (IFieldEdit)field119;
            fieldEdit119.Name_2 = "zwcl";
            fieldEdit119.Type_2 = esriFieldType.esriFieldTypeSingle;
            fieldEdit119.IsNullable_2 = true;
            fieldEdit119.AliasName_2 = "作物产量";
            fieldEdit119.DefaultValue_2 = 0;
            fieldEdit119.Editable_2 = true;
            fieldEdit119.Precision_2 = 5;
            fieldEdit119.Scale_2 = 0;
            fields.AddField(field119);

            //灌溉能力字段
            IField field120 = new FieldClass();
            IFieldEdit fieldEdit120 = (IFieldEdit)field120;
            fieldEdit120.Name_2 = "ggnl";
            fieldEdit120.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit120.IsNullable_2 = true;
            fieldEdit120.AliasName_2 = "灌溉能力";
            fieldEdit120.DefaultValue_2 = "";
            fieldEdit120.Editable_2 = true;
            fieldEdit120.Length_2 = 1;
            fields.AddField(field120);

            //农田林网化率字段
            IField field121 = new FieldClass();
            IFieldEdit fieldEdit121 = (IFieldEdit)field121;
            fieldEdit121.Name_2 = "ntlwhl";
            fieldEdit121.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit121.IsNullable_2 = true;
            fieldEdit121.AliasName_2 = "农田林网化率";
            fieldEdit121.DefaultValue_2 = "";
            fieldEdit121.Editable_2 = true;
            fieldEdit121.Length_2 = 1;
            fields.AddField(field121);

            //风蚀状况字段
            IField field122 = new FieldClass();
            IFieldEdit fieldEdit122 = (IFieldEdit)field122;
            fieldEdit122.Name_2 = "fszk";
            fieldEdit122.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit122.IsNullable_2 = true;
            fieldEdit122.AliasName_2 = "风蚀状况";
            fieldEdit122.DefaultValue_2 = "";
            fieldEdit122.Editable_2 = true;
            fieldEdit122.Length_2 = 1;
            fields.AddField(field122);

            //土壤表层结构字段
            IField field123 = new FieldClass();
            IFieldEdit fieldEdit123 = (IFieldEdit)field123;
            fieldEdit123.Name_2 = "trbcjg";
            fieldEdit123.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit123.IsNullable_2 = true;
            fieldEdit123.AliasName_2 = "土壤表层结构";
            fieldEdit123.DefaultValue_2 = "";
            fieldEdit123.Editable_2 = true;
            fieldEdit123.Length_2 = 1;
            fields.AddField(field123);

            //荒漠化沙地可治理度字段
            IField field124 = new FieldClass();
            IFieldEdit fieldEdit124 = (IFieldEdit)field124;
            fieldEdit124.Name_2 = "kzld";
            fieldEdit124.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit124.IsNullable_2 = true;
            fieldEdit124.AliasName_2 = "荒漠化沙地可治理度";
            fieldEdit124.DefaultValue_2 = "";
            fieldEdit124.Editable_2 = true;
            fieldEdit124.Length_2 = 1;
            fields.AddField(field124);

            //治理措施字段
            IField field125 = new FieldClass();
            IFieldEdit fieldEdit125 = (IFieldEdit)field125;
            fieldEdit125.Name_2 = "zlcs";
            fieldEdit125.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit125.IsNullable_2 = true;
            fieldEdit125.AliasName_2 = "治理措施";
            fieldEdit125.DefaultValue_2 = "";
            fieldEdit125.Editable_2 = true;
            fieldEdit125.Length_2 = 3;
            fields.AddField(field125);

            //治理程度字段
            IField field126 = new FieldClass();
            IFieldEdit fieldEdit126 = (IFieldEdit)field126;
            fieldEdit126.Name_2 = "zlcd";
            fieldEdit126.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit126.IsNullable_2 = true;
            fieldEdit126.AliasName_2 = "治理程度";
            fieldEdit126.DefaultValue_2 = "";
            fieldEdit126.Editable_2 = true;
            fieldEdit126.Length_2 = 1;
            fields.AddField(field126);

            //石漠化状况字段
            IField field127 = new FieldClass();
            IFieldEdit fieldEdit127 = (IFieldEdit)field127;
            fieldEdit127.Name_2 = "smhzk";
            fieldEdit127.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit127.IsNullable_2 = true;
            fieldEdit127.AliasName_2 = "石漠化状况";
            fieldEdit127.DefaultValue_2 = "";
            fieldEdit127.Editable_2 = true;
            fieldEdit127.Length_2 = 1;
            fields.AddField(field127);

            //石漠化程度字段
            IField field128 = new FieldClass();
            IFieldEdit fieldEdit128 = (IFieldEdit)field128;
            fieldEdit128.Name_2 = "smhcd";
            fieldEdit128.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit128.IsNullable_2 = true;
            fieldEdit128.AliasName_2 = "石漠化程度";
            fieldEdit128.DefaultValue_2 = "";
            fieldEdit128.Editable_2 = true;
            fieldEdit128.Length_2 = 1;
            fields.AddField(field128);

            //石漠化演变类型字段
            IField field129 = new FieldClass();
            IFieldEdit fieldEdit129 = (IFieldEdit)field129;
            fieldEdit129.Name_2 = "smhyblx";
            fieldEdit129.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit129.IsNullable_2 = true;
            fieldEdit129.AliasName_2 = "石漠化演变类型";
            fieldEdit129.DefaultValue_2 = "";
            fieldEdit129.Editable_2 = true;
            fieldEdit129.Length_2 = 1;
            fields.AddField(field129);

            //岩溶地貌字段
            IField field130 = new FieldClass();
            IFieldEdit fieldEdit130 = (IFieldEdit)field130;
            fieldEdit130.Name_2 = "yrdm";
            fieldEdit130.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit130.IsNullable_2 = true;
            fieldEdit130.AliasName_2 = "岩溶地貌";
            fieldEdit130.DefaultValue_2 = "";
            fieldEdit130.Editable_2 = true;
            fieldEdit130.Length_2 = 1;
            fields.AddField(field130);

            //核实标记字段
            IField field131 = new FieldClass();
            IFieldEdit fieldEdit131 = (IFieldEdit)field131;
            fieldEdit131.Name_2 = "hsbj";
            fieldEdit131.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit131.IsNullable_2 = true;
            fieldEdit131.AliasName_2 = "核实标记";
            fieldEdit131.DefaultValue_2 = "";
            fieldEdit131.Editable_2 = true;
            fieldEdit131.Length_2 = 2;
            fields.AddField(field131);

            //光伏用地标注字段
            IField field132 = new FieldClass();
            IFieldEdit fieldEdit132 = (IFieldEdit)field132;
            fieldEdit132.Name_2 = "gfyd";
            fieldEdit132.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit132.IsNullable_2 = true;
            fieldEdit132.AliasName_2 = "光伏用地标注";
            fieldEdit132.DefaultValue_2 = "";
            fieldEdit132.Editable_2 = true;
            fieldEdit132.Length_2 = 1;
            fields.AddField(field132);

            //调查人员字段
            IField field133 = new FieldClass();
            IFieldEdit fieldEdit133 = (IFieldEdit)field133;
            fieldEdit133.Name_2 = "dc_ry";
            fieldEdit133.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit133.IsNullable_2 = true;
            fieldEdit133.AliasName_2 = "调查人员";
            fieldEdit133.DefaultValue_2 = "";
            fieldEdit133.Editable_2 = true;
            fieldEdit133.Length_2 = 20;
            fields.AddField(field133);

            //调查日期字段
            IField field134 = new FieldClass();
            IFieldEdit fieldEdit134 = (IFieldEdit)field134;
            fieldEdit134.Name_2 = "dc_rq";
            fieldEdit134.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit134.IsNullable_2 = true;
            fieldEdit134.AliasName_2 = "调查日期";
            fieldEdit134.DefaultValue_2 = "";
            fieldEdit134.Editable_2 = true;
            fieldEdit134.Length_2 = 8;
            fields.AddField(field134);

            //自定义的备注字段
            IField field135 = new FieldClass();
            IFieldEdit fieldEdit135 = (IFieldEdit)field135;
            fieldEdit135.Name_2 = "memo";
            fieldEdit135.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit135.IsNullable_2 = true;
            fieldEdit135.AliasName_2 = "备注";
            fieldEdit135.DefaultValue_2 = "";
            fieldEdit135.Editable_2 = true;
            fieldEdit135.Length_2 = 50;
            fields.AddField(field135);
        }

        public static void GenerateSlzyzcFields(IFieldsEdit fields) {
            if (fields == null)
            {
                System.Windows.Forms.MessageBox.Show("字段集合对象不存在");
                return;
            }

            //资产清查标识码字段
            IField field01 = new FieldClass();
            IFieldEdit fieldEdit01 = (IFieldEdit)field01;
            fieldEdit01.Name_2 = "ZCQCBSM";
            fieldEdit01.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit01.IsNullable_2 = true;
            fieldEdit01.AliasName_2 = "资产清查标识码";
            fieldEdit01.DefaultValue_2 = "";
            fieldEdit01.Editable_2 = true;
            fieldEdit01.Length_2 = 22;
            fields.AddField(field01);

            //要素代码
            IField field02 = new FieldClass();
            IFieldEdit fieldEdit02 = (IFieldEdit)field02;
            fieldEdit02.Name_2 = "YSDM";
            fieldEdit02.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit02.IsNullable_2 = true;
            fieldEdit02.AliasName_2 = "要素代码";
            fieldEdit02.DefaultValue_2 = "";
            fieldEdit02.Editable_2 = true;
            fieldEdit02.Length_2 = 10;
            fields.AddField(field02);

            //行政区代码字段
            IField field03 = new FieldClass();
            IFieldEdit fieldEdit03 = (IFieldEdit)field03;
            fieldEdit03.Name_2 = "XZQDM";
            fieldEdit03.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit03.IsNullable_2 = true;
            fieldEdit03.AliasName_2 = "行政区代码";
            fieldEdit03.DefaultValue_2 = "";
            fieldEdit03.Editable_2 = true;
            fieldEdit03.Length_2 = 6;
            fields.AddField(field03);

            //行政区名称字段
            IField field04 = new FieldClass();
            IFieldEdit fieldEdit04 = (IFieldEdit)field04;
            fieldEdit04.Name_2 = "XZQMC";
            fieldEdit04.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit04.IsNullable_2 = true;
            fieldEdit04.AliasName_2 = "行政区名称";
            fieldEdit04.DefaultValue_2 = "";
            fieldEdit04.Editable_2 = true;
            fieldEdit04.Length_2 = 60;
            fields.AddField(field04);

            //国土调查图斑标识码字段
            IField field05 = new FieldClass();
            IFieldEdit fieldEdit05 = (IFieldEdit)field05;
            fieldEdit05.Name_2 = "GTDCTBBSM";
            fieldEdit05.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit05.IsNullable_2 = true;
            fieldEdit05.AliasName_2 = "国土调查图斑标识码";
            fieldEdit05.DefaultValue_2 = "";
            fieldEdit05.Editable_2 = true;
            fieldEdit05.Length_2 = 18;
            fields.AddField(field05);

            //国土调查图斑编号字段
            IField field06 = new FieldClass();
            IFieldEdit fieldEdit06 = (IFieldEdit)field06;
            fieldEdit06.Name_2 = "GTDCTBBH";
            fieldEdit06.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit06.IsNullable_2 = true;
            fieldEdit06.AliasName_2 = "国土调查图斑编号";
            fieldEdit06.DefaultValue_2 = "";
            fieldEdit06.Editable_2 = true;
            fieldEdit06.Length_2 = 8;
            fields.AddField(field06);

            //国土调查地类编码字段
            IField field07 = new FieldClass();
            IFieldEdit fieldEdit07 = (IFieldEdit)field07;
            fieldEdit07.Name_2 = "GTDCDLBM";
            fieldEdit07.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit07.IsNullable_2 = true;
            fieldEdit07.AliasName_2 = "国土调查地类编码";
            fieldEdit07.DefaultValue_2 = "";
            fieldEdit07.Editable_2 = true;
            fieldEdit07.Length_2 = 5;
            fields.AddField(field07);

            //国土调查地类名称字段
            IField field08 = new FieldClass();
            IFieldEdit fieldEdit08 = (IFieldEdit)field08;
            fieldEdit08.Name_2 = "GTDCDLMC";
            fieldEdit08.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit08.IsNullable_2 = true;
            fieldEdit08.AliasName_2 = "国土调查地类名称";
            fieldEdit08.DefaultValue_2 = "";
            fieldEdit08.Editable_2 = true;
            fieldEdit08.Length_2 = 60;
            fields.AddField(field08);

            //权属单位代码字段
            IField field09 = new FieldClass();
            IFieldEdit fieldEdit09 = (IFieldEdit)field09;
            fieldEdit09.Name_2 = "QSDWDM";
            fieldEdit09.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit09.IsNullable_2 = true;
            fieldEdit09.AliasName_2 = "权属单位代码";
            fieldEdit09.DefaultValue_2 = "";
            fieldEdit09.Editable_2 = true;
            fieldEdit09.Length_2 = 19;
            fields.AddField(field09);

            //权属单位名称字段
            IField field10 = new FieldClass();
            IFieldEdit fieldEdit10 = (IFieldEdit)field10;
            fieldEdit10.Name_2 = "QSDWMC";
            fieldEdit10.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit10.IsNullable_2 = true;
            fieldEdit10.AliasName_2 = "权属单位名称";
            fieldEdit10.DefaultValue_2 = "";
            fieldEdit10.Editable_2 = true;
            fieldEdit10.Length_2 = 60;
            fields.AddField(field10);

            //坐落单位代码字段
            IField field11 = new FieldClass();
            IFieldEdit fieldEdit11 = (IFieldEdit)field11;
            fieldEdit11.Name_2 = "ZLDWDM";
            fieldEdit11.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit11.IsNullable_2 = true;
            fieldEdit11.AliasName_2 = "坐落单位代码";
            fieldEdit11.DefaultValue_2 = "";
            fieldEdit11.Editable_2 = true;
            fieldEdit11.Length_2 = 19;
            fields.AddField(field11);

            //坐落单位名称字段
            IField field12 = new FieldClass();
            IFieldEdit fieldEdit12 = (IFieldEdit)field12;
            fieldEdit12.Name_2 = "ZLDWMC";
            fieldEdit12.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit12.IsNullable_2 = true;
            fieldEdit12.AliasName_2 = "坐落单位名称";
            fieldEdit12.DefaultValue_2 = "";
            fieldEdit12.Editable_2 = true;
            fieldEdit12.Length_2 = 60;
            fields.AddField(field12);

            //国土调查图斑面积字段
            IField field13 = new FieldClass();
            IFieldEdit fieldEdit13 = (IFieldEdit)field13;
            fieldEdit13.Name_2 = "GTDCTBMJ";
            fieldEdit13.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit13.IsNullable_2 = true;
            fieldEdit13.AliasName_2 = "国土调查图斑面积";
            fieldEdit13.DefaultValue_2 = 0;
            fieldEdit13.Editable_2 = true;
            fieldEdit13.Precision_2 = 15;
            fieldEdit13.Scale_2 = 2;
            fields.AddField(field13);

            //林业局字段
            IField field14 = new FieldClass();
            IFieldEdit fieldEdit14 = (IFieldEdit)field14;
            fieldEdit14.Name_2 = "LYJ";
            fieldEdit14.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit14.IsNullable_2 = true;
            fieldEdit14.AliasName_2 = "林业局";
            fieldEdit14.DefaultValue_2 = "";
            fieldEdit14.Editable_2 = true;
            fieldEdit14.Length_2 = 60;
            fields.AddField(field14);

            //林场字段
            IField field15 = new FieldClass();
            IFieldEdit fieldEdit15 = (IFieldEdit)field15;
            fieldEdit15.Name_2 = "LC";
            fieldEdit15.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit15.IsNullable_2 = true;
            fieldEdit15.AliasName_2 = "林场";
            fieldEdit15.DefaultValue_2 = "";
            fieldEdit15.Editable_2 = true;
            fieldEdit15.Length_2 = 60;
            fields.AddField(field15);

            //普查图斑编码字段
            IField field16 = new FieldClass();
            IFieldEdit fieldEdit16 = (IFieldEdit)field16;
            fieldEdit16.Name_2 = "PCTBBM";
            fieldEdit16.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit16.IsNullable_2 = true;
            fieldEdit16.AliasName_2 = "普查图斑编码";
            fieldEdit16.DefaultValue_2 = "";
            fieldEdit16.Editable_2 = true;
            fieldEdit16.Length_2 = 15;
            fields.AddField(field16);

            //普查地类字段
            IField field17 = new FieldClass();
            IFieldEdit fieldEdit17 = (IFieldEdit)field17;
            fieldEdit17.Name_2 = "PCDL";
            fieldEdit17.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit17.IsNullable_2 = true;
            fieldEdit17.AliasName_2 = "普查地类";
            fieldEdit17.DefaultValue_2 = "";
            fieldEdit17.Length_2 = 60;
            fields.AddField(field17);

            //子图斑面积字段
            IField field18 = new FieldClass();
            IFieldEdit fieldEdit18 = (IFieldEdit)field18;
            fieldEdit18.Name_2 = "ZTBMJ";
            fieldEdit18.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit18.IsNullable_2 = true;
            fieldEdit18.AliasName_2 = "子图斑面积";
            fieldEdit18.DefaultValue_2 = 0;
            fieldEdit18.Editable_2 = true;
            fieldEdit18.Precision_2 = 15;
            fieldEdit18.Scale_2 = 2;
            fields.AddField(field18);

            //国土调查土地权属字段
            IField field19 = new FieldClass();
            IFieldEdit fieldEdit19 = (IFieldEdit)field19;
            fieldEdit19.Name_2 = "GTDCTDQS";
            fieldEdit19.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit19.IsNullable_2 = true;
            fieldEdit19.AliasName_2 = "国土调查土地权属";
            fieldEdit19.DefaultValue_2 = "";
            fieldEdit19.Editable_2 = true;
            fieldEdit19.Length_2 = 2;
            fields.AddField(field19);

            //林木所有权字段
            IField field20 = new FieldClass();
            IFieldEdit fieldEdit20 = (IFieldEdit)field20;
            fieldEdit20.Name_2 = "LM_SUOYQ";
            fieldEdit20.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit20.IsNullable_2 = true;
            fieldEdit20.AliasName_2 = "林木所有权";
            fieldEdit20.DefaultValue_2 = "";
            fieldEdit20.Editable_2 = true;
            fieldEdit20.Length_2 = 2;
            fields.AddField(field20);

            //林种字段
            IField field21 = new FieldClass();
            IFieldEdit fieldEdit21 = (IFieldEdit)field21;
            fieldEdit21.Name_2 = "LZ";
            fieldEdit21.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit21.IsNullable_2 = true;
            fieldEdit21.AliasName_2 = "林种";
            fieldEdit21.DefaultValue_2 = "";
            fieldEdit21.Editable_2 = true;
            fieldEdit21.Length_2 = 3;
            fields.AddField(field21);

            //优势种(组)字段
            IField field22 = new FieldClass();
            IFieldEdit fieldEdit22 = (IFieldEdit)field22;
            fieldEdit22.Name_2 = "YSSZ";
            fieldEdit22.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit22.IsNullable_2 = true;
            fieldEdit22.AliasName_2 = "优势种(组)";
            fieldEdit22.DefaultValue_2 = "";
            fieldEdit22.Editable_2 = true;
            fieldEdit22.Length_2 = 6;
            fields.AddField(field22);

            //起源字段
            IField field23 = new FieldClass();
            IFieldEdit fieldEdit23 = (IFieldEdit)field23;
            fieldEdit23.Name_2 = "QY";
            fieldEdit23.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit23.IsNullable_2 = true;
            fieldEdit23.AliasName_2 = "起源";
            fieldEdit23.DefaultValue_2 = "";
            fieldEdit23.Editable_2 = true;
            fieldEdit23.Length_2 = 2;
            fields.AddField(field23);

            //郁闭度/覆盖度字段
            IField field24 = new FieldClass();
            IFieldEdit fieldEdit24 = (IFieldEdit)field24;
            fieldEdit24.Name_2 = "YBD";
            fieldEdit24.Type_2 = esriFieldType.esriFieldTypeDouble ;
            fieldEdit24.IsNullable_2 = true;
            fieldEdit24.AliasName_2 = "郁闭度/覆盖度";
            fieldEdit24.DefaultValue_2 = 0;
            fieldEdit24.Editable_2 = true;
            fieldEdit24.Precision_2 = 15;
            fieldEdit24.Scale_2 = 2;
            fields.AddField(field24);

            //平均年龄字段
            IField field25 = new FieldClass();
            IFieldEdit fieldEdit25 = (IFieldEdit)field25;
            fieldEdit25.Name_2 = "PJNL";
            fieldEdit25.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit25.IsNullable_2 = true;
            fieldEdit25.AliasName_2 = "平均年龄";
            fieldEdit25.DefaultValue_2 = 0;
            fieldEdit25.Editable_2 = true;
            fieldEdit25.Length_2 = 3;
            fields.AddField(field25);

            //龄组字段
            IField field26 = new FieldClass();
            IFieldEdit fieldEdit26 = (IFieldEdit)field26;
            fieldEdit26.Name_2 = "LING_ZU";
            fieldEdit26.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit26.IsNullable_2 = true;
            fieldEdit26.AliasName_2 = "龄组";
            fieldEdit26.DefaultValue_2 = "";
            fieldEdit26.Editable_2 = true;
            fieldEdit26.Length_2 = 10;
            fields.AddField(field26);

            //平均树高字段
            IField field27 = new FieldClass();
            IFieldEdit fieldEdit27 = (IFieldEdit)field27;
            fieldEdit27.Name_2 = "PJSG";
            fieldEdit27.Type_2 = esriFieldType.esriFieldTypeDouble ;
            fieldEdit27.IsNullable_2 = true;
            fieldEdit27.AliasName_2 = "平均树高";
            fieldEdit27.DefaultValue_2 = 0;
            fieldEdit27.Editable_2 = true;
            fieldEdit27.Precision_2 = 15;
            fieldEdit27.Scale_2 = 2;
            fields.AddField(field27);

            //平均胸径字段
            IField field28 = new FieldClass();
            IFieldEdit fieldEdit28 = (IFieldEdit)field28;
            fieldEdit28.Name_2 = "PJXJ";
            fieldEdit28.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit28.IsNullable_2 = true;
            fieldEdit28.AliasName_2 = "平均胸径";
            fieldEdit28.DefaultValue_2 = 0;
            fieldEdit28.Editable_2 = true;
            fieldEdit28.Precision_2 = 15;
            fieldEdit28.Scale_2 = 2;
            fields.AddField(field28);

            //每公顷株树字段
            IField field29 = new FieldClass();
            IFieldEdit fieldEdit29 = (IFieldEdit)field29;
            fieldEdit29.Name_2 = "MGQZS";
            fieldEdit29.Type_2 = esriFieldType.esriFieldTypeInteger;
            fieldEdit29.IsNullable_2 = true;
            fieldEdit29.AliasName_2 = "每公顷株树";
            fieldEdit29.DefaultValue_2 = 0;
            fieldEdit29.Editable_2 = true;
            fieldEdit29.Length_2 = 6;
            fields.AddField(field29);

            //子图斑蓄积字段
            IField field30 = new FieldClass();
            IFieldEdit fieldEdit30 = (IFieldEdit)field30;
            fieldEdit30.Name_2 = "ZTBXJ";
            fieldEdit30.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit30.IsNullable_2 = true;
            fieldEdit30.AliasName_2 = "子图斑蓄积";
            fieldEdit30.DefaultValue_2 = 0;
            fieldEdit30.Editable_2 = true;
            fieldEdit30.Precision_2 = 15;
            fieldEdit30.Scale_2 = 2;
            fields.AddField(field30);

            //林地等字段
            IField field31 = new FieldClass();
            IFieldEdit fieldEdit31 = (IFieldEdit)field31;
            fieldEdit31.Name_2 = "LD_DENG";
            fieldEdit31.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit31.IsNullable_2 = true;
            fieldEdit31.AliasName_2 = "林地等";
            fieldEdit31.DefaultValue_2 = 0;
            fieldEdit31.Editable_2 = true;
            fieldEdit31.Length_2 = 2;
            fields.AddField(field31);

            //林地核算价格字段
            IField field32 = new FieldClass();
            IFieldEdit fieldEdit32 = (IFieldEdit)field32;
            fieldEdit32.Name_2 = "LDHSJG";
            fieldEdit32.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit32.AliasName_2 = "林地核算价格";
            fieldEdit32.DefaultValue_2 = 0;
            fieldEdit32.Editable_2 = true;
            fieldEdit32.Precision_2 = 15;
            fieldEdit32.Scale_2 = 5;
            fields.AddField(field32);

            //林木核算价格字段
            IField field33 = new FieldClass();
            IFieldEdit fieldEdit33 = (IFieldEdit)field33;
            fieldEdit33.Name_2 = "LMHSJG";
            fieldEdit33.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit33.IsNullable_2 = true;
            fieldEdit33.AliasName_2 = "林木核算价格";
            fieldEdit33.DefaultValue_2 = 0;
            fieldEdit33.Editable_2 = true;
            fieldEdit33.Precision_2 = 15;
            fieldEdit33.Scale_2 = 5;
            fields.AddField(field33);

            //林地资产字段
            IField field34 = new FieldClass();
            IFieldEdit fieldEdit34 = (IFieldEdit)field34;
            fieldEdit34.Name_2 = "LDZC";
            fieldEdit34.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit34.IsNullable_2 = true;
            fieldEdit34.AliasName_2 = "林地资产";
            fieldEdit34.DefaultValue_2 = 0;
            fieldEdit34.Editable_2 = true;
            fieldEdit34.Precision_2 = 15;
            fieldEdit34.Scale_2 = 0;
            fields.AddField(field34);

            //林木资产字段
            IField field35 = new FieldClass();
            IFieldEdit fieldEdit35 = (IFieldEdit)field35;
            fieldEdit35.Name_2 = "LMZC";
            fieldEdit35.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit35.IsNullable_2 = true;
            fieldEdit35.AliasName_2 = "林木资产";
            fieldEdit35.DefaultValue_2 = 0;
            fieldEdit35.Editable_2 = true;
            fieldEdit35.Precision_2 = 15;
            fieldEdit35.Scale_2 = 0;
            fields.AddField(field35);

            //经济价值字段
            IField field36 = new FieldClass();
            IFieldEdit fieldEdit36 = (IFieldEdit)field36;
            fieldEdit36.Name_2 = "JJJZ";
            fieldEdit36.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit36.IsNullable_2 = true;
            fieldEdit36.AliasName_2 = "经济价值";
            fieldEdit36.DefaultValue_2 = 0;
            fieldEdit36.Editable_2 = true;
            fieldEdit36.Precision_2 = 15;
            fieldEdit36.Scale_2 = 0;
            fields.AddField(field36);

            //划入城镇开发边界面积字段
            IField field37 = new FieldClass();
            IFieldEdit fieldEdit37 = (IFieldEdit)field37;
            fieldEdit37.Name_2 = "CZKFBJMJ";
            fieldEdit37.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit37.IsNullable_2 = true;
            fieldEdit37.AliasName_2 = "划入城镇开发边界面积";
            fieldEdit37.DefaultValue_2 = 0;
            fieldEdit37.Editable_2 = true;
            fieldEdit37.Precision_2 = 15;
            fieldEdit37.Scale_2 = 2;
            fields.AddField(field37);

            //飞入地标识字段
            IField field38 = new FieldClass();
            IFieldEdit fieldEdit38 = (IFieldEdit)field38;
            fieldEdit38.Name_2 = "FRDBS";
            fieldEdit38.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit38.IsNullable_2 = true;
            fieldEdit38.AliasName_2 = "飞入地标识";
            fieldEdit38.DefaultValue_2 = "";
            fieldEdit38.Editable_2 = true;
            fieldEdit38.Length_2 = 1;
            fields.AddField(field38);

            //区域扩展代码字段
            IField field39 = new FieldClass();
            IFieldEdit fieldEdit39 = (IFieldEdit)field39;
            fieldEdit39.Name_2 = "QYKZDM";
            fieldEdit39.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit39.IsNullable_2 = true;
            fieldEdit39.AliasName_2 = "区域扩展代码";
            fieldEdit39.DefaultValue_2 = "";
            fieldEdit39.Editable_2 = true;
            fieldEdit39.Length_2 = 19;
            fields.AddField(field39);

            //备注字段
            IField field40 = new FieldClass();
            IFieldEdit fieldEdit40 = (IFieldEdit)field40;
            fieldEdit40.Name_2 = "BZ";
            fieldEdit40.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit40.IsNullable_2 = true;
            fieldEdit40.AliasName_2 = "备注";
            fieldEdit40.DefaultValue_2 = "";
            fieldEdit40.Editable_2 = true;
            fieldEdit40.Length_2 = 254;
            fields.AddField(field40);
        }

        public static void GenerateSlzyzc_dltbFields(IFieldsEdit fields) {
            if (fields == null)
            {
                System.Windows.Forms.MessageBox.Show("字段集合对象不存在");
                return;
            }

            //资产清查标识码字段
            IField field01 = new FieldClass();
            IFieldEdit fieldEdit01 = (IFieldEdit)field01;
            fieldEdit01.Name_2 = "ZCQCBSM";
            fieldEdit01.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit01.IsNullable_2 = true;
            fieldEdit01.AliasName_2 = "资产清查标识码";
            fieldEdit01.DefaultValue_2 = "";
            fieldEdit01.Editable_2 = true;
            fieldEdit01.Length_2 = 22;
            fields.AddField(field01);

            //要素代码
            IField field02 = new FieldClass();
            IFieldEdit fieldEdit02 = (IFieldEdit)field02;
            fieldEdit02.Name_2 = "YSDM";
            fieldEdit02.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit02.IsNullable_2 = true;
            fieldEdit02.AliasName_2 = "要素代码";
            fieldEdit02.DefaultValue_2 = "";
            fieldEdit02.Editable_2 = true;
            fieldEdit02.Length_2 = 10;
            fields.AddField(field02);

            //行政区代码字段
            IField field03 = new FieldClass();
            IFieldEdit fieldEdit03 = (IFieldEdit)field03;
            fieldEdit03.Name_2 = "XZQDM";
            fieldEdit03.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit03.IsNullable_2 = true;
            fieldEdit03.AliasName_2 = "行政区代码";
            fieldEdit03.DefaultValue_2 = "";
            fieldEdit03.Editable_2 = true;
            fieldEdit03.Length_2 = 6;
            fields.AddField(field03);

            //行政区名称字段
            IField field04 = new FieldClass();
            IFieldEdit fieldEdit04 = (IFieldEdit)field04;
            fieldEdit04.Name_2 = "XZQMC";
            fieldEdit04.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit04.IsNullable_2 = true;
            fieldEdit04.AliasName_2 = "行政区名称";
            fieldEdit04.DefaultValue_2 = "";
            fieldEdit04.Editable_2 = true;
            fieldEdit04.Length_2 = 60;
            fields.AddField(field04);

            //国土调查图斑标识码字段
            IField field05 = new FieldClass();
            IFieldEdit fieldEdit05 = (IFieldEdit)field05;
            fieldEdit05.Name_2 = "GTDCTBBSM";
            fieldEdit05.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit05.IsNullable_2 = true;
            fieldEdit05.AliasName_2 = "国土调查图斑标识码";
            fieldEdit05.DefaultValue_2 = "";
            fieldEdit05.Editable_2 = true;
            fieldEdit05.Length_2 = 18;
            fields.AddField(field05);

            //国土调查图斑编号字段
            IField field06 = new FieldClass();
            IFieldEdit fieldEdit06 = (IFieldEdit)field06;
            fieldEdit06.Name_2 = "GTDCTBBH";
            fieldEdit06.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit06.IsNullable_2 = true;
            fieldEdit06.AliasName_2 = "国土调查图斑编号";
            fieldEdit06.DefaultValue_2 = "";
            fieldEdit06.Editable_2 = true;
            fieldEdit06.Length_2 = 8;
            fields.AddField(field06);

            //国土调查地类编码字段
            IField field07 = new FieldClass();
            IFieldEdit fieldEdit07 = (IFieldEdit)field07;
            fieldEdit07.Name_2 = "GTDCDLBM";
            fieldEdit07.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit07.IsNullable_2 = true;
            fieldEdit07.AliasName_2 = "国土调查地类编码";
            fieldEdit07.DefaultValue_2 = "";
            fieldEdit07.Editable_2 = true;
            fieldEdit07.Length_2 = 5;
            fields.AddField(field07);

            //国土调查地类名称字段
            IField field08 = new FieldClass();
            IFieldEdit fieldEdit08 = (IFieldEdit)field08;
            fieldEdit08.Name_2 = "GTDCDLMC";
            fieldEdit08.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit08.IsNullable_2 = true;
            fieldEdit08.AliasName_2 = "国土调查地类名称";
            fieldEdit08.DefaultValue_2 = "";
            fieldEdit08.Editable_2 = true;
            fieldEdit08.Length_2 = 60;
            fields.AddField(field08);

            //国土调查土地权属字段
            IField field09 = new FieldClass();
            IFieldEdit fieldEdit09 = (IFieldEdit)field09;
            fieldEdit09.Name_2 = "GTDCTDQS";
            fieldEdit09.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit09.IsNullable_2 = true;
            fieldEdit09.AliasName_2 = "国土调查土地权属";
            fieldEdit09.DefaultValue_2 = "";
            fieldEdit09.Editable_2 = true;
            fieldEdit09.Length_2 = 2;
            fields.AddField(field09);

            //权属单位代码字段
            IField field10 = new FieldClass();
            IFieldEdit fieldEdit10 = (IFieldEdit)field10;
            fieldEdit10.Name_2 = "QSDWDM";
            fieldEdit10.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit10.IsNullable_2 = true;
            fieldEdit10.AliasName_2 = "权属单位代码";
            fieldEdit10.DefaultValue_2 = "";
            fieldEdit10.Editable_2 = true;
            fieldEdit10.Length_2 = 19;
            fields.AddField(field10);

            //权属单位名称字段
            IField field11 = new FieldClass();
            IFieldEdit fieldEdit11 = (IFieldEdit)field11;
            fieldEdit11.Name_2 = "QSDWMC";
            fieldEdit11.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit11.IsNullable_2 = true;
            fieldEdit11.AliasName_2 = "权属单位名称";
            fieldEdit11.DefaultValue_2 = "";
            fieldEdit11.Editable_2 = true;
            fieldEdit11.Length_2 = 60;
            fields.AddField(field11);

            //坐落单位代码字段
            IField field12 = new FieldClass();
            IFieldEdit fieldEdit12 = (IFieldEdit)field12;
            fieldEdit12.Name_2 = "ZLDWDM";
            fieldEdit12.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit12.IsNullable_2 = true;
            fieldEdit12.AliasName_2 = "坐落单位代码";
            fieldEdit12.DefaultValue_2 = "";
            fieldEdit12.Editable_2 = true;
            fieldEdit12.Length_2 = 19;
            fields.AddField(field12);

            //坐落单位名称字段
            IField field13 = new FieldClass();
            IFieldEdit fieldEdit13 = (IFieldEdit)field13;
            fieldEdit13.Name_2 = "ZLDWMC";
            fieldEdit13.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit13.IsNullable_2 = true;
            fieldEdit13.AliasName_2 = "坐落单位名称";
            fieldEdit13.DefaultValue_2 = "";
            fieldEdit13.Editable_2 = true;
            fieldEdit13.Length_2 = 60;
            fields.AddField(field13);

            //国土调查图斑面积字段
            IField field14 = new FieldClass();
            IFieldEdit fieldEdit14 = (IFieldEdit)field14;
            fieldEdit14.Name_2 = "GTDCTBMJ";
            fieldEdit14.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit14.IsNullable_2 = true;
            fieldEdit14.AliasName_2 = "国土调查图斑面积";
            fieldEdit14.DefaultValue_2 = 0;
            fieldEdit14.Editable_2 = true;
            fieldEdit14.Precision_2 = 15;
            fieldEdit14.Scale_2 = 2;
            fields.AddField(field14);

            //林地等字段
            IField field15 = new FieldClass();
            IFieldEdit fieldEdit15 = (IFieldEdit)field15;
            fieldEdit15.Name_2 = "LD_DENG";
            fieldEdit15.Type_2 = esriFieldType.esriFieldTypeSmallInteger;
            fieldEdit15.IsNullable_2 = true;
            fieldEdit15.AliasName_2 = "林地等";
            fieldEdit15.DefaultValue_2 = 0;
            fieldEdit15.Editable_2 = true;
            fieldEdit15.Length_2 = 2;
            fields.AddField(field15);

            //核算价格字段
            IField field16 = new FieldClass();
            IFieldEdit fieldEdit16 = (IFieldEdit)field16;
            fieldEdit16.Name_2 = "HSJG";
            fieldEdit16.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit16.AliasName_2 = "核算价格";
            fieldEdit16.DefaultValue_2 = 0;
            fieldEdit16.Editable_2 = true;
            fieldEdit16.Precision_2 = 15;
            fieldEdit16.Scale_2 = 5;
            fields.AddField(field16);

            //经济价值字段
            IField field17 = new FieldClass();
            IFieldEdit fieldEdit17 = (IFieldEdit)field17;
            fieldEdit17.Name_2 = "JJJZ";
            fieldEdit17.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit17.IsNullable_2 = true;
            fieldEdit17.AliasName_2 = "经济价值";
            fieldEdit17.DefaultValue_2 = 0;
            fieldEdit17.Editable_2 = true;
            fieldEdit17.Precision_2 = 15;
            fieldEdit17.Scale_2 = 0;
            fields.AddField(field17);

            //划入城镇开发边界面积字段
            IField field18 = new FieldClass();
            IFieldEdit fieldEdit18 = (IFieldEdit)field18;
            fieldEdit18.Name_2 = "CZKFBJMJ";
            fieldEdit18.Type_2 = esriFieldType.esriFieldTypeDouble;
            fieldEdit18.IsNullable_2 = true;
            fieldEdit18.AliasName_2 = "划入城镇开发边界面积";
            fieldEdit18.DefaultValue_2 = 0;
            fieldEdit18.Editable_2 = true;
            fieldEdit18.Precision_2 = 15;
            fieldEdit18.Scale_2 = 2;
            fields.AddField(field18);

            //飞入地标识字段
            IField field19 = new FieldClass();
            IFieldEdit fieldEdit19 = (IFieldEdit)field19;
            fieldEdit19.Name_2 = "FRDBS";
            fieldEdit19.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit19.IsNullable_2 = true;
            fieldEdit19.AliasName_2 = "飞入地标识";
            fieldEdit19.DefaultValue_2 = "";
            fieldEdit19.Editable_2 = true;
            fieldEdit19.Length_2 = 1;
            fields.AddField(field19);

            //区域扩展代码字段
            IField field20 = new FieldClass();
            IFieldEdit fieldEdit20 = (IFieldEdit)field20;
            fieldEdit20.Name_2 = "QYKZDM";
            fieldEdit20.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit20.IsNullable_2 = true;
            fieldEdit20.AliasName_2 = "区域扩展代码";
            fieldEdit20.DefaultValue_2 = "";
            fieldEdit20.Editable_2 = true;
            fieldEdit20.Length_2 = 19;
            fields.AddField(field20);

            //备注字段
            IField field21 = new FieldClass();
            IFieldEdit fieldEdit21 = (IFieldEdit)field21;
            fieldEdit21.Name_2 = "BZ";
            fieldEdit21.Type_2 = esriFieldType.esriFieldTypeString;
            fieldEdit21.IsNullable_2 = true;
            fieldEdit21.AliasName_2 = "备注";
            fieldEdit21.DefaultValue_2 = "";
            fieldEdit21.Editable_2 = true;
            fieldEdit21.Length_2 = 254;
            fields.AddField(field21);
        }
    }
}
