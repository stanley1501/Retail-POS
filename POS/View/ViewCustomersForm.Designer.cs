﻿namespace POS.View
{
    partial class ViewCustomersForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView_customers = new System.Windows.Forms.ListView();
            this.button_deleteSelectedCustomer = new System.Windows.Forms.Button();
            this.button_addNewCustomer = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView_customers
            // 
            this.listView_customers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_customers.Location = new System.Drawing.Point(12, 12);
            this.listView_customers.Name = "listView_customers";
            this.listView_customers.Size = new System.Drawing.Size(679, 426);
            this.listView_customers.TabIndex = 0;
            this.listView_customers.UseCompatibleStateImageBehavior = false;
            this.listView_customers.SelectedIndexChanged += new System.EventHandler(this.listView_customers_SelectedIndexChanged);
            // 
            // button_deleteSelectedCustomer
            // 
            this.button_deleteSelectedCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_deleteSelectedCustomer.Location = new System.Drawing.Point(697, 12);
            this.button_deleteSelectedCustomer.Name = "button_deleteSelectedCustomer";
            this.button_deleteSelectedCustomer.Size = new System.Drawing.Size(98, 23);
            this.button_deleteSelectedCustomer.TabIndex = 1;
            this.button_deleteSelectedCustomer.Text = "Delete selected";
            this.button_deleteSelectedCustomer.UseVisualStyleBackColor = true;
            this.button_deleteSelectedCustomer.Click += new System.EventHandler(this.button_deleteSelectedCustomer_Click);
            // 
            // button_addNewCustomer
            // 
            this.button_addNewCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addNewCustomer.Location = new System.Drawing.Point(697, 41);
            this.button_addNewCustomer.Name = "button_addNewCustomer";
            this.button_addNewCustomer.Size = new System.Drawing.Size(98, 23);
            this.button_addNewCustomer.TabIndex = 2;
            this.button_addNewCustomer.Text = "Add new";
            this.button_addNewCustomer.UseVisualStyleBackColor = true;
            this.button_addNewCustomer.Click += new System.EventHandler(this.button_addNewCustomer_Click);
            // 
            // button_close
            // 
            this.button_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_close.Location = new System.Drawing.Point(697, 415);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(98, 23);
            this.button_close.TabIndex = 3;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // ViewCustomersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_addNewCustomer);
            this.Controls.Add(this.button_deleteSelectedCustomer);
            this.Controls.Add(this.listView_customers);
            this.Name = "ViewCustomersForm";
            this.Text = "Customers";
            this.Load += new System.EventHandler(this.ViewCustomersForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_customers;
        private System.Windows.Forms.Button button_deleteSelectedCustomer;
        private System.Windows.Forms.Button button_addNewCustomer;
        private System.Windows.Forms.Button button_close;
    }
}