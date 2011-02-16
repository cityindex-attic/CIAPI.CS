namespace CIAPI.CS.Koans.KoanRunner
{
    public static class Assert
    {
        public static void That(bool isTrue, string message)
        {
            if (isTrue) return;

            throw new FailedToReachEnlightenmentException(message);
        }
    }
}