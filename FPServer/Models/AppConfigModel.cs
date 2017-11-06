using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPServer.Models
{
    [Table(name: "AppConfig")]
    public class AppConfigModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        public string Key { get; set; }

        public string Value { get; set; }


    }
}
