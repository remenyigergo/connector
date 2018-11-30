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
        AlreadySeen = 606
    }
}
