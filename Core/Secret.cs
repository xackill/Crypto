using System;

namespace Core
{
    public static class Secret
    {
        // Global
        public const string ConnectionString = 
            @"Server=SPARKLING\SQLEXPRESS;database=Test;Integrated Security = true";

        // Distributed Currency
        public const int MinerReward = 50;

        // Anonymous Currency
        public static readonly byte[] BankPublicKey = Convert.FromBase64String("BgIAAACkAABSU0ExAAQAAAEAAQBNvIbBCTF5+2w+RSO6gUr5WZxnAKn7tlOzBmY/iK+n5j7tmtpHDaZmZslBuIDqwQlOHEDv9nYv3AaNXrU2Cwz3VDlyhju0o13RzeRoK2TDcfZZBMNhKE83X1jqg5BLhsow3n6I604kK49CPLo1TVQ1MB9dY/WKfZuP42NVfGhk4w==");
        public static readonly byte[] BankPublicPrivateKey = Convert.FromBase64String("BwIAAACkAABSU0EyAAQAAAEAAQBNvIbBCTF5+2w+RSO6gUr5WZxnAKn7tlOzBmY/iK+n5j7tmtpHDaZmZslBuIDqwQlOHEDv9nYv3AaNXrU2Cwz3VDlyhju0o13RzeRoK2TDcfZZBMNhKE83X1jqg5BLhsow3n6I604kK49CPLo1TVQ1MB9dY/WKfZuP42NVfGhk4+Ut6DlrXav5sEPCxZ/bCZmDbMAG5EIz8lKTJt+lm/p6wpkqtYFi/eer7mgMmbi1am4xrgR8nxWC/asBGZzoV/BJrqDAKn3is89ESleydUTXujp3n4tY3gaPm4jz0JU91Q0LGh/8Og6gOFV9CFL06Lyhm5SyYwUREICjFcLdhDTyQbZ8gT2rZuX4VfyOrLZCtXVOD/9BDQskvpYHBNO8rRr3O9dcCFRO0t2KH4KXiJ1mKOQj59B80hHlcLuffSj910mWbG6f6Cyv4JWoIRxAhsPFJdootRd+93xAQPv73EW47S92U/YZTngNlhRl/4p+bD+rjcvO3V+4c257zpCfJTL8hMRa2FUn3RMczoaiKUU7BweMna++5uZipYA3ZeLeXw5dybBK7JUSVCz53jjslrEOUbZKHIyxw5je1Ej6wTGFGal3UfTEh+DEr+/ZHKT9taRppzybkxsamfxTLtpZW/hSlfV5I5B6C3LilXXVpvqG52Znli/q0wRZtgP9hC6D4kBlGTtbwMoDRb3ve1fRj8AH4wcnb3kijxwp7eyjAyAvpcFL7W2UPjHdnpKLQDfLt6ElTLiodR2K0F9cJvFcDgA=");

        public const int EnvelopeSignCount = 8;
        public const int SecretsCount = 8;
    }
}
