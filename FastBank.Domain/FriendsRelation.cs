namespace FastBank.Domain
{
    public class FriendsRelation
    {
        public FriendsRelation(Guid realationId, Customer friend1, Customer friend2, bool blocked)
        {
            RealationId = realationId;
            Friend1 = friend1;
            Friend2 = friend2;
            Blocked = blocked;
        }

        public Guid RealationId { get; set; }
        public Customer Friend1 { get; set; }
        public Customer Friend2 { get; set; }
        public bool Blocked {  get; set; }
    }
}

