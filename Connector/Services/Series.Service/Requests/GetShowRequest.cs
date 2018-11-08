using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Series.Parsers.TvMaze;
using Series.Service.Models;

namespace Series.Service.Response
{
    public class GetShowRequest
    {
        public string Title;
        public EpisodeStarted Episode;

    }
}
