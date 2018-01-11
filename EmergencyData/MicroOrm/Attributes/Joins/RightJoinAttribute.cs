namespace EmergencyData.MicroOrm.Attributes.Joins
{
    public class RightJoinAttribute : JoinAttributeBase
    {
        public RightJoinAttribute()
        {
        }

        public RightJoinAttribute(string tableName, string key, string externalKey)
            : base(tableName, key, externalKey)
        {
        }
    }
}
