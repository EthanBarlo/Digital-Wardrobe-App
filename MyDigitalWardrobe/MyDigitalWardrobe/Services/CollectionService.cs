using System.Threading.Tasks;
using System.Collections.Generic;
using SQLite;
using Xamarin.Essentials;
using MyDigitalWardrobe.Models;
using System.IO;

namespace MyDigitalWardrobe.Services
{
    public static class CollectionService
    {
        private static SQLiteAsyncConnection _connection;

        /// <summary>
        /// Initilises the connection with the database, and creates the collection table if it doesnt exist.
        /// </summary>
        /// <returns></returns>
        private static async Task Init()
        {
            if (_connection != null)
                return;
            _connection = App.Database;
            await _connection.CreateTableAsync<Collection>();
            _connection = App.Database;
        }

        /// <summary>
        /// Retrives all collection from the database.
        /// </summary>
        /// <returns>A list of all collections within the database.</returns>
        public static async Task<List<Collection>> GetCollectionsAsync()
        {
            await Init();
            return await _connection.Table<Collection>().ToListAsync();
        }


        /// <summary>
        /// Returns the collection object that the provided item is a part of.
        /// </summary>
        /// <param name="item">Item we are searching for</param>
        /// <returns>Collection Object</returns>
        public static async Task<Collection> GetCollectionFromItemAsync(Item item)
        {
            await Init();
            var collection = await _connection.Table<Collection>().Where(c => c.ID == item.Collection).FirstOrDefaultAsync();
            return collection;
        }
        

        /// <summary>
        /// Gets a list of all collections, as well as all items within those collections.
        /// </summary>
        /// <returns>List<(string Name, List<Item> Items)></returns>
        public static async Task<List<CollectionWithItems>> GetCollectionsWithItemsAsync()
        {
            await Init();
            List<CollectionWithItems> collectionsWithItems = new List<CollectionWithItems>();

            List<Collection> collections = await _connection.Table<Collection>().ToListAsync();
            foreach (var c in collections)
            {
                var items = await ItemService.GetItemsFromCollection(c.ID);
                collectionsWithItems.Add(new CollectionWithItems
                {
                    Name = c.Name,
                    Items = items
                });
            }
            return collectionsWithItems;
        }

        /// <summary>
        /// Creates a new entry in the database.
        /// </summary>
        /// <param name="item">The new collection to enter.</param>
        /// <returns>Total number of rows created.</returns>
        public static async Task<int> SaveCollectionAsync(Collection collection)
        {
            await Init();
            return await _connection.InsertAsync(collection);
        }
        
        /// <summary>
        /// Updates an existing collection with new data.
        /// </summary>
        /// <param name="collection">The updated item object.</param>
        /// <returns>Total number of rows updated.</returns>
        public static async Task<int> UpdateCollectionAsync(Collection collection)
        {
            await Init();
            return await _connection.UpdateAsync(collection);
        }
        
        /// <summary>
        /// Deletes a collection from the database.
        /// </summary>
        /// <param name="id">The item identity to delete.</param>
        /// <returns>Total number of rows deleted.</returns>
        public static async Task<int> DeleteCollectionAsync(int id)
        {
            await Init();
            return await _connection.DeleteAsync<Collection>(id);
        }

        /// <summary>
        /// ONLY TO BE USED WHEN UNIT TESTING, SETS THE DATABASE SOURCE
        /// </summary>
        /// <param name="connection"></param>
        public static async Task UNIT_TESTING_SetDatabaseSource(SQLiteAsyncConnection connection)
        {
            _connection = connection;
            await _connection.CreateTableAsync<Collection>();
        }
    }
}
