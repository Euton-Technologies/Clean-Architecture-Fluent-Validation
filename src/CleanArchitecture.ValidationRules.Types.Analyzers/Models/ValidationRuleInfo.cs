namespace CleanArchitecture.ValidatonRules.Types.Analyzers.Models
{
    /// <summary>
    /// Validation Rule Info to map data transfer objects and persistence layer objects
    /// </summary>
    internal class ValidationRuleInfo : IEquatable<ValidationRuleInfo>
    {
        public ValidationRuleInfo(string dto, string persistenceObject)
        {
            Dto = dto;
            PersistenceObjects = new List<string> { persistenceObject };
        }
        public String Dto { get; set; }
        public List<String> PersistenceObjects { get; set; }

        public bool Equals(ValidationRuleInfo? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Dto == other.Dto &&
                PersistenceObjects.Count == other.PersistenceObjects.Count &&
                PersistenceObjects.All(o => other.PersistenceObjects.Any(a => o == a));
        }

        public override bool Equals(object? obj)
            => ReferenceEquals(this, obj) ||
                obj is ValidationRuleInfo other && Equals(other);

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + Dto.GetHashCode();

            foreach(var item in PersistenceObjects)
            {
                hash = hash * 23 + item.GetHashCode();
            }

            return hash;
        }

        public static bool operator ==(ValidationRuleInfo? left, ValidationRuleInfo? right)
            => Equals(left, right);

        public static bool operator !=(ValidationRuleInfo? left, ValidationRuleInfo? right)
            => !Equals(left, right);
    }
}
