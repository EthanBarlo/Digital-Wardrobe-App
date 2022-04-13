using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDigitalWardrobe.Services;
using MyDigitalWardrobe.Models;
using System.Threading.Tasks;
using SQLite;
using System;

namespace Unit_Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetDatabaseConnection_ShouldReturnAnOpenDatabase()
        {
            var result = new SQLiteAsyncConnection("TestDatabase.db3");

            Assert.IsNotNull(result);

            await ItemService.UNIT_TESTING_SetDatabaseSource(result);
            await CollectionService.UNIT_TESTING_SetDatabaseSource(result);
        }

        [TestMethod]
        public async Task GetAllItems_ShouldReturnListOfItems()
        {
            // Setup the database
            await GetDatabaseConnection_ShouldReturnAnOpenDatabase();
            
            // Act
            var result = await ItemService.GetItemsAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddItemThenRetrieveAll_ShouldReturnItemWeAdded()
        {
            // Setup the database
            await GetDatabaseConnection_ShouldReturnAnOpenDatabase();
            
            // Arange
            Item newItem = new Item()
            {
                Name = "Test Item",
                Price = 23.23M,
                Collection = 1,
                ItemImage = "http://placekitten.com/200/300",
                RecieptImage = "http://placekitten.com/g/200/300",
                PurchasedName = "Testco",
                DatePurchased = DateTime.Now,
                WarrantyEnd = DateTime.Now,
            };

            // Act
            var ItemsBeforeAdding = await ItemService.GetItemsAsync();
            await ItemService.SaveItemAsync(newItem);
            var ItemsAfterAdding = await ItemService.GetItemsAsync();

            // Assert
            Assert.IsTrue(ItemsAfterAdding.Count > ItemsBeforeAdding.Count);
        }

        [TestMethod]
        public async Task GetAllCollections_ShouldReturnListOfCollections()
        {
            // Setup the database
            await GetDatabaseConnection_ShouldReturnAnOpenDatabase();

            // Act
            var result = await CollectionService.GetCollectionsAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddCollectionThenRetrieveAll_ShouldHaveMoreCollectionsThanBeforeAdding()
        {
            // Setup the database
            await GetDatabaseConnection_ShouldReturnAnOpenDatabase();

            // Arange
            Collection newCollection = new Collection()
            {
                Name = "Test Collection",
            };

            // Act
            var ItemsBeforeAdding = await CollectionService.GetCollectionsAsync();
            await CollectionService.SaveCollectionAsync(newCollection);
            var ItemsAfterAdding = await CollectionService.GetCollectionsAsync();

            // Assert
            Assert.IsTrue(ItemsAfterAdding.Count > ItemsBeforeAdding.Count);
        }        
    }
}
