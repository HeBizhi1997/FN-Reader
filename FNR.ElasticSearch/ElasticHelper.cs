using Elasticsearch.Net;
using FNR.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNR.ElasticSearch
{
    public class ElasticHelper
    {
        private static readonly string indexName = "fnreadertest001";

        public static ElasticClient GetElasticClient()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node).DefaultIndex(indexName);

            return new ElasticClient(settings);
        }

        public static void CreateIndex()
        {
            ElasticClient elastic = GetElasticClient();
            if (!elastic.IndexExists(indexName).Exists)
            {
                var descriptor=new CreateIndexDescriptor(indexName)
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
            elastic.DeleteIndex(new DeleteIndexDescriptor(indexName).AllIndices());
        }
    }
}
