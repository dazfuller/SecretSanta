namespace SecretSanta
{
    public class Participant
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Participant p)
            {
                return p.Name.Equals(Name) && p.Email.Equals(Email);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() & Email.GetHashCode();
        }
    }
}
