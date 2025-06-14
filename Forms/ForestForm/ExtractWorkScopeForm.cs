using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using TestArcMapAddin2.Components;
using TestArcMapAddin2.Forms.ForestForm;
using Point = System.Drawing.Point;

namespace TestArcMapAddin2.Forms
{
    /// <summary>
    /// Form for extracting forest work scope based on forest-grassland-wetland inventory data
    /// </summary>
    public partial class ExtractWorkScopeForm : ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public ExtractWorkScopeForm()
        {
            InitializeComponent(); // Changed from InitializeComponents
            
            // Wire up the browse button events
            this.btnBrowseInventory.Click += (sender, e) => BrowseForData(this.txtInventoryData, "选择林草湿荒普查数据", "图层文件 (*.shp)|*.shp|所有文件 (*.*)|*.*");
            this.btnBrowseBoundary.Click += (sender, e) => BrowseForData(this.txtUrbanBoundary, "选择城镇开发边界数据", "图层文件 (*.shp)|*.shp|所有文件 (*.*)|*.*");
            this.btnLoadCurrentMap.Click += BtnLoadCurrentMap_Click;
        }
        
        // Removed InitializeComponents() method as it's now in the Designer.cs file

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
                // and load the relevant layers
                
                // For demo purposes, just fill in some sample paths
                this.txtInventoryData.Text = @"C:\GIS_Data\林草湿荒普查数据.shp"; // New way
                this.txtUrbanBoundary.Text = @"C:\GIS_Data\城镇开发边界.shp"; // New way
                
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
            if (string.IsNullOrEmpty(this.txtInventoryData.Text) || string.IsNullOrEmpty(this.txtUrbanBoundary.Text)) // New way
            {
                MessageBox.Show("请先选择所需数据文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false);
                return;
            }
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessExtractWorkScope(this.txtInventoryData.Text, this.txtUrbanBoundary.Text)); // New way
        }
        
        private void ProcessExtractWorkScope(string inventoryDataPath, string boundaryDataPath)
        {
            try
            {
                UpdateStatus("正在准备数据...");
                Log("开始提取工作范围流程");
                Log($"使用林草湿荒普查数据: {inventoryDataPath}");
                Log($"使用城镇开发边界数据: {boundaryDataPath}");
                
                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled) return;
                    
                    switch (i)
                    {
                        case 0:
                            UpdateStatus("正在加载林草湿荒普查数据...");
                            Log("加载林草湿荒普查数据中...");
                            break;
                        case 20:
                            UpdateStatus("正在加载城镇开发边界数据...");
                            Log("加载城镇开发边界数据中...");
                            break;
                        case 30:
                            UpdateStatus("正在提取地类为林地且土地权属为国有的图斑...");
                            Log("提取地类为林地且土地权属为国有的图斑...");
                            break;
                        case 50:
                            UpdateStatus("正在提取地类为林地且土地权属为集体且位于城镇开发边界内的图斑...");
                            Log("提取地类为林地且土地权属为集体且位于城镇开发边界内的图斑...");
                            break;
                        case 70:
                            UpdateStatus("正在合并筛选结果...");
                            Log("合并筛选结果...");
                            break;
                        case 85:
                            UpdateStatus("正在清理临时数据...");
                            Log("清理临时数据...");
                            break;
                        case 95:
                            UpdateStatus("正在导出工作范围图层...");
                            Log("导出工作范围图层...");
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("工作范围提取完成");
                Log("共提取：283个图斑，总面积1265.42公顷");
                Log("其中：国有林地图斑 245个，面积1086.23公顷");
                Log("集体林地但在城镇开发边界内图斑 38个，面积179.19公顷");
                
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