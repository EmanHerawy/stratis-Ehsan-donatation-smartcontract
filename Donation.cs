using Stratis.SmartContracts;
[Deploy]
public class RegistrationContract : SmartContract
{
    public RegistrationContract(ISmartContractState smartContractState)
   : base(smartContractState)
    {
        this.Admin = Message.Sender;
    }

    // public enum Authorization : uint
    // {
    //     pending = 0,
    //     rejected = 1,
    //     approved = 2, banned = 3
    // }

    // public struct Organization
    // {
    //     public string licence;
    //     public string auditReport;
    //     public string passport;
    //     public string name;
    //     public string bankAccount;
    //     public Authorization status;
    //     public Address cryptoAddress;
    //     //Organization(string licence,
    //     //     string auditReport,
    //     // string passport,
    //     // string name,
    //     //string bankAccount,
    //     //Authorization status,
    //     // Address cryptoAddress)
    //     //{
    //     //    this.licence = licence;
    //     //    this.auditReport = auditReport;
    //     //    this.passport = passport;
    //     //    this.name = name;
    //     //    this.bankAccount = bankAccount;
    //     //    this.status = status;
    //     //    this.cryptoAddress = cryptoAddress;

    //     //}
    // }
    public int Index
    {
        get => this.Users.Length;
        //private set => PersistentState.SetAddress(nameof(Admin), value);
    }
    public Address User(int index)
    {
        return this.Users[index];
        //private set => PersistentState.SetAddress(nameof(Admin), value);
    }
    public Address Admin
    {
        get => PersistentState.GetAddress(nameof(Admin));
        private set => PersistentState.SetAddress(nameof(Admin), value);
    }
    public Address[] Users
    {
        get => PersistentState.GetArray<Address>((nameof(Users)));
        private set => PersistentState.SetArray(nameof(Users), value);
    }
    // public int GetUserCount(){
    //     return this.Users.Length;
    // }
    // public Address GetUser(int index)
    // {
    //     return this.Users[index];
    // }
    // public Organization Charities
    // {
    //     get => PersistentState.GetStruct<Organization>((nameof(Charities)));
    //     private set => PersistentState.SetStruct<Organization>(nameof(Charities), value);
    // }
    //  public Organization GeCharities(Address address)
    //  {
    //      return PersistentState.GetStruct<Organization>($"Charities:{address}");
    //  }

    //  private void SetCharities(Address address, Organization value)
    //  {
    //      PersistentState.SetStruct<Organization>($"Charities:{address}", value);
    //  }
    //public Organization GeCharities(Address address)
    //{
    //    return this.PersistentState.GetStruct<Organization>($"Charities:[{address}]");
    //}
    //private void SetCharities(Address address, Organization value)
    //{
    //    this.PersistentState.SetStruct<Organization>($"Charities:[{address}]", value);
    //}
    public Address createAccount()
    {
        var createResult = Create<UserWalletContract>(0, new object[] { Message.Sender , this.Admin});
        Assert(createResult.Success);
        Address[] memoryUsers = this.GetArrayCopy(this.Users);

        memoryUsers[this.Index] = createResult.NewContractAddress;
        Users = memoryUsers;
        return createResult.NewContractAddress;
    }
    // we should avoid this solution as looping is gas consumer
    private Address[] GetArrayCopy(Address[] users)
    {
        var TempArray = new Address[this.Index + 1];
        for (int i = 0; i < this.Index; i++)
        {
            TempArray[i] = users[i];

        }
        return TempArray;
    }
    // public bool requestToApprove(
    //       string licence,
    //       string auditReport,
    //       string passport,
    //       string name,

    //       string bankAccount,
    //       Address cryptoAddress)
    // {
    //     //TODO: add validation
    //     //  Assert(Users.Equals(Message.Sender), "unregistered user");
    //     var org = new Organization
    //     {
    //         auditReport = auditReport,
    //         cryptoAddress = cryptoAddress,
    //         licence = licence,
    //         name = name,
    //         passport = passport,
    //         status = (uint)Authorization.pending
    //     };
    //     this.SetCharities(Message.Sender, org);
    //     return true;

    // }
    // public bool AdminManageRequest(Address Manger, uint status)
    // {
    //     var org = this.GeCharities(Manger);
    //     org.status = (Authorization)status;
    //     this.SetCharities(Manger, org);
    //     return true;
    // }
}

public class UserWalletContract : SmartContract
{
    public UserWalletContract(ISmartContractState smartContractState, Address Owner, Address Admin)
: base(smartContractState)
    {
        this.Admin = Admin;
        this.Owner = Owner;
    }
    enum StatusType : uint
    {
        Initialized = 0,
        Rejected = 1,
        Approved = 2, Banned = 3,
        Submited = 4
    }
    public Address Owner
    {
        get => PersistentState.GetAddress(nameof(Owner));
        private set => PersistentState.SetAddress(nameof(Owner), value);
    }
    public Address Admin
    {
        get => PersistentState.GetAddress(nameof(Admin));
        private set => PersistentState.SetAddress(nameof(Admin), value);
    }
    public string Name
    {
        get => PersistentState.GetString(nameof(Name));
        private set => PersistentState.SetString(nameof(Name), value);
    }
    public string Licence
    {
        get => PersistentState.GetString(nameof(Licence));
        private set => PersistentState.SetString(nameof(Licence), value);
    }
    public string AuditReport
    {
        get => PersistentState.GetString(nameof(AuditReport));
        private set => PersistentState.SetString(nameof(AuditReport), value);
    }
    public string Passport
    {
        get => PersistentState.GetString(nameof(Passport));
        private set => PersistentState.SetString(nameof(Passport), value);
    }

    public string BankAccount
    {
        get => PersistentState.GetString(nameof(BankAccount));
        private set => PersistentState.SetString(nameof(BankAccount), value);
    }
    public uint State
    {
        get => PersistentState.GetUInt32(nameof(State));
        private set => PersistentState.SetUInt32(nameof(State), value);
    }
    public Address[] Campaigns
    {
        get => PersistentState.GetArray<Address>(nameof(Campaigns));
        private set => PersistentState.SetArray(nameof(Campaigns), value);
    }
    public Address CryptoAddress
    {
        get => PersistentState.GetAddress(nameof(CryptoAddress));
        private set => PersistentState.SetAddress(nameof(CryptoAddress), value);
    }
        public int Index
    {
        get => this.Campaigns.Length;
    }
    public bool RequestToPublish(string Licence,
          string AuditReport,
          string Passport,
          string Name,

          string BankAccount,
          Address CryptoAddress)
    {
        Assert(this.Message.Sender == this.Owner);
        Assert(this.State != (uint)StatusType.Banned);
        this.Licence = Licence;
        this.AuditReport = AuditReport;
        this.Passport = Passport;
        this.Name = Name;

        this.BankAccount = BankAccount;
        this.CryptoAddress = CryptoAddress;
        this.State = (uint)StatusType.Submited;
        return true;
    }
    public bool AdminManageRequestToPublish(uint status)
    {
        Assert(this.Message.Sender == this.Admin);
        Assert(status == (uint)StatusType.Approved || status == (uint)StatusType.Rejected || status == (uint)StatusType.Banned);


            this.State = status;

        return true;
    }
    public Address IssueCampaign(ulong Cap, string Name, ulong EndDate)
    {
        Assert(this.State==(uint)StatusType.Approved);
         Assert(this.Message.Sender == this.Owner);
         /*Address owner, Address Admin, Address WalletContract, ulong Cap, string Name, ulong EndDate */
        var createResult = Create<CampaignContract>(0, new object[] {  this.Owner, this.Admin,this.Address, Cap,  Name,  EndDate });
        Assert(createResult.Success);
        Address[] TempArray = this.GetArrayCopy(this.Campaigns);

        TempArray[this.Index] = createResult.NewContractAddress;
        Campaigns = TempArray;
        return createResult.NewContractAddress;
    }
        private Address[] GetArrayCopy(Address[] arr)
    {
        var TempArray = new Address[this.Index + 1];
        for (int i = 0; i < this.Index; i++)
        {
            TempArray[i] = arr[i];

        }
        return TempArray;
    }
}


public class CampaignContract : SmartContract
{
    public enum StatusType : uint
    {
        Issued = 0,
        Submited = 1,
        Rejected = 2,
        Opened = 3,
        Finished = 4
    }
    // this for dao stages
    public enum Stages : uint
    {
        Stage1 = 0,
        Stage2 = 1,
        Stage3 = 2
    }
    public CampaignContract(ISmartContractState smartContractState, Address owner, Address Admin, Address WalletContract, ulong Cap, string Name, ulong EndDate)
    : base(smartContractState)
    {
        this.Owner = owner;
        this.Admin = Admin;
        this.Name = Name;
        this.WalletContract = WalletContract;
        this.EndDate = EndDate;
        this.Cap = Cap;
        this.State = (uint)StatusType.Issued;
    }


    public Address Owner
    {
        get => PersistentState.GetAddress(nameof(Owner));
        private set => PersistentState.SetAddress(nameof(Owner), value);
    }
    public Address Admin
    {
        get => PersistentState.GetAddress(nameof(Admin));
        private set => PersistentState.SetAddress(nameof(Admin), value);
    }
    // later on we can use the parent class to add extra feature
    public Address WalletContract
    {
        get => PersistentState.GetAddress(nameof(WalletContract));
        private set => PersistentState.SetAddress(nameof(WalletContract), value);
    }

    public string Name
    {
        get => PersistentState.GetString(nameof(Name));
        private set => PersistentState.SetString(nameof(Name), value);
    }
    public string Licence
    {
        get => PersistentState.GetString(nameof(Licence));
        private set => PersistentState.SetString(nameof(Licence), value);
    }
    public string AuditReport
    {
        get => PersistentState.GetString(nameof(AuditReport));
        private set => PersistentState.SetString(nameof(AuditReport), value);
    }
    public string Passport
    {
        get => PersistentState.GetString(nameof(Passport));
        private set => PersistentState.SetString(nameof(Passport), value);
    }
    public string MangerName
    {
        get => PersistentState.GetString(nameof(MangerName));
        private set => PersistentState.SetString(nameof(MangerName), value);
    }
    public string BankAccount
    {
        get => PersistentState.GetString(nameof(BankAccount));
        private set => PersistentState.SetString(nameof(BankAccount), value);
    }
    public Address CryptoAddress
    {
        get => PersistentState.GetAddress(nameof(CryptoAddress));
        private set => PersistentState.SetAddress(nameof(CryptoAddress), value);
    }

    public ulong StartDate
    {
        get => PersistentState.GetUInt64(nameof(StartDate));
        private set => PersistentState.SetUInt64(nameof(StartDate), value);
    }
    public ulong EndDate
    {
        get => PersistentState.GetUInt64(nameof(EndDate));
        private set => PersistentState.SetUInt64(nameof(EndDate), value);
    }
    public ulong TotalSupply
    {
        get => PersistentState.GetUInt64(nameof(this.TotalSupply));
        private set => PersistentState.SetUInt64(nameof(this.TotalSupply), value);
    }
    public ulong Cap
    {
        get => PersistentState.GetUInt64(nameof(this.Cap));
        private set => PersistentState.SetUInt64(nameof(this.Cap), value);
    }
    public uint State
    {
        get => PersistentState.GetUInt32(nameof(State));
        private set => PersistentState.SetUInt32(nameof(State), value);
    }
    public ulong GetBalance(Address address)
    {
        return PersistentState.GetUInt64($"Balance:{address}");
    }

    private void SetBalance(Address address, ulong value)
    {
        PersistentState.SetUInt64($"Balance:{address}", value);
    }
    private bool TransferTo(Address to, ulong amount)
    {
        if (amount == 0)
        {


            return false;
        }

        //  use checked and unchecked to prevent overflow & or to overflow
        SetBalance(to, checked(GetBalance(to) + amount));

        return true;
    }
    public bool RequestToPublish(string Licence,
          string AuditReport,
          string Passport,
          string MangerName,

          string BankAccount,
          Address CryptoAddress)
    {
        Assert(this.Message.Sender == this.Owner);
        this.Licence = Licence;
        this.AuditReport = AuditReport;
        this.Passport = Passport;
        this.MangerName = MangerName;

        this.BankAccount = BankAccount;
        this.CryptoAddress = CryptoAddress;
        this.State = (uint)StatusType.Submited;
        return true;
    }
    public bool AdminManageRequestToPublish(bool status)
    {
        Assert(this.Message.Sender == this.Admin);
        if (status)
        {
            this.State = (uint)StatusType.Opened;
            this.StartDate = this.Block.Number;
        }
        else
        {
            this.State = (uint)StatusType.Rejected;
        }
        return true;
    }
    public void Donate()
    {
        Assert(this.Block.Number < this.EndDate);
        Assert(this.TotalSupply < this.Cap);
        Assert(this.State == (uint)StatusType.Opened);
        Assert(this.Message.Value > 0);
        Assert(this.TransferTo(this.Message.Sender, this.Message.Value));
        this.TotalSupply += this.Message.Value;

    }

    public bool Withdraw()
    {
        Assert(this.State == (uint)StatusType.Finished);
        // we will add extra functionality here by implementing dao 
        ITransferResult transferResult = Transfer(this.Owner, this.Balance);

        return transferResult.Success;
    }

}


public class FactoryContract : SmartContract
{
    public FactoryContract(ISmartContractState smartContractState, Address owner)
    : base(smartContractState)
    {
        this.Admin = owner;
    }
    public Address Admin
    {
        get => PersistentState.GetAddress(nameof(Admin));
        private set => PersistentState.SetAddress(nameof(Admin), value);
    }
    public Address RegistrationFactory
    {
        get => PersistentState.GetAddress(nameof(RegistrationFactory));
        private set => PersistentState.SetAddress(nameof(RegistrationFactory), value);
    }
}