using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities.Identity
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!; // 建議存 hash
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => RevokedAt == null && !IsExpired;
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
    }
}