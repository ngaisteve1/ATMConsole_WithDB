using System;
namespace BankATMRepo
{
    public class VMThirdPartyTransfer
    {
        public int AccountId { get; set; }
        public decimal TransferAmount { get; set; }
        public long RecipientBankAccountNumber { get; set; }

        public string RecipientBankAccountName { get; set; }
    }
}
