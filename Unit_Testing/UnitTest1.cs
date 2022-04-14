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
        // Xamarin Forms will not allow you to use the SQLite connection in a unit test.
        // Because of this we create a new connection, in which we can test the CRUD operations.
        public async Task EstablishDatabaseTestingConnection()
        {
            var result = new SQLiteAsyncConnection("TestDatabase.db3");
            await ItemService.UNIT_TESTING_SetDatabaseSource(result);
            await CollectionService.UNIT_TESTING_SetDatabaseSource(result);
        }

        [TestMethod]
        public async Task GetAllItems_ShouldReturnListOfItems()
        {
            // Setup the database
            await EstablishDatabaseTestingConnection();
            
            // Act
            var result = await ItemService.GetItemsAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddItemThenRetrieveAll_ShouldReturnItemWeAdded()
        {
            // Setup the database
            await EstablishDatabaseTestingConnection();
            
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
        public async Task DeleteLastItem_ShouldDeleteTheLastItem()
        {
            // Setup the database
            await EstablishDatabaseTestingConnection();

            // Act
            var ItemsBeforeAdding = await ItemService.GetItemsAsync();
            await ItemService.DeleteItemAsync(ItemsBeforeAdding[ItemsBeforeAdding.Count - 1].ID);
            var ItemsAfterAdding = await ItemService.GetItemsAsync();

            // Assert
            Assert.IsTrue(ItemsAfterAdding.Count < ItemsBeforeAdding.Count);
        }

        [TestMethod]
        public async Task GetAllCollections_ShouldReturnListOfCollections()
        {
            // Setup the database
            await EstablishDatabaseTestingConnection();

            // Act
            var result = await CollectionService.GetCollectionsAsync();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task AddCollectionThenRetrieveAll_ShouldHaveMoreCollectionsThanBeforeAdding()
        {
            // Setup the database
            await EstablishDatabaseTestingConnection();

            // Arange
            Collection newCollection = new Collection()
            {
                Name = "Test Collection",
            };

            // Act
            var CollectionsBeforeAdding = await CollectionService.GetCollectionsAsync();
            await CollectionService.SaveCollectionAsync(newCollection);
            var CollectionsAfterAdding = await CollectionService.GetCollectionsAsync();

            // Assert
            Assert.IsTrue(CollectionsAfterAdding.Count > CollectionsBeforeAdding.Count);
        }

        [TestMethod]
        public async Task DeleteLastCollection_ShouldRemoveTheLastCollection()
        {
            // Setup the database
            await EstablishDatabaseTestingConnection();

            // Act
            var CollectionsBeforeAdding = await CollectionService.GetCollectionsAsync();
            await CollectionService.DeleteCollectionAsync(CollectionsBeforeAdding[CollectionsBeforeAdding.Count-1].ID);
            var CollectionsAfterAdding = await CollectionService.GetCollectionsAsync();

            // Assert
            Assert.IsTrue(CollectionsAfterAdding.Count < CollectionsBeforeAdding.Count);
        }
    }
}
