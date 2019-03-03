using System;
using BankATMRepo;

namespace BankATMAdmin
{
    class TestProgram
    {
        static void Main(string[] args)
        {
            MeyBankATMAdmin atm = new MeyBankATMAdmin();       
            atm.Execute();

        }
    }
}
