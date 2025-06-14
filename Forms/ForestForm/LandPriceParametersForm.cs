using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestArcMapAddin2.Forms.ForestForm;

namespace TestArcMapAddin2.Forms
{
    /// <summary>
    /// Form for extracting land benchmark price parameters
    /// </summary>
    public partial class LandPriceParametersForm : ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public LandPriceParametersForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "提取林地基准价格参数";
            this.titleLabel.Text = "3. 林地基准价格参数提取";
            
            this.descriptionTextBox.Text = 
                "梳理提取县（区）林地定级指标及权重信息、基准价格信息、" +
                "基准价格修正因子及权重信息、收益还原率等数据。";
            
            // Add custom controls specific to this form
            GroupBox dataSourceGroupBox = new GroupBox
            {
                Text = "数据来源",
                Location = new Point(15, 150),
                Size = new Size(600, 90)
            };
            
            Label lblWorkBaseMap = new Label
            {
                Text = "工作底图数据:",
                Location = new Point(15, 25),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtWorkBaseMap = new TextBox
            {
                Location = new Point(170, 25),
                Size = new Size(320, 20),
                ReadOnly = true
            };
            
            Button btnBrowseWorkBaseMap = new Button
            {
                Text = "浏览...",
                Location = new Point(500, 24),
                Size = new Size(80, 23)
            };
            
            Button btnLoadCurrentMap = new Button
            {
                Text = "加载当前地图数据",
                Location = new Point(200, 55),
                Size = new Size(180, 25)
            };
            
            // Add controls to the group box
            dataSourceGroupBox.Controls.Add(lblWorkBaseMap);
            dataSourceGroupBox.Controls.Add(txtWorkBaseMap);
            dataSourceGroupBox.Controls.Add(btnBrowseWorkBaseMap);
            dataSourceGroupBox.Controls.Add(btnLoadCurrentMap);
            
            // Add parameters group box
            GroupBox parametersGroupBox = new GroupBox
            {
                Text = "价格参数提取选项",
                Location = new Point(15, 250),
                Size = new Size(600, 120)
            };
            
            CheckBox chkLevelIndicators = new CheckBox
            {
                Text = "提取林地定级指标及权重信息",
                Location = new Point(20, 25),
                Size = new Size(250, 20),
                Checked = true
            };
            
            CheckBox chkBasePrice = new CheckBox
            {
                Text = "提取基准价格信息",
                Location = new Point(20, 50),
                Size = new Size(250, 20),
                Checked = true
            };
            
            CheckBox chkModifiers = new CheckBox
            {
                Text = "提取基准价格修正因子及权重信息",
                Location = new Point(20, 75),
                Size = new Size(250, 20),
                Checked = true
            };
            
            CheckBox chkYieldRate = new CheckBox
            {
                Text = "提取收益还原率",
                Location = new Point(300, 25),
                Size = new Size(200, 20),
                Checked = true
            };
            
            CheckBox chkExportExcel = new CheckBox
            {
                Text = "将参数导出到Excel",
                Location = new Point(300, 50),
                Size = new Size(200, 20),
                Checked = true
            };
            
            Button btnSelectExportFolder = new Button
            {
                Text = "选择导出文件夹",
                Location = new Point(300, 75),
                Size = new Size(150, 25),
                Enabled = true
            };
            
            parametersGroupBox.Controls.Add(chkLevelIndicators);
            parametersGroupBox.Controls.Add(chkBasePrice);
            parametersGroupBox.Controls.Add(chkModifiers);
            parametersGroupBox.Controls.Add(chkYieldRate);
            parametersGroupBox.Controls.Add(chkExportExcel);
            parametersGroupBox.Controls.Add(btnSelectExportFolder);
            
            // Adjust the location of the existing log text box
            this.logTextBox.Location = new Point(15, 380);
            this.logTextBox.Size = new Size(600, 110);
            
            // Adjust other control positions
            this.statusLabel.Location = new Point(15, 500);
            this.progressBar.Location = new Point(15, 525);
            
            // Add the group boxes to the main panel
            this.mainPanel.Controls.Add(dataSourceGroupBox);
            this.mainPanel.Controls.Add(parametersGroupBox);
            
            // Wire up the button events
            btnBrowseWorkBaseMap.Click += (sender, e) => BrowseForData(txtWorkBaseMap, "选择工作底图数据", "图层文件 (*.shp)|*.shp|所有文件 (*.*)|*.*");
            btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
            btnSelectExportFolder.Click += BtnSelectExportFolder_Click;
        }

        private void BrowseForData(TextBox textBox, string title, string filter)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = title;
                dlg.Filter = filter;
                
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = dlg.FileName;
                }
            }
        }
        
        private void BtnLoadCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在从当前地图加载数据...");
                // In a real implementation, this would use ArcObjects to access the current map
                
                // For demo purposes, just fill in a sample path
                var txtWorkBaseMap = this.mainPanel.Controls.Find("txtWorkBaseMap", true)[0] as TextBox;
                txtWorkBaseMap.Text = @"C:\GIS_Data\森林工作底图.shp";
                
                Log("数据已从当前地图加载");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }
        
        private void BtnSelectExportFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "选择参数导出文件夹";
                dlg.ShowNewFolderButton = true;
                
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Log($"已选择导出文件夹: {dlg.SelectedPath}");
                }
            }
        }

        protected override void StartButton_Click(object sender, EventArgs e)
        {
            base.StartButton_Click(sender, e);
            
            // Get the file paths from the text boxes
            var txtWorkBaseMap = this.mainPanel.Controls.Find("txtWorkBaseMap", true)[0] as TextBox;
            
            if (string.IsNullOrEmpty(txtWorkBaseMap.Text))
            {
                MessageBox.Show("请先选择工作底图数据文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }
            
            // Get processing options
            var chkLevelIndicators = this.mainPanel.Controls.Find("chkLevelIndicators", true)[0] as CheckBox;
            var chkBasePrice = this.mainPanel.Controls.Find("chkBasePrice", true)[0] as CheckBox;
            var chkModifiers = this.mainPanel.Controls.Find("chkModifiers", true)[0] as CheckBox;
            var chkYieldRate = this.mainPanel.Controls.Find("chkYieldRate", true)[0] as CheckBox;
            var chkExportExcel = this.mainPanel.Controls.Find("chkExportExcel", true)[0] as CheckBox;
            
            bool extractLevelIndicators = chkLevelIndicators.Checked;
            bool extractBasePrice = chkBasePrice.Checked;
            bool extractModifiers = chkModifiers.Checked;
            bool extractYieldRate = chkYieldRate.Checked;
            bool exportToExcel = chkExportExcel.Checked;
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessExtractPriceParameters(
                txtWorkBaseMap.Text,
                extractLevelIndicators,
                extractBasePrice,
                extractModifiers,
                extractYieldRate,
                exportToExcel));
        }
        
        private void ProcessExtractPriceParameters(
            string workBaseMapPath,
            bool extractLevelIndicators,
            bool extractBasePrice,
            bool extractModifiers,
            bool extractYieldRate,
            bool exportToExcel)
        {
            try
            {
                UpdateStatus("正在准备数据...");
                Log("开始提取林地基准价格参数流程");
                Log($"使用工作底图数据: {workBaseMapPath}");
                
                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled) return;
                    
                    switch (i)
                    {
                        case 0:
                            UpdateStatus("正在加载工作底图数据...");
                            Log("加载工作底图数据中...");
                            break;
                        case 15:
                            if (extractLevelIndicators)
                            {
                                UpdateStatus("正在提取林地定级指标及权重信息...");
                                Log("提取林地定级指标及权重信息...");
                            }
                            break;
                        case 35:
                            if (extractBasePrice)
                            {
                                UpdateStatus("正在提取基准价格信息...");
                                Log("提取基准价格信息...");
                            }
                            break;
                        case 55:
                            if (extractModifiers)
                            {
                                UpdateStatus("正在提取基准价格修正因子及权重信息...");
                                Log("提取基准价格修正因子及权重信息...");
                            }
                            break;
                        case 75:
                            if (extractYieldRate)
                            {
                                UpdateStatus("正在提取收益还原率...");
                                Log("提取收益还原率...");
                            }
                            break;
                        case 90:
                            if (exportToExcel)
                            {
                                UpdateStatus("正在导出参数到Excel...");
                                Log("导出参数到Excel...");
                            }
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("林地基准价格参数提取完成");
                if (extractLevelIndicators)
                    Log("- 提取林地定级指标: 坡度、土壤肥力、交通便利度等8项指标及权重");
                if (extractBasePrice)
                    Log("- 提取基准价格信息: 共4个分区，价格范围 2.45-8.72万元/公顷");
                if (extractModifiers)
                    Log("- 提取修正因子: 区位、林种、林龄等6个因子及权重");
                if (extractYieldRate)
                    Log("- 提取收益还原率: 平均收益还原率 3.86%");
                if (exportToExcel)
                    Log("- 已导出参数到Excel文件");
                
                // Complete the processing
                this.Invoke(new Action(() => OnProcessingComplete(true)));
            }
            catch (Exception ex)
            {
                Log($"错误: {ex.Message}");
                this.Invoke(new Action(() => 
                {
                    MessageBox.Show($"处理过程中出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    OnProcessingComplete(false);
                }));
            }
        }
        
        protected override void CancelButton_Click(object sender, EventArgs e)
        {
            base.CancelButton_Click(sender, e);
            processingCancelled = true;
        }
    }
}