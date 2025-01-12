namespace TZ.Backend.Console.Adapter.Interfaces
{
    public interface IProduct
    {
        #region Use to fill database

        public Task FillDatabase();

        #endregion

        public Task<string> Get(params string?[] flags);
        public Task<string> Buy(string? id);
        public Task<string> Restock(params string?[] flags);
    }
}
