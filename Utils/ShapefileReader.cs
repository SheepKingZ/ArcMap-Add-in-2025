using System;
using System.IO;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

namespace ForestResourcePlugin
{
    /// <summary>
    /// Utility class for reading Shapefile data
    /// </summary>
    public class ShapefileReader
    {
        /// <summary>
        /// Gets a list of field names from a Shapefile
        /// </summary>
        /// <param name="shapefilePath">Full path to the Shapefile (.shp)</param>
        /// <returns>List of field names in the Shapefile</returns>
        public static List<string> GetShapefileFieldNames(string shapefilePath)
        {
            List<string> fieldNames = new List<string>();

            try
            {
                // Validate file path
                if (string.IsNullOrEmpty(shapefilePath) || !File.Exists(shapefilePath))
                {
                    throw new FileNotFoundException("指定的Shapefile文件不存在", shapefilePath);
                }

                if (!shapefilePath.ToLower().EndsWith(".shp"))
                {
                    throw new ArgumentException("文件必须是Shapefile格式(.shp)", nameof(shapefilePath));
                }

                // Get the directory and filename without extension
                string directory = Path.GetDirectoryName(shapefilePath);
                string fileName = Path.GetFileNameWithoutExtension(shapefilePath);

                // Create a workspace factory and open the shapefile
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);

                // Get the field information
                IFields fields = featureClass.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    fieldNames.Add(field.Name);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow
                System.Diagnostics.Debug.WriteLine($"Error reading Shapefile fields: {ex.Message}");
                throw;
            }

            return fieldNames;
        }
        
        /// <summary>
        /// Gets a sample of data from the Shapefile
        /// </summary>
        /// <param name="shapefilePath">Full path to the Shapefile (.shp)</param>
        /// <param name="maxRows">Maximum number of rows to return</param>
        /// <returns>DataTable containing sample data</returns>
        public static System.Data.DataTable GetShapefileData(string shapefilePath, int maxRows = 10)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            
            try
            {
                // Validate file path
                if (string.IsNullOrEmpty(shapefilePath) || !File.Exists(shapefilePath))
                {
                    throw new FileNotFoundException("指定的Shapefile文件不存在", shapefilePath);
                }

                // Get the directory and filename without extension
                string directory = Path.GetDirectoryName(shapefilePath);
                string fileName = Path.GetFileNameWithoutExtension(shapefilePath);

                // Create a workspace factory and open the shapefile
                IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
                IFeatureWorkspace featureWorkspace = (IFeatureWorkspace)workspaceFactory.OpenFromFile(directory, 0);
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(fileName);

                // Get the field information and create columns
                IFields fields = featureClass.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.get_Field(i);
                    if (field.Type != esriFieldType.esriFieldTypeGeometry && 
                        field.Type != esriFieldType.esriFieldTypeBlob &&
                        field.Type != esriFieldType.esriFieldTypeRaster &&
                        field.Type != esriFieldType.esriFieldTypeGlobalID &&
                        field.Type != esriFieldType.esriFieldTypeGUID &&
                        field.Type != esriFieldType.esriFieldTypeXML)
                    {
                        dataTable.Columns.Add(field.Name);
                    }
                }

                // Create a query filter to get features
                IQueryFilter queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = "";

                // Get features and populate data table
                IFeatureCursor featureCursor = featureClass.Search(queryFilter, true);
                IFeature feature;
                int rowCount = 0;

                while ((feature = featureCursor.NextFeature()) != null && rowCount < maxRows)
                {
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    
                    for (int i = 0; i < fields.FieldCount; i++)
                    {
                        IField field = fields.get_Field(i);
                        if (dataTable.Columns.Contains(field.Name))
                        {
                            object value = feature.get_Value(i);
                            dataRow[field.Name] = value ?? DBNull.Value;
                        }
                    }
                    
                    dataTable.Rows.Add(dataRow);
                    rowCount++;
                }
                
                // Release resources
                System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading Shapefile data: {ex.Message}");
                throw;
            }
            
            return dataTable;
        }
    }
}