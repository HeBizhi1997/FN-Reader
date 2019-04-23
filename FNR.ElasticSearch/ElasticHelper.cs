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

        public static void CreateIndex()
        {
            ElasticClient elastic = GetElasticClient();
            if (!elastic.IndexExists(ElasticsearchConfiguration.DefaultIndex).Exists)
            {
                var descriptor=new CreateIndexDescriptor(ElasticsearchConfiguration.DefaultIndex)
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

        public static void DeleteIndex()
        {
            ElasticClient elastic = GetElasticClient();
            elastic.DeleteIndex(new DeleteIndexDescriptor(ElasticsearchConfiguration.DefaultIndex).AllIndices());
        }
    }
}
