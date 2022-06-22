namespace SqlKata.Net6
{
    public partial class Query
    {
        public Query AsDelete()
        {
            Method = "delete";
            return this;
        }

    }
}
