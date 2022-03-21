using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using MyDigitalWardrobe.Models;
using System.Threading.Tasks;

namespace MyDigitalWardrobe
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _connection;

        public Database(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<Item>();
        }


        #region Items
        /// <summary>
        /// Retrives all items from the database.
        /// </summary>
        /// <returns>A list of all Items within the database.</returns>
        public async Task<List<Item>> GetItemsAsync()
        {
            return await _connection.Table<Item>().ToListAsync();
        }
        /// <summary>
        /// Creates a new entry in the database.
        /// </summary>
        /// <param name="item">The new item to enter.</param>
        /// <returns>Total number of rows created.</returns>
        public async Task<int> SaveItemAsync(Item item)
        {
            return await _connection.InsertAsync(item);
        }
        /// <summary>
        /// Updates an existing item with new data.
        /// </summary>
        /// <param name="item">The updated item object.</param>
        /// <returns>Total number of rows updated.</returns>
        public async Task<int> UpdateItemAsync(Item item)
        {
            return await _connection.UpdateAsync(item);
        }
        /// <summary>
        /// Deletes an item from the database.
        /// </summary>
        /// <param name="item">The item to delete.</param>
        /// <returns>Total number of rows deleted.</returns>
        public async Task<int> DeleteItemAsync(Item item)
        {
            return await _connection.DeleteAsync(item);
        }
        #endregion
    }
}
