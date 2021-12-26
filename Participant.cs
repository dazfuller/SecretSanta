using System;

namespace SecretSanta
{
    public class Participant : IEquatable<Participant>
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Participant) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Email);
        }

        public bool Equals(Participant? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Email == other.Email;
        }
    }
}