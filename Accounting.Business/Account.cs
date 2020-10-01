using Accounting.DataLayer.Context;
using Accounting.ViewModels.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.Business
{
    public class Account
    {
        public static ReportNewModel ReportFormMain()
        {
            ReportNewModel rp = new ReportNewModel();
            using (UnitOfWork db = new UnitOfWork())
            {
                DateTime startDate = new DateTime(DateTime.Now.Year,DateTime.Now.Month,01);
                DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);

                var recieve = db.AccountingRepository.Get(r => r.TypeID ==1 && r.DateTitle>= startDate && r.DateTitle <=EndDate).Select(r=>r.Amount).ToList();
                var pay = db.AccountingRepository.Get(r => r.TypeID == 2 && r.DateTitle >= startDate && r.DateTitle <= EndDate).Select(r => r.Amount).ToList();

                rp.Recieve = recieve.Sum();
                rp.Pay = pay.Sum();
                rp.AccountBalance = (recieve.Sum() - pay.Sum());
            }
            return rp;
        }
    }
}
