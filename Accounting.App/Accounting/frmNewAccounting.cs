using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer.Context;
using ValidationComponents;

namespace Accounting.App
{
    public partial class frmNewAccounting : Form
    {
      private UnitOfWork db;
        public int AccountId = 0;
        public frmNewAccounting()
        {
            InitializeComponent();
        }

        private void frmNewAccounting_Load(object sender, EventArgs e)
        {
            db = new UnitOfWork();
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers();
            if (AccountId != 0)
            {
                var account = db.AccountingRepository.GetById(AccountId);
                txtDescription.Text = account.Description;
                txtName.Text = db.CustomerRepository.GetCustomerNameById(account.CustomerID);
                txtAmount.Text = account.Amount.ToString();
                if (account.TypeID == 1)
                {
                    rbRecieve.Checked = true;
                }
                else
                {
                    rbPay.Checked = true;
                }
                this.Text = "Edit";
                btnSave.Text = "Submit";
                db.Dispose();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            dgvCustomers.AutoGenerateColumns = false;
            dgvCustomers.DataSource = db.CustomerRepository.GetNameCustomers(txtFilter.Text);
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                if (rbPay.Checked || rbRecieve.Checked)
                {
                    db = new UnitOfWork();
                    DataLayer.Accounting accounting = new DataLayer.Accounting()
                    {
                        Amount = int.Parse(txtAmount.Value.ToString()),
                        CustomerID = db.CustomerRepository.GetCustomerByName(txtName.Text),
                        TypeID = (rbRecieve.Checked) ? 1 : 2,
                        DateTitle = DateTime.Now,
                        Description = txtDescription.Text
                    };
                    if (AccountId == 0)
                    {
                        db.AccountingRepository.Insert(accounting);
                        db.Save();
                    }
                    else
                    {
                        
                            accounting.ID = AccountId;
                            db.AccountingRepository.Update(accounting);
                            
                        
                    }
                    db.Save();
                    db.Dispose();
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Please choose the transaction type");
                }
            }
        }

    }
}
