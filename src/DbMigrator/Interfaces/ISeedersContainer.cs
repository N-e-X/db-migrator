namespace DbMigrator.Interfaces
{
    /// <summary>
    /// Contains database seeders
    /// </summary>
    internal interface ISeedersContainer
    {
        /// <summary>
        /// Enumeration of seeders for sequential application
        /// </summary>
        IEnumerable<ISeeder> Seeders { get; }
    }
}
