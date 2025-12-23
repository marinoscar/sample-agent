using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    public static class MessageSqlExtensions
    {
        public static string ToPostgresInsertSql(this Message _)
        {
            return """
        INSERT INTO message (
            thread_id,
            message_text,
            serialized_message,
            utc_created_at
        )
        VALUES (
            @ThreadId,
            @MessageText,
            @SerializedMessage,
            @UtcCreatedAt
        )
        RETURNING id;
        """;
        }
    }
}
