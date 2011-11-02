using CIAPI.DTO;
using CIAPI.IntegrationTests;
using CIAPI.IntegrationTests.Streaming;
using CIAPI.Rpc;
using NUnit.Framework;

[TestFixture]
public class WatchListFixture : RpcFixtureBase
{
    [Test]
    public void HowToUseWatchLists()
    {

        var rpcClient = BuildRpcClient();

        #region Setting up

        // just deleting any existing watchlists - 

        ListWatchlistResponseDTO watchlists = rpcClient.Watchlist.GetWatchlists();
        foreach (ApiClientAccountWatchlistDTO watchlist in watchlists.ClientAccountWatchlists)
        {
            rpcClient.Watchlist.DeleteWatchlist(new ApiDeleteWatchlistRequestDTO
                                                    {
                                                        WatchlistId = watchlist.WatchlistId
                                                    });
        }


        #endregion

        // look ma, no watchlists
        watchlists = rpcClient.Watchlist.GetWatchlists();
        Assert.AreEqual(0, watchlists.ClientAccountWatchlists.Length);

        // create a watchlist. 
        // NOTE: do not specify WatchListId in either the ApiSaveWatchlistRequestDTO or any of the ApiClientAccountWatchlistItemDTO
        // this field is assigned by the server and is used only to delete the watchlist(s)

        // need a watchlist item
        var newWatchListItems = new[] {
                            new ApiClientAccountWatchlistItemDTO
                            {
                                DisplayOrder = 1,
                                MarketId = 71442
                            }
                        };

        // need a watchlist in which to place item
        var newWatchList = new ApiClientAccountWatchlistDTO
                    {
                        WatchlistDescription = "new watchlist",
                        Items = newWatchListItems
                    };

        // need an API input DTO containing the watchlist

        var watchListsToSave = new ApiSaveWatchlistRequestDTO
        {
            Watchlist = newWatchList
        };

        var saveResponse = rpcClient.Watchlist.SaveWatchlist(watchListsToSave);
        // this response is void of properties - nothing to test


        // lets verify that our watchlist was saved
        watchlists = rpcClient.Watchlist.GetWatchlists();
        Assert.AreEqual(1, watchlists.ClientAccountWatchlists.Length);

        // we need the id from the sole watchlist in order to delete it.
        int watchListToDelete = watchlists.ClientAccountWatchlists[0].WatchlistId;

        var watchListsToDelete = new ApiDeleteWatchlistRequestDTO
            {
                WatchlistId = watchListToDelete
            };

        rpcClient.Watchlist.DeleteWatchlist(watchListsToDelete);

        // verify that there are no watchlistsToSave
        watchlists = rpcClient.Watchlist.GetWatchlists();
        Assert.AreEqual(0, watchlists.ClientAccountWatchlists.Length);

        rpcClient.LogOut();

        rpcClient.Dispose();
    }
}