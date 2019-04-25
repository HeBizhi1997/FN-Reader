using System.Collections.Generic;
using Nest;

namespace FNR.Model
{
    [ElasticsearchType(IdProperty = "id", Name = "user")]
    public class User
    {
        [Text(Name = "id", Index = true)]
        public int Id { get; set; }

        [Text(Name = "name", Index = true, Analyzer = "ik_smart")]
        public string Name { get; set; }

        [Text(Name = "gender", Index = true)]
        public int Gender { get; set; }  //男0 女1

        //[Text(Name = "avatar", Index = true)]
        //public string Avatar { get; set; }  //头像

        [Text(Name = "level", Index = true)]
        public int Level { get; set; }  //权限

        [Text(Name = "password", Index = true)]
        public string Password { get; set; }

        [Text(Name = "books", Index = true)]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
