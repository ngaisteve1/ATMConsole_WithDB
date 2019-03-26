using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMRepository.Repository
{
    public interface IDb
    {
        void CheckDbConnection();
        void ClearDb();
    }
}
