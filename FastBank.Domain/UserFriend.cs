namespace FastBank.Domain
{
    public class UserFriend
    {
        public UserFriend(Guid relationId, Customer friend1, Customer friend2, bool blocked)
        {
            RelationId = relationId;
            Friend1 = friend1;
            Friend2 = friend2;
            Blocked = blocked;
        }

        public Guid RelationId { get; set; }
        public Customer Friend1 { get; set; }
        public Customer Friend2 { get; set; }
        public bool Blocked {  get; set; }
    }
}

