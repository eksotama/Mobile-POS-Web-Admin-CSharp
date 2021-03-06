﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using libPOS.BLL;
using System.Globalization;

namespace MobilePOS
{
    public partial class Employees : System.Web.UI.Page
    {
        private void DoAdd()
        {
            //
            hdnMethod.Value = "add";
            //
            mvMain.ActiveViewIndex = 1;
            hdnEmpID.Value = "0";
            clearFields();

            rfvPassword.Enabled = true;
            rfvConPass.Enabled = true;
            tbEmpPassword.Enabled = true;
            tbEmpConfirmPass.Enabled = true;
            tbEmpID.Enabled = true;

            //
            loadUserLevel();
            loadKioskList();

            ddlUserLevel.SelectedItem.Value = "0";
            ddlKioskList.SelectedItem.Value = "0";
            //
           
        }

        private void DoEdit(string id)
        {
            hdnEmpID.Value = id;
            hdnMethod.Value = "edit";
            //
            loadUserLevel();
            loadKioskList();
            //
            clearFields();
            //
            var ins = Employee.SelectEmployeeByID(id);

            if (ins.UserLevelID != 3)
            {
                tbEmpUsername.Text = ins.Username;
                tbEmpUsername.Enabled = true;
                rfvUsername.Enabled = true;
            }
            else
            {
                tbEmpUsername.Enabled = false;
                rfvUsername.Enabled = false;
            }

            

            tbEmpPassword.Enabled = false;
            rfvPassword.Enabled = false;
            tbEmpConfirmPass.Enabled = false;
            rfvConPass.Enabled = false;

            ddlUserLevel.ClearSelection();
            ddlUserLevel.Items.FindByValue(ins.UserLevelID+"").Selected = true;
            tbEmpID.Text = ins.EmpID;
            tbEmpID.Enabled = false;

            tbEmpFirstname.Text = ins.Firstname;
            tbEmpLastname.Text = ins.Lastname;
            tbEmpDepartment.Text = ins.Department;

            ddlKioskList.ClearSelection();
            ddlKioskList.Items.FindByValue(ins.KioskID + "").Selected = true;

            DateTime date = (DateTime)ins.DateHired;
            tbEmpDateHired.Text = date.ToString("yyyy-MM-dd");

            tbEmpMobile.Text = ins.MobileNo;
            tbEmpEmail.Text = ins.Email;
        }

        private void loadKioskList()
        {
            ddlKioskList.DataSource = Kiosk.GetAllKiosk();
            ddlKioskList.DataTextField = "Name";
            ddlKioskList.DataValueField = "KioskID";
            ddlKioskList.DataBind();
            //
            ListItem item = new ListItem();
            item.Text = "   [<<<Select Here>>>]   ";
            item.Value = "0";
            ddlKioskList.Items.Insert(0, item);
        }

        private void loadUserLevel()
        {
            ddlUserLevel.DataSource = Common.GetUserLevel();
            ddlUserLevel.DataTextField = "Description";
            ddlUserLevel.DataValueField = "UserLevelID";
            ddlUserLevel.DataBind();
            //
            ListItem item = new ListItem();
            item.Text = "   [<<<Select Here>>>]   ";
            item.Value = "0";
            ddlUserLevel.Items.Insert(0, item);
        }

        private void clearFields()
        {
            tbEmpUsername.Text = "";
            tbEmpPassword.Text = "";
            tbEmpConfirmPass.Text = "";
            
            tbEmpID.Text = "";
            tbEmpFirstname.Text = "";
            tbEmpLastname.Text = "";
            tbEmpDepartment.Text = "";
            
            tbEmpDateHired.Text = "";
            tbEmpMobile.Text = "";
            tbEmpEmail.Text = "";
        }

        private void CallDataGrid()
        {
            gvEmployee.DataSource = Employee.GetAllEmployees();
            gvEmployee.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack){
                mvMain.ActiveViewIndex = 0;

                CallDataGrid();
            }
        }

        protected void gvEmployee_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch(e.CommandName){
                case "DoEdit":
                    string empid = e.CommandArgument.ToString();
                    DoEdit(empid);
                    mvMain.ActiveViewIndex = 1;
                    break;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvMain.ActiveViewIndex = 0;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var ins = new Employee();

            string id = hdnEmpID.Value;

            string userlevel = ddlUserLevel.SelectedItem.Value;

            if (userlevel != "3")
            {
                ins.Username = tbEmpUsername.Text.Trim();
                ins.Password = tbEmpPassword.Text.Trim();
            }
            else
            {
                ins.Username = "";
                ins.Password = "";
            }

            ins.UserLevelID = Convert.ToInt32(ddlUserLevel.SelectedItem.Value);
            ins.Firstname = tbEmpFirstname.Text.Trim().ToUpper();
            ins.Lastname = tbEmpLastname.Text.Trim().ToUpper();
            ins.Department = tbEmpDepartment.Text.Trim().ToUpper();
            ins.KioskID = Convert.ToInt32(ddlKioskList.SelectedItem.Value);

            ins.DateHired = DateTime.ParseExact(tbEmpDateHired.Text.Trim(), "yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture);

            ins.MobileNo = tbEmpMobile.Text.Trim().ToUpper();
            ins.Email = tbEmpEmail.Text.Trim().ToUpper();
            //
            bool isSuccess = true;
            string msg = ""; ;
            //
            if (id == "0")
            { // insert method
                ins.EmpID = tbEmpID.Text.Trim();
                ins.DateCreated = DateTime.Now;

                try
                {
                    Employee.InsertEmployee(ins, GlobalAccess.EmpID);
                    msg = "Insert Success";
                }
                catch (Exception ee)
                {
                    isSuccess = false;
                    (Master as Frame).PopUp(ee.Message);
                }
            }
            else
            { // update method
                ins.EmpID = id;
                ins.DateUpdated = DateTime.Now;

                try
                {
                    Employee.UpdateEmployee(ins, GlobalAccess.EmpID);
                    msg = "Update Success";
                }
                catch (Exception ee)
                {
                    isSuccess = false;
                    (Master as Frame).PopUp(ee.Message);
                }
            }

            if (isSuccess)
            {
                (Master as Frame).PopUp(msg);
                CallDataGrid();
                mvMain.ActiveViewIndex = 0;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DoAdd();
        }

        protected void ddlUserLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sel = Convert.ToInt32(ddlUserLevel.SelectedItem.Value);

            if (hdnMethod.Value == "add")
            {
                if (sel == 3)
                {
                    pnlUserPass.Enabled = false;
                    rfvUsername.Enabled = false;
                    rfvPassword.Enabled = false;
                    cvConPass.Enabled = false;
                    rfvConPass.Enabled = false;
                }
                else
                {
                    pnlUserPass.Enabled = true;
                    rfvUsername.Enabled = true;
                    rfvPassword.Enabled = true;
                    cvConPass.Enabled = true;
                    rfvConPass.Enabled = true;
                }
            }
            else
            {
                rfvUsername.Enabled = false;
                rfvPassword.Enabled = false;
                cvConPass.Enabled = false;
                rfvConPass.Enabled = false;

                if (sel == 3)
                {
                    tbEmpUsername.Text = "";
                    tbEmpUsername.Enabled = false;
                }
                else
                {
                    tbEmpUsername.Enabled = true;
                    rfvUsername.Enabled = true;
                }
                
            }
            
        }


    }
}