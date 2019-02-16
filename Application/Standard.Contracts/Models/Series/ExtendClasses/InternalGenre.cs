namespace Standard.Contracts.Models.Series.ExtendClasses
{
    public class InternalGenre
    {
        public string Name;


        public InternalGenre(string genre)
        {
            this.Name = genre;
        }

        public override bool Equals(object obj)
        {
            var key = obj as InternalGenre;

            if (key == null)
                return false;

            return Name.Equals(key.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }
}
