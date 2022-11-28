namespace CleanArchitecture.ValidatonRules.Types.Analyzers.Models
{
    /// <summary>
    /// Validation rule used to map a data annotation from a persistence layer object to create a Fluent Validation Rule
    /// </summary>
    internal class ValidationRule
    {
        internal ValidationRule(string name)
        {
            Name = name;
            Rules = new List<string>();
        }
        public string Name { get; set; }
        public List<string> Rules { get; set; }
    }
}
