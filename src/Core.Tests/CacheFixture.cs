using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CIAPI.DTO;
using NUnit.Framework;

namespace CIAPI.Core.Tests
{
    [TestFixture]
    public class CacheFixture
    {
        [Test]
        public void ItemCanBeCached()
        {
            var c = new RequestCache(TimeSpan.FromMilliseconds(10));
            c.Add("foo", new CacheItem<ErrorResponseDTO>() { Expiration = DateTimeOffset.UtcNow.AddSeconds(2), ItemState = CacheItemState.Complete });
            new AutoResetEvent(false).WaitOne(1000);
            var actual = c.Get<ErrorResponseDTO>("foo");
            Assert.IsNotNull(actual);
        }

        [Test, ExpectedException(ExpectedMessage = "item for foo was not found in the cache")]
        public void ItemCanExpireAndBePurged()
        {
            var c = new RequestCache(TimeSpan.FromMilliseconds(10));
            c.Add("foo", new CacheItem<ErrorResponseDTO>() { Expiration = DateTimeOffset.UtcNow.AddSeconds(2), ItemState = CacheItemState.Complete });
            new AutoResetEvent(false).WaitOne(3000);
            c.Get<ErrorResponseDTO>("foo");
            
        }

    }
}
