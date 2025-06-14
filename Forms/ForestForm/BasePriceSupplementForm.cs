using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using TestArcMapAddin2.Forms.ForestForm;

namespace TestArcMapAddin2.Forms
{
    /// <summary>
    /// Form for supplementing forest benchmark prices
    /// </summary>
    public partial class BasePriceSupplementForm : ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public BasePriceSupplementForm()
        {
            InitializeComponents();
        }
        
        private void InitializeComponents()
        {
            this.Text = "补充完善林地基准地价";
            this.titleLabel.Text = "4. 补充完善林地基准地价";
            
            this.descriptionTextBox.Text = 
                "针对森林图斑在空间挂接基准地价时，出现落空的情况，利用上述林地定级指标及权重信息、" +
                "基准价格信息，进行补充运算，确保森林图斑基准价格无缺漏。";
            
            // Add custom controls specific to this form
            GroupBox dataSourceGroupBox = new GroupBox
            {
                Text = "数据来源",
                Location = new Point(15, 150),
                Size = new Size(600, 120)
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
            
            Label lblParameters = new Label
            {
                Text = "价格参数数据:",
                Location = new Point(15, 55),
                Size = new Size(150, 20),
                TextAlign = ContentAlignment.MiddleRight
            };
            
            TextBox txtParameters = new TextBox
            {
                Location = new Point(170, 55),
                Size = new Size(320, 20),
                ReadOnly = true
            };
            
            Button btnBrowseParameters = new Button
            {
                Text = "浏览...",
                Location = new Point(500, 54),
                Size = new Size(80, 23)
            };
            
            Button btnLoadCurrentMap = new Button
            {
                Text = "加载当前地图数据",
                Location = new Point(200, 85),
                Size = new Size(180, 25)
            };
            
            // Add controls to the group box
            dataSourceGroupBox.Controls.Add(lblWorkBaseMap);
            dataSourceGroupBox.Controls.Add(txtWorkBaseMap);
            dataSourceGroupBox.Controls.Add(btnBrowseWorkBaseMap);
            dataSourceGroupBox.Controls.Add(lblParameters);
            dataSourceGroupBox.Controls.Add(txtParameters);
            dataSourceGroupBox.Controls.Add(btnBrowseParameters);
            dataSourceGroupBox.Controls.Add(btnLoadCurrentMap);
            
            // Add supplementation method group box
            GroupBox methodGroupBox = new GroupBox
            {
                Text = "补充方法",
                Location = new Point(15, 280),
                Size = new Size(600, 90)
            };
            
            RadioButton rbNearest = new RadioButton
            {
                Text = "基于最近邻图斑价格",
                Location = new Point(20, 25),
                Size = new Size(200, 20),
                Checked = true
            };
            
            RadioButton rbInterpolation = new RadioButton
            {
                Text = "空间插值法",
                Location = new Point(20, 55),
                Size = new Size(200, 20)
            };
            
            RadioButton rbRegression = new RadioButton
            {
                Text = "回归分析法",
                Location = new Point(240, 25),
                Size = new Size(200, 20)
            };
            
            RadioButton rbAverage = new RadioButton
            {
                Text = "分区平均值法",
                Location = new Point(240, 55),
                Size = new Size(200, 20)
            };
            
            CheckBox chkManualAdjust = new CheckBox
            {
                Text = "允许手动调整异常值",
                Location = new Point(450, 25),
                Size = new Size(150, 20),
                Checked = true
            };
            
            methodGroupBox.Controls.Add(rbNearest);
            methodGroupBox.Controls.Add(rbInterpolation);
            methodGroupBox.Controls.Add(rbRegression);
            methodGroupBox.Controls.Add(rbAverage);
            methodGroupBox.Controls.Add(chkManualAdjust);
            
            // Adjust the location of the existing log text box
            this.logTextBox.Location = new Point(15, 380);
            this.logTextBox.Size = new Size(600, 110);
            
            // Adjust other control positions
            this.statusLabel.Location = new Point(15, 500);
            this.progressBar.Location = new Point(15, 525);
            
            // Add the group boxes to the main panel
            this.mainPanel.Controls.Add(dataSourceGroupBox);
            this.mainPanel.Controls.Add(methodGroupBox);
            
            // Wire up the button events
            btnBrowseWorkBaseMap.Click += (sender, e) => BrowseForData(txtWorkBaseMap, "选择工作底图数据", "图层文件 (*.shp)|*.shp|所有文件 (*.*)|*.*");
            btnBrowseParameters.Click += (sender, e) => BrowseForData(txtParameters, "选择价格参数数据", "Excel文件 (*.xlsx)|*.xlsx|所有文件 (*.*)|*.*");
            btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
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
                var txtParameters = this.mainPanel.Controls.Find("txtParameters", true)[0] as TextBox;
                
                txtWorkBaseMap.Text = @"C:\GIS_Data\森林工作底图.shp";
                txtParameters.Text = @"C:\GIS_Data\林地价格参数.xlsx";
                
                Log("数据已从当前地图加载");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log($"错误: {ex.Message}");
            }
        }

        protected override void StartButton_Click(object sender, EventArgs e)
        {
            base.StartButton_Click(sender, e);
            
            // Get the file paths from the text boxes
            var txtWorkBaseMap = this.mainPanel.Controls.Find("txtWorkBaseMap", true)[0] as TextBox;
            var txtParameters = this.mainPanel.Controls.Find("txtParameters", true)[0] as TextBox;
            
            if (string.IsNullOrEmpty(txtWorkBaseMap.Text) || string.IsNullOrEmpty(txtParameters.Text))
            {
                MessageBox.Show("请先选择所需数据文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }
            
            // Get supplementation method
            string supplementMethod = "nearest"; // default
            
            var rbNearest = this.mainPanel.Controls.Find("rbNearest", true)[0] as RadioButton;
            var rbInterpolation = this.mainPanel.Controls.Find("rbInterpolation", true)[0] as RadioButton;
            var rbRegression = this.mainPanel.Controls.Find("rbRegression", true)[0] as RadioButton;
            var rbAverage = this.mainPanel.Controls.Find("rbAverage", true)[0] as RadioButton;
            var chkManualAdjust = this.mainPanel.Controls.Find("chkManualAdjust", true)[0] as CheckBox;
            
            if (rbNearest.Checked)
                supplementMethod = "nearest";
            else if (rbInterpolation.Checked)
                supplementMethod = "interpolation";
            else if (rbRegression.Checked)
                supplementMethod = "regression";
            else if (rbAverage.Checked)
                supplementMethod = "average";
                
            bool allowManualAdjustment = chkManualAdjust.Checked;
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessSupplementBasePrice(
                txtWorkBaseMap.Text,
                txtParameters.Text,
                supplementMethod,
                allowManualAdjustment));
        }
        
        private void ProcessSupplementBasePrice(
            string workBaseMapPath,
            string parametersPath,
            string supplementMethod,
            bool allowManualAdjustment)
        {
            try
            {
                UpdateStatus("正在准备数据...");
                Log("开始补充完善林地基准地价流程");
                Log($"使用工作底图数据: {workBaseMapPath}");
                Log($"使用价格参数数据: {parametersPath}");
                Log($"补充方法: {supplementMethod}");
                Log($"允许手动调整异常值: {allowManualAdjustment}");
                
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
                        case 10:
                            UpdateStatus("正在加载价格参数数据...");
                            Log("加载价格参数数据中...");
                            break;
                        case 20:
                            UpdateStatus("正在筛选无基准价格的图斑...");
                            Log("筛选无基准价格的图斑中...");
                            break;
                        case 30:
                            UpdateStatus("正在计算补充价格...");
                            Log($"使用{supplementMethod}方法计算补充价格中...");
                            break;
                        case 60:
                            UpdateStatus("正在验证价格合理性...");
                            Log("验证价格合理性中...");
                            break;
                        case 80:
                            if (allowManualAdjustment)
                            {
                                UpdateStatus("正在调整异常价格...");
                                Log("调整异常价格中...");
                                // In a real app, might show a dialog here to adjust values
                            }
                            break;
                        case 90:
                            UpdateStatus("正在更新图斑价格数据...");
                            Log("更新图斑价格数据中...");
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("林地基准地价补充完成");
                Log("补充成功: 27图斑");
                Log("价格范围: 2.45-8.72万元/公顷");
                Log("平均价格: 4.83万元/公顷");
                
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