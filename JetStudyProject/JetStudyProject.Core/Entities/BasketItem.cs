using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Core.Entities
{
    public class BasketItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
