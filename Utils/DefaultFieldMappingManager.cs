using System.Collections.Generic;

namespace ForestResourcePlugin.Utils
{
    /// <summary>
    /// 提供硬编码的字段映射配置，取代从外部XML文件加载。
    /// </summary>
    public static class DefaultFieldMappingManager
    {
        /// <summary>
        /// 获取完整的字段映射模板。
        /// Key: 目标字段名 (TargetField)
        /// Value: 源字段名 (SourceField)
        /// </summary>
        /// <returns>一个包含所有字段映射的字典。</returns>
        public static Dictionary<string, string> GetFullFieldMappings()
        {
            return new Dictionary<string, string>
            {
                // 标识码
                { "bsm", "BSM" },
                // 要素代码
                { "ysdm", "YSDM" },
                // 图斑预编号
                { "tbybh", "TBYBH" },
                // 图斑编号
                { "tbbh", "TBBH" },
                // 地类编码
                { "dlbm", "DLBM" },
                // 地类名称
                { "dlmc", "DLMC" },
                // 权属性质
                { "qsxz", "QSXZ" },
                // 权属单位代码
                { "qsdwdm", "QSDWDM" },
                // 权属单位名称
                { "qsdwmc", "QSDWMC" },
                // 坐落单位代码
                { "zldwdm", "ZLDWDM" },
                // 坐落单位名称
                { "zldwmc", "ZLDWMC" },
                // 图斑面积
                { "tbmj", "TBMJ" },
                // 扣除地类编码
                { "kcdlbm", "KCDLBM" },
                // 扣除地类系数
                { "kcxs", "KCXS" },
                // 扣除地类面积
                { "kcmj", "KCMJ" },
                // 图斑地类面积
                { "tbdlmj", "TBDLMJ" },
                // 耕地类型
                { "gdlx", "GDLX" },
                // 耕地坡度级别
                { "gdpdjb", "GDPDJB" },
                // 线性图斑宽度
                { "xxtbkd", "XXTBKD" },
                // 图斑细化代码
                { "tbxhdm", "TBXHDM" },
                // 图斑细化名称
                { "tbxhmc", "TBXHMC" },
                // 种植属性代码
                { "zzsxdm", "ZZSXDM" },
                // 种植属性名称
                { "zzsxmc", "ZZSXMC" },
                // 耕地等别
                { "gddb", "GDDB" },
                // 飞入地标识
                { "frdbs", "FRDBS" },
                // 城镇村属性码
                { "czcsxm", "CZCSXM" },
                // 数据年份
                { "sjnf", "SJNF" },
                // 国土备注
                { "bz", "BZ" },
                // 省（区、市）
                { "sheng", "SHENG" },
                // 县（市、旗）
                { "xian", "XIAN" },
                // 乡
                { "xiang", "XIANG" },
                // 村
                { "cun", "CUN" },
                // 林业局（牧场）
                { "lin_ye_ju", "LIN_YE_JU" },
                // 林场(分场)
                { "lin_chang", "LIN_CHANG" },
                // 林(草)班
                { "lin_ban", "LIN_BAN" },
                // 小班号
                { "xiao_ban", "XIAO_BAN" },
                // 流域
                { "ly", "LY" },
                // 生态区位
                { "stqw", "STQW" },
                // 生态区位名称
                { "stqwmc", "STQWMC" },
                // 地貌
                { "di_mao", "DI_MAO" },
                // 海拔
                { "hai_ba", "HAI_BA" },
                // 坡向
                { "po_xiang", "PO_XIANG" },
                // 坡位
                { "po_wei", "PO_WEI" },
                // 坡度
                { "po_du", "PO_DU" },
                // 土壤类型
                { "tu_rang_lx", "TU_RANG_LX" },
                // 土层厚度
                { "tu_ceng_hd", "TU_CENG_HD" },
                // 土壤质地
                { "tu_rang_zd", "TU_RANG_ZD" },
                // 土地所有权属
                { "ld_qs", "LD_QS" },
                // 土地使用权属
                { "tdsyqs", "TDSYQS" },
                // 小班面积
                { "xbmj", "XBMJ" },
                // 地类
                { "di_lei", "DI_LEI" },
                // 植被覆盖类型
                { "zbfglx", "ZBFGLX" },
                // 植被结构
                { "zb_jg", "ZB_JG" },
                // 植被总盖度
                { "zbgd", "ZBGD" },
                // 郁闭度
                { "yu_bi_du", "YU_BI_DU" },
                // 灌木盖度
                { "gmgd", "GMGD" },
                // 草本盖度
                { "cbgd", "CBGD" },
                // 优势树（灌）种
                { "you_shi_sz", "YOU_SHI_SZ" },
                // 起源
                { "qi_yuan", "QI_YUAN" },
                // 平均年龄
                { "pingjun_nl", "PINGJUN_NL" },
                // 龄组
                { "ling_zu", "LING_ZU" },
                // 平均胸径
                { "pingjun_xj", "PINGJUN_XJ" },
                // 平均树高
                { "pingjun_sg", "PINGJUN_SG" },
                // 每公顷株数
                { "mei_gq_zs", "MEI_GQ_ZS" },
                // 每公顷蓄积
                { "mei_gq_xj", "MEI_GQ_XJ" },
                // 蓄积量
                { "huo_lm_xj", "HUO_LM_XJ" },
                // 生物量
                { "sheng_wu_l", "SHENG_WU_L" },
                // 碳储量
                { "tan_chu_l", "TAN_CHU_L" },
                // 林木所有权属
                { "lmqs", "LMQS" },
                // 林木使用权属
                { "lmsyqs", "LMSYQS" },
                // 林种
                { "lin_zhong", "LIN_ZHONG" },
                // 树种组成
                { "shuzhong_zc", "SHUZHONG_ZC" },
                // 事权等级
                { "shi_quan_d", "SHI_QUAN_D" },
                // 公益林变化类型
                { "gyl_bhlx", "GYL_BHLX" },
                // 林地管理类型
                { "ldgl_lx", "LDGL_LX" },
                // 林地保护等级
                { "bh_dj", "BH_DJ" },
                // 林地质量等级
                { "zl_dj", "ZL_DJ" },
                // 草原分区
                { "cy_fq", "CY_FQ" },
                // 草原起源
                { "cd_qy", "CD_QY" },
                // 草原类
                { "cd_l", "CD_L" },
                // 草原型
                { "cd_xing", "CD_XING" },
                // 优势草种
                { "ys_caoz", "YS_CAOZ" },
                // 功能类别
                { "cdgn", "CDGN" },
                // 单位面积鲜草产量
                { "xc_cl", "XC_CL" },
                // 小班鲜草产量
                { "xb_xccl", "XB_XCCL" },
                // 可食牧草比例
                { "ksmcbl", "KSMCBL" },
                // 小班干草产量
                { "xb_gccl", "XB_GCCL" },
                // 小班可食干牧草产量
                { "xb_ksgccl", "XB_KSGCCL" },
                // 小班可食鲜牧草产量
                { "xb_ksxccl", "XB_KSXCCL" },
                // 利用方式
                { "cdlyfs", "CDLYFS" },
                // 放牧时长
                { "fm_shch", "FM_SHCH" },
                // 草原类别
                { "cylb", "CYLB" },
                // 裸斑面积比例
                { "lbmj_bl", "LBMJ_BL" },
                // 草原退化类型
                { "thlx", "THLX" },
                // 草原退化程度
                { "thcd", "THCD" },
                // 禁牧与草畜平衡
                { "jm_ph", "JM_PH" },
                // 草原管理类型
                { "cdgl_lx", "CDGL_LX" },
                // 湿地管理分级
                { "sd_dj", "SD_DJ" },
                // 重要湿地名称
                { "zysd_bm", "ZYSD_BM" },
                // 湿地保护形式
                { "bhdsx", "BHDSX" },
                // 自然保护地
                { "bhddm", "BHDDM" },
                // 湿地利用方式
                { "sdlyfs", "SDLYFS" },
                // 受威胁状况
                { "sdwxzk", "SDWXZK" },
                // 湿地管理类型
                { "sdgllx", "SDGLLX" },
                // 荒漠调查区类型
                { "dcqlx", "DCQLX" },
                // 气候类型
                { "qhlx", "QHLX" },
                // 沙化类型
                { "shlx", "SHLX" },
                // 沙化程度
                { "shcd", "SHCD" },
                // 所属沙漠沙地
                { "sssmsd", "SSSMSD" },
                // 荒漠化类型
                { "hmhlx", "HMHLX" },
                // 荒漠化程度
                { "hmhcd", "HMHCD" },
                // 基岩裸露度/土壤砾石含量
                { "trls", "TRLS" },
                // 覆沙厚度
                { "fshd", "FSHD" },
                // 沙丘高度
                { "sqgd", "SQGD" },
                // 侵蚀沟面积比例
                { "qsg", "QSG" },
                // 盐碱斑占地率
                { "yjb", "YJB" },
                // 作物产量下降率
                { "zwclxjl", "ZWCLXJL" },
                // 作物缺苗率
                { "zwqml", "ZWQML" },
                // 作物产量
                { "zwcl", "ZWCL" },
                // 灌溉能力
                { "ggnl", "GGNL" },
                // 农田林网化率
                { "ntlwhl", "NTLWHL" },
                // 风蚀状况
                { "fszk", "FSZK" },
                // 土壤表层结构
                { "trbcjg", "TRBCJG" },
                // 荒漠化沙化可治理度
                { "kzld", "KZLD" },
                // 治理措施
                { "zlcs", "ZLCS" },
                // 治理程度
                { "zlcd", "ZLCD" },
                // 石漠化状况
                { "smhzk", "SMHZK" },
                // 石漠化程度
                { "smhcd", "SMHCD" },
                // 石漠化演变类型
                { "smhyblx", "SMHYBLX" },
                // 岩溶地貌
                { "yrdm", "YRDM" },
                // 核实标记
                { "hsbj", "HSBJ" },
                // 光伏用地标注
                { "gfyd", "GFYD" },
                // 调查人员
                { "dc_ry", "DC_RY" },
                // 调查日期
                { "dc_rq", "DC_RQ" }
            };
        }
    }
}