using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Enum
{
    public enum MediaExistIn
    {
        //DB : 1, TVMAZE: 2, TMDB: 3, Egyik sem: -1, Request hiba: -2
        REQUESTERROR = -1,
        NONE = 0,
        MONGO = 1,
        TVMAZE = 2,
        TMDB = 3,
    }
}
