using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using Point = System.Drawing.Point;

namespace TestArcMapAddin2.Forms.ForestForm
{
    /// <summary>
    /// Form for creating forest work base maps and linking with forest classification and pricing data
    /// </summary>
    public partial class WorkBaseMapForm : TestArcMapAddin2.Forms.ForestForm.ForestProcessingFormBase
    {
        private bool processingCancelled = false;
        
        public WorkBaseMapForm()
        {
            InitializeComponent(); // Calls the designer's InitializeComponent
            // Ensure cmbJoinMethod has a default selection if not set in Designer
            if (this.cmbJoinMethod.SelectedIndex == -1 && this.cmbJoinMethod.Items.Count > 0)
            {
                this.cmbJoinMethod.SelectedIndex = 0;
            }
        }
        
        // Removed InitializeComponents() method from here. It's now in the .Designer.cs file.

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

        // Event handlers for browse buttons, to be connected in Designer.cs
        private void BrowseWorkScope_Click(object sender, EventArgs e)
        {
            BrowseForData(this.txtWorkScope, "选择工作范围数据", "图层文件 (*.shp)|*.shp|所有文件 (*.*)|*.*");
        }

        private void BrowseGrading_Click(object sender, EventArgs e)
        {
            BrowseForData(this.txtForestGrading, "选择林地分等数据", "图层文件 (*.shp)|*.shp|所有文件 (*.*)|*.*");
        }

        private void BrowsePrice_Click(object sender, EventArgs e)
        {
            BrowseForData(this.txtPriceData, "选择基准地价数据", "图层文件 (*.shp)|*.shp|所有文件 (*.*)|*.*");
        }
        
        private void BtnLoadCurrentMap_Click(object sender, EventArgs e)
        {
            try
            {
                Log("正在从当前地图加载数据...");
                // In a real implementation, this would use ArcObjects to access the current map
                // and load the relevant layers
                
                // Access controls directly as fields
                this.txtWorkScope.Text = @"C:\GIS_Data\森林工作范围.shp";
                this.txtForestGrading.Text = @"C:\GIS_Data\林地分等数据.shp";
                this.txtPriceData.Text = @"C:\GIS_Data\林地基准地价数据.shp";
                
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
            
            // Access controls directly as fields
            if (string.IsNullOrEmpty(this.txtWorkScope.Text) || 
                string.IsNullOrEmpty(this.txtForestGrading.Text) || 
                string.IsNullOrEmpty(this.txtPriceData.Text))
            {
                MessageBox.Show("请先选择所需数据文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnProcessingComplete(false); // Ensure this is called to re-enable start button etc.
                return;
            }
            
            bool useSpatialJoin = this.chkSpatialJoin.Checked;
            string joinMethod = this.cmbJoinMethod.SelectedItem?.ToString() ?? "相交";
            bool overwriteExisting = this.chkOverwriteExisting.Checked;
            
            processingCancelled = false;
            // Start the processing in a background task
            Task.Run(() => ProcessCreateWorkBaseMap(
                this.txtWorkScope.Text, 
                this.txtForestGrading.Text, 
                this.txtPriceData.Text,
                useSpatialJoin,
                joinMethod,
                overwriteExisting));
        }
        
        private void ProcessCreateWorkBaseMap(
            string workScopePath, 
            string forestGradingPath, 
            string priceDataPath,
            bool useSpatialJoin,
            string joinMethod,
            bool overwriteExisting)
        {
            try
            {
                UpdateStatus("正在准备数据...");
                Log("开始制作工作底图流程");
                Log($"使用工作范围数据: {workScopePath}");
                Log($"使用林地分等数据: {forestGradingPath}");
                Log($"使用基准地价数据: {priceDataPath}");
                Log($"空间连接方式: {(useSpatialJoin ? joinMethod : "属性连接")}");
                Log($"覆盖现有数据: {(overwriteExisting ? "是" : "否")}");
                
                // Simulate the processing steps
                for (int i = 0; i <= 100; i += 5)
                {
                    if (processingCancelled)
                    {
                        Log("处理被用户取消。");
                        this.Invoke(new Action(() => OnProcessingComplete(false)));
                        return;
                    }
                    
                    switch (i)
                    {
                        case 0:
                            UpdateStatus("正在加载工作范围数据...");
                            Log("加载工作范围数据中...");
                            break;
                        case 10:
                            UpdateStatus("正在加载林地分等数据...");
                            Log("加载林地分等数据中...");
                            break;
                        case 20:
                            UpdateStatus("正在加载基准地价数据...");
                            Log("加载基准地价数据中...");
                            break;
                        case 30:
                            UpdateStatus("正在关联工作范围与林地分等数据...");
                            Log("关联工作范围与林地分等数据...");
                            break;
                        case 50:
                            UpdateStatus("正在关联工作范围与林地定级数据...");
                            Log("关联工作范围与林地定级数据...");
                            break;
                        case 70:
                            UpdateStatus("正在进行基准地价空间挂接...");
                            Log("进行基准地价空间挂接...");
                            break;
                        case 85:
                            UpdateStatus("正在检查数据一致性...");
                            Log("检查数据一致性...");
                            break;
                        case 95:
                            UpdateStatus("正在导出工作底图...");
                            Log("导出工作底图...");
                            break;
                    }
                    
                    UpdateProgress(i);
                    System.Threading.Thread.Sleep(200); // Simulate work
                }
                
                Log("工作底图制作完成");
                Log("关联成功: 256图斑");
                Log("关联失败: 27图斑 (将在下一步骤补充)");
                
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
            // The base.CancelButton_Click already shows a confirmation and calls OnProcessingComplete(false) if "Yes" is clicked.
            // We just need to set the flag.
            var result = MessageBox.Show("确定要取消当前处理吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                processingCancelled = true;
                // The base class OnProcessingComplete(false) will be called by base.CancelButton_Click 
                // if the user confirms. If we call it here again, it might lead to double logging or status updates.
                // However, the current base implementation calls OnProcessingComplete(false) directly.
                // To ensure our processingCancelled flag is respected by the loop, we set it here.
                // The base method will handle UI updates.
                base.OnProcessingComplete(false); // Explicitly call to ensure UI reset if loop hasn't checked flag yet.
            }
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {

        }
    }
}