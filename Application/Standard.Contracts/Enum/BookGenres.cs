using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Standard.Contracts.Enum
{
    public enum Genres
    {
        [Description("Sci-fi")]
        StarWars,
        StarTrek,
        MassEffect,

        [Description("Fantasy")]
        GameOfThrones,
        Warcraft,
        Withcer,
        AmberChronicles,

        [Description("Crime")]
        SherlockHolmes,
        Keresztapa,
    }
}
