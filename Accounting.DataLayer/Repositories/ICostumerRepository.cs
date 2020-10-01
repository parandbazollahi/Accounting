using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.ViewModels;
using Accounting.ViewModels.Customers;

namespace Accounting.DataLayer.Repositories
{
     public interface ICustomerRepository
    {
        List<Customers> GetAllCustomers();

        IEnumerable<Customers> GetCustomersByFillter(string parameter);

        List<ListCustomerViewModel> GetNameCustomers (string filter = "");

        Customers GetCustomersByID (int customerId);

        bool InsertCustomer(Customers customer);

        bool UpdateCustomer(Customers customer);

        bool DeleteCustomer(Customers customer);

        bool DeleteCustomer(int customerId);

        int GetCustomerByName (string name );

        string GetCustomerNameById (int customerId);

      }
}
