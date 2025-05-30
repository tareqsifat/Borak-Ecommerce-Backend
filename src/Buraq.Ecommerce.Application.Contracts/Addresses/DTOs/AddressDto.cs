using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.Addresses.DTOs
{
    public class AddressDto : FullAuditedEntityDto<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }

}
