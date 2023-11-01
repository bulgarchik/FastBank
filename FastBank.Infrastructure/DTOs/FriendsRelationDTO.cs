using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace FastBank.Infrastructure.DTOs
{
    [Table("FriendRelations")]
    public class FriendsRelationDTO
    {
        private FriendsRelationDTO() { }

        public FriendsRelationDTO(Guid relationId, User user, User friend, bool blocked)
        {
            RelationId = relationId;
            UserId = user.Id;
            FriendId = friend.Id;
            Blocked = blocked;
        }

        [Key]
        public Guid RelationId { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserDTO User { get; set; } = null!;
        public Guid FriendId { get; set; }
        [ForeignKey(nameof(FriendId))]
        public UserDTO Friend { get; set; } = null!;
        public bool Blocked {  get; set; }
    }
}

