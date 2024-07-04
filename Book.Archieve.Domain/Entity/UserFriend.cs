using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book.Archieve.Domain.Entity
{
    [Table("FriendShip")]
    public class UserFriend
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int FriendId { get; set; }
        [ForeignKey(nameof(FriendId))]
        public User Friend { get; set; }
    }
}
