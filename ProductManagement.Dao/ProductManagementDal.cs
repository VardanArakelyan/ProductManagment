using ProductManagement.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Dal
{
    public class ProductManagementDal : IProductManagementDal
    {
        private String _connectionString;

        public ProductManagementDal()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Local"].ConnectionString;
        }

        public async Task<int> GetCount(string filterName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string strWhere = "";
                if (!string.IsNullOrEmpty(filterName))
                {
                    strWhere = $"WHERE Name Like '%{filterName}%'";
                }
                string sqlQuery = $"SELECT COUNT(*) FROM Product {strWhere}";
                
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        int count = 0;
                        while (await reader.ReadAsync())
                        {
                            count = (int)reader[0];
                            break;
                        }
                        reader.Close();
                        return count;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Something went wrong while retrieving products count", ex);
                }
            }
        }

        public async Task<List<Product>> GetProducts(int limitStart, int rowsCount, string nameFilter = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string strWhere = "";
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    strWhere = $"WHERE Name Like '%{nameFilter}%'";
                }

                string sqlQuery = $"SELECT * FROM Product {strWhere} ORDER BY Id DESC OFFSET {limitStart} ROWS FETCH NEXT {rowsCount} ROWS ONLY";

                

                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        var products = new List<Product>();
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                Id = (int)reader["Id"],
                                Code = (long)reader["Code"],
                                Name = (string)reader["Name"],
                                Price = (decimal)reader["Price"],
                                Barcode = (string)reader["Barcode"]
                            });
                        }
                        reader.Close();

                        return products;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Something went wrong while retrieving products limited from {limitStart}, rows count = {rowsCount}", ex);
                }
            }
        }

        public async Task DeleteById(int productId)
        {
            string sqlQuery = String.Format("delete from Product where Id = {0}", productId);

            using (SqlConnection connection =
                new SqlConnection(_connectionString))
            {
                
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();

                        int rowsDeletedCount = await command.ExecuteNonQueryAsync();
                        if (rowsDeletedCount != 1)
                            throw new Exception($"Could not delete product with id {productId}");

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Something went wrong when trying to delete product with id {productId}", ex);
                }
            }
        }

        public async Task<int> Insert(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sqlQuery = $"INSERT INTO [ProductManagement].[dbo].[Product](Code, Name, Price, Barcode) OUTPUT Inserted.ID VALUES({product.Code}, N'{product.Name}', {product.Price}, N'{product.Barcode}'); ";
                
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        int insertedId = (int)(await command.ExecuteScalarAsync());
                        return insertedId;
                    }
                }
                catch (SqlException sqlExc)
                {
                    string msg = "";
                    foreach (SqlError error in sqlExc.Errors)
                    {
                        msg += $"{error.Number}: {error.Message}.{Environment.NewLine}";
                    }
                    throw new Exception($"Insert faild. Errore message: {msg}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Something went wrong while inserting new item", ex);
                }
            }
        }

        public async Task<int> Update(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sqlQuery = $"UPDATE [ProductManagement].[dbo].[Product] SET Code = {product.Code}, Name = N'{product.Name}', Price = {product.Price}, Barcode = N'{product.Barcode}' WHERE Id = {product.Id}; ";
                
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                        return product.Id.Value;
                    }
                }
                catch (SqlException sqlExc)
                {
                    string msg = "";
                    foreach (SqlError error in sqlExc.Errors)
                    {
                        msg += $"{error.Number}: {error.Message}.{Environment.NewLine}";
                    }
                    throw new Exception($"Update faild. Errore message: {msg}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Something went wrong while updating product with id - {product.Id}", ex);
                }
            }
        }

        public async Task<List<int>> GetSimilarProductIds(Product productToMatch)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlQuery = $"SELECT Id FROM Product WHERE Code={productToMatch.Code} OR Name=N'{productToMatch.Name}'";
                if (!string.IsNullOrEmpty(productToMatch.Barcode))
                {
                    sqlQuery += $" OR Barcode=N'{productToMatch.Barcode}'";
                }
                
                try
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        List<int> ids = new List<int>();
                        while (await reader.ReadAsync())
                        {
                            ids.Add((int)reader[0]);
                        }
                        reader.Close();

                        return ids;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Something went wrong while retrieving matching product with code - {productToMatch.Code}", ex);
                }
            }
        }

        public async Task GenerateRandomProducts(int countToGenerate)
        {
            DataTable table = GenerateDataTable(countToGenerate);
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                {
                    copy.DestinationTableName = "Product";
                    try
                    {
                        await copy.WriteToServerAsync(table);
                    }
                    catch (SqlException sqlExc)
                    {
                        string msg = "";
                        foreach (SqlError error in sqlExc.Errors)
                        {
                            msg += $"{error.Number}: {error.Message}.{Environment.NewLine}";
                        }
                        throw new Exception($"Insert faild. Errore message: {msg}");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Something went wrong while inserting new item", ex);
                    }
                }
            }
        }

        private DataTable GenerateDataTable(int countToGenerate)
        {
            DataTable table = new DataTable("Product");
            DataColumn columnId = new DataColumn
            {
                DataType = Type.GetType("System.Int32"),
                ColumnName = "Id",
                AutoIncrement = true
            };
            table.Columns.Add(columnId);
            DataColumn[] keys = new DataColumn[1];
            keys[0] = columnId;
            table.PrimaryKey = keys;


            var column = new DataColumn
            {
                DataType = typeof(long),
                AllowDBNull = false,
                Caption = "Code",
                ColumnName = "Code"
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = typeof(String),
                AllowDBNull = false,
                Caption = "Name",
                ColumnName = "Name"
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = typeof(Decimal),
                AllowDBNull = false,
                Caption = "Price",
                ColumnName = "Price"
            };
            table.Columns.Add(column);

            column = new DataColumn
            {
                DataType = typeof(String),
                AllowDBNull = true,
                Caption = "Barcode",
                ColumnName = "Barcode"
            };
            table.Columns.Add(column);

            // Add 10 rows and set values. 
            DataRow row;
            for (int i = 0; i < countToGenerate; i++)
            {
                row = table.NewRow();
                row["Code"] = DateTime.Now.Ticks;
                row["Name"] = Guid.NewGuid().ToString();
                row["Price"] = i;
                row["Barcode"] = Guid.NewGuid().ToString();
                table.Rows.Add(row);
            }

            return table;
        }
    }
}