using FNR.Model;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace FNR.ElasticSearch
{
    public class ElasticHelper
    {

        public static ElasticClient GetElasticClient()
        {
            var settings = new ConnectionSettings(ElasticsearchConfiguration.Connection).DefaultIndex(ElasticsearchConfiguration.DefaultIndex);

            return new ElasticClient(settings);
        }

        public static ElasticClient GetElasticClient(string userIndex)
        {
            var settings = new ConnectionSettings(ElasticsearchConfiguration.Connection).DefaultIndex(userIndex);

            return new ElasticClient(settings);
        }

        public static void CreateIndex()
        {
            ElasticClient elastic = GetElasticClient();
            if (!elastic.IndexExists(ElasticsearchConfiguration.DefaultIndex).Exists)
            {
                var descriptor = new CreateIndexDescriptor(ElasticsearchConfiguration.DefaultIndex)
                    .Settings(s => s.NumberOfShards(5).NumberOfReplicas(1))
                        .Mappings(ms => ms
                            .Map<Novel>(m => m
                                .AutoMap()
                                .Properties(ps => ps
                                    .Nested<Section>(n => n
                                        .Name(c => c.Sections)))));
                elastic.CreateIndex(descriptor);
            }
        }

        public static void CreateUserIndex()
        {
            ElasticClient elastic = GetElasticClient(ElasticsearchConfiguration.UserIndex);
            if (!elastic.IndexExists(ElasticsearchConfiguration.UserIndex).Exists)
            {
                var descriptor = new CreateIndexDescriptor(ElasticsearchConfiguration.UserIndex)
                    .Settings(s => s.NumberOfShards(5).NumberOfReplicas(1))
                        .Mappings(ms => ms
                            .Map<User>(m => m
                                .AutoMap()
                                .Properties(ps => ps
                                    .Nested<Book>(n => n
                                        .Name(c => c.Books)))));
                elastic.CreateIndex(descriptor);
            }
        }

        public static void Insert(User user)
        {
            ElasticClient elastic = GetElasticClient(ElasticsearchConfiguration.UserIndex);
            IIndexResponse bulkIndexResponse = elastic.Index(user, p => p.Type(typeof(User)).Id(user.Id).Refresh(null));
        }

        public static void Insert(List<Novel> list)
        {
            ElasticClient elastic = GetElasticClient();
            var Descriptor = new BulkDescriptor();
            Descriptor.CreateMany(list);
            var result = elastic.Bulk(Descriptor);
        }

        public static void Insert(Novel novel)
        {
            ElasticClient elastic = GetElasticClient();
            IIndexResponse bulkIndexResponse = elastic.Index(novel, p => p.Type(typeof(Novel)).Id(novel.Id).Refresh(null));
        }

        public static List<Novel> Query(string queryStr)
        {
            var searchResponse = GetElasticClient().Search<Novel>(es => es.Query(q =>
                q.QueryString(qs => qs.Query(queryStr))));

            return searchResponse.Documents.ToList();
        }

        public static List<Novel> Query(string queryStr, int count)
        {
            var searchResponse = GetElasticClient().Search<Novel>(es => es.Query(q =>
                q.QueryString(qs => qs.Query(queryStr))).Size(count));

            return searchResponse.Documents.ToList();
        }

        public static List<User> QueryUser(string queryStr, int count = 5)
        {
            var searchResponse = GetElasticClient(ElasticsearchConfiguration.UserIndex).Search<User>(es => es.Query(q =>
                q.QueryString(qs => qs.Query(queryStr))).Size(count));

            return searchResponse.Documents.ToList();
        }

        public static void DeleteIndex()
        {
            ElasticClient elastic = GetElasticClient();
            elastic.DeleteIndex(new DeleteIndexDescriptor(ElasticsearchConfiguration.DefaultIndex).Index(ElasticsearchConfiguration.DefaultIndex));
        }
        public static void DeleteIndex(string userIndex)
        {
            ElasticClient elastic = GetElasticClient();
            elastic.DeleteIndex(new DeleteIndexDescriptor(ElasticsearchConfiguration.DefaultIndex).Index(userIndex));
        }
    }
}
