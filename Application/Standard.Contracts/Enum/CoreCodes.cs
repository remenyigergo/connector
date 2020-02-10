namespace Standard.Contracts.Enum
{
    public enum CoreCodes
    {
        NoError = 1,
        CommonGenericError = -1000,
        MalformedRequest = 100,
        AlreadyImported = 600,
        UpToDate = 601,
        AlreadyAdded = 602,
        NotAdded = 603,
        AlreadyStarted = 604,
        SeriesNotFound = 605,
        AlreadySeen = 606,
        UpdateFailed = 607,
        EpisodeStartedUpdated = 608,
        EpisodeStartedNotUpdated = 609,
        EpisodeRated = 610,
        EpisodeNotRated = 611,
        FollowEpisodeError = 612,
        ProcessNotFound = 613,
        ModuleNotActivated = 614,
        RecommendFailed = 615,
        EpisodeNotFound = 616,
        EpisodeStartedAndSeenNotFound = 617,
        UserNotFound = 618,


        MovieNotFound = 650,
        MovieNotSeen = 651,
        MovieNotStarted = 652,

        FeedsNull = 660,
        MessagesNull = 661,

        GroupNotFound = 670

    }
}
