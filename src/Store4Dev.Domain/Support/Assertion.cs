namespace Store4Dev.Domain.Support
{
    public static class Assertion
    {
        public static void Between(int value, int min, int max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void Between(long value, long min, long max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void Between(decimal value, decimal min, decimal max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void NotBetween(int value, int min, int max, string message)
        {
            if (value > min || value < max)
                throw new DomainException(message);
        }

        public static void NotBetween(long value, long min, long max, string message)
        {
            if (value > min || value < max)
                throw new DomainException(message);
        }

        public static void NotBetween(decimal value, decimal min, decimal max, string message)
        {
            if (value > min || value < max)
                throw new DomainException(message);
        }

        public static void GreaterThan(int value, int min, string message)
        {
            if (value <= min)
                throw new DomainException(message);
        }

        public static void GreaterThan(long value, long min, string message)
        {
            if (value <= min)
                throw new DomainException(message);
        }

        public static void GreaterThan(decimal value, decimal min, string message)
        {
            if (value <= min)
                throw new DomainException(message);
        }

        public static void GreaterThanEqual(int value, int min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void GreaterThanEqual(long value, long min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void GreaterThanEqual(decimal value, decimal min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void LessThan(int value, int max, string message)
        {
            if (value >= max)
                throw new DomainException(message);
        }

        public static void LessThan(long value, long max, string message)
        {
            if (value >= max)
                throw new DomainException(message);
        }

        public static void LessThan(decimal value, decimal max, string message)
        {
            if (value >= max)
                throw new DomainException(message);
        }

        public static void LessThanEqual(int value, int max, string message)
        {
            if (value > max)
                throw new DomainException(message);
        }

        public static void LessThanEqual(long value, long max, string message)
        {
            if (value > max)
                throw new DomainException(message);
        }

        public static void LessThanEqual(decimal value, decimal max, string message)
        {
            if (value > max)
                throw new DomainException(message);
        }

        public static void MaxLength(string text, int max, string message)
        {
            var length = text.Trim().Length;
            if (length > max)
                throw new DomainException(message);
        }

        public static void MinLength(string text, int min, string message)
        {
            var length = text.Trim().Length;
            if (length < min)
                throw new DomainException(message);
        }

        public static void Type<T>(object obj, string message)
        {
            Type(typeof(T), obj, message);
        }

        public static void Type(Type type, object obj, string message)
        {
            if (!(obj.GetType() == type))
                throw new DomainException(message);
        }

        public static void True(bool expression, string message)
        {
            if (!expression)
                throw new DomainException(message);
        }

        public static void False(bool expression, string message)
        {
            if (expression)
                throw new DomainException(message);
        }

        public static void Null(object obj, string message)
        {
            if (obj != null)
                throw new DomainException(message);
        }

        public static void NotNull(object obj, string message)
        {
            if (obj == null)
                throw new DomainException(message);
        }

        public static void Empty(string text, string message)
        {
            if (text.Trim() != string.Empty)
                throw new DomainException(message);
        }

        public static void NotEmpty(string text, string message)
        {
            if (text.Trim() == string.Empty)
                throw new DomainException(message);
        }

        public static void Empty(Guid guid, string message)
        {
            if (guid != Guid.Empty)
                throw new DomainException(message);
        }

        public static void NotEmpty(Guid guid, string message)
        {
            if (guid == Guid.Empty)
                throw new DomainException(message);
        }

        public static void NotNullOrEmpty(string text, string message)
        {
            if (string.IsNullOrEmpty(text))
                throw new DomainException(message);
        }

        public static void Equal(object obj1, object obj2, string message)
        {
            if (!obj1.Equals(obj2))
                throw new DomainException(message);
        }

        public static void NotEqual(object obj1, object obj2, string message)
        {
            if (obj1.Equals(obj2))
                throw new DomainException(message);
        }

    }
}
