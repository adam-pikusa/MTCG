using MTCG.Models;

namespace MTCG.DAL
{
    public interface IDatabase
    {
        bool Init();
        bool Clear();
        
        bool CreateUser(string username, string password);
        bool CreateUser(string userId, string username, string password);
        bool CheckUserLogin(string username, string password);
        bool GetUserData(string username, out UserData data);
        bool SetUserData(string username, UserData data);
        bool GetUserStats(string username, out UserStats UserStats);
        bool SetUserStats(string username, UserStats userStats);
        bool GetUserId(string username, out string userId);
        bool GetUserStackCards(string userId, out Card[] cards);
        bool ChangeCoinAmount(string userId, long coinAmountDelta);

        bool CreateCard(Card card);
        bool GetCard(string cardId, out Card card);

        bool SetDeck(string userId, string[] cardIds);
        bool GetDeck(string userId, out string[] cardIds);
        bool GetDeck(string userId, out Deck deck);
        bool SwapCardDecks(string cardId, string targetDeckUserId);

        bool GetScoreBoard(out (string, int)[] eloRatings);

        bool CreateBattleChallenge(string username);
        bool GetBattles(out string[] usernames);
        bool EndBattle(string username);    

        bool AddFriend(string username, string usernameFriend);
        bool RemoveFriend(string username, string usernameFriend);
        bool GetFriendBattles(string username, out string[] usernames);

        bool CreateTradeListing(string username, TradeListing listing);
        bool GetTradeListings(string username, out TradeListing[] listings);
        bool CancelTradeListing(string tradeListingId);
        bool CreateTradeOffer(string tradeListingId, TradeOffer offer);
        bool GetTradeOffers(string username, out TradeOffer[] offers);
        bool CancelTradeOffer(string tradeOfferId);
        bool AcceptTradeOffer(string tradeListingId);

        bool CreatePack(Card[] cards);
        bool GetPacks(out string[] packIds);
        bool GetPackCards(out (string pack_id, string card_id)[] packCards);
        bool BuyPack(string userId, string packId);
    }
}
