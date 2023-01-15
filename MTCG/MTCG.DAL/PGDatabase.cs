using MTCG.Models;
using MTCG.BL;
using Npgsql;
using System.Data;
using MTCG.Models.Components;
using static MTCG.Models.Card;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace MTCG.DAL
{
    public class PGDatabase : IDatabase
    {
        ILogger log = Logging.Get<PGDatabase>();

        IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=1234;Database=mtcgdbtest");
        PasswordHasher passwordHasher = new();

        ~PGDatabase()
        {
            connection.Close();
        }

        IDbCommand CreateCommand(string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        IDbDataParameter CreateStringParam(IDbCommand command, string colName, object value)
        {
            var result = command.CreateParameter();
            result.DbType = DbType.String;
            result.ParameterName = colName;
            result.Value = value;
            return result;
        }

        IDbDataParameter CreateInt64Param(IDbCommand command, string colName, long value)
        {
            var result = command.CreateParameter();
            result.DbType = DbType.Int64;
            result.ParameterName = colName;
            result.Value = value;
            return result;
        }

        IDbDataParameter CreateInt32Param(IDbCommand command, string colName, int value)
        {
            var result = command.CreateParameter();
            result.DbType = DbType.Int32;
            result.ParameterName = colName;
            result.Value = value;
            return result;
        }

        List<Component> GetCardComponents(string cardId)
        {           
            var result = new List<Component>();

            var command = CreateCommand("SELECT component_data FROM card_component WHERE card_id=@card_id;");
            command.Parameters.Add(CreateStringParam(command, "card_id", cardId));
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                {
                    var data = reader.GetString(0);
                    result.Add(CardDeserializer.DeserializeComponent(data));
                    log.LogTrace("deserialized card component of {0}=> {1}", cardId, result[result.Count-1]);
                }

            return result;
        }

        public bool Init()
        {
            log.LogInformation("Database init");
            connection.Open();
            return true;
        }

        public bool Clear()
        {
            var command = CreateCommand(
                "TRUNCATE " +
                "card, " +
                "card_component, " +
                "deck, " +
                "marketplace_card_packs, " +
                "user_card, " +
                "users, friends, " +
                "battle_challenges, " +
                "transaction_history;");
            command.ExecuteNonQuery();
            log.LogInformation("cleared database tables");
            return true;
        }

        void SaveTransaction(string userId, long amount)
        {
            var command = CreateCommand("INSERT INTO transaction_history (user_id, coin_amount, \"time_stamp\") VALUES (@user_id, @coin_amount, CURRENT_TIMESTAMP);");
            command.Parameters.Add(CreateStringParam(command, "user_id", userId));
            command.Parameters.Add(CreateInt64Param(command, "coin_amount", amount));
            command.ExecuteNonQuery();
            log.LogDebug("added entry to transaction history: user {0} paid/got {1} coins, timestamped at current time", userId, amount);
        }

        bool UserIdExists(string userId)
        {
            var command = CreateCommand("SELECT user_id FROM users WHERE user_id=@user_id;");
            command.Parameters.Add(CreateStringParam(command, "user_id", userId));
            return command.ExecuteScalar() as string == userId;
        }

        public bool CreateUser(string username, string password) 
            => CreateUser(Guid.NewGuid().ToString(), username, password);

        public bool CreateUser(string userId, string username, string password)
        {
            var command = CreateCommand(
                "INSERT INTO users (user_id, username, password_hash, coins, bio, profile_image, wins, losses, elo)" +
                "VALUES(@user_id, @username, @password_hash, 20, NULL, NULL, 0, 0, 100);");

            command.Parameters.Add(CreateStringParam(command, "user_id", userId));
            command.Parameters.Add(CreateStringParam(command, "username", username));
            command.Parameters.Add(CreateStringParam(command, "password_hash", passwordHasher.Hash(password)));

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                log.LogError(exc.Message);
                return false;
            }

            return true;
        }

        public bool CreateCard(Card card)
            => CreateCard(Guid.NewGuid().ToString(), card);

        public bool CreateCard(string cardId, Card card)
        {
            var command = CreateCommand(
                "INSERT INTO card (card_id, card_name, \"element\", card_type, base_damage)" +
                "VALUES(@card_id, @card_name, @card_element, @card_type, @base_damage);");

            var parameters = new IDbDataParameter[]
            {
                CreateStringParam(command, "card_id", cardId),
                CreateStringParam(command, "card_name", card.Name),
                CreateStringParam(command, "card_element", card.Element.ToString()),
                CreateStringParam(command, "card_type", card.Type.ToString()),
                CreateInt64Param(command, "base_damage", card.Damage)
            };

            foreach (var param in parameters) command.Parameters.Add(param);
            command.ExecuteNonQuery();
            return true;
        }

        public bool CheckUserLogin(string username, string password)
        {
            var command = CreateCommand("SELECT password_hash FROM users WHERE username = @username;");
            command.Parameters.Add(CreateStringParam(command, "username", username));
            var hash = (string)command.ExecuteScalar();

            return hash == passwordHasher.Hash(password);
        }

        public bool GetUserData(string username, out UserData data)
        {
            var command = CreateCommand("SELECT profile_name, bio, profile_image FROM users WHERE username=@username;");
            command.Parameters.Add(CreateStringParam(command, "username", username));

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                data = new UserData
                {
                    Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                    Bio = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Image = reader.IsDBNull(2) ? null : reader.GetString(2)
                };
            }

            return true;
        }

        public bool SetUserData(string username, UserData data)
        {
            var command = CreateCommand("UPDATE users SET profile_name=@profile_name, bio=@bio, profile_image=@profile_image WHERE username=@username;");
            command.Parameters.Add(CreateStringParam(command, "username", username));
            command.Parameters.Add(CreateStringParam(command, "profile_name", data.Name != null ? data.Name : DBNull.Value));
            command.Parameters.Add(CreateStringParam(command, "bio", data.Bio != null ? data.Bio : DBNull.Value));
            command.Parameters.Add(CreateStringParam(command, "profile_image", data.Image != null ? data.Image : DBNull.Value));
            
            command.ExecuteNonQuery();
            return true;
        }

        public bool GetUserStats(string username, out UserStats userStats)
        {
            var command = CreateCommand("SELECT wins, losses, elo FROM users WHERE username=@username;");
            command.Parameters.Add(CreateStringParam(command, "username", username));

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                userStats = new UserStats
                {
                    Wins = reader.GetInt32(0),
                    Losses = reader.GetInt32(1),
                    Elo = reader.GetInt32(2)
                };
            }

            return true;
        }

        public bool SetUserStats(string username, UserStats userStats)
        {
            log.LogDebug("updating stats of {0} to {1},{2},{3}", username, userStats.Wins, userStats.Losses, userStats.Elo);

            var command = CreateCommand("UPDATE users SET wins=@wins, losses=@losses, elo=@elo WHERE username=@username;");
            command.Parameters.Add(CreateInt32Param(command, "wins", userStats.Wins));
            command.Parameters.Add(CreateInt32Param(command, "losses", userStats.Losses));
            command.Parameters.Add(CreateInt32Param(command, "elo", userStats.Elo));
            command.Parameters.Add(CreateStringParam(command, "username", username));
            command.ExecuteNonQuery();
            return true;
        }

        public bool SetDeck(string userId, string[] cardIds)
        {
            foreach (var cardId in cardIds)
            {
                log.LogDebug("checking if {0} owns {1}", userId, cardId);
                var command = CreateCommand("SELECT 123 FROM user_card WHERE user_id=@user_id AND card_id=@card_id;");
                command.Parameters.Add(CreateStringParam(command, "user_id", userId));
                command.Parameters.Add(CreateStringParam(command, "card_id", cardId));
                var check = command.ExecuteScalar();
                if (check == null || (int)check != 123)
                {
                    log.LogDebug("user does not own card to be inserted into deck");
                    return false;
                }
            }

            {
                var command = CreateCommand("DELETE FROM deck WHERE user_id=@user_id;");
                command.Parameters.Add(CreateStringParam(command, "user_id", userId));
                command.ExecuteNonQuery();
            }

            foreach (var cardId in cardIds)
            {
                var command = CreateCommand("INSERT INTO deck (user_id, card_id) VALUES (@user_id, @card_id);");
                command.Parameters.Add(CreateStringParam(command, "user_id", userId));
                command.Parameters.Add(CreateStringParam(command, "card_id", cardId));
                var insertCheck = command.ExecuteScalar();
            }

            return true;
        }

        public bool GetDeck(string userId, out string[] cardIds)
        {
            var command = CreateCommand("SELECT card_id FROM deck WHERE user_id=@user_id;");
            command.Parameters.Add(CreateStringParam(command, "user_id", userId));

            var result = new List<string>();
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add(
                        reader.GetString(0));

            cardIds = result.ToArray();
            return true;
        }

        public bool GetScoreBoard(out (string, int)[] eloRatings)
        {
            var command = CreateCommand("SELECT username, elo FROM users ORDER BY elo DESC;");
            
            var result = new List<(string, int)>();
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add((
                        reader.GetString(0), 
                        reader.GetInt32(1)));

            eloRatings = result.ToArray();
            return true;
        }

        public bool CreateTradeListing(string username, TradeListing listing)
        {
            throw new NotImplementedException();
        }

        public bool GetTradeListings(string username, out TradeListing[] listings)
        {
            throw new NotImplementedException();
        }

        public bool CancelTradeListing(string tradeListingId)
        {
            throw new NotImplementedException();
        }

        public bool CreateTradeOffer(string tradeListingId, TradeOffer offer)
        {
            throw new NotImplementedException();
        }

        public bool GetTradeOffers(string username, out TradeOffer[] offers)
        {
            throw new NotImplementedException();
        }

        public bool CancelTradeOffer(string tradeOfferId)
        {
            throw new NotImplementedException();
        }

        public bool AcceptTradeOffer(string tradeListingId)
        {
            throw new NotImplementedException();
        }

        public bool CreatePack(Card[] cards)
        {
            var newPackId = Guid.NewGuid();

            foreach (var card in cards)
            {
                log.LogDebug("adding card to database: {0} ", card);

                var cardGuid = card.Guid ?? Guid.NewGuid();
                CreateCard(cardGuid.ToString(), card);

                {
                    var command = CreateCommand("INSERT INTO marketplace_card_packs (pack_id, card_id) VALUES (@pack_id, @card_id);");
                    command.Parameters.Add(CreateStringParam(command, "pack_id", newPackId.ToString()));
                    command.Parameters.Add(CreateStringParam(command, "card_id", cardGuid.ToString()));
                    command.ExecuteNonQuery();
                }

                foreach (var component in card.Components)
                {
                    var data = CardDeserializer.SerializeComponent(component);
                    var command = CreateCommand("INSERT INTO card_component (card_id,component_data) VALUES (@card_id,@c_data);");
                    command.Parameters.Add(CreateStringParam(command, "card_id", cardGuid.ToString()));
                    command.Parameters.Add(CreateStringParam(command, "c_data", data));
                    command.ExecuteNonQuery();
                    log.LogTrace("serialized {0} to [{1}]", component, data);
                }
            }

            return true;
        }

        public bool GetPacks(out string[] packIds)
        {
            var result = new List<string>();
            var command = CreateCommand("SELECT DISTINCT pack_id FROM marketplace_card_packs;");
            
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add(reader.GetString(0));

            packIds = result.ToArray();
            return true;
        }

        public bool GetPackCards(out (string pack_id, string card_id)[] packCards)
        {
            var result = new List<(string,string)>();
            var command = CreateCommand("SELECT pack_id, card_id FROM marketplace_card_packs;");

            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add((
                        reader.GetString(0), 
                        reader.GetString(1)));

            packCards = result.ToArray();
            return true;
        }

        public bool BuyPack(string userId, string packId)
        {
            long coins;

            {
                var command = CreateCommand("SELECT coins FROM users WHERE user_id=@user_id;");
                command.Parameters.Add(CreateStringParam(command, "user_id", userId));
                var result = command.ExecuteScalar();
                
                if (result == null) return false;
                
                coins = (long)result;

                if (coins < 5) return false;
            }

            if (!ChangeCoinAmount(userId, -5)) return false;

            {
                var command = CreateCommand(
                    "INSERT INTO user_card " +
                    "(user_id, card_id) " +
                    "SELECT @user_id, marketplace_card_packs.card_id " +
                    "FROM marketplace_card_packs " +
                    "WHERE pack_id=@pack_id;");
                command.Parameters.Add(CreateStringParam(command, "user_id", userId));
                command.Parameters.Add(CreateStringParam(command, "pack_id", packId));
                command.ExecuteNonQuery();
            }

            {
                var command = CreateCommand("DELETE FROM marketplace_card_packs WHERE pack_id=@pack_id;");
                command.Parameters.Add(CreateStringParam(command, "pack_id", packId));
                command.ExecuteNonQuery();
            }

            return true;
        }

        public bool GetUserId(string username, out string userId)
        {
            var command = CreateCommand("SELECT user_id FROM users WHERE username=@username;");
            command.Parameters.Add(CreateStringParam(command, "username", username));
            var result = command.ExecuteScalar();
            
            if (result == null)
            {
                userId = null;
                return false;
            }

            userId = (string)result;
            return true;
        }

        public bool SwapCardDecks(string cardId, string targetDeckUserId)
        {
            int deletedCount;
            
            {
                var command = CreateCommand("DELETE FROM deck WHERE card_id=@card_id;");
                command.Parameters.Add(CreateStringParam(command, "card_id", cardId));
                var deleted = command.ExecuteScalar();
                if (deleted == null) return false;
                deletedCount = (int)deleted;
                if (deletedCount != 1) return false;
            }

            {
                var command = CreateCommand("INSERT INTO deck (user_id, card_id) VALUES (@user_id, @card_id);");
                command.Parameters.Add(CreateStringParam(command, "user_id", targetDeckUserId));
                command.Parameters.Add(CreateStringParam(command, "card_id", cardId));
                command.ExecuteNonQuery();
            }

            return true;
        }

        public bool GetCard(string cardId, out Card card)
        {
            var command = CreateCommand(
                "SELECT card_id, card_name, \"element\", card_type, base_damage " +
                "FROM card WHERE card_id=@card_id;");

            command.Parameters.Add(CreateStringParam(command, "card_id", cardId));

            using (var reader = command.ExecuteReader())
            {
                reader.Read();
                card = new Card(
                    Guid.Parse(reader.GetString(0)),
                    reader.GetString(1),
                    Enum.Parse<CardType>(reader.GetString(3)),
                    Enum.Parse<ElementType>(reader.GetString(2)),
                    reader.GetInt64(4));
            }

            card.Components = GetCardComponents(cardId);

            log.LogDebug("got card from database:{0}", card);

            return true;
        }

        public bool GetUserStackCards(string userId, out Card[] cards)
        {
            var cardIds = new List<string>();
            var result = new List<Card>();

            var command = CreateCommand("SELECT card_id FROM user_card WHERE user_id=@user_id;");
            command.Parameters.Add(CreateStringParam(command, "user_id", userId));
            using (var reader = command.ExecuteReader())
                while(reader.Read())
                    cardIds.Add(reader.GetString(0));

            foreach (var cardId in cardIds)
                if (GetCard(cardId, out var card))
                    result.Add(card);

            cards = result.ToArray();
            return true;
        }

        public bool GetDeck(string userId, out Deck deck)
        {
            if (!GetDeck(userId, out string[] cardIds))
            {
                deck = null;
                return false;
            }

            var result = new Deck();
            foreach (var cardId in cardIds)
                if (GetCard(cardId, out var card))
                    result.Add(card);

            deck = result;
            return true;
        }

        public bool CreateBattleChallenge(string username)
        {
            var command = CreateCommand("INSERT INTO battle_challenges (username) VALUES (@username);");
            command.Parameters.Add(CreateStringParam(command, "username", username));
            command.ExecuteNonQuery();
            return true;
        }

        public bool GetBattles(out string[] usernames)
        {
            var result = new List<string>();
            var command = CreateCommand("SELECT username FROM battle_challenges;");
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add(
                        reader.GetString(0).Trim());
            
            usernames = result.ToArray();
            return true;
        }

        public bool AddFriend(string username, string usernameFriend)
        {
            if (!GetUserId(usernameFriend, out var _)) return false;

            if (usernameFriend == null || usernameFriend == null) return false; 
            if (usernameFriend == username) return false;

            string 
                usernameA = username, 
                usernameB = usernameFriend;
            
            if (usernameA.CompareTo(usernameB) > 0) 
            {
                usernameA = usernameFriend;
                usernameB = username;
            }

            var command = CreateCommand("INSERT INTO friends VALUES (@user_a, @user_b);");
            command.Parameters.Add(CreateStringParam(command, "user_a", usernameA));
            command.Parameters.Add(CreateStringParam(command, "user_b", usernameB));
            command.ExecuteNonQuery();
            return true;
        }

        public bool RemoveFriend(string username, string usernameFriend)
        {
            if (usernameFriend == null || usernameFriend == null) return false;
            if (usernameFriend == username) return false;

            string
                usernameA = username,
                usernameB = usernameFriend;

            if (usernameA.CompareTo(usernameB) > 0)
            {
                usernameA = usernameFriend;
                usernameB = username;
            }

            var command = CreateCommand("DELETE FROM friends WHERE usernameA=@usernameA AND usernameB=@usernameB;");
            command.Parameters.Add(CreateStringParam(command, "usernameA", usernameA));
            command.Parameters.Add(CreateStringParam(command, "usernameB", usernameB));
            command.ExecuteNonQuery();

            return true;
        }

        public bool GetFriendBattles(string username, out string[] usernames)
        {
            if (!GetBattles(out var allBattlers))
            {
                usernames = null;
                return false;
            }

            var result = new List<string>();

            foreach (var battler in allBattlers)
            {
                string usernameA = username, usernameB = battler;

                if (usernameA.CompareTo(usernameB) > 0)
                {
                    usernameA = battler;
                    usernameB = username;
                }

                var command = CreateCommand("SELECT 123 FROM friends WHERE usernameA=@usernameA AND usernameB=@usernameB;");
                command.Parameters.Add(CreateStringParam(command, "usernameA", usernameA));
                command.Parameters.Add(CreateStringParam(command, "usernameB", usernameB));
                var check = command.ExecuteScalar();
                if (check != null && (int)check == 123)
                    result.Add(battler);
            }

            usernames = result.ToArray();
            return true;
        }

        public bool EndBattle(string username)
        {
            var command = CreateCommand("DELETE FROM battle_challenges WHERE username=@username;");
            command.Parameters.Add(CreateStringParam(command, "username", username));
            command.ExecuteNonQuery();
            return true;
        }

        public bool ChangeCoinAmount(string userId, long coinAmountDelta)
        {
            if (!UserIdExists(userId)) return false;

            var command = CreateCommand("UPDATE users SET coins=coins+@coins WHERE user_id=@user_id;");
            command.Parameters.Add(CreateInt64Param(command, "coins", coinAmountDelta));
            command.Parameters.Add(CreateStringParam(command, "user_id", userId));
            command.ExecuteNonQuery();

            SaveTransaction(userId, coinAmountDelta);
            
            return true;
        }
    }
}
