using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.DTOs.BasketDTOs
{
    public class BasketItemDto
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string ImageSrc { get; set; }
    }
}
