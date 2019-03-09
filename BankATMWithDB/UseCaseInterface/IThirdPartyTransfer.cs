using BankATMRepo;

public interface IThirdPartyTransfer {
    void PerformThirdPartyTransfer(BankAccount bankAccount, BankATMRepo.VMThirdPartyTransfer vmThirdPartyTransfer);
}