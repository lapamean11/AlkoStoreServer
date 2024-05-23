namespace AlkoStoreServer.CustomAttributes
{
    public class ReferenceAttribute : Attribute
    {
        public Type Reference;

        public ReferenceAttribute(Type Reference)
        {
            this.Reference = Reference;
        }
    }
}
