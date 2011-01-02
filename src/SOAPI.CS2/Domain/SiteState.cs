namespace SOAPI.CS2.Domain
{
    ///<summary>
    ///</summary>
    public enum SiteState
    {
        ///<summary>
        ///</summary>
        Normal,
        ///<summary>
        ///</summary>
        // ReSharper disable InconsistentNaming
        Closed_Beta,
        // ReSharper restore InconsistentNaming
        ///<summary>
        ///</summary>
        // ReSharper disable InconsistentNaming
        Open_Beta,
        // ReSharper restore InconsistentNaming
        ///<summary>
        ///</summary>
        // ReSharper disable InconsistentNaming
        Linked_Meta
        ,
        ///<summary>
        ///</summary>
        Unknown = -1,

        Deleted = -2

        // ReSharper restore InconsistentNaming
    }
}