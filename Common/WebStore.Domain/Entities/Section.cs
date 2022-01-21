

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Section : NamedEntity, IOrderEntity
    {
        public int Order { get; set; }

        public int? ParentId { get; set; } = null;// если внешний ключ допускает значение null, то навигационное свойство опциональьно. Если нет - обязательно

        [ForeignKey(nameof(ParentId))]//атрибут, указавающий внешний ключ навигационного свойства
        public Section Parent { get; set; } //Навигационное свойство

        public ICollection<Product> Products { get; set; }
    }
}
