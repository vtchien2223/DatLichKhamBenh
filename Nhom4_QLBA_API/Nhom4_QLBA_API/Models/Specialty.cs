using System.Numerics;

namespace Nhom4_QLBA_API.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string? SpecialtyName { get; set; }
        public ICollection<Doctors>? Doctors { get; set; }
    }
}
