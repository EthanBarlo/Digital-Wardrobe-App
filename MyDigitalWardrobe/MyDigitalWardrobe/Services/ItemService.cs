using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite;
using Xamarin.Essentials;
using MyDigitalWardrobe.Models;
using System.IO;
using System;
using System.Linq;

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
        ///  TODO
        /// </summary>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public static async Task<List<Item>> GetItemsFromCollection(int collectionId)
        {
            await Init();
            //return await _connection.Table<Item>().Where(i => i.Collection == collectionId).ToListAsync();
            var items = await _connection.QueryAsync<Item>($"SELECT * FROM Item WHERE ID = {collectionId}");
            return items.ToList();
            //return (await _connection.Table<Item>().ToListAsync()).Where(i => i.Collection == collectionId).ToList();
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
            return await _connection.DeleteAsync<Item>(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The next ID to be used for a new item.</returns>
        public static async Task<int> GetNextItemID()
        {
            await Init();
            var item = await _connection.QueryAsync<Item>("SELECT * FROM Item ORDER BY ID DESC LIMIT 1");
            if (item.Count == 0)
                return 1;
            return item[0].ID + 1;
        }

        /// <summary>
        /// ONLY TO BE USED WHEN UNIT TESTING, SETS THE DATABASE SOURCE
        /// </summary>
        /// <param name="connection"></param>
        public static async Task UNIT_TESTING_SetDatabaseSource(SQLiteAsyncConnection connection)
        {
            _connection = connection;
            await _connection.CreateTableAsync<Item>();
        }
    }
}
