using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities
{

    /// <summary>
    /// Сотрудник
    /// </summary>
    public class Employee : Entity
    {
        /// <summary>
        /// Фамиля
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string? Patronomic { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }
    }
}
