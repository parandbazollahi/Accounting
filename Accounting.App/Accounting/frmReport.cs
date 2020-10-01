using Accounting.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Accounting.ViewModels;


namespace Accounting.App
{
    public partial class frmReport : Form
    {
        public int TypeID = 0;
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            
            using (UnitOfWork db = new UnitOfWork())
            {
                List< Accounting.ViewModels.Customers.ListCustomerViewModel> list = new List<Accounting.ViewModels.Customers.ListCustomerViewModel>();
                list.Add(new Accounting.ViewModels.Customers.ListCustomerViewModel()
                {
                    CustomerID = 0,
                    Fullname = "Plase Select"

                });

                list.AddRange(db.CustomerRepository.GetNameCustomers());
                cbCustomer.DataSource = list;
                cbCustomer.DisplayMember = "Fullname";
                cbCustomer.ValueMember = "CustomerID";
            }
            if (TypeID == 1)
            {
                this.Text = "Recieve Reports";
            }
            else
            {
                this.Text = "Pay Reports";
            }
        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            Filter();
        }
        void Filter()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                List<DataLayer.Accounting> result = new List<DataLayer.Accounting>();
                DateTime? startDate;
                DateTime? endDate;
                if ((int)cbCustomer.SelectedValue != 0)
                {
                    int customerId = int.Parse(cbCustomer.SelectedValue.ToString());
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID && a.CustomerID == customerId));
                }
                else
                {
                    result.AddRange(db.AccountingRepository.Get(a => a.TypeID == TypeID));
                }
                if (btnFromDate.Text != "  /  /")
                {
                    startDate = Convert.ToDateTime(btnFromDate.Text);
                    result = result.Where(r => r.DateTitle >= startDate.Value).ToList();
                }

                if (btnToDate.Text != "  /  /")
                {
                    endDate = Convert.ToDateTime(btnToDate.Text);
                    result = result.Where(r => r.DateTitle <= endDate.Value).ToList();
                }

                

               
                //dgReport.AutoGenerateColumns = false;
                //dgReport.DataSource = result;
                dgReport.Rows.Clear();
                foreach (var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgReport.Rows.Add(accounting.ID, customerName, accounting.Amount, accounting.DateTitle, accounting.Description);
                }
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if (MessageBox.Show("Are you sure to delete?", "Notice", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgReport.CurrentRow != null)
            {
                int id = int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                frmNewAccounting frmNew = new frmNewAccounting();
                frmNew.AccountId = id;
                if (frmNew.ShowDialog() == DialogResult.OK)
                {
                    Filter();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtPrint = new DataTable();
            dtPrint.Columns.Add("Customer");
            dtPrint.Columns.Add("Amount");
            dtPrint.Columns.Add("Date");
            dtPrint.Columns.Add("Description");
            foreach (DataGridViewRow item in dgReport.Rows)
            {
                dtPrint.Rows.Add(
                    item.Cells[0].Value.ToString(),
                    item.Cells[1].Value.ToString(),
                    item.Cells[2].Value.ToString(),
                    item.Cells[3].Value.ToString()
                    );
                stiPrint.Load(Application.StartupPath+ "/Report.mrt");
                stiPrint.RegData("DT",dtPrint);
                stiPrint.Show();
            }
        }
    }
}
