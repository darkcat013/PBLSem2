namespace Construx.App.Constants
{
    public class Statuses
    {
        public const string Approved = "Approved";
        public const string NotActive = "Not Active";
        public const string UnderVerification = "Under Verification";
        public const string NeedsDetails = "Needs Details";
    }
    public enum StatusesIds : int
    {
        Approved=1,
        NotActive=2,
        UnderVerification=3,
        NeedsDetails = 4
    }
}
