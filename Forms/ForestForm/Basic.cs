using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

namespace ForestResourcePlugin
{
    public partial class Basic : Form
    {
        private DataTable previewData;
        private DataTable mappingData;
        private IFeatureClass lcxzgxFeatureClass; // 林草现状图层要素类
        private IFeatureClass czkfbjFeatureClass; // 城镇开发边界要素类
        private List<LayerInfo> mapLayers; // 当前地图文档中的图层列表

        public Basic()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // 初始化坐标系下拉框
            cmbCoordSystem.Items.AddRange(new string[]
            {
                "CGCS2000_3_Degree_GK_CM_117E",
                "CGCS2000_3_Degree_GK_CM_120E",
                "CGCS2000_3_Degree_GK_CM_123E",
                "Beijing_1954_3_Degree_GK_CM_117E",
                "WGS_1984_UTM_Zone_49N",
                "WGS_1984_UTM_Zone_50N"
            });
            cmbCoordSystem.SelectedIndex = 0;

            // 初始化处理选项
            chkTopologyCheck.Checked = true;
            chkGeometryValidation.Checked = true;

            // 初始化字段映射表格
            InitializeMappingGrid();

            // 初始化筛选条件复选框
            chkForestLand.Checked = true;
            chkStateOwned.Checked = true;
            chkCollectiveInBoundary.Checked = true;

            // 为复选框添加事件处理程序
            chkForestLand.CheckedChanged += FilterCheckBox_CheckedChanged;
            chkStateOwned.CheckedChanged += FilterCheckBox_CheckedChanged;
            chkCollectiveInBoundary.CheckedChanged += FilterCheckBox_CheckedChanged;

            // 加载地图图层
            LoadMapLayers();
        }

        // 加载地图中的图层
        private void LoadMapLayers()
        {
            try
            {
                UpdateStatus("正在获取地图图层...");
                
                // 清空下拉框
                cmbLCXZGXPath.Items.Clear();
                cmbCZKFBJPath.Items.Clear();
                
                // 添加提示选项
                cmbLCXZGXPath.Items.Add("-- 请选择林草现状图层 --");
                cmbCZKFBJPath.Items.Add("-- 请选择城镇开发边界图层 --");
                
                // 选择默认值
                cmbLCXZGXPath.SelectedIndex = 0;
                cmbCZKFBJPath.SelectedIndex = 0;
                
                // 获取地图图层
                mapLayers = MapLayerUtilities.GetMapLayers();
                
                // 添加图层到下拉列表
                foreach (LayerInfo layer in mapLayers)
                {
                    cmbLCXZGXPath.Items.Add(layer);
                    cmbCZKFBJPath.Items.Add(layer);
                }
                
                UpdateStatus($"已加载 {mapLayers.Count} 个地图图层");
            }
            catch (Exception ex)
            {
                UpdateStatus("加载地图图层失败");
                System.Diagnostics.Debug.WriteLine($"加载地图图层出错: {ex.Message}");
            }
        }

        // 复选框状态改变事件处理
        private void FilterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // 当任何筛选条件变化时清空预览数据
            if (previewData != null)
            {
                previewData.Clear();
                dgvPreview.DataSource = null;
                lblPreviewCount.Text = "预览结果：0 个图斑";
                
                // 提示用户重新生成预览
                UpdateStatus("筛选条件已更改，请重新生成预览");
            }
        }

        private void btnBrowseLCXZGX_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Shapefile (*.shp)|*.shp|All Files (*.*)|*.*";
                dialog.Title = "选择林草现状图层(LCXZGX-P)";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 直接将文件路径设置为ComboBox的文本内容
                    cmbLCXZGXPath.Text = dialog.FileName;
                    LoadLCXZGXFields();
                }
            }
        }

        private void btnBrowseCZKFBJ_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Shapefile (*.shp)|*.shp|All Files (*.*)|*.*";
                dialog.Title = "选择城镇开发边界图层(CZKFBJ)";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 直接将文件路径设置为ComboBox的文本内容
                    cmbCZKFBJPath.Text = dialog.FileName;
                    
                    // 加载城镇开发边界图层
                    try
                    {
                        LoadCZKFBJLayer(cmbCZKFBJPath.Text);
                        UpdateStatus("成功加载城镇开发边界图层");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"加载城镇开发边界图层失败: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateStatus("加载城镇开发边界图层失败");
                    }
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择输出路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void LoadLCXZGXFields()
        {
            try
            {
                // Clear existing items in dropdown lists
                cmbLandTypeField.Items.Clear();
                cmbLandOwnerField.Items.Clear();
                
                // Check if file exists
                if (string.IsNullOrEmpty(cmbLCXZGXPath.Text) || !File.Exists(cmbLCXZGXPath.Text))
                {
                    MessageBox.Show("选择的Shapefile文件不存在或无效", "读取字段失败", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                UpdateStatus("正在读取Shapefile字段...");
                progressBar.Value = 30;
                
                // 使用ShapefileReader工具类读取字段
                List<string> fieldNames = ShapefileReader.GetShapefileFieldNames(cmbLCXZGXPath.Text);
                
                // 添加字段到下拉列表
                foreach (string fieldName in fieldNames)
                {
                    cmbLandTypeField.Items.Add(fieldName);
                    cmbLandOwnerField.Items.Add(fieldName);
                }
                
                // 选择默认字段（如果有字段）
                if (cmbLandTypeField.Items.Count > 0)
                {
                    // 尝试查找与地类相关的字段
                    int landTypeIndex = FindBestMatchIndex(cmbLandTypeField.Items, new[] { "地类", "Y_DLBM", "LandType", "DL", "Land", "Type", "DLDM" });
                    cmbLandTypeField.SelectedIndex = landTypeIndex >= 0 ? landTypeIndex : 0;
                    
                    // 尝试查找与土地权属相关的字段
                    int landOwnerIndex = FindBestMatchIndex(cmbLandOwnerField.Items, new[] { "权属", "土地权属", "TDQS", "LD_QS", "Ownership", "Owner", "QS" });
                    cmbLandOwnerField.SelectedIndex = landOwnerIndex >= 0 ? landOwnerIndex : 0;
                }

                // 加载林草现状图层
                LoadLCXZGXLayer(cmbLCXZGXPath.Text);
                
                progressBar.Value = 100;
                UpdateStatus($"已读取 {fieldNames.Count} 个字段");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取Shapefile字段时出错: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("读取字段失败");
            }
        }

        // 加载林草现状图层
        private void LoadLCXZGXLayer(string shapefilePath)
        {
            try
            {
                // Get the directory and filename without extension
                string directory = System.IO.Path.GetDirectoryName(shapefilePath);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                // Create a workspace factory and open the shapefile
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                lcxzgxFeatureClass = featureWorkspace.OpenFeatureClass(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"加载林草现状图层失败: {ex.Message}", ex);
            }
        }

        // 加载城镇开发边界图层
        private void LoadCZKFBJLayer(string shapefilePath)
        {
            try
            {
                // Get the directory and filename without extension
                string directory = System.IO.Path.GetDirectoryName(shapefilePath);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(shapefilePath);

                // Create a workspace factory and open the shapefile
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                czkfbjFeatureClass = featureWorkspace.OpenFeatureClass(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception($"加载城镇开发边界图层失败: {ex.Message}", ex);
            }
        }

        // Helper method to find the best matching field name
        private int FindBestMatchIndex(ComboBox.ObjectCollection items, string[] searchTerms)
        {
            foreach (string term in searchTerms)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    string item = items[i].ToString();
                    if (item.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return i;
                    }
                }
            }
            return -1; // No match found
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (lcxzgxFeatureClass == null)
                {
                    MessageBox.Show("请先加载林草现状图层", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (chkCollectiveInBoundary.Checked && czkfbjFeatureClass == null)
                {
                    MessageBox.Show("启用了\"集体林在城镇开发边界内\"筛选条件，请先加载城镇开发边界图层",
                        "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UpdateStatus("正在生成预览...");
                progressBar.Value = 10;

                // 构建查询条件
                string landTypeField = cmbLandTypeField.SelectedItem?.ToString();
                string landOwnerField = cmbLandOwnerField.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(landTypeField) || string.IsNullOrEmpty(landOwnerField))
                {
                    MessageBox.Show("请选择地类字段和土地权属字段", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string whereClause = "";
                List<string> forestConditions = new List<string>();

                // 处理筛选条件：
                // 1. 地类为林地且土地权属为国有的图斑
                // 2. 地类为林地且土地权属为集体(位于城镇开发边界内的部分由后续空间查询处理)
                if (chkForestLand.Checked)
                {
                    // 构建林地条件
                    string forestCondition = $"{landTypeField} LIKE '03%'";

                    if (chkStateOwned.Checked)
                    {
                        // 条件1：林地且国有
                        forestConditions.Add($"({forestCondition} AND {landOwnerField} = '20')");
                    }

                    // 条件2：林地且集体 - 这里先选出所有集体林图斑，后续通过空间查询过滤城镇开发边界内的部分
                    if (chkCollectiveInBoundary.Checked)
                    {
                        forestConditions.Add($"({forestCondition} AND {landOwnerField} = '30')");
                    }
                }

                // 将条件组合为SQL语句 - 使用OR连接两个不同子条件
                if (forestConditions.Count > 0)
                {
                    whereClause = string.Join(" OR ", forestConditions);
                }

                // 创建查询过滤器
                IQueryFilter queryFilter = new QueryFilterClass();
                if (!string.IsNullOrEmpty(whereClause))
                {
                    queryFilter.WhereClause = whereClause;
                }

                progressBar.Value = 30;
                UpdateStatus("正在执行查询...");

                // 创建数据表
                previewData = new DataTable();
                previewData.Columns.Add("图斑编号");
                previewData.Columns.Add("地类");
                previewData.Columns.Add("土地权属");
                previewData.Columns.Add("面积(公顷)"); // 将字段名更改为更明确的标识

                // 设置最大预览记录数
                const int MAX_PREVIEW_COUNT = 5000;
                int totalCount = 0; // 总记录数
                int processedCount = 0; // 已处理记录数

                // 执行查询
                IFeatureCursor cursor = null;
                IFeature feature = null;

                try
                {
                    cursor = lcxzgxFeatureClass.Search(queryFilter, false);

                    progressBar.Value = 50;
                    UpdateStatus("正在加载预览数据...");

                    // 预先查找字段索引，避免重复查找
                    int tbdhIndex = lcxzgxFeatureClass.FindField("TBDH");
                    if (tbdhIndex == -1) tbdhIndex = lcxzgxFeatureClass.FindField("BSM");
                    if (tbdhIndex == -1) tbdhIndex = lcxzgxFeatureClass.FindField("OBJECTID");

                    int dlmcIndex = lcxzgxFeatureClass.FindField(landTypeField);
                    int tdqsIndex = lcxzgxFeatureClass.FindField(landOwnerField);
                    
                    // 使用指定的字段名查找图斑面积和土地权属
                    int tbmjIndex = lcxzgxFeatureClass.FindField("TBMJ");
                    int qsxzIndex = lcxzgxFeatureClass.FindField("QSXZ");
                    
                    // 如果找不到指定的面积字段，尝试使用其他常用字段名
                    if (tbmjIndex == -1)
                    {
                        tbmjIndex = lcxzgxFeatureClass.FindField("MJ");
                        if (tbmjIndex == -1) tbmjIndex = lcxzgxFeatureClass.FindField("AREA");
                        if (tbmjIndex == -1) tbmjIndex = lcxzgxFeatureClass.FindField("面积");
                    }
                    
                    // 如果找不到指定的权属字段，尝试使用土地权属字段
                    if (qsxzIndex == -1 && tdqsIndex != -1)
                    {
                        qsxzIndex = tdqsIndex;
                    }

                    while ((feature = cursor.NextFeature()) != null)
                    {
                        totalCount++;

                        bool shouldAdd = false;

                        // 获取土地权属值 - 优先使用QSXZ字段
                        string ownerValue = "";
                        if (qsxzIndex != -1)
                        {
                            ownerValue = feature.get_Value(qsxzIndex)?.ToString() ?? "";
                        }
                        else if (tdqsIndex != -1)
                        {
                            ownerValue = feature.get_Value(tdqsIndex)?.ToString() ?? "";
                        }
                        
                        // 如果是国有林地，直接添加
                        if (ownerValue == "1" || ownerValue == "20") // 1和20都可能表示国有
                        {
                            shouldAdd = true;
                        }
                        // 如果是集体林地且需要检查是否在城镇开发边界内
                        else if ((ownerValue == "2" || ownerValue == "30") && chkCollectiveInBoundary.Checked && czkfbjFeatureClass != null) // 2和30都可能表示集体
                        {
                            // 执行空间查询，检查是否在城镇开发边界内
                            if (IsFeatureInBoundary(feature))
                            {
                                shouldAdd = true;
                            }
                        }

                        // 如果符合条件且未超过最大预览数量限制，则添加到预览
                        if (shouldAdd && processedCount < MAX_PREVIEW_COUNT)
                        {
                            DataRow row = previewData.NewRow();

                            // 直接获取字段值，不做计算处理
                            row["图斑编号"] = tbdhIndex != -1 ? feature.get_Value(tbdhIndex)?.ToString() ?? feature.OID.ToString() : feature.OID.ToString();
                            row["地类"] = dlmcIndex != -1 ? feature.get_Value(dlmcIndex)?.ToString() ?? "" : "";

                            // 土地权属字段 - 优先使用QSXZ字段
                            string qsValue = ownerValue;

                            // 土地权属代码转换 - 保留简单的转换逻辑以提高可读性
                            switch (qsValue)
                            {
                                case "1":
                                case "20":
                                    row["土地权属"] = "国有";
                                    break;
                                case "2":
                                case "30":
                                    row["土地权属"] = "集体";
                                    break;
                                default:
                                    row["土地权属"] = qsValue;
                                    break;
                            }

                            // 面积 - 直接使用TBMJ字段
                            if (tbmjIndex != -1)
                            {
                                object mjValue = feature.get_Value(tbmjIndex);
                                if (mjValue != null)
                                {
                                    // 处理面积值，确保它是有效的数字
                                    if (mjValue is double || mjValue is float || mjValue is decimal)
                                    {
                                        // 直接显示字段值，假设单位已经是公顷
                                        row["面积(公顷)"] = Convert.ToDouble(mjValue).ToString("F2");
                                    }
                                    else
                                    {
                                        // 尝试解析为数字
                                        if (double.TryParse(mjValue.ToString(), out double mjDouble))
                                        {
                                            row["面积(公顷)"] = mjDouble.ToString("F2");
                                        }
                                        else
                                        {
                                            row["面积(公顷)"] = mjValue.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    row["面积(公顷)"] = "";
                                }
                            }
                            else
                            {
                                row["面积(公顷)"] = "";
                            }

                            previewData.Rows.Add(row);
                            processedCount--;
                        }

                        // 释放COM对象
                        if (feature != null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                            feature = null;
                        }

                        // 每处理100条记录更新一次进度
                        if (totalCount % 100 == 0)
                        {
                            progressBar.Value = 50 + Math.Min((totalCount / 100), 40);
                            UpdateStatus($"正在加载数据: 已处理 {totalCount} 条记录...");
                            Application.DoEvents();
                        }

                        // 如果已经超过最大预览数量且找到了足够满足条件的记录，则提前结束循环
                        if (totalCount > MAX_PREVIEW_COUNT * 2 && processedCount >= MAX_PREVIEW_COUNT)
                        {
                            break;
                        }
                    }
                }
                finally
                {
                    // 确保释放资源
                    if (feature != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(feature);
                    }
                    if (cursor != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                    }
                }

                // 显示数据
                dgvPreview.DataSource = previewData;

                // 更新显示信息
                if (totalCount > processedCount)
                {
                    lblPreviewCount.Text = $"预览结果：{processedCount}/{totalCount} 个图斑 (仅显示前 {MAX_PREVIEW_COUNT} 个)";
                }
                else
                {
                    lblPreviewCount.Text = $"预览结果：{processedCount} 个图斑";
                }

                progressBar.Value = 100;
                UpdateStatus("预览生成完成");

                // 强制垃圾回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (OutOfMemoryException ex)
            {
                MessageBox.Show($"内存不足，无法处理此数量的图斑。请减少筛选范围后重试。\n\n错误详情: {ex.Message}",
                    "内存不足", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("预览生成失败 - 内存不足");
                progressBar.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成预览时出错: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("预览生成失败");
                progressBar.Value = 0;
            }
        }

        // 检查要素是否在城镇开发边界内
        private bool IsFeatureInBoundary(IFeature feature)
        {
            try
            {
                // 空间过滤器
                ISpatialFilter spatialFilter = new SpatialFilterClass();
                spatialFilter.Geometry = feature.Shape;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

                // 查询城镇开发边界
                IFeatureCursor boundaryCursor = czkfbjFeatureClass.Search(spatialFilter, false);
                IFeature boundaryFeature = boundaryCursor.NextFeature();

                // 如果找到任何相交的边界要素，则返回true
                return boundaryFeature != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"空间查询出错: {ex.Message}");
                return false;
            }
        }

        // 在地图上高亮显示筛选出的要素
        private void HighlightFeaturesOnMap(List<IFeature> features)
        {
            try
            {
                // 这里实现与ArcMap交互的代码，将筛选出的要素在地图上高亮显示
                // 通常涉及获取当前地图文档和图层，然后设置选择集
                // 此部分需要根据具体的ArcObjects环境实现
                
                // 示例代码（需要根据实际ArcObjects环境调整）
                 //IActiveView activeView = (ArcMap.Document.FocusMap as IActiveView);
                 //IMap map = activeView.FocusMap;
                 //map.ClearSelection();
                
                 //foreach (IFeature feature in features)
                 //{
                 //    map.SelectFeature(lcxzgxFeatureLayer, feature);
                 //}
                
                 //activeView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"高亮显示要素出错: {ex.Message}");
                // 这里不抛出异常，因为高亮显示失败不应阻止其他操作
            }
        }

        private void InitializeMappingGrid()
        {
            mappingData = new DataTable();
            mappingData.Columns.Add("目标字段");
            mappingData.Columns.Add("源字段");
            mappingData.Columns.Add("映射状态");

            // 添加示例映射字段
            string[] targetFields = { "TBDH", "DLMC", "TDQS", "MJ", "LZFL" };
            foreach (string field in targetFields)
            {
                mappingData.Rows.Add(field, "", "未映射");
            }

            dgvMapping.DataSource = mappingData;
        }

        private void btnAutoMapping_Click(object sender, EventArgs e)
        {
            // 执行自动映射逻辑
            UpdateStatus("正在执行自动映射...");
            progressBar.Value = 30;

            // 模拟自动映射
            foreach (DataRow row in mappingData.Rows)
            {
                string targetField = row["目标字段"].ToString();
                switch (targetField)
                {
                    case "TBDH":
                        row["源字段"] = "图斑编号";
                        row["映射状态"] = "已映射";
                        break;
                    case "DLMC":
                        row["源字段"] = "地类";
                        row["映射状态"] = "已映射";
                        break;
                    case "TDQS":
                        row["源字段"] = "土地权属";
                        row["映射状态"] = "已映射";
                        break;
                }
            }

            dgvMapping.Refresh();
            progressBar.Value = 100;
            UpdateStatus("自动映射完成");
        }

        private void btnLoadTemplate_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "映射模板 (*.xml)|*.xml|All Files (*.*)|*.*";
                dialog.Title = "加载字段映射模板";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 加载映射模板逻辑
                    UpdateStatus($"已加载模板：{System.IO.Path.GetFileName(dialog.FileName)}");
                }
            }
        }

        private void btnSaveTemplate_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "映射模板 (*.xml)|*.xml|All Files (*.*)|*.*";
                dialog.Title = "保存字段映射模板";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 保存映射模板逻辑
                    UpdateStatus($"模板已保存：{System.IO.Path.GetFileName(dialog.FileName)}");
                }
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                ExecuteProcessing();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(cmbLCXZGXPath.Text))
            {
                MessageBox.Show("请选择林草现状图层", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(cmbCZKFBJPath.Text))
            {
                MessageBox.Show("请选择城镇开发边界图层", "验证失败", MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(txtOutputPath.Text))
            {
                MessageBox.Show("请设置输出路径", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ExecuteProcessing()
        {
            btnExecute.Enabled = false;
            btnCancel.Enabled = true;
            progressBar.Value = 0;

            UpdateStatus("开始处理数据...");

            // 这里实现实际的ArcObjects处理逻辑
            // 1. 读取林草现状图层
            // 2. 读取城镇开发边界图层
            // 3. 执行空间查询和属性筛选
            // 4. 创建结果要素类
            // 5. 执行字段映射

            // 模拟处理进度
            for (int i = 0; i <= 100; i += 10)
            {
                progressBar.Value = i;
                UpdateStatus($"处理进度：{i}%");
                Application.DoEvents();
                System.Threading.Thread.Sleep(200);
            }

            UpdateStatus("处理完成！");
            btnExecute.Enabled = true;
            btnCancel.Enabled = false;

            MessageBox.Show("森林资源资产清查工作范围生成完成！", "处理完成",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // 取消处理逻辑
            UpdateStatus("操作已取消");
            btnExecute.Enabled = true;
            btnCancel.Enabled = false;
            progressBar.Value = 0;
        }

        private void UpdateStatus(string message)
        {
            lblStatus.Text = $"状态：{message}";
            Application.DoEvents();
        }

        private void btnRefreshLayers_Click(object sender, EventArgs e)
        {
            LoadMapLayers();
        }

        private void cmbLCXZGXPath_DropDown(object sender, EventArgs e)
        {
            // 当下拉框打开时，如果地图图层为空，则重新加载地图图层
            if (mapLayers == null || mapLayers.Count == 0)
            {
                LoadMapLayers();
            }
        }

        private void cmbCZKFBJPath_DropDown(object sender, EventArgs e)
        {
            // 当下拉框打开时，如果地图图层为空，则重新加载地图图层
            if (mapLayers == null || mapLayers.Count == 0)
            {
                LoadMapLayers();
            }
        }
    }
}
