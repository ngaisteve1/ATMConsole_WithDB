using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankATMRepository
{
    public interface IMessagePrinter
    {
        void PrintMessage(string message, bool iDontKnow);
    }

    public class MockMessagePrinter : IMessagePrinter
    {
        public string Message { get; private set; }

        public void PrintMessage(string message, bool idontKnow)
        {
            Message = message;
        }
    }

    
}
