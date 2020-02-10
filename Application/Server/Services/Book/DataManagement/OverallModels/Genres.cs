using System.ComponentModel;

namespace Book.DataManagement.OverallModels
{
    public enum Genres
    {
        [Description("Sci-fi")] StarWars = 1,
        StarTrek,
        MassEffect,

        [Description("Fantasy")] GameOfThrones,
        Warcraft,
        Withcer,
        AmberChronicles,

        [Description("Crime")] SherlockHolmes,
        Keresztapa
    }
}