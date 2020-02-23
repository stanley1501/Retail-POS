﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ObjectModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using POS;

namespace Model.DataAccessLayer
{
    public class TransactionDAO : ITransactionDAO
    {
        public IList<Transaction> getAllTransactions()
        {
            IList<Transaction> transList = new List<Transaction>();

            //string getAllTransactionsQuery = "SELECT * FROM Transactions;";
            string getAllTransactionsQuery = "SELECT Transactions.TransactionID, Transactions.Timestamp_, Customers.CustomerID, Customers.FullName, Staff.StaffID, Staff.FullName, " +
                                             "Products.ProductIDNumber, Products.Description_, Products.Price " +
                                             "FROM(((Transactions " + 
                                              "FULL JOIN Customers ON Transactions.CustomerID = Customers.CustomerID)" + 
                                              "INNER JOIN Products ON Transactions.ProductID = Products.ProductID)" +
                                              "INNER JOIN Staff ON Transactions.StaffID = Staff.StaffID);";

            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING))
                {
                    // define the command object
                    SqlCommand cmd = new SqlCommand(getAllTransactionsQuery, conn);

                    // try a connection
                    conn.Open();

                    // execute the query
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transaction transaction = new Transaction();
                        transaction.setTransactionID(reader.GetInt32(0));
                        transaction.setTimestamp(reader.GetDateTime(1).ToString());

                        transList.Add(transaction);
                    }
                }
            }
            catch (Exception ex)
            {     
                throw;
            }

            return transList;
        }

        // actually, may not want to delete transactions...
        public void deleteTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public void addTransaction(ValueTuple<int,int,Dictionary<string,int>> items)
        {
            // extract staff and customer information
            int staffID = items.Item1;
            int customerID = items.Item2;

            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING))
                {
                    // try a connection
                    conn.Open();

                    // create command object
                    SqlCommand cmd = conn.CreateCommand();

                    // parameterise
                    SqlParameter customerIDparam = new SqlParameter();
                    customerIDparam.ParameterName = "@customerID";
                    cmd.Parameters.Add(customerIDparam);

                    SqlParameter productIDparam = new SqlParameter();
                    productIDparam.ParameterName = "@productID";
                    cmd.Parameters.Add(productIDparam);

                    SqlParameter staffIDparam = new SqlParameter();
                    staffIDparam.ParameterName = "@staffID";
                    cmd.Parameters.Add(staffIDparam);

                    SqlParameter quantityParam = new SqlParameter();
                    quantityParam.ParameterName = "@quantity";
                    cmd.Parameters.Add(quantityParam);

                    foreach (KeyValuePair<string, int> keyVal in items.Item3)
                    {
                        // extract product information
                        string productIDnumber = keyVal.Key;
                        int productQuantity = keyVal.Value;
                        ProductDAO productDAO = new ProductDAO();
                        Product currProduct = productDAO.getProduct(productIDnumber);
                        long productID = currProduct.getProductID();
                        int productInStock = currProduct.getQuantity();

                        // assign values to parameters
                        customerIDparam.Value = customerID;
                        productIDparam.Value = productID;
                        staffIDparam.Value = staffID;
                        quantityParam.Value = productInStock - productQuantity;

                        for (int i = 1; i <= productQuantity; i++)
                        {
                            // start transaction
                            SqlTransaction transaction = conn.BeginTransaction("Transaction");

                            try
                            {
                                // must assign both transaction object and connection
                                // to command object for a pending local transaction
                                cmd.Connection = conn;
                                cmd.Transaction = transaction;

                                // update Transactions table
                                if (customerID != 0)
                                {
                                    /*
                                    cmd.Parameters.AddWithValue("@customerID", customerID);
                                    cmd.Parameters.AddWithValue("@productID", productID);
                                    cmd.Parameters.AddWithValue("@staffID", staffID);
                                    cmd.Parameters.AddWithValue("@quantity", (productInStock - productQuantity));
                                    */
                                    cmd.CommandText = "INSERT INTO Transactions (Timestamp_, CustomerID, StaffID, ProductID)" +
                                                      "VALUES (SYSDATETIME(),@customerID,@staffID,@productID);";
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                {
                                    /*
                                    cmd.Parameters.AddWithValue("@productID", productID);
                                    cmd.Parameters.AddWithValue("@staffID", staffID);
                                    cmd.Parameters.AddWithValue("@quantity", (productInStock - productQuantity));
                                    */
                                    cmd.CommandText = "INSERT INTO Transactions (Timestamp_, StaffID, ProductID)" +
                                                      "VALUES (SYSDATETIME(),@staffID,@productID);";
                                    cmd.ExecuteNonQuery();
                                }

                                // update Products table (inventory)
                                cmd.CommandText = "UPDATE Products SET Quantity = @quantity WHERE ProductID = " + productID + ";";
                                cmd.ExecuteNonQuery();

                                // commit transaction
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                // rollback transaction and pass the error to the top level
                                transaction.Rollback();

                                throw;
                            }
                        }
                    }
                }
            }
            catch (SqlException SqlEx)
            {
                throw;
            }
        }

        public int addTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
            /*
            // one-one relationship between transaction and product
            // transaction may or may not have a customer (member) associated with it.
            string createTransactionQuery = null;
            
            if (transaction.getCustomer()==null)
            {
                // non-member transaction
                createTransactionQuery = "BEGIN TRANSACTION Trans " +
                                            "  BEGIN TRY " +
                                            " " +
                                            "      --New record in Transactions table " +
                                            "      INSERT INTO Transactions (Timestamp_, CustomerID, StaffID, ProductID) " +
                                            "      VALUES (SYSDATETIME(),@varCustomerID,@varStaffID,@varProductID); " +
                                            " " +
                                            "      --Update inventory (Products table)" +
                                            "      --First retrieve the current quantity of this product" +
                                            "      DECLARE @varCurrQuantity int; " +
                                            "      SET @varCurrQuantity = (SELECT Quantity FROM Products WHERE ProductID=@varProductID); " +
                                            "      --and update it" +
                                            "      UPDATE Products " +
                                            "      SET Quantity = @varQuantity-1 " +
                                            "      WHERE ProductID = @varProductID; " +
                                            "      COMMIT TRANSACTION Trans " +  
                                            "  END TRY " +
                                            "  BEGIN CATCH " +
                                            "      ROLLBACK TRANSACTION Trans " +
                                            "       THROW; " +
                                            " END CATCH";
            }
            else
            {
                // member transaction
            }
    
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING))
                {
                    // define the command object
                    SqlCommand cmd = new SqlCommand(createTransactionQuery, conn);

                    // parameterise
                    SqlParameter customerParam = new SqlParameter();
                    customerParam.ParameterName = "@varCustomerID";
                    if (transaction.getCustomer()!=null)
                    {
                        customerParam.Value = transaction.getCustomer().getID();
                    }
                    else
                    {
                        customerParam.Value = null;
                    }
                    cmd.Parameters.Add(customerParam);

                    SqlParameter staffParam = new SqlParameter();
                    staffParam.ParameterName = "@varStaffID";
                    staffParam.Value = transaction.getStaff().getID();
                    cmd.Parameters.Add(staffParam);

                    SqlParameter productParam = new SqlParameter();
                    productParam.ParameterName = "@varProductID";
                    productParam.Value = transaction.getProduct().getProductID();
                    cmd.Parameters.Add(productParam);

                    // attempt a connection
                    conn.Open();

                    // execute it
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // pass it to the top level.
                throw;
            
            }
            */
        }
        
    }
}
