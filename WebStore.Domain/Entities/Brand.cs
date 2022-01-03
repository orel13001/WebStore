

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    [Table("Brands")] //свойство задаёт имя таблицы 
    [Index(nameof(Name), IsUnique = true)]
    public class Brand : NamedEntity, IOrderEntity
    {
        [Column("BrandOrder")]//свойство задаёт имя столбца
        public int Order { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
