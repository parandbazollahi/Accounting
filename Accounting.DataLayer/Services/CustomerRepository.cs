using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.Repositories;
using Accounting.ViewModels.Customers;

namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private Accounting_DBEntities db;
        public CustomerRepository(Accounting_DBEntities context)
        {
            db = context;
        }

        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                db.Entry(customer).State = System.Data.Entity.EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                var customer = GetCustomersByID(customerId);
                DeleteCustomer(customer);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<Customers> GetAllCustomers()
        {
            return db.Customers.ToList();
        }

        public IEnumerable<Customers> GetCustomersByFillter(string parameter)
        {
            return db.Customers.Where(C => C.Fullname.Contains(parameter) || C.Email.Contains(parameter) || C.Address.Contains(parameter)).ToList();

        }

        public Customers GetCustomersByID(int customerId)
        {
            return db.Customers.Find(customerId);
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                db.Customers.Add(customer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateCustomer(Customers customer)
        {
            //try
            //{
            var local = db.Set<Customers>()
                         .Local
                         .FirstOrDefault(f => f.CustomerID == customer.CustomerID);
            if (local != null)
            {
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(customer).State = System.Data.Entity.EntityState.Modified;
            return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public List<ListCustomerViewModel> GetNameCustomers(string filter = "")
        {
            if (filter == "")
            {
                return db.Customers.Select(c => new ListCustomerViewModel
                {
                    CustomerID = c.CustomerID,
                    Fullname = c.Fullname
                }).ToList();

            }
            return db.Customers.Where(c => c.Fullname.Contains(filter)).Select(c => new ListCustomerViewModel
            {
                CustomerID = c.CustomerID,
                Fullname = c.Fullname
            }).ToList();
        }

       public int GetCustomerByName(string name)
        {
            return db.Customers.First(c => c.Fullname == name).CustomerID;
        }

         public string GetCustomerNameById(int customerId)
        {
            return db.Customers.Find(customerId).Fullname; 
        }
    }
}
