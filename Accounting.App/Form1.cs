using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.Business;
using Accounting.ViewModels.Accounting;


namespace Accounting.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            FrmCustomer frmCustomer = new FrmCustomer();
            frmCustomer.ShowDialog();
        }

        private void btnNewAccounting_Click(object sender, EventArgs e)
        {
            
            frmNewAccounting frmNewAccounting = new frmNewAccounting();
            frmNewAccounting.ShowDialog();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new frmReport();
            frmReport.TypeID = 2;
            frmReport.ShowDialog();
        }

        private void btnReportRecieve_Click(object sender, EventArgs e)
        {
            frmReport frmReport = new frmReport();
            frmReport.TypeID = 1;
            frmReport.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            
            this.Hide();
            frmLogin frmLogin = new frmLogin();
            if(frmLogin.ShowDialog() == DialogResult.OK)
            {
                this.Show();
                lblDate.Text = DateTime.Now.ToString();
                lblTime.Text = DateTime.Now.ToShortTimeString();
                report();
            }
            else
            {
                Application.Exit();
            }
        }

        void report()
        {
            Accounting.ViewModels.Accounting.ReportNewModel report = Account.ReportFormMain();
            lblPay.Text = report.Pay.ToString("#,0");
            lblRecieve.Text = report.Recieve.ToString("#,0");
            lblBalance.Text = report.AccountBalance.ToString("#,0");

           
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnEditLogin_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.IsEdit = true;
            frmLogin.ShowDialog();
        }
    }
}
