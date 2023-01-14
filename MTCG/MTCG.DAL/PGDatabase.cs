using MTCG.Models;
using MTCG.BL;
using Npgsql;
using System.Data;
using MTCG.Models.Components;
using static MTCG.Models.Card;
using Microsoft.Extensions.Logging;

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
            command.CommandText= commandText;
            return command;
        }

        IDbDataParameter CreateStringParam(IDbCommand command, string colName, string value)
        {
            var result = command.CreateParameter();
            result.DbType= DbType.String;
            result.ParameterName = colName;
            result.Value = value;
            return result;
        }

        IDbDataParameter CreateInt64Param(IDbCommand command, string colName, long value)
        {
            var result = command.CreateParameter();
            result.DbType= DbType.Int64;
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
                    result.Add(
                        CardDeserializer.DeserializeComponent(
                            reader.GetString(0)));

            return result;
        }

        public bool Init()
        {
            log.LogInformation("Database init");
            connection.Open();
            return true;
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

        public bool SetUserData(string username, UserData data)
        {
            var command = CreateCommand("UPDATE users SET profile_name=@profile_name, bio=@bio, profile_image=@profile_image WHERE username=@username;");
            command.Parameters.Add(CreateStringParam(command, "username", username));
            command.Parameters.Add(CreateStringParam(command, "profile_name", data.Name));
            command.Parameters.Add(CreateStringParam(command, "bio", data.Bio));
            command.Parameters.Add(CreateStringParam(command, "profile_image", data.Image));
            command.ExecuteNonQuery();
            return true;
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
                var cardGuid = card.Guid ?? Guid.NewGuid();
                CreateCard(cardGuid.ToString(), card);

                var command = CreateCommand("INSERT INTO marketplace_card_packs (pack_id, card_id) VALUES (@pack_id, @card_id);");
                command.Parameters.Add(CreateStringParam(command, "pack_id", newPackId.ToString()));
                command.Parameters.Add(CreateStringParam(command, "card_id", cardGuid.ToString()));
                command.ExecuteNonQuery();
            }

            return true;
        }

        public bool GetPacks(out string[] packIds)
        {
            var result = new List<string>();
            var command = CreateCommand("SELECT pack_id FROM marketplace_card_packs;");
            
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    result.Add(reader.GetString(0));

            packIds = result.ToArray();
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

            {
                var command = CreateCommand("UPDATE users SET coins=@coins WHERE user_id=@user_id;");
                command.Parameters.Add(CreateInt64Param(command, "coins", coins - 5));
                command.Parameters.Add(CreateStringParam(command, "user_id", userId));
                command.ExecuteNonQuery();
            }

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

        public bool GetDeck(string userId, out Card[] deck)
        {
            if (!GetDeck(userId, out string[] cardIds))
            {
                deck = null;
                return false;
            }

            var result = new List<Card>();
            foreach (var cardId in cardIds)
                if (GetCard(cardId, out var card))
                    result.Add(card);

            deck = result.ToArray();
            return true;
        }
    }
}
