using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite;
using Xamarin.Essentials;
using MyDigitalWardrobe.Models;
using System.IO;

namespace MyDigitalWardrobe.Services
{
    public static class ItemService
    {
        private static SQLiteAsyncConnection _connection;

        /// <summary>
        /// Initilises the connection with the database, and creates the item table if it doesnt exist.
        /// </summary>
        /// <returns></returns>
        private static async Task Init()
        {
            if (_connection != null) 
                return;
            _connection = App.Database;
            await _connection.CreateTableAsync<Item>();
        }


        #region Items
        /// <summary>
        /// Retrives all items from the database.
        /// </summary>
        /// <returns>A list of all Items within the database.</returns>
        public static async Task<List<Item>> GetItemsAsync()
        {
            await Init();
            return await _connection.Table<Item>().ToListAsync();
        }
        /// <summary>
        /// Creates a new entry in the database.
        /// </summary>
        /// <param name="item">The new item to enter.</param>
        /// <returns>Total number of rows created.</returns>
        public static async Task<int> SaveItemAsync(Item item)
        {
            await Init();
            return await _connection.InsertAsync(item);
        }
        /// <summary>
        /// Updates an existing item with new data.
        /// </summary>
        /// <param name="item">The updated item object.</param>
        /// <returns>Total number of rows updated.</returns>
        public static async Task<int> UpdateItemAsync(Item item)
        {
            await Init();
            return await _connection.UpdateAsync(item);
        }
        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="id">The item identity to delete.</param>
        /// <returns>Total number of rows deleted.</returns>
        public static async Task<int> DeleteItemAsync(int id)
        {
            await Init();
            return await _connection.DeleteAsync(id);
        }
        #endregion
    }
}
