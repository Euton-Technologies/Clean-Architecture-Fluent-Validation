namespace CleanArchitecture.ValidationRules.Types
{
    /// <summary>
    /// Attribute for marking a data transfer object for mapping to a persistence layer object.
    /// If the persistence layer object has a data annotation, then a Fluent Validation rule can be 
    /// created so that the code isn't duplicated.
    /// </summary>
    public class ExtendValidationAttribute : Attribute
    {
        public ExtendValidationAttribute(Type type)
        { }
    }
}
