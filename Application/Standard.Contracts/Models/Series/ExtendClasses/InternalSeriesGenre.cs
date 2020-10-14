namespace Standard.Contracts.Models.Series.ExtendClasses
{
    public class InternalSeriesGenre
    {
        public string Name { get; set; }


        public InternalSeriesGenre(string genre)
        {
            Name = genre;
        }

        public override bool Equals(object obj)
        {
            var key = obj as InternalSeriesGenre;

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