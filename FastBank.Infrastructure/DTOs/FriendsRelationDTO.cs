using System.ComponentModel.DataAnnotations.Schema;

namespace FastBank.Infrastructure.DTOs
{
    public class FriendsRelationDTO
    {
        public FriendsRelationDTO(Guid relationId, Customer friend1, Customer friend2, bool blocked)
        {
            RelationId = relationId;
            Friend1 = friend1;
            Friend2 = friend2;
            Blocked = blocked;
        }

        public Guid RelationId { get; set; }
        public Guid Friend1Id { get; set; }
        [ForeignKey(nameof(Friend1Id))]
        public Customer Friend1 { get; set; }
        public Guid Friend2Id { get; set; }
        [ForeignKey(nameof(Friend2Id))]
        public Customer Friend2 { get; set; }
        public bool Blocked {  get; set; }
    }
}

