using System;

namespace Core
{
    public static class Secret
    {
        public const string ConnectionString = 
            @"Server=SPARKLING\SQLEXPRESS;database=Test;Integrated Security = true";

        public const int IntervalLength = 1000;
        public const int PrimeCertainty = 100;
        public const int BytesCount = 128;
    }
}
