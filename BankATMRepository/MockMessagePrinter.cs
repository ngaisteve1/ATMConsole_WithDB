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

        public void PrintMessage(string message, bool success)
        {
            //Message = message;
            // Console UI.
            if (success)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();

        }
    }

    
}
