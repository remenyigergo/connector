using System.Collections.Generic;

namespace Series.Dto.RequestDtoModels
{
    public class StartedAndSeenEpisodesDto
    {
        public List<EpisodeSeenDto> seenEpisodeList;
        public List<EpisodeStartedDto> startedEpisodeList;
    }
}
