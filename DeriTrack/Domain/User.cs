
namespace DeriTrack.Domain
{
    public class User
    {
        public long Id { get; set; }
        public Email Email { get; set; }
        public bool IsActive { get; set; }
    }
}