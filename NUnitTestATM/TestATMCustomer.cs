using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using MeybankATMSystem;
using BankATMRepository;

namespace NUnitTestATM
{
    [TestFixture]
    public class TestATMCustomer
    {
        [TestCase("Amount needs to be more than zero. Try again.",0)]
        [TestCase("Amount needs to be more than zero. Try again.",-1)]
        [TestCase("Key in the deposit amount only with multiply of 10. Try again.", 1)]
        [TestCase("Key in the deposit amount only with multiply of 10. Try again.", 5)]
        [TestCase("You have successfully deposited RM10", 10)]
        [TestCase("You have successfully deposited RM50", 50)]
        public void ShowErrorMessage_OnPlaceDeposit(string expectedMessage, decimal transactionAmount)
        {
            // Arrange - Start
            var mock = new MockMessagePrinter();

            MeybankATM atmCustomer = new MeybankATM(new RepoBankAccount(), new RepoTransaction(), mock);

            BankAccount bankAccount = new BankAccount()
            {
                FullName = "John",
                AccountNumber = 333111,
                CardNumber = 123,
                PinCode = 111111,
                Balance = 2000.00m,
                isLocked = false
            };

            //decimal transactionAmount = 1;
            //var expectedMessage = "Amount needs to be more than zero. Try again.";

            // Arrange - End

            // Act
            atmCustomer.PlaceDeposit(bankAccount, transactionAmount);

            // Assert                       
            Assert.AreEqual(expectedMessage, mock.Message);
        }
    }
}
