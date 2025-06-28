using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // 用于CountySelectionForm
using System.IO;

namespace TestArcMapAddin2.Forms
{
    public partial class BasicDataPreparationForm : Form
    {
        // 修改私有字段 - 合并林草湿荒普查数据和城镇开发边界数据路径为一个
        private string dataSourcePath = "";
        private string outputGDBPath = "";

        public BasicDataPreparationForm()
        {
            InitializeComponent();
            InitializeFormState();
        }

        private void InitializeFormState()
        {
            // 根据当前共享状态或默认值初始化标签
            lblWorkspace.Text = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? "未选择工作空间" : SharedWorkflowState.WorkspacePath;
            lblWorkspace.ForeColor = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? Color.Black : Color.DarkGreen;

            // 初始化合并后的数据源路径状态
            if (!string.IsNullOrEmpty(dataSourcePath))
            {
                txtDataPath.Text = dataSourcePath;
                txtDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtDataPath.Text = "请选择包含林草湿荒普查与城镇开发边界数据的文件夹";
                txtDataPath.ForeColor = Color.Gray;
            }

            if (!string.IsNullOrEmpty(outputGDBPath))
            {
                txtOutputGDBPath.Text = outputGDBPath;
                txtOutputGDBPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtOutputGDBPath.Text = "请选择输出结果GDB路径";
                txtOutputGDBPath.ForeColor = Color.Gray;
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasWorkspace = !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath);
            bool hasDataSource = !string.IsNullOrEmpty(dataSourcePath);
            bool hasOutputGDB = !string.IsNullOrEmpty(outputGDBPath);

            // 基础数据源选择按钮始终启用
            btnBrowseData.Enabled = true;
            btnBrowseOutputGDB.Enabled = true;

            // OK按钮需要所有必要信息都完成后才启用
            bool allDataSourcesSelected = hasWorkspace && hasDataSource && hasOutputGDB;
            
            btnOK.Enabled = allDataSourcesSelected;
        }

        private void BtnSelectWorkspace_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择目标数据库路径（File Geodatabase .gdb）";
                if (!string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
                {
                    dialog.SelectedPath = SharedWorkflowState.WorkspacePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.SelectedPath.EndsWith(".gdb", StringComparison.OrdinalIgnoreCase) || System.IO.Directory.Exists(dialog.SelectedPath))
                    {
                        SharedWorkflowState.WorkspacePath = dialog.SelectedPath;
                        lblWorkspace.Text = SharedWorkflowState.WorkspacePath;
                        lblWorkspace.ForeColor = Color.DarkGreen;
                        InitializeFormState(); // 重新初始化以反映状态变化
                    }
                    else
                    {
                        MessageBox.Show("请选择有效的File Geodatabase (.gdb) 路径！", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            UpdateButtonStates();
        }

        // 新的合并后的数据源浏览方法
        private void BtnBrowseData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择包含林草湿荒普查与城镇开发边界数据的文件夹";
                if (!string.IsNullOrEmpty(dataSourcePath))
                {
                    dialog.SelectedPath = dataSourcePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    dataSourcePath = dialog.SelectedPath;
                    txtDataPath.Text = dataSourcePath;
                    txtDataPath.ForeColor = Color.DarkGreen;
                    
                    // 保存到共享状态中
                    SharedWorkflowState.DataSourcePath = dataSourcePath;
                    
                    // 查找包含LCXZGX_P的文件（林草湿荒普查数据）
                    List<ForestResourcePlugin.LCXZGXFileInfo> lcxzgxFiles = FindFilesWithPattern(dataSourcePath, "LCXZGX_P");
                    
                    // 保存到共享数据管理器
                    ForestResourcePlugin.SharedDataManager.SetLCXZGXFiles(lcxzgxFiles);
                    
                    // 查找包含CZKFBJ的文件（城镇开发边界数据）
                    List<ForestResourcePlugin.LCXZGXFileInfo> czkfbjFiles = FindFilesWithPattern(dataSourcePath, "CZKFBJ");
                    
                    // 保存到共享数据管理器
                    ForestResourcePlugin.SharedDataManager.SetCZKFBJFiles(czkfbjFiles);
                    
                    // 显示文件搜索结果
                    int totalFiles = lcxzgxFiles.Count + czkfbjFiles.Count;
                    if (totalFiles > 0)
                    {
                        MessageBox.Show($"在同一文件夹中找到：\n- {lcxzgxFiles.Count} 个林草湿荒普查数据文件\n- {czkfbjFiles.Count} 个城镇开发边界数据文件", 
                            "文件搜索结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("未找到相关数据文件，请确认选择的文件夹是否正确。", "提示", 
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                    UpdateButtonStates();
                }
            }
        }

        // 输出GDB路径浏览方法保持不变
        private void BtnBrowseOutputGDB_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择输出结果GDB路径";
                if (!string.IsNullOrEmpty(outputGDBPath))
                {
                    dialog.SelectedPath = outputGDBPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputGDBPath = dialog.SelectedPath;
                    txtOutputGDBPath.Text = outputGDBPath;
                    txtOutputGDBPath.ForeColor = Color.DarkGreen;
                    
                    // 保存到共享状态
                    SharedWorkflowState.OutputGDBPath = outputGDBPath;
                    
                    MessageBox.Show("输出结果GDB路径选择完成。", "成功", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // 验证所有必需的路径都已选择
            if (string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
            {
                MessageBox.Show("请选择工作空间。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(dataSourcePath))
            {
                MessageBox.Show("请选择数据源文件夹。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("请选择输出结果GDB路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 可以在这里实现具体的业务逻辑，比如开始处理数据
            if (chkCreateCountyFolders.Checked)
            {
                MessageBox.Show($"准备为每个县创建文件夹并生成结果表格。\n" +
                              $"工作空间：{SharedWorkflowState.WorkspacePath}\n" +
                              $"数据源文件夹：{dataSourcePath}\n" +
                              $"输出GDB路径：{outputGDBPath}", 
                              "处理确认", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // 更新公共属性，供外部访问选择的路径
        public string DataSourcePath => dataSourcePath;
        public string OutputGDBPath => outputGDBPath;
        public bool CreateCountyFolders => chkCreateCountyFolders.Checked;

        private void BasicDataPreparationForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 提取路径中的第一级文件夹名称（县名）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="rootDir">根目录</param>
        /// <returns>第一级文件夹名称（县名）</returns>
        private string ExtractCountyNameFromPath(string filePath, string rootDir)
        {
            try
            {
                // 规范化路径
                string normalizedRoot = System.IO.Path.GetFullPath(rootDir).TrimEnd('\\', '/');
                string normalizedFile = System.IO.Path.GetFullPath(filePath);
                
                // 计算相对路径
                string relativePath = "";
                if (normalizedFile.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = normalizedFile.Substring(normalizedRoot.Length).TrimStart('\\', '/');
                }
                else
                {
                    // 路径不匹配，尝试从完整路径提取
                    System.Diagnostics.Debug.WriteLine($"警告: 文件路径 {normalizedFile} 不在根目录 {normalizedRoot} 下");
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }
                
                // 分割路径并获取第一级目录名称（县名）
                string[] pathParts = relativePath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 1)
                {
                    // 返回第一级目录名称，这应该是县名
                    string countyName = pathParts[0];
                    System.Diagnostics.Debug.WriteLine($"从路径 {relativePath} 提取县名: {countyName}");
                    return countyName;
                }
                else
                {
                    // 兜底方案：如果路径解析失败，使用文件名
                    string fallbackName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    System.Diagnostics.Debug.WriteLine($"警告: 无法从路径提取县名，使用文件名: {fallbackName}");
                    return fallbackName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"提取县名时出错: {ex.Message}");
                return System.IO.Path.GetFileNameWithoutExtension(filePath);
            }
        }

        /// <summary>
        /// 查找指定目录下名称包含特定字符串的文件
        /// </summary>
        /// <param name="rootDir">根目录</param>
        /// <param name="pattern">名称匹配模式</param>
        /// <returns>文件信息列表</returns>
        private List<ForestResourcePlugin.LCXZGXFileInfo> FindFilesWithPattern(string rootDir, string pattern)
        {
            var result = new List<ForestResourcePlugin.LCXZGXFileInfo>();
            
            try
            {
                System.Diagnostics.Debug.WriteLine($"开始在 {rootDir} 目录下查找包含 {pattern} 的文件");
                
                // 1. 首先查找GDB要素类
                System.Diagnostics.Debug.WriteLine("第1步：查找GDB要素类...");
                var gdbFeatureClasses = ForestResourcePlugin.GdbFeatureClassFinder.FindFeatureClassesWithPattern(
                    rootDir, pattern, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);
                
                // 将找到的GDB要素类添加到结果中
                result.AddRange(gdbFeatureClasses);
                System.Diagnostics.Debug.WriteLine($"找到 {gdbFeatureClasses.Count} 个GDB要素类");
                
                // 2. 再查找Shapefile文件
                System.Diagnostics.Debug.WriteLine("第2步：查找Shapefile文件...");
                
                // 确保目录存在
                if (Directory.Exists(rootDir))
                {
                    string[] files = System.IO.Directory.GetFiles(rootDir, "*.shp", System.IO.SearchOption.AllDirectories);
                    System.Diagnostics.Debug.WriteLine($"在 {rootDir} 目录下找到 {files.Length} 个Shapefile文件");
                    
                    // 筛选包含指定模式的Shapefile
                    int matchCount = 0;
                    foreach (string filePath in files)
                    {
                        if (filePath.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // 提取县名（第一级文件夹名称）
                            string countyName = ExtractCountyNameFromPath(filePath, rootDir);
                            
                            result.Add(new ForestResourcePlugin.LCXZGXFileInfo 
                            { 
                                FullPath = filePath,
                                DisplayName = countyName,
                                IsGdb = false,
                                GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon // 假设为面
                            });
                            
                            matchCount++;
                            System.Diagnostics.Debug.WriteLine($"找到匹配的Shapefile文件[{matchCount}]: {filePath}, 县名: {countyName}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 目录 {rootDir} 不存在");
                }
                
                System.Diagnostics.Debug.WriteLine($"共找到 {result.Count} 个匹配文件 (GDB要素类: {gdbFeatureClasses.Count}, Shapefile: {result.Count - gdbFeatureClasses.Count})");
                
                // 输出详细的结果信息
                for (int i = 0; i < result.Count; i++)
                {
                    var item = result[i];
                    System.Diagnostics.Debug.WriteLine($"结果[{i+1}]: {item.DisplayName}, 路径: {item.FullPath}, 类型: {(item.IsGdb ? "GDB要素类" : "Shapefile")}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找文件时出错: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                MessageBox.Show($"查找文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return result;
        }

        /// <summary>
        /// 提取路径中的第二级文件夹名称
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="rootDir">根目录</param>
        /// <returns>第二级文件夹名称</returns>
        [Obsolete("此方法已过时，请使用 ExtractCountyNameFromPath 方法")]
        private string ExtractSecondLevelFolderName(string filePath, string rootDir)
        {
            // 保留原方法以维持向后兼容性，但标记为过时
            // 现在调用新的县名提取方法
            return ExtractCountyNameFromPath(filePath, rootDir);
        }
    }
}