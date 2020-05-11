namespace Orbitfog.Core.Database.Mapper
{
    public class DbDataReaderMapperConfiguration
    {
        static DbDataReaderMapperConfiguration()
        {
            Default = new DbDataReaderMapperConfiguration();
        }

        public static DbDataReaderMapperConfiguration Default { get; }

        public bool CaseSensitive { get; set; }
        public bool UseProperties { get; set; }
        public bool UseFields { get; set; }

        public DbDataReaderMapperConfiguration()
        {
            CaseSensitive = true;
            UseProperties = true;
            UseFields = true;
        }
    }
}
