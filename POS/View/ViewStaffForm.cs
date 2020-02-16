﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Model.ObjectModel;
using Model.ServiceLayer;

namespace POS.View
{
    public partial class ViewStaffForm : Form
    {
        // controller dependency injection
        private StaffController controller;

        public ViewStaffForm()
        {
            InitializeComponent();

            // prepare the listView
            listView_staff.Columns.Add("ID");
            listView_staff.Columns.Add("Full name");
            listView_staff.Columns.Add("Password");
            listView_staff.Columns.Add("Privelege");
            listView_staff.View = System.Windows.Forms.View.Details;
            listView_staff.GridLines = true;

            // can't delete anything until something is selected
            button_deleteSelectedStaff.Enabled = false;

            // controller dependency injection
            controller = StaffController.getInstance();

            // subscribe to model events
            StaffOps.OnGetAllStaff += new EventHandler<GetAllStaffEventArgs>(staffEventHandler);

            // populate the list view upon loading
            populateView(StaffOps.getAllStaff());

        }

        #region UI event handlers
        private void button_addNewStaff_Click(object sender, EventArgs e)
        {
            NewStaffForm newStaffForm = new NewStaffForm();
            newStaffForm.Show();
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_deleteSelectedStaff_Click(object sender, EventArgs e)
        {
            if (controller != null)
            {
                try
                {
                    int idNum;
                    Int32.TryParse(listView_staff.SelectedItems[0].SubItems[0].Text, out idNum);
                    controller.deleteStaff(idNum);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void listView_staff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView_staff.SelectedItems.Count>0)
            {
                // only allowed to select one item at a time
                foreach (ListViewItem selectedItem in listView_staff.SelectedItems)
                {
                    selectedItem.Selected = false;
                }
            }

            if (listView_staff.SelectedItems.Count==1)
            {
                button_deleteSelectedStaff.Enabled = true;
            }
            else
            {
                button_deleteSelectedStaff.Enabled = false;
            }
        }
        #endregion

        private void staffEventHandler(object sender, GetAllStaffEventArgs e)
        {
            populateView(e.getList());
        }

        private void populateView(List<Staff> staffList)
        {
            // clear the listView
            foreach (ListViewItem staffListViewItem in listView_staff.Items)
            {
                listView_staff.Items.Remove(staffListViewItem);
            }

            // populate it
            foreach (Staff staff in staffList)
            {
                string[] itemArr = new string[4];
                itemArr[0] = staff.getID().ToString();
                itemArr[1] = staff.getName();
                itemArr[2] = staff.getPasswordHash();
                itemArr[3] = staff.getPrivelege().ToString();

                ListViewItem staffListViewItem = new ListViewItem(itemArr);
                listView_staff.Items.Add(staffListViewItem);
            }
        }
    }
}
