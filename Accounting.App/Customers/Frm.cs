using Accounting.App.Customers;
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

namespace Accounting.App
{
    public partial class FrmCustomer : Form
    {
        public FrmCustomer()
        {
            InitializeComponent();
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        void BindGrid()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.AutoGenerateColumns = false;
                dgvCustomers.DataSource = db.CustomerRepository.GetAllCustomers();
            }
        }

        private void btnRefreshCustomer_Click(object sender, EventArgs e)
        {
            TxtFillter.Text = "";
            BindGrid();
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                dgvCustomers.DataSource = db.CustomerRepository.GetCustomersByFillter(TxtFillter.Text);
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow != null)
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    string name = dgvCustomers.CurrentRow.Cells[1].Value.ToString();
                    if(MessageBox.Show($"Are you sure to delete {name}","Attention" ,MessageBoxButtons.YesNo,MessageBoxIcon.Warning)== DialogResult.Yes) { 
                    int customerId = int.Parse(dgvCustomers.CurrentRow.Cells[0].Value.ToString());
                    db.CustomerRepository.DeleteCustomer(customerId);
                    db.Save();
                    BindGrid();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select the Customer!");
            }
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            frmAddOrEditCustomer frmAddOrEditCustomer = new frmAddOrEditCustomer();
            if(frmAddOrEditCustomer.ShowDialog() == DialogResult.OK)
            {
                BindGrid();
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow!= null)
            {
                int customerId = int.Parse(dgvCustomers.CurrentRow.Cells[0].Value.ToString());
                frmAddOrEditCustomer frmAddOrEdit = new frmAddOrEditCustomer();
                frmAddOrEdit.customerId = customerId;
                if ( frmAddOrEdit.ShowDialog() == DialogResult.OK)
                {
                    BindGrid();
                }
            }
        }
    }
}
