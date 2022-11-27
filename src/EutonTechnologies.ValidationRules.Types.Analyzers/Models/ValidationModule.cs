namespace EutonTechnologies.ValidatonRules.Types.Analyzers.Models
{
    /// <summary>
    /// This represents a data transfer object that maps a persistence layer object.  
    /// The name/namespace is for the data transfer object.
    /// The rules map to a data annotation on a persistence layer object.
    /// </summary>
    internal class ValidationModule
    {
        internal ValidationModule(string name, string @namespace)
        {
            Name = name;
            Namespace = @namespace;
            Rules = new List<ValidationRule>();
        }

        public string Name { get; set; }
        public string Namespace { get; set; }
        public List<ValidationRule> Rules { get; set; }
    }
}
