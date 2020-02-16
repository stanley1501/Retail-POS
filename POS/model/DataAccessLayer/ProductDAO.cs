﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;
using POS;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Model.DataAccessLayer
{
    public class ProductDAO : IProductDAO
    {
        public IList<Product> getAllProducts()
        {
            IList<Product> products = new List<Product>();

            string queryGetAllProducts = "SELECT * FROM Products;";

            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand(queryGetAllProducts, conn);

                    // try a connection
                    conn.Open();

                    // execute the query
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product.setProductID(reader.GetInt32(0));
                        product.setProductIDNumber(reader.GetString(1));
                        product.setDescription(reader.GetString(2));
                        product.setQuantity(reader.GetInt32(3));
                        // a SQL float is a .NET double
                        double dprice = reader.GetDouble(4);
                        float fprice = Convert.ToSingle(dprice);
                        product.setPrice(fprice);

                        products.Add(product);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }

            return products;
        }

        public Product getProduct(string idNumber)
        {
            Product product = new Product();

            string queryGetProduct = "SELECT * FROM Products " +
                                     "WHERE ProductIDNumber = @id;";

            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand(queryGetProduct, conn);

                    // parameterise
                    SqlParameter idParam = new SqlParameter();
                    idParam.ParameterName = "@id";
                    idParam.Value = idNumber;
                    cmd.Parameters.Add(idParam);

                    // attempt a connection
                    conn.Open();

                    // execute the query
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        
                        product.setProductID(reader.GetInt32(0));
                        product.setProductIDNumber(reader.GetString(1));
                        product.setDescription(reader.GetString(2));
                        product.setQuantity(reader.GetInt32(3));
                        // a SQL float is a .NET double
                        double dprice = reader.GetDouble(4);
                        float fprice = Convert.ToSingle(dprice);
                        product.setPrice(fprice);

                        
                        /*
                        long productID = reader.GetInt32(0);
                        string productIDNumber = reader.GetString(1);
                        string description = reader.GetString(2);
                        int quantity = reader.GetInt32(3);
                        float price = reader.GetFloat(4);

                        product = new Product(productID, productIDNumber, description, quantity, price);
                        */
                    }

                    return product;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
        }

        // this works
        public int deleteProduct(Product product)
        {
            string queryDeleteProduct = "DELETE FROM Products " +
                                        "WHERE ProductID" +
                                        "" +
                                        " = @id;";
            int result = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand(queryDeleteProduct, conn);

                    // parameterise
                    SqlParameter idParam = new SqlParameter();
                    idParam.ParameterName = "@id";
                    idParam.Value = product.getProductID();
                    cmd.Parameters.Add(idParam);

                    // attempt a connection
                    conn.Open();

                    // execute the query
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }

            return result;
        }

        // this works
        public int addProduct(Product product)
        {
            string queryAddProduct = "INSERT INTO Products (ProductIDNumber,Description_,Quantity,Price) " +
                                     "VALUES (@idNumber, @description, @quantity, @price);";
            int result = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING))
                {
                    SqlCommand cmd = new SqlCommand(queryAddProduct, conn);

                    // parameterise
                    /*
                    SqlParameter idParam = new SqlParameter();
                    idParam.ParameterName = "@id";
                    idParam.Value = product.getProductID();
                    cmd.Parameters.Add(idParam);
                    */

                    SqlParameter idNumberParam = new SqlParameter();
                    idNumberParam.ParameterName = "@idNumber";
                    idNumberParam.Value = product.getProductIDNumber();
                    cmd.Parameters.Add(idNumberParam);

                    SqlParameter descParam = new SqlParameter();
                    descParam.ParameterName = "@description";
                    descParam.Value = product.getDescription();
                    cmd.Parameters.Add(descParam);

                    SqlParameter quantityParam = new SqlParameter();
                    quantityParam.ParameterName = "@quantity";
                    quantityParam.Value = product.getQuantity();
                    cmd.Parameters.Add(quantityParam);

                    SqlParameter priceParam = new SqlParameter();
                    priceParam.ParameterName = "@price";
                    priceParam.Value = product.getPrice();
                    cmd.Parameters.Add(priceParam);

                    // attempt a connection
                    conn.Open();

                    // execute the query
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return result;
        }

        public int updateProduct(Product newProduct, Product oldProduct)
        {
            throw new NotImplementedException();
        }
    }
}
