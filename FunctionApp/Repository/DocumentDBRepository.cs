using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Linq;
using System.Linq.Expressions;
using Repository.Models;

namespace Repository
{
    public class DocumentDbRepository<T> where T : class
    {
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly DocumentClient _client;

        public DocumentDbRepository(string collectionId, string databaseId = "TestDb")
        {
            _databaseId = databaseId;
            _collectionId = collectionId;

            _client = new DocumentClient(new Uri("https://localhost:8081"),
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId))
                .Where(predicate)
                .AsDocumentQuery();

            var results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var res = await _client.ReadDocumentAsync<T>(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
            return (T)(dynamic)res;

        }

        //private static async Task CreateDatabaseIfNotExistsAsync()
        //{
        //    try
        //    {
        //        await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            await client.CreateDatabaseAsync(new Database { Id = _databaseId });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private static async Task CreateCollectionIfNotExistsAsync()
        //{
        //    try
        //    {
        //        await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId));
        //    }
        //    catch (DocumentClientException e)
        //    {
        //        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            await client.CreateDocumentCollectionAsync(
        //                UriFactory.CreateDatabaseUri(_databaseId),
        //                new DocumentCollection { Id = _collectionId },
        //                new RequestOptions { OfferThroughput = 1000 });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}



    }

   
}
