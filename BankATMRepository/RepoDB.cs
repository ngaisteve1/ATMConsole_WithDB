using BankATMRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMRepository
{
    public class RepoDB : IDb, IMessagePrinter
    {
        private readonly AppDbContext db = null;
        private readonly MockMessagePrinter mockMessage = null;

        public RepoDB()
        {
            db = new AppDbContext();
            mockMessage = new MockMessagePrinter();
        }

        public void CheckDbConnection()
        {
            try
            {
                db.Database.Connection.Open();
            }
            catch
            {
                mockMessage.PrintMessage("System error. Please contact system administrator.", false);

                Environment.Exit(1);
            }
            finally
            {
                db.Database.Connection.Close();
            }
            
            
        }

        public void ClearDb()
        {
            db.Dispose();
        }

        public void PrintMessage(string message, bool iDontKnow)
        {
            throw new NotImplementedException();
        }
    }
}
