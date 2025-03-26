using System.Numerics;

namespace nhom4_quanlyadmin.Models
{
    public class Specialty
    {
        public int Id { get; set; }
        public string? SpecialtyName { get; set; }
        public ICollection<Doctors>? Doctors { get; set; }
    }
}
