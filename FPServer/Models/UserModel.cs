using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPServer.Models
{
    [Table(name: "Users")]
    public class UserModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 明文
        /// </summary>
        [Key]
        public string LID { get; set; }

        /// <summary>
        /// SHA256+AES128 加密方式
        /// </summary>
        public string PWD { get; set; }

        /// <summary>
        /// 额外滑动字段
        /// </summary>
        public string EXT { get; set; }


    }




}
