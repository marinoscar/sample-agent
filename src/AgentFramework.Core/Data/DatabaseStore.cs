using Luval.DbConnectionMate;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public abstract class DatabaseStore : ChatMessageStore
    {

        protected abstract Func<DbConnection> CreateConnection { get; }
        protected abstract string SqlInsertStatement { get; }

        protected abstract string SelectSqlStatement { get; }


        public string ThreadId { get; set; } = default!;

        public override async Task AddMessagesAsync(IEnumerable<ChatMessage> messages, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(ThreadId)) throw new InvalidOperationException("ThreadId must be set before adding messages to the store.");
            await PersistMessagesAsync(messages, cancellationToken);
        }

        public override async Task<IEnumerable<ChatMessage>> GetMessagesAsync(CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(ThreadId)) throw new InvalidOperationException("ThreadId must be set before retrieving messages from the store.");
            return (await GetMessagesByThreadIdAsync(ThreadId, cancellationToken)).Select(i => FromMessage(i));
        }

        public override JsonElement Serialize(JsonSerializerOptions? jsonSerializerOptions = null)
        {
            return JsonSerializer.SerializeToElement(this.ThreadId, jsonSerializerOptions);
        }

        protected virtual string SerializeMessage(ChatMessage message)
        {
            return JsonSerializer.Serialize(message, new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
        }

        protected virtual Message CreateMessage(ChatMessage chatMessage)
        {
            return new Message()
            {
                ThreadId = this.ThreadId,
                MessageText = chatMessage.Text,
                SerializedMessage = SerializeMessage(chatMessage),
                UtcCreatedAt = chatMessage.CreatedAt.HasValue ? chatMessage.CreatedAt.Value.UtcDateTime : DateTime.UtcNow
            };
        }

        protected virtual ChatMessage FromMessage(Message message)
        {
            return JsonSerializer.Deserialize<ChatMessage>(message.SerializedMessage, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            }) ?? throw new InvalidOperationException("Could not deserialize the ChatMessage from the stored message.");
        }

        protected virtual async Task<IEnumerable<Message>> PersistMessagesAsync(IEnumerable<ChatMessage> messages, CancellationToken cancellationToken = default)
        {
            var result = new List<Message>();
            if (CreateConnection == null) throw new InvalidOperationException("Database connection is not initialized.");
            if (string.IsNullOrWhiteSpace(SqlInsertStatement)) throw new InvalidOperationException("SQL Insert Statement is not initialized.");

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync();
                var tran = await conn.BeginTransactionAsync();
                foreach (var message in messages)
                {
                    var msg = CreateMessage(message);
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.Connection = conn;
                        cmd.CommandTimeout = 300;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = SqlInsertStatement;

                        // ThreadId
                        var pThreadId = cmd.CreateParameter();
                        pThreadId.ParameterName = "ThreadId";
                        pThreadId.Value = msg.ThreadId;
                        cmd.Parameters.Add(pThreadId);

                        // MessageText
                        var pMessageText = cmd.CreateParameter();
                        pMessageText.ParameterName = "MessageText";
                        pMessageText.Value = msg.MessageText;
                        cmd.Parameters.Add(pMessageText);

                        // SerializedMessage
                        var pSerializedMessage = cmd.CreateParameter();
                        pSerializedMessage.ParameterName = "SerializedMessage";
                        pSerializedMessage.Value = msg.SerializedMessage;
                        cmd.Parameters.Add(pSerializedMessage);

                        // UtcCreatedAt
                        var pUtcCreatedAt = cmd.CreateParameter();
                        pUtcCreatedAt.ParameterName = "UtcCreatedAt";
                        pUtcCreatedAt.Value = msg.UtcCreatedAt;
                        cmd.Parameters.Add(pUtcCreatedAt);

                        var id = (ulong)await cmd.ExecuteScalarAsync(cancellationToken);
                        msg.Id = (ulong)id;

                        result.Add(msg);

                    }
                }
                await tran.CommitAsync();
                await conn.CloseAsync();
            }
            return result;
        }


        protected virtual async Task<IEnumerable<Message>> GetMessagesByThreadIdAsync(
                                                                                        string threadId,
                                                                                        CancellationToken cancellationToken = default)
        {
            if (CreateConnection == null)
                throw new InvalidOperationException("Database connection is not initialized.");

            if (string.IsNullOrWhiteSpace(SelectSqlStatement))
                throw new InvalidOperationException("SQL Select Statement is not initialized.");

            if (string.IsNullOrWhiteSpace(threadId))
                throw new ArgumentException("ThreadId is required.", nameof(threadId));

            var result = new List<Message>();

            using (var conn = CreateConnection())
            {
                await conn.OpenAsync(cancellationToken);

                using (var cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandTimeout = 300;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = SelectSqlStatement;

                    // ThreadId parameter
                    var pThreadId = cmd.CreateParameter();
                    pThreadId.ParameterName = "ThreadId";
                    pThreadId.Value = threadId;
                    cmd.Parameters.Add(pThreadId);

                    using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            var msg = new Message
                            {
                                Id = (ulong)reader.GetInt64(0),          // id
                                ThreadId = reader.GetString(1),          // thread_id
                                MessageText = reader.GetString(2),       // message_text
                                SerializedMessage = reader.GetString(3), // serialized_message
                                UtcCreatedAt = reader.GetDateTime(4)     // utc_created_at
                            };

                            result.Add(msg);
                        }
                    }
                }

                await conn.CloseAsync();
            }

            return result;
        }


    }
}
