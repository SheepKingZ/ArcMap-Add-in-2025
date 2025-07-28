using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ForestResourcePlugin;
using TestArcMapAddin2.Utils;

namespace TestArcMapAddin2.Forms
{
    public partial class CZCDYDForm : Form
    {
        private string selectedSlzyzcPath = "";
        private string selectedCyzyzcPath = "";
        private string selectedSdzyzcPath = "";
        private string selectedCzcdydPath = "";
        private string selectedOutputPath = ""; // 新数据输出路径
        private string selectedResultOutputPath = ""; // 新增：现有结果文件夹路径

        // 存储按县代码分组的文件信息
        private Dictionary<string, CountyFileGroup> countyFileGroups = new Dictionary<string, CountyFileGroup>();

        // 存储选中的县列表
        private List<string> selectedCounties = new List<string>();

        // 存储发现的CZCDYDQC文件映射 (县代码 -> shapefile路径)
        private Dictionary<string, string> czcdydqcFileMap = new Dictionary<string, string>();

        public CZCDYDForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // 初始化控件状态
            btnOK.Enabled = false;
            progressBar.Value = 0;
            lblStatus.Text = "请先选择数据源文件夹...";
            lblStatus.ForeColor = Color.DarkBlue;

            // 初始化县选择列表
            InitializeCountySelectionList();
        }

        /// <summary>
        /// 初始化县选择列表界面
        /// </summary>
        private void InitializeCountySelectionList()
        {
            checkedListBoxCounties.CheckOnClick = true;
            checkedListBoxCounties.ItemCheck += CheckedListBoxCounties_ItemCheck;

            // 初始状态下禁用全选和全不选按钮
            btnSelectAll.Enabled = false;
            btnSelectNone.Enabled = false;
        }

        /// <summary>
        /// 县选择列表项目检查事件
        /// </summary>
        private void CheckedListBoxCounties_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var checkedListBox = sender as CheckedListBox;
            if (checkedListBox == null) return;

            // 延迟更新选中状态，因为ItemCheck事件在状态改变前触发
            this.BeginInvoke(new Action(() =>
            {
                selectedCounties.Clear();
                foreach (var item in checkedListBox.CheckedItems)
                {
                    var countyInfo = item.ToString();
                    // 从显示文本中提取县代码 "县名(县代码)"
                    var match = Regex.Match(countyInfo, @"\((\d{6})\)");
                    if (match.Success)
                    {
                        selectedCounties.Add(match.Groups[1].Value);
                    }
                }
                UpdateButtonStates();
                UpdateStatusLabel();
            }));
        }

        private void UpdateButtonStates()
        {
            bool hasValidCounties = countyFileGroups.Any(kvp => kvp.Value.IsComplete);
            bool hasSelectedCounties = selectedCounties.Count > 0 &&
                                     countyFileGroups.Any(kvp => selectedCounties.Contains(kvp.Key) && kvp.Value.IsComplete);

            btnOK.Enabled = hasSelectedCounties;
            btnSelectAll.Enabled = checkedListBoxCounties.Items.Count > 0;
            btnSelectNone.Enabled = checkedListBoxCounties.Items.Count > 0 && checkedListBoxCounties.CheckedItems.Count > 0;
        }

        /// <summary>
        /// 更新状态标签
        /// </summary>
        private void UpdateStatusLabel()
        {
            if (countyFileGroups.Count == 0)
            {
                lblStatus.Text = "请先选择数据源文件夹...";
                lblStatus.ForeColor = Color.DarkBlue;
            }
            else if (selectedCounties.Count == 0)
            {
                int completeCount = countyFileGroups.Count(kvp => kvp.Value.IsComplete);
                lblStatus.Text = $"已扫描到 {countyFileGroups.Count} 个县的数据，其中 {completeCount} 个县数据完整，请选择要处理的县...";
                lblStatus.ForeColor = Color.DarkOrange;
            }
            else
            {
                lblStatus.Text = $"已选择 {selectedCounties.Count} 个县进行处理，点击'开始处理'继续...";
                lblStatus.ForeColor = Color.DarkGreen;
            }
        }

        /// <summary>
        /// 更新县选择列表显示
        /// </summary>
        private void UpdateCountySelectionList()
        {
            checkedListBoxCounties.Items.Clear();

            foreach (var kvp in countyFileGroups.OrderBy(x => x.Key))
            {
                string countyCode = kvp.Key;
                var fileGroup = kvp.Value;

                // 使用CountyCodeMapper获取县名
                string countyName = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyNameFromCode(countyCode);

                // 显示格式：县名(县代码) - 文件完整性状态
                string displayText = $"{countyName}({countyCode})";
                if (!fileGroup.IsComplete)
                {
                    displayText += " - 文件不完整";
                }

                checkedListBoxCounties.Items.Add(displayText);

                // 如果文件完整，默认选中
                if (fileGroup.IsComplete)
                {
                    checkedListBoxCounties.SetItemChecked(checkedListBoxCounties.Items.Count - 1, true);
                }
            }

            // 更新按钮状态和状态标签
            UpdateButtonStates();
            UpdateStatusLabel();
        }

        #region 事件处理程序

        private void btnBrowseSlzyzc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择SLZYZC_DLTB源文件夹";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "正在扫描SLZYZC_DLTB文件...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedSlzyzcPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "SLZYZC_DLTB");
                        ProcessFoundFiles(files, FileType.SLZYZC_DLTB);

                        txtSlzyzcPath.Text = $"{dialog.SelectedPath} (找到 {files.Count} 个文件)";
                        txtSlzyzcPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "扫描SLZYZC_DLTB文件时出错";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"扫描SLZYZC_DLTB文件时出错: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseCyzyzc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择CYZYZC_DLTB源文件夹";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "正在扫描CYZYZC_DLTB文件...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedCyzyzcPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "CYZYZC_DLTB");
                        ProcessFoundFiles(files, FileType.CYZYZC_DLTB);

                        txtCyzyzcPath.Text = $"{dialog.SelectedPath} (找到 {files.Count} 个文件)";
                        txtCyzyzcPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "扫描CYZYZC_DLTB文件时出错";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"扫描CYZYZC_DLTB文件时出错: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseSdzyzc_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择SDZYZC_DLTB源文件夹";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "正在扫描SDZYZC_DLTB文件...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedSdzyzcPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "SDZYZC_DLTB");
                        ProcessFoundFiles(files, FileType.SDZYZC_DLTB);

                        txtSdzyzcPath.Text = $"{dialog.SelectedPath} (找到 {files.Count} 个文件)";
                        txtSdzyzcPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "扫描SDZYZC_DLTB文件时出错";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"扫描SDZYZC_DLTB文件时出错: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseCzcdyd_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择CZCDYD数据源文件夹";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "正在扫描CZCDYD文件...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedCzcdydPath = dialog.SelectedPath;
                        var files = FindShapefilesWithPattern(dialog.SelectedPath, "CZCDYD");
                        ProcessFoundFiles(files, FileType.CZCDYD);

                        txtCzcdydPath.Text = $"{dialog.SelectedPath} (找到 {files.Count} 个文件)";
                        txtCzcdydPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateCountySelectionList();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "扫描CZCDYD文件时出错";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"扫描CZCDYD文件时出错: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择CZCDYDQC结果输出文件夹";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    selectedOutputPath = dialog.SelectedPath;
                    txtOutputPath.Text = dialog.SelectedPath;
                    txtOutputPath.ForeColor = Color.DarkGreen;
                }
            }
        }

        // 新增：结果输出路径选择事件处理程序
        private void btnBrowseResultOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "选择现有CZCDYDQC结果文件夹";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        lblStatus.Text = "正在扫描CZCDYDQC结果文件...";
                        lblStatus.ForeColor = Color.Blue;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        Application.DoEvents();

                        selectedResultOutputPath = dialog.SelectedPath;

                        // 扫描结果文件夹中的CZCDYDQC文件
                        var czcdydqcFiles = FindCZCDYDQCShapefiles(dialog.SelectedPath);
                        czcdydqcFileMap.Clear();

                        foreach (var file in czcdydqcFiles)
                        {
                            var countyCode = ExtractCountyCodeFromCZCDYDQCFileName(file);
                            if (!string.IsNullOrEmpty(countyCode))
                            {
                                czcdydqcFileMap[countyCode] = file;
                            }
                        }

                        txtResultOutputPath.Text = $"{dialog.SelectedPath} (找到 {czcdydqcFiles.Count} 个CZCDYDQC文件)";
                        txtResultOutputPath.ForeColor = Color.DarkGreen;

                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;

                        UpdateStatusLabel();
                    }
                    catch (Exception ex)
                    {
                        progressBar.Style = ProgressBarStyle.Continuous;
                        progressBar.Value = 0;
                        lblStatus.Text = "扫描CZCDYDQC结果文件时出错";
                        lblStatus.ForeColor = Color.Red;
                        MessageBox.Show($"扫描CZCDYDQC结果文件时出错: {ex.Message}", "错误",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxCounties.Items.Count; i++)
            {
                // 只选中文件完整的县
                var displayText = checkedListBoxCounties.Items[i].ToString();
                if (!displayText.Contains("文件不完整"))
                {
                    checkedListBoxCounties.SetItemChecked(i, true);
                }
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxCounties.Items.Count; i++)
            {
                checkedListBoxCounties.SetItemChecked(i, false);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCounties.Count == 0)
                {
                    MessageBox.Show("请选择要处理的县。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                lblStatus.Text = "正在处理选中的县数据...";
                lblStatus.ForeColor = Color.Blue;
                progressBar.Value = 0;
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Maximum = selectedCounties.Count;

                var processingResults = new List<string>();

                // 判断处理模式：新建输出还是写入现有结果
                bool writeToExistingResults = !string.IsNullOrEmpty(selectedResultOutputPath) && czcdydqcFileMap.Count > 0;

                string outputBaseDir;
                if (writeToExistingResults)
                {
                    outputBaseDir = selectedResultOutputPath;
                }
                else
                {
                    // 创建新的输出目录
                    outputBaseDir = !string.IsNullOrEmpty(selectedOutputPath) ? selectedOutputPath :
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CZCDYDQC_Output");
                    if (!Directory.Exists(outputBaseDir))
                    {
                        Directory.CreateDirectory(outputBaseDir);
                    }
                }

                for (int i = 0; i < selectedCounties.Count; i++)
                {
                    string countyCode = selectedCounties[i];
                    string countyName = ForestResourcePlugin.Utils.CountyCodeMapper.GetCountyNameFromCode(countyCode);

                    lblStatus.Text = $"正在处理 {countyName}({countyCode})...";
                    Application.DoEvents();

                    if (countyFileGroups.ContainsKey(countyCode))
                    {
                        var fileGroup = countyFileGroups[countyCode];

                        try
                        {
                            string targetShapefilePath;

                            if (writeToExistingResults && czcdydqcFileMap.ContainsKey(countyCode))
                            {
                                // 写入现有的CZCDYDQC文件
                                targetShapefilePath = czcdydqcFileMap[countyCode];
                            }
                            else
                            {
                                // 创建新的输出文件
                                string countyOutputDir = Path.Combine(outputBaseDir, $"{countyName}_{countyCode}");
                                if (!Directory.Exists(countyOutputDir))
                                {
                                    Directory.CreateDirectory(countyOutputDir);
                                }
                                targetShapefilePath = Path.Combine(countyOutputDir, $"({countyCode})CZCDYDQC.shp");
                            }

                            // 构建CZCDYD处理器参数
                            var countyFiles = new CZCDYDProcessor.CountyFiles
                            {
                                CountyCode = countyCode,
                                SlzyzcDltbFile = fileGroup.SlzyzcDltbFile,
                                CyzyzcDltbFile = fileGroup.CyzyzcDltbFile,
                                SdzyzcDltbFile = fileGroup.SdzyzcDltbFile,
                                CzcdydFile = fileGroup.CzcdydFile,
                                OutputDirectory = Path.GetDirectoryName(targetShapefilePath)
                            };

                            // 创建进度回调
                            CZCDYDProcessor.ProgressCallback progressCallback = (percentage, message) =>
                            {
                                // 计算总体进度
                                int overallProgress = (i * 100 + percentage) / selectedCounties.Count;

                                // 更新UI需要在UI线程中执行
                                if (this.InvokeRequired)
                                {
                                    this.Invoke(new Action(() =>
                                    {
                                        lblStatus.Text = $"{countyName}: {message}";
                                        Application.DoEvents();
                                    }));
                                }
                                else
                                {
                                    lblStatus.Text = $"{countyName}: {message}";
                                    Application.DoEvents();
                                }
                            };

                            // 执行CZCDYDQC处理
                            var processor = new CZCDYDProcessor();
                            var result = processor.ProcessCountyCZCDYDQC(countyFiles, progressCallback);

                            if (result.Success)
                            {
                                string mode = writeToExistingResults ? "更新现有文件" : "创建新文件";
                                processingResults.Add($"{countyName}({countyCode}) - 处理完成 ({mode})");
                                System.Diagnostics.Debug.WriteLine($"县 {countyName}({countyCode}) 处理成功");
                                System.Diagnostics.Debug.WriteLine($"  输出文件: {result.OutputPath}");
                                System.Diagnostics.Debug.WriteLine($"  处理要素数: {result.ProcessedFeatureCount}");
                            }
                            else
                            {
                                processingResults.Add($"{countyName}({countyCode}) - 处理失败: {result.ErrorMessage}");
                                System.Diagnostics.Debug.WriteLine($"县 {countyName}({countyCode}) 处理失败: {result.ErrorMessage}");
                            }
                        }
                        catch (Exception ex)
                        {
                            processingResults.Add($"{countyName}({countyCode}) - 处理异常: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"处理县 {countyName}({countyCode}) 时出现异常: {ex.Message}");
                        }
                    }

                    progressBar.Value = i + 1;
                    Application.DoEvents();
                }

                lblStatus.Text = "处理完成！";
                lblStatus.ForeColor = Color.DarkGreen;

                string processingMode = writeToExistingResults ? "更新现有CZCDYDQC文件" : "创建新CZCDYDQC文件";
                string resultMessage = $"{processingMode}完成！\n\n输出目录：{outputBaseDir}\n\n处理结果：\n{string.Join("\n", processingResults)}";
                MessageBox.Show(resultMessage, "处理完成", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 询问是否打开输出目录
                if (MessageBox.Show("是否打开输出目录查看结果？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(outputBaseDir);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"打开输出目录时出错: {ex.Message}");
                    }
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 0;
                lblStatus.Text = "处理失败";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show($"处理过程中出现错误: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 在指定目录下查找包含特定模式的shapefile
        /// </summary>
        /// <param name="rootPath">根目录路径</param>
        /// <param name="pattern">文件名模式</param>
        /// <returns>找到的shapefile列表</returns>
        private List<string> FindShapefilesWithPattern(string rootPath, string pattern)
        {
            var foundFiles = new List<string>();

            try
            {
                // 递归查找所有.shp文件
                var shpFiles = Directory.GetFiles(rootPath, "*.shp", SearchOption.AllDirectories);

                foreach (var file in shpFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    // 检查文件名是否包含指定模式
                    if (fileName.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        foundFiles.Add(file);
                        System.Diagnostics.Debug.WriteLine($"找到文件: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找文件时出错: {ex.Message}");
                throw;
            }

            return foundFiles;
        }

        /// <summary>
        /// 查找CZCDYDQC shapefile文件
        /// </summary>
        /// <param name="rootPath">根目录路径</param>
        /// <returns>找到的CZCDYDQC shapefile列表</returns>
        private List<string> FindCZCDYDQCShapefiles(string rootPath)
        {
            var foundFiles = new List<string>();

            try
            {
                // 递归查找所有.shp文件
                var shpFiles = Directory.GetFiles(rootPath, "*.shp", SearchOption.AllDirectories);

                foreach (var file in shpFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    // 查找符合(六位县代码)CZCDYDQC模式的文件
                    if (Regex.IsMatch(fileName, @"^\(\d{6}\)CZCDYDQC$", RegexOptions.IgnoreCase))
                    {
                        foundFiles.Add(file);
                        System.Diagnostics.Debug.WriteLine($"找到CZCDYDQC文件: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找CZCDYDQC文件时出错: {ex.Message}");
                throw;
            }

            return foundFiles;
        }

        /// <summary>
        /// 从CZCDYDQC文件名中提取六位县代码
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>县代码，如果未找到则返回null</returns>
        private string ExtractCountyCodeFromCZCDYDQCFileName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // 查找括号中的六位数字
            var match = Regex.Match(fileName, @"^\((\d{6})\)CZCDYDQC$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// 从文件名中提取六位县代码
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>县代码，如果未找到则返回null</returns>
        private string ExtractCountyCodeFromFileName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            // 查找括号中的六位数字
            var match = Regex.Match(fileName, @"^\((\d{6})\)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // 如果没有找到括号格式，尝试查找文件名开头的六位数字（兼容旧格式）
            match = Regex.Match(fileName, @"^(\d{6})");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        /// <summary>
        /// 处理找到的文件，按县代码分组
        /// </summary>
        /// <param name="files">文件列表</param>
        /// <param name="fileType">文件类型</param>
        private void ProcessFoundFiles(List<string> files, FileType fileType)
        {
            foreach (var file in files)
            {
                var countyCode = ExtractCountyCodeFromFileName(file);
                if (!string.IsNullOrEmpty(countyCode))
                {
                    if (!countyFileGroups.ContainsKey(countyCode))
                    {
                        countyFileGroups[countyCode] = new CountyFileGroup { CountyCode = countyCode };
                    }

                    // 根据文件类型设置相应的文件路径
                    switch (fileType)
                    {
                        case FileType.SLZYZC_DLTB:
                            countyFileGroups[countyCode].SlzyzcDltbFile = file;
                            break;
                        case FileType.CYZYZC_DLTB:
                            countyFileGroups[countyCode].CyzyzcDltbFile = file;
                            break;
                        case FileType.SDZYZC_DLTB:
                            countyFileGroups[countyCode].SdzyzcDltbFile = file;
                            break;
                        case FileType.CZCDYD:
                            countyFileGroups[countyCode].CzcdydFile = file;
                            break;
                    }

                    System.Diagnostics.Debug.WriteLine($"文件归类: {Path.GetFileName(file)} -> 县代码: {countyCode}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"警告: 无法从文件名提取县代码: {Path.GetFileName(file)}");
                }
            }
        }

        #endregion

        #region 内部类和枚举

        /// <summary>
        /// 文件类型枚举
        /// </summary>
        private enum FileType
        {
            SLZYZC_DLTB,
            CYZYZC_DLTB,
            SDZYZC_DLTB,
            CZCDYD
        }

        /// <summary>
        /// 县级文件组信息
        /// </summary>
        private class CountyFileGroup
        {
            public string CountyCode { get; set; }
            public string SlzyzcDltbFile { get; set; }
            public string CyzyzcDltbFile { get; set; }
            public string SdzyzcDltbFile { get; set; }
            public string CzcdydFile { get; set; }

            /// <summary>
            /// 检查是否所有必需的文件都存在
            /// </summary>
            public bool IsComplete => !string.IsNullOrEmpty(SlzyzcDltbFile) &&
                                     !string.IsNullOrEmpty(CyzyzcDltbFile) &&
                                     !string.IsNullOrEmpty(SdzyzcDltbFile) &&
                                     !string.IsNullOrEmpty(CzcdydFile);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取选中的县代码列表
        /// </summary>
        public List<string> SelectedCounties => new List<string>(selectedCounties);

        /// <summary>
        /// 获取按县代码分组的文件信息
        /// </summary>
        private Dictionary<string, CountyFileGroup> CountyFileGroups =>
            new Dictionary<string, CountyFileGroup>(countyFileGroups);

        /// <summary>
        /// 获取选择的SLZYZC_DLTB源文件夹路径
        /// </summary>
        public string SelectedSlzyzcPath => selectedSlzyzcPath;

        /// <summary>
        /// 获取选择的CYZYZC_DLTB源文件夹路径
        /// </summary>
        public string SelectedCyzyzcPath => selectedCyzyzcPath;

        /// <summary>
        /// 获取选择的SDZYZC_DLTB源文件夹路径
        /// </summary>
        public string SelectedSdzyzcPath => selectedSdzyzcPath;

        /// <summary>
        /// 获取选择的CZCDYD数据源文件夹路径
        /// </summary>
        public string SelectedCzcdydPath => selectedCzcdydPath;

        /// <summary>
        /// 获取选择的输出路径
        /// </summary>
        public string SelectedOutputPath => selectedOutputPath;

        /// <summary>
        /// 获取选择的结果输出路径
        /// </summary>
        public string SelectedResultOutputPath => selectedResultOutputPath;

        #endregion
    }
}