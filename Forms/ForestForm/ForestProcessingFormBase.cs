using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using Point = System.Drawing.Point;

namespace TestArcMapAddin2.Forms.ForestForm
{
    /// <summary>
    /// Base form for forest processing operations that provides common functionality
    /// </summary>
    public partial class ForestProcessingFormBase : Form
    {
        // Delegate for function completion
        public delegate void ProcessCompletedEventHandler(object sender, bool success);
        
        // Event that fires when processing is completed
        public event ProcessCompletedEventHandler ProcessCompleted;
        
        public ForestProcessingFormBase()
        {
            InitializeComponent(); // Changed from InitializeBaseComponents
        }

        private void ForestProcessingFormBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if processing is ongoing
            if (cancelButton.Enabled)
            {
                var result = MessageBox.Show("处理尚未完成，确定要关闭吗？", "确认", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        protected virtual void StartButton_Click(object sender, EventArgs e)
        {
            // Base implementation - override in derived classes
            startButton.Enabled = false;
            cancelButton.Enabled = true;
            closeButton.Enabled = false;
            
            // Start the actual processing in derived classes
        }

        protected virtual void CancelButton_Click(object sender, EventArgs e)
        {
            // Base implementation - override in derived classes
            var result = MessageBox.Show("确定要取消当前处理吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                // Cancel the processing in derived classes
                OnProcessingComplete(false);
            }
        }

        protected virtual void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        protected void Log(string message)
        {
            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new Action(() => 
                {
                    logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
                    logTextBox.ScrollToCaret();
                }));
            }
            else
            {
                logTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
                logTextBox.ScrollToCaret();
            }
        }
        
        protected void UpdateStatus(string status)
        {
            if (statusLabel.InvokeRequired)
            {
                statusLabel.Invoke(new Action(() => statusLabel.Text = status));
            }
            else
            {
                statusLabel.Text = status;
            }
            Application.DoEvents();
        }
        
        protected void UpdateProgress(int value)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action(() => progressBar.Value = Math.Max(0, Math.Min(100, value))));
            }
            else
            {
                progressBar.Value = Math.Max(0, Math.Min(100, value));
            }
            Application.DoEvents();
        }
        
        protected void OnProcessingComplete(bool success)
        {
            startButton.Enabled = true;
            cancelButton.Enabled = false;
            closeButton.Enabled = true;
            
            if (success)
            {
                UpdateStatus("处理已成功完成！");
                Log("处理已成功完成！");
            }
            else
            {
                UpdateStatus("处理已取消或失败。");
                Log("处理已取消或失败。");
            }
            
            // Raise the event
            ProcessCompleted?.Invoke(this, success);
        }
    }
}