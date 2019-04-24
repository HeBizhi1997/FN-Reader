using System;

namespace FNR.ElasticSearch
{
    public static class ElasticsearchConfiguration
    {
        public static string Host
        {
            get { return "http://localhost"; }
        }
        public static long Port
        {
            get { return 9200; }
        }
        public static Uri Connection
        {
            get { return new Uri(string.Format("{0}:{1}", Host, Port)); }
        }
        public static string DefaultIndex
        {
            get { return "fnreadertest002"; }
        }

        public static string UserIndex
        {
            get { return "fnreadertestuser002"; }
        }
    }
}
