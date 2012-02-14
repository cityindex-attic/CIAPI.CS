using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CIAPI.CS.Koans.KoanRunner
{
    public class KoanAssert
    {
        public static void That(bool isTrue, string message)
        {
            try
            {
                Assert.That(isTrue, message);
            }
            catch (Exception ex)
            {
                throw new FailedToReachEnlightenmentException(ex.Message);
            }
        }
        public static void That(object actual, IResolveConstraint expression)
        {
            That(actual, expression, string.Empty);
        }
        public static void That(object actual, IResolveConstraint expression, string message)
        {
            try
            {
                Assert.That(actual, expression, message);
            }
            catch (Exception ex)
            {
                throw new FailedToReachEnlightenmentException(ex.Message);
            }
        }

        public static void Fail(string message)
        {
            throw new FailedToReachEnlightenmentException(message);
        }
    }
}