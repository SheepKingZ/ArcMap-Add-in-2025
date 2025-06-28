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
        // 新增私有字段存储路径
        private string forestSurveyDataPath = "";
        private string urbanBoundaryDataPath = "";
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

            // 初始化新增数据源路径状态
            if (!string.IsNullOrEmpty(forestSurveyDataPath))
            {
                txtForestSurveyDataPath.Text = forestSurveyDataPath;
                txtForestSurveyDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtForestSurveyDataPath.Text = "请选择林草湿荒普查数据源文件夹";
                txtForestSurveyDataPath.ForeColor = Color.Gray;
            }

            if (!string.IsNullOrEmpty(urbanBoundaryDataPath))
            {
                txtUrbanBoundaryDataPath.Text = urbanBoundaryDataPath;
                txtUrbanBoundaryDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtUrbanBoundaryDataPath.Text = "请选择城镇开发边界数据源文件夹";
                txtUrbanBoundaryDataPath.ForeColor = Color.Gray;
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
            bool hasForestSurveyData = !string.IsNullOrEmpty(forestSurveyDataPath);
            bool hasUrbanBoundaryData = !string.IsNullOrEmpty(urbanBoundaryDataPath);
            bool hasOutputGDB = !string.IsNullOrEmpty(outputGDBPath);

            // 基础数据源选择按钮始终启用
            btnBrowseForestSurveyData.Enabled = true;
            btnBrowseUrbanBoundaryData.Enabled = true;
            btnBrowseOutputGDB.Enabled = true;

            // OK按钮需要所有必要信息都完成后才启用
            bool allDataSourcesSelected = hasWorkspace && hasForestSurveyData && hasUrbanBoundaryData && hasOutputGDB;
            
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

        // 新增事件处理方法：林草湿荒普查数据源浏览
        private void BtnBrowseForestSurveyData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择林草湿荒普查数据源文件夹";
                if (!string.IsNullOrEmpty(forestSurveyDataPath))
                {
                    dialog.SelectedPath = forestSurveyDataPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    forestSurveyDataPath = dialog.SelectedPath;
                    txtForestSurveyDataPath.Text = forestSurveyDataPath;
                    txtForestSurveyDataPath.ForeColor = Color.DarkGreen;
                    
                    // 查找包含LCXZGX_P的文件
                    List<ForestResourcePlugin.LCXZGXFileInfo> lcxzgxFiles = FindFilesWithPattern(forestSurveyDataPath, "LCXZGX_P");
                    
                    // 保存到共享数据管理器
                    ForestResourcePlugin.SharedDataManager.SetLCXZGXFiles(lcxzgxFiles);
                    
                    if (lcxzgxFiles.Count > 0)
                    {
                        MessageBox.Show($"已找到 {lcxzgxFiles.Count} 个包含LCXZGX_P的文件。", "文件搜索结果", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    MessageBox.Show("林草湿荒普查数据源选择完成。", "成功", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        // 新增事件处理方法：城镇开发边界数据源浏览
        private void BtnBrowseUrbanBoundaryData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择城镇开发边界数据源文件夹";
                if (!string.IsNullOrEmpty(urbanBoundaryDataPath))
                {
                    dialog.SelectedPath = urbanBoundaryDataPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    urbanBoundaryDataPath = dialog.SelectedPath;
                    txtUrbanBoundaryDataPath.Text = urbanBoundaryDataPath;
                    txtUrbanBoundaryDataPath.ForeColor = Color.DarkGreen;
                    
                    // 查找包含CZKFBJ的文件
                    List<ForestResourcePlugin.LCXZGXFileInfo> czkfbjFiles = FindFilesWithPattern(urbanBoundaryDataPath, "CZKFBJ");
                    
                    // 保存到共享数据管理器
                    ForestResourcePlugin.SharedDataManager.SetCZKFBJFiles(czkfbjFiles);
                    
                    if (czkfbjFiles.Count > 0)
                    {
                        MessageBox.Show($"已找到 {czkfbjFiles.Count} 个包含CZKFBJ的文件。", "文件搜索结果", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                    MessageBox.Show("城镇开发边界数据源选择完成。", "成功", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        // 新增事件处理方法：输出GDB路径浏览
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

            if (string.IsNullOrEmpty(forestSurveyDataPath))
            {
                MessageBox.Show("请选择林草湿荒普查数据源。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(urbanBoundaryDataPath))
            {
                MessageBox.Show("请选择城镇开发边界数据源。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                              $"林草湿荒普查数据源：{forestSurveyDataPath}\n" +
                              $"城镇开发边界数据源：{urbanBoundaryDataPath}\n" +
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

        // 公共属性，供外部访问选择的路径
        public string ForestSurveyDataPath => forestSurveyDataPath;
        public string UrbanBoundaryDataPath => urbanBoundaryDataPath;
        public string OutputGDBPath => outputGDBPath;
        public bool CreateCountyFolders => chkCreateCountyFolders.Checked;

        private void BasicDataPreparationForm_Load(object sender, EventArgs e)
        {

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
                            // 提取第二级文件夹名称作为显示名称
                            string displayName = ExtractSecondLevelFolderName(filePath, rootDir);
                            
                            result.Add(new ForestResourcePlugin.LCXZGXFileInfo 
                            { 
                                FullPath = filePath,
                                DisplayName = displayName,
                                IsGdb = false,
                                GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon // 假设为面
                            });
                            
                            matchCount++;
                            System.Diagnostics.Debug.WriteLine($"找到匹配的Shapefile文件[{matchCount}]: {filePath}, 显示名称: {displayName}");
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
        private string ExtractSecondLevelFolderName(string filePath, string rootDir)
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
                    // 路径不匹配，使用完整路径的文件夹
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }
                
                // 分割路径并获取第二级目录名称
                string[] pathParts = relativePath.Split('\\', '/');
                if (pathParts.Length >= 2)
                {
                    return pathParts[1]; // 第二级目录
                }
                else if (pathParts.Length == 1)
                {
                    // 如果没有第二级目录，返回文件名（不含扩展名）
                    return System.IO.Path.GetFileNameWithoutExtension(filePath);
                }
                else
                {
                    // 兜底方案
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"提取文件夹名称时出错: {ex.Message}");
                return System.IO.Path.GetFileNameWithoutExtension(filePath);
            }
        }
    }
}