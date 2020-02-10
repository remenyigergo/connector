﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Series.DataManagement.MongoDB.SeriesFunctionModels
{
    public class EpisodeComment
    {
        public int UserId;
        public string TvMazeId;
        public string TmdbId;
        public int SeasonNumber;
        public int EpisodeNumber;
        public string Message;
    }
}