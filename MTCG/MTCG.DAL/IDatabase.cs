using MTCG.Models;

namespace MTCG.DAL
{
    public interface IDatabase
    {
        bool Init();
        
        bool CreateUser(string username, string password);
        bool CreateUser(string userId, string username, string password);
        bool CheckUserLogin(string username, string password);
        bool SetUserData(string username, UserData data);
        bool GetUserData(string username, out UserData data);
        bool GetUserStats(string username, out UserStats UserStats);
        bool GetUserId(string username, out string userId);

        bool CreateCard(Card card);
        bool GetCard(string cardId, out Card card);

        bool SetDeck(string userId, string[] cardIds);
        bool GetDeck(string userId, out string[] cardIds);
        bool SwapCardDecks(string cardId, string targetDeckUserId);

        bool GetScoreBoard(out (string, int)[] eloRatings);

        bool CreateTradeListing(string username, TradeListing listing);
        bool GetTradeListings(string username, out TradeListing[] listings);
        bool CancelTradeListing(string tradeListingId);
        bool CreateTradeOffer(string tradeListingId, TradeOffer offer);
        bool GetTradeOffers(string username, out TradeOffer[] offers);
        bool CancelTradeOffer(string tradeOfferId);
        bool AcceptTradeOffer(string tradeListingId);

        bool CreatePack(Card[] cards);
        bool BuyPack(string userId);
    }
}
