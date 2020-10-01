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
using ValidationComponents;
using System.IO;


namespace Accounting.App.Customers
{
    public partial class frmAddOrEditCustomer : Form
    {
        public int customerId = 0;
        UnitOfWork db = new UnitOfWork();
        public frmAddOrEditCustomer()
        {
            InitializeComponent();
        }

        private void pcCustomer_Click(object sender, EventArgs e)
        {

        }

        private void btnSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pcCustomer.ImageLocation = openFile.FileName;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                String imageName = Guid.NewGuid().ToString() + Path.GetExtension(pcCustomer.ImageLocation);
                String path = Application.StartupPath + "/Images/";
                if (!Directory.Exists( path ))
                {
                    Directory.CreateDirectory(path);
                }
                pcCustomer.Image.Save(path + imageName);
                DataLayer.Customers customer = new DataLayer.Customers()
                {
                    Fullname = txtFullname.Text,
                    Address = txtAddress.Text,
                    Mobile = txtMobile.Text,
                    Email = txtEmail.Text,
                    CustomerImage = imageName
                };
                if (customerId == 0) { 
                db.CustomerRepository.InsertCustomer(customer);
                }
                else { 
                    customer.CustomerID  = customerId;
                    db.CustomerRepository.UpdateCustomer(customer);
                    }
                db.Save();
                DialogResult = DialogResult.OK;

            }
        }

        private void frmAddOrEditCustomer_Load(object sender, EventArgs e)
        {
            if (customerId != 0)
            {
                this.Text = "Edit";
                btnSave.Text = "Save Edit";
                var customer = db.CustomerRepository.GetCustomersByID(customerId);
                txtAddress.Text = customer.Address;
                txtEmail.Text = customer.Email;
                txtFullname.Text = customer.Fullname;
                txtMobile.Text = customer.Mobile;
                pcCustomer.ImageLocation = Application.StartupPath + "/Images/" + customer.CustomerImage;
            }
        }
    }
}
