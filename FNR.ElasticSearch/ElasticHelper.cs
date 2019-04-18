using Elasticsearch.Net;
using FNR.DataStructure;
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
                var createIndexResponse = elastic.CreateIndex(indexName);
                var mappingBlogPost = elastic.Map<Novel>(s => s.AutoMap());
            }
            //var indexExist = elastic.IndexExists(indexName);
            //if (!indexExist.Exists)
            //{
            //    //基本配置                
            //    IIndexState indexState = new IndexState()
            //    {
            //        Settings = new IndexSettings()
            //        {
            //            NumberOfReplicas = 1,//副本数               
            //            NumberOfShards = 6//分片数                
            //        }
            //    };
            //    //ICreateIndexResponse response = client.CreateIndex(IndexName, p => p.Mappings(m => m.Map<ES_PUB_Stock>(mp => mp.AutoMap())));  
            //    ICreateIndexResponse response = elastic.CreateIndex(indexName, p => p
            //    .InitializeUsing(indexState)
            //        .Mappings(ms =>
            //            ms.Map<Novel>(m =>
            //                m.AutoMap()
            //                    .Properties(ps =>
            //                        ps.Nested<Section>(n =>
            //                            n.Name(c => c.Sections))))));
            //    if (response.IsValid)
            //    {
            //        string msg = string.Format("索引创建成功！");
            //        Console.WriteLine(msg);
            //    }
            //    else
            //    {
            //        string msg = string.Format("索引创建失败！");
            //        Console.WriteLine(msg);
            //    }
            //}

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
    }
}
