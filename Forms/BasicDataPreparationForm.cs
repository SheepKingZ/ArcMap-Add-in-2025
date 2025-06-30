using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestArcMapAddin2.Forms; // ����CountySelectionForm
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry; // ���������ڿռ�ο�ϵͳ
using ESRI.ArcGIS.esriSystem; // ���������ڿռ�ο�ϵͳ
using System.Reflection; // ���������ڷ������

namespace TestArcMapAddin2.Forms
{
    public partial class BasicDataPreparationForm : Form
    {
        // �޸�˽���ֶ� - �ϲ��ֲ�ʪ���ղ����ݺͳ��򿪷��߽�����·��Ϊһ��
        private string dataSourcePath = "";
        private string outputGDBPath = "";

        /// <summary>
        /// CGCS2000����ϵWKT����
        /// </summary>
        private const string CGCS2000_WKT = @"GEOGCS[""GCS_China_Geodetic_Coordinate_System_2000"",DATUM[""D_China_2000"",SPHEROID[""CGCS2000"",6378137.0,298.257222101]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]]";

        /// <summary>
        /// CGCS2000 3�ȴ�37��ͶӰ����ϵWKT����
        /// </summary>
        private const string CGCS2000_3_DEGREE_GK_ZONE_37_WKT = @"PROJCS[""GCS_China_Geodetic_Coordinate_System_2000_3_Degree_GK_Zone_37"",GEOGCS[""GCS_China_Geodetic_Coordinate_System_2000"",DATUM[""D_China_2000"",SPHEROID[""CGCS2000"",6378137.0,298.257222101]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]],PROJECTION[""Gauss_Kruger""],PARAMETER[""False_Easting"",37500000.0],PARAMETER[""False_Northing"",0.0],PARAMETER[""Central_Meridian"",111.0],PARAMETER[""Scale_Factor"",1.0],PARAMETER[""Latitude_Of_Origin"",0.0],UNIT[""Meter"",1.0]]";

        public BasicDataPreparationForm()
        {
            InitializeComponent();
            InitializeFormState();
        }

        private void InitializeFormState()
        {
            // ���ݵ�ǰ����״̬��Ĭ��ֵ��ʼ����ǩ
            lblWorkspace.Text = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? "δѡ�����ռ�" : SharedWorkflowState.WorkspacePath;
            lblWorkspace.ForeColor = string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath) ? Color.Black : Color.DarkGreen;

            // ��ʼ���ϲ��������Դ·��״̬
            if (!string.IsNullOrEmpty(dataSourcePath))
            {
                txtDataPath.Text = dataSourcePath;
                txtDataPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtDataPath.Text = "��ѡ������ֲ�ʪ���ղ�����򿪷��߽����ݵ��ļ���";
                txtDataPath.ForeColor = Color.Gray;
            }

            if (!string.IsNullOrEmpty(outputGDBPath))
            {
                txtOutputGDBPath.Text = outputGDBPath;
                txtOutputGDBPath.ForeColor = Color.DarkGreen;
            }
            else
            {
                txtOutputGDBPath.Text = "��ѡ��������GDB·��";
                txtOutputGDBPath.ForeColor = Color.Gray;
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            bool hasWorkspace = !string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath);
            bool hasDataSource = !string.IsNullOrEmpty(dataSourcePath);
            bool hasOutputGDB = !string.IsNullOrEmpty(outputGDBPath);

            // ��������Դѡ��ťʼ������
            btnBrowseData.Enabled = true;
            btnBrowseOutputGDB.Enabled = true;

            // OK��ť��Ҫ���б�Ҫ��Ϣ����ɺ������
            bool allDataSourcesSelected = hasWorkspace && hasDataSource && hasOutputGDB;

            btnOK.Enabled = allDataSourcesSelected;
        }

        private void BtnSelectWorkspace_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��Ŀ�����ݿ�·����File Geodatabase .gdb��";
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
                        InitializeFormState(); // ���³�ʼ���Է�ӳ״̬�仯
                    }
                    else
                    {
                        MessageBox.Show("��ѡ����Ч��File Geodatabase (.gdb) ·����", "����",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            UpdateButtonStates();
        }

        // �µĺϲ��������Դ�������
        private void BtnBrowseData_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ������ֲ�ʪ���ղ�����򿪷��߽����ݵ��ļ���";
                if (!string.IsNullOrEmpty(dataSourcePath))
                {
                    dialog.SelectedPath = dataSourcePath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    dataSourcePath = dialog.SelectedPath;
                    txtDataPath.Text = dataSourcePath;
                    txtDataPath.ForeColor = Color.DarkGreen;

                    // ���浽����״̬��
                    SharedWorkflowState.DataSourcePath = dataSourcePath;

                    // ���Ұ���LCXZGX_P���ļ����ֲ�ʪ���ղ����ݣ�
                    List<ForestResourcePlugin.LCXZGXFileInfo> lcxzgxFiles = FindFilesWithPattern(dataSourcePath, "LCXZGX_P");

                    // ���浽�������ݹ�����
                    ForestResourcePlugin.SharedDataManager.SetLCXZGXFiles(lcxzgxFiles);

                    // ���Ұ���CZKFBJ���ļ������򿪷��߽����ݣ�
                    List<ForestResourcePlugin.LCXZGXFileInfo> czkfbjFiles = FindFilesWithPattern(dataSourcePath, "CZKFBJ");

                    // ���浽�������ݹ�����
                    ForestResourcePlugin.SharedDataManager.SetCZKFBJFiles(czkfbjFiles);

                    // ��ʾ�ļ��������
                    int totalFiles = lcxzgxFiles.Count + czkfbjFiles.Count;
                    if (totalFiles > 0)
                    {
                        MessageBox.Show($"��ͬһ�ļ������ҵ���\n- {lcxzgxFiles.Count} ���ֲ�ʪ���ղ������ļ�\n- {czkfbjFiles.Count} �����򿪷��߽������ļ�",
                            "�ļ��������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("δ�ҵ���������ļ�����ȷ��ѡ����ļ����Ƿ���ȷ��", "��ʾ",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    UpdateButtonStates();
                }
            }
        }

        // ���GDB·������������ֲ���
        private void BtnBrowseOutputGDB_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "ѡ��������GDB·��";
                if (!string.IsNullOrEmpty(outputGDBPath))
                {
                    dialog.SelectedPath = outputGDBPath;
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    outputGDBPath = dialog.SelectedPath;
                    txtOutputGDBPath.Text = outputGDBPath;
                    txtOutputGDBPath.ForeColor = Color.DarkGreen;

                    // ���浽����״̬
                    SharedWorkflowState.OutputGDBPath = outputGDBPath;

                    MessageBox.Show("������GDB·��ѡ����ɡ�", "�ɹ�",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateButtonStates();
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            // ��֤���б����·������ѡ��
            if (string.IsNullOrEmpty(SharedWorkflowState.WorkspacePath))
            {
                MessageBox.Show("��ѡ�����ռ䡣", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(dataSourcePath))
            {
                MessageBox.Show("��ѡ������Դ�ļ��С�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(outputGDBPath))
            {
                MessageBox.Show("��ѡ��������GDB·����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ����������ʵ�־����ҵ���߼������翪ʼ��������
            if (chkCreateCountyFolders.Checked)
            {

                MessageBox.Show($"׼��Ϊÿ���ش����ļ��в����ɽ�����\n" +
                              $"�����ռ䣺{SharedWorkflowState.WorkspacePath}\n" +
                              $"����Դ�ļ��У�{dataSourcePath}\n" +
                              $"���GDB·����{outputGDBPath}",
                              "����ȷ��", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Ϊÿ���ش������ݿ⼰��ɹ���
            /*  if (!String.IsNullOrEmpty(dataSourcePath))
              {
                  System.IO.DirectoryInfo theFolder = new System.IO.DirectoryInfo(dataSourcePath);
                  System.IO.DirectoryInfo[] dir_Countries = theFolder.GetDirectories();
                  foreach (System.IO.DirectoryInfo dirInfo in dir_Countries) {
                      String curDir = dirInfo.FullName;
                      String countryName = curDir.Substring(curDir.LastIndexOf('\\')+1);
                      if(!CreateTable4Country(outputGDBPath, countryName))
                      {
                          MessageBox.Show("����"+countryName+"���ݿ�ʧ��");
                      }
                  }
              }*/



            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ���¹������ԣ����ⲿ����ѡ���·��
        public string DataSourcePath => dataSourcePath;
        public string OutputGDBPath => outputGDBPath;
        public bool CreateCountyFolders => chkCreateCountyFolders.Checked;

        private void BasicDataPreparationForm_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ��ȡ·���еĵ�һ���ļ������ƣ�������
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <param name="rootDir">��Ŀ¼</param>
        /// <returns>��һ���ļ������ƣ�������</returns>
        private string ExtractCountyNameFromPath(string filePath, string rootDir)
        {
            try
            {
                // �淶��·��
                string normalizedRoot = System.IO.Path.GetFullPath(rootDir).TrimEnd('\\', '/');
                string normalizedFile = System.IO.Path.GetFullPath(filePath);

                // �������·��
                string relativePath = "";
                if (normalizedFile.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = normalizedFile.Substring(normalizedRoot.Length).TrimStart('\\', '/');
                }
                else
                {
                    // ·����ƥ�䣬���Դ�����·����ȡ
                    System.Diagnostics.Debug.WriteLine($"����: �ļ�·�� {normalizedFile} ���ڸ�Ŀ¼ {normalizedRoot} ��");
                    return System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));
                }

                // �ָ�·������ȡ��һ��Ŀ¼���ƣ�������
                string[] pathParts = relativePath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (pathParts.Length >= 1)
                {
                    // ���ص�һ��Ŀ¼���ƣ���Ӧ��������
                    string countyName = pathParts[0];
                    System.Diagnostics.Debug.WriteLine($"��·�� {relativePath} ��ȡ����: {countyName}");
                    return countyName;
                }
                else
                {
                    // ���׷��������·������ʧ�ܣ�ʹ���ļ���
                    string fallbackName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    System.Diagnostics.Debug.WriteLine($"����: �޷���·����ȡ������ʹ���ļ���: {fallbackName}");
                    return fallbackName;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"��ȡ����ʱ����: {ex.Message}");
                return System.IO.Path.GetFileNameWithoutExtension(filePath);
            }
        }

        /// <summary>
        /// ����ָ��Ŀ¼�����ư����ض��ַ������ļ�
        /// </summary>
        /// <param name="rootDir">��Ŀ¼</param>
        /// <param name="pattern">����ƥ��ģʽ</param>
        /// <returns>�ļ���Ϣ�б�</returns>
        private List<ForestResourcePlugin.LCXZGXFileInfo> FindFilesWithPattern(string rootDir, string pattern)
        {
            var result = new List<ForestResourcePlugin.LCXZGXFileInfo>();

            try
            {
                System.Diagnostics.Debug.WriteLine($"��ʼ�� {rootDir} Ŀ¼�²��Ұ��� {pattern} ���ļ�");

                // 1. ���Ȳ���GDBҪ����
                System.Diagnostics.Debug.WriteLine("��1��������GDBҪ����...");
                var gdbFeatureClasses = ForestResourcePlugin.GdbFeatureClassFinder.FindFeatureClassesWithPattern(
                    rootDir, pattern, ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon);

                // ���ҵ���GDBҪ������ӵ������
                result.AddRange(gdbFeatureClasses);
                System.Diagnostics.Debug.WriteLine($"�ҵ� {gdbFeatureClasses.Count} ��GDBҪ����");

                // 2. �ٲ���Shapefile�ļ�
                System.Diagnostics.Debug.WriteLine("��2��������Shapefile�ļ�...");

                // ȷ��Ŀ¼����
                if (Directory.Exists(rootDir))
                {
                    string[] files = System.IO.Directory.GetFiles(rootDir, "*.shp", System.IO.SearchOption.AllDirectories);
                    System.Diagnostics.Debug.WriteLine($"�� {rootDir} Ŀ¼���ҵ� {files.Length} ��Shapefile�ļ�");

                    // ɸѡ����ָ��ģʽ��Shapefile
                    int matchCount = 0;
                    foreach (string filePath in files)
                    {
                        if (filePath.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // ��ȡ��������һ���ļ������ƣ�
                            string countyName = ExtractCountyNameFromPath(filePath, rootDir);

                            result.Add(new ForestResourcePlugin.LCXZGXFileInfo
                            {
                                FullPath = filePath,
                                DisplayName = countyName,
                                IsGdb = false,
                                GeometryType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon // ����Ϊ��
                            });

                            matchCount++;
                            System.Diagnostics.Debug.WriteLine($"�ҵ�ƥ���Shapefile�ļ�[{matchCount}]: {filePath}, ����: {countyName}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"����: Ŀ¼ {rootDir} ������");
                }

                System.Diagnostics.Debug.WriteLine($"���ҵ� {result.Count} ��ƥ���ļ� (GDBҪ����: {gdbFeatureClasses.Count}, Shapefile: {result.Count - gdbFeatureClasses.Count})");

                // �����ϸ�Ľ����Ϣ
                for (int i = 0; i < result.Count; i++)
                {
                    var item = result[i];
                    System.Diagnostics.Debug.WriteLine($"���[{i + 1}]: {item.DisplayName}, ·��: {item.FullPath}, ����: {(item.IsGdb ? "GDBҪ����" : "Shapefile")}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"�����ļ�ʱ����: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"�쳣����: {ex}");
                MessageBox.Show($"�����ļ�ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        /// <summary>
        /// ��ȡ·���еĵڶ����ļ�������
        /// </summary>
        /// <param name="filePath">�ļ�·��</param>
        /// <param name="rootDir">��Ŀ¼</param>
        /// <returns>�ڶ����ļ�������</returns>
        [Obsolete("�˷����ѹ�ʱ����ʹ�� ExtractCountyNameFromPath ����")]
        private string ExtractSecondLevelFolderName(string filePath, string rootDir)
        {
            // ����ԭ������ά���������ԣ������Ϊ��ʱ
            // ���ڵ����µ�������ȡ����
            return ExtractCountyNameFromPath(filePath, rootDir);
        }

        private Boolean CreateTable4Country(String path, String countryName)
        {
            if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(countryName))
            {
                DirectoryInfo sourceFolder = new DirectoryInfo(path);
                DirectoryInfo countryFolder = sourceFolder.CreateSubdirectory(countryName);
                if (countryFolder.Exists)
                {
                    Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
                    IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);

                    IWorkspaceName workspaceName = workspaceFactory.Create(countryFolder.FullName, countryName + ".gdb", null, 0);

                    // Cast the workspace name object to the IName interface and open the workspace.
                    ESRI.ArcGIS.esriSystem.IName name = (ESRI.ArcGIS.esriSystem.IName)workspaceName;
                    IWorkspace workspace = (IWorkspace)name.Open();

                    if (!CreateResultTable("LCXZGX", workspace))
                    {
                        MessageBox.Show("������״Ҫ����ʧ��");
                        return false;
                    }
                    if (!CreateResultTable("SLZYZC", workspace))
                    {
                        MessageBox.Show("�������ɹ�Ҫ����ʧ��");
                        return false;
                    }
                    if (!CreateResultTable("SLZYZC_DLTB", workspace))
                    {
                        MessageBox.Show("��������ֵ�Ҫ����ʧ��");
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }

        //���ݳɹ�����������
        private Boolean CreateResultTable(String TableName, IWorkspace workspace)
        {
            String featureClassName = TableName;
            IFeatureClass featureClass = null;
            IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspace;

            try
            {
                // ���Ҫ�����Ƿ��Ѵ���
                bool featureClassExists = false;
                try
                {
                    featureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                    featureClassExists = true;
                    System.Diagnostics.Debug.WriteLine($"Ҫ����{featureClassName}�Ѵ��ڣ�������CGCS2000����ϵ");

                    // Ϊ�Ѵ��ڵ�Ҫ��������CGCS2000����ϵ
                    SetCoordinateSystemForExistingFeatureClass(featureClass, featureClassName);

                    return true;
                }
                catch
                {
                    // Ҫ���಻���ڣ���������
                    featureClassExists = false;
                }

                if (!featureClassExists)
                {
                    System.Diagnostics.Debug.WriteLine($"��ʼ����{featureClassName}Ҫ���ಢ����CGCS2000����ϵ");

                    ESRI.ArcGIS.esriSystem.UID CLSID = new ESRI.ArcGIS.esriSystem.UIDClass();
                    CLSID.Value = "esriGeoDatabase.Feature";

                    IObjectClassDescription objectClassDescription = new FeatureClassDescriptionClass();
                    IFields fields = objectClassDescription.RequiredFields;
                    IFieldsEdit fieldsEdit = (IFieldsEdit)fields;

                    // ���ݱ���������Ӧ���ֶ�
                    switch (featureClassName)
                    {
                        case "LCXZGX":
                            FeatureClassFieldsTemplate.GenerateLcxzgxFields(fieldsEdit);
                            break;
                        case "SLZYZC":
                            FeatureClassFieldsTemplate.GenerateSlzyzcFields(fieldsEdit);
                            break;
                        case "SLZYZC_DLTB":
                            FeatureClassFieldsTemplate.GenerateSlzyzc_dltbFields(fieldsEdit);
                            break;
                    }

                    fields = (IFields)fieldsEdit;

                    // ���Ҽ����ֶβ�����CGCS2000����ϵ
                    String strShapeField = "";
                    for (int j = 0; j < fields.FieldCount; j++)
                    {
                        IField field = fields.get_Field(j);
                        if (field.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            strShapeField = field.Name;

                            // ��ȡ�����ֶζ���
                            IGeometryDef geometryDef = field.GeometryDef;
                            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

                            // ����������CGCS2000�ռ�ο�ϵͳ
                            ISpatialReference cgcs2000SpatialRef = CreateCGCS2000SpatialReference();
                            if (cgcs2000SpatialRef != null)
                            {
                                geometryDefEdit.SpatialReference_2 = cgcs2000SpatialRef;
                                System.Diagnostics.Debug.WriteLine($"Ϊ{featureClassName}�ļ����ֶ�{strShapeField}����CGCS2000����ϵ");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"���棺�޷�Ϊ{featureClassName}����CGCS2000����ϵ����ʹ��Ĭ������ϵ");
                            }
                            break;
                        }
                    }

                    // Use IFieldChecker to create a validated fields collection.
                    IFieldChecker fieldChecker = new FieldCheckerClass();
                    IEnumFieldError enumFieldError = null;
                    IFields validatedFields = null;
                    fieldChecker.ValidateWorkspace = (IWorkspace)workspace;
                    fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

                    if (enumFieldError != null)
                    {
                        MessageBox.Show("�ֶ�У��ʧ�ܣ�" + featureClassName);
                        return false;
                    }

                    // ����Ҫ����
                    featureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields, CLSID, null, esriFeatureType.esriFTSimple, strShapeField, "");

                    if (featureClass == null)
                    {
                        MessageBox.Show("Ϊ" + featureClassName + "����Ҫ����ʧ��");
                        return false;
                    }

                    System.Diagnostics.Debug.WriteLine($"�ɹ�����{featureClassName}Ҫ���ಢ����CGCS2000����ϵ");
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����/����{featureClassName}Ҫ����ʱ����: {ex.Message}");
                MessageBox.Show($"����/����{featureClassName}Ҫ����ʱ����: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // �ͷ�COM����
                if (featureClass != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(featureClass);
                }
            }
        }

        /// <summary>
        /// ����CGCS2000ͶӰ����ϵ������ʹ��3�ȴ�37��ͶӰ����ϵ��
        /// </summary>
        /// <returns>CGCS2000����ϵ����</returns>
        private ISpatialReference CreateCGCS2000SpatialReference()
        {
            // ���ȳ��Դ���ͶӰ����ϵ
            ISpatialReference projectedSpatialRef = CreateCGCS2000ProjectedSpatialReference();
            if (projectedSpatialRef != null)
            {
                return projectedSpatialRef;
            }

            // ���ͶӰ����ϵ����ʧ�ܣ�ʹ�õ�������ϵ��Ϊ����
            return CreateCGCS2000GeographicSpatialReference();
        }

        /// <summary>
        /// ����CGCS2000 3�ȴ�37��ͶӰ����ϵ
        /// </summary>
        /// <returns>CGCS2000 3�ȴ�37��ͶӰ����ϵ����</returns>
        private ISpatialReference CreateCGCS2000ProjectedSpatialReference()
        {
            try
            {
                // ����1������ʹ���Զ���EPSG���봴��CGCS2000 3�ȴ�37��ͶӰ����ϵ
                try
                {
                    // �����ռ�ο�ϵͳ�����ӿ�
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);
                    ISpatialReferenceFactory spatialRefFactory = spatialRefEnvObj as ISpatialReferenceFactory;

                    if (spatialRefFactory != null)
                    {
                        // ����ʹ�ÿ��ܵ�EPSG���루CGCS2000 3�ȴ�ͶӰ����ϵͨ����4491+���ŵ���ʽ��
                        try
                        {
                           
                            IProjectedCoordinateSystem projectedCS = spatialRefFactory.CreateProjectedCoordinateSystem(4525);
                            if (projectedCS != null)
                            {
                                System.Diagnostics.Debug.WriteLine("�ɹ�ʹ��EPSG 4528����CGCS2000 3�ȴ�37��ͶӰ����ϵ");
                                return projectedCS as ISpatialReference;
                            }
                        }
                        catch (Exception ex1)
                        {
                            System.Diagnostics.Debug.WriteLine($"ʹ��EPSG 4528����ͶӰ����ϵʧ��: {ex1.Message}");
                        }
                    }
                }
                catch (Exception ex1)
                {
                    System.Diagnostics.Debug.WriteLine($"ʹ��EPSG���봴��CGCS2000ͶӰ����ϵʧ��: {ex1.Message}");
                }

                // ����2��ʹ��WKT�ַ�������ͶӰ����ϵ
                try
                {
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);

                    // ʹ�÷������CreateESRISpatialReferenceFromPRJString����
                    System.Reflection.MethodInfo createFromPrjMethod = spatialRefEnvType.GetMethod("CreateESRISpatialReferenceFromPRJString");
                    if (createFromPrjMethod != null)
                    {
                        object[] parameters = new object[] { CGCS2000_3_DEGREE_GK_ZONE_37_WKT, null, null };
                        object result = createFromPrjMethod.Invoke(spatialRefEnvObj, parameters);

                        if (result != null && result is ISpatialReference)
                        {
                            System.Diagnostics.Debug.WriteLine("�ɹ�ʹ��WKT�ַ�������CGCS2000 3�ȴ�37��ͶӰ����ϵ");
                            return result as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"ʹ��WKT�ַ�������CGCS2000ͶӰ����ϵʧ��: {ex2.Message}");
                }

                System.Diagnostics.Debug.WriteLine("����ͶӰ����ϵ����������ʧ��");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����CGCS2000ͶӰ����ϵʱ�����������: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ����CGCS2000��������ϵ�����÷�����
        /// </summary>
        /// <returns>CGCS2000��������ϵ����</returns>
        private ISpatialReference CreateCGCS2000GeographicSpatialReference()
        {
            try
            {
                // ����1������ʹ��EPSG����4490����CGCS2000��������ϵ
                try
                {
                    // �����ռ�ο�ϵͳ�����ӿ�
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);
                    ISpatialReferenceFactory spatialRefFactory = spatialRefEnvObj as ISpatialReferenceFactory;

                    if (spatialRefFactory != null)
                    {
                        // ʹ��EPSG����4490����CGCS2000��������ϵ
                        IGeographicCoordinateSystem geographicCS = spatialRefFactory.CreateGeographicCoordinateSystem(4490);
                        if (geographicCS != null)
                        {
                            System.Diagnostics.Debug.WriteLine("�ɹ�ʹ��EPSG 4490����CGCS2000��������ϵ");
                            return geographicCS as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex1)
                {
                    System.Diagnostics.Debug.WriteLine($"ʹ��EPSG 4490����CGCS2000ʧ��: {ex1.Message}");
                }

                // ����2�����÷��� - ���Դ�WKT�ַ�������
                try
                {
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);

                    // ʹ�÷������CreateESRISpatialReferenceFromPRJString����
                    System.Reflection.MethodInfo createFromPrjMethod = spatialRefEnvType.GetMethod("CreateESRISpatialReferenceFromPRJString");
                    if (createFromPrjMethod != null)
                    {
                        object[] parameters = new object[] { CGCS2000_WKT, null, null };
                        object result = createFromPrjMethod.Invoke(spatialRefEnvObj, parameters);

                        if (result != null && result is ISpatialReference)
                        {
                            System.Diagnostics.Debug.WriteLine("�ɹ�ʹ��WKT�ַ�������CGCS2000��������ϵ");
                            return result as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"ʹ��WKT�ַ�������CGCS2000ʧ��: {ex2.Message}");
                }

                // ����3�����ı��÷��� - ����һ��ͨ�õĵ�������ϵ
                try
                {
                    Type spatialRefEnvType = Type.GetTypeFromProgID("esriGeometry.SpatialReferenceEnvironment");
                    object spatialRefEnvObj = Activator.CreateInstance(spatialRefEnvType);
                    ISpatialReferenceFactory spatialRefFactory = spatialRefEnvObj as ISpatialReferenceFactory;

                    if (spatialRefFactory != null)
                    {
                        // ����WGS84��������ϵ��Ϊ����
                        IGeographicCoordinateSystem wgs84CS = spatialRefFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                        if (wgs84CS != null)
                        {
                            System.Diagnostics.Debug.WriteLine("���棺ʹ��WGS84����ϵ��Ϊ���÷���");
                            return wgs84CS as ISpatialReference;
                        }
                    }
                }
                catch (Exception ex3)
                {
                    System.Diagnostics.Debug.WriteLine($"������������ϵҲʧ��: {ex3.Message}");
                }

                System.Diagnostics.Debug.WriteLine("���д���CGCS2000��������ϵ�ķ�����ʧ����");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"����CGCS2000��������ϵʱ�����������: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Ϊ�Ѵ��ڵ�Ҫ��������CGCS2000����ϵ
        /// </summary>
        /// <param name="featureClass">Ҫ����</param>
        /// <param name="featureClassName">Ҫ��������</param>
        private void SetCoordinateSystemForExistingFeatureClass(IFeatureClass featureClass, string featureClassName)
        {
            try
            {
                // ����CGCS2000�ռ�ο�ϵͳ
                ISpatialReference cgcs2000SpatialRef = CreateCGCS2000SpatialReference();
                if (cgcs2000SpatialRef == null)
                {
                    System.Diagnostics.Debug.WriteLine($"�޷�Ϊ{featureClassName}��������ϵ���ռ�ο�ϵͳ����ʧ��");
                    return;
                }

                // ��Ҫ�����ȡ�������ݼ�����
                IGeoDataset geoDataset = (IGeoDataset)featureClass;

                // ��鵱ǰ����ϵ
                ISpatialReference currentSpatialRef = geoDataset.SpatialReference;
                if (currentSpatialRef != null)
                {
                    System.Diagnostics.Debug.WriteLine($"{featureClassName}��ǰ��������ϵ��׼������ΪCGCS2000");
                }

                // ת��ΪIGeoDatasetSchemaEdit�������޸�����ϵ
                IGeoDatasetSchemaEdit schemaEdit = (IGeoDatasetSchemaEdit)geoDataset;

                // ΪҪ���ඨ��CGCS2000����ϵ
                schemaEdit.AlterSpatialReference(cgcs2000SpatialRef);

                System.Diagnostics.Debug.WriteLine($"�ɹ�Ϊ{featureClassName}Ҫ��������CGCS2000����ϵ");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ϊ{featureClassName}��������ϵʱ����: {ex.Message}");
                // ���׳��쳣��ȷ�������������̼���
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "��ѡ��������Ŀ¼";
            // folderBrowser.ShowNewFolderButton = true;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                dataSourcePath = folderBrowser.SelectedPath;
            }

            if (!String.IsNullOrEmpty(dataSourcePath))
            {
                System.IO.DirectoryInfo theFolder = new System.IO.DirectoryInfo(dataSourcePath);
                System.IO.DirectoryInfo[] dir_Countries = theFolder.GetDirectories();
                foreach (System.IO.DirectoryInfo dirInfo in dir_Countries)
                {
                    String curDir = dirInfo.FullName;
                    String countryName = curDir.Substring(curDir.LastIndexOf('\\') + 1);
                    if (!CreateTable4Country(outputGDBPath, countryName))
                    {
                        MessageBox.Show("����" + countryName + "���ݿ�ʧ��");
                        return;
                    }
                }
                MessageBox.Show("�ѽ����ɹ����ݿ⼰��ṹ");
            }
            else
            {
                MessageBox.Show("Դ·��Ϊ��");
            }
        }
    }
}