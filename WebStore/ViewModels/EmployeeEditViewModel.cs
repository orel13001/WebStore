using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    /// <summary>
    /// ViewModel сотрудника - оболочка для действия "редактирование" сотрудника
    /// </summary>
    public class EmployeeEditViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(255,MinimumLength = 2,ErrorMessage = "Длинна должна быть от 2 до 255 символов")]
        [RegularExpression("([A-Z][a-z]+)|([А-ЯЁ][а-яё]+)", ErrorMessage ="Ошибка формата")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Длинна должна быть от 2 до 255 символов")]
        [RegularExpression("([A-Z][a-z]+)|([А-ЯЁ][а-яё]+)", ErrorMessage = "Ошибка формата")]
        public string LastName { get; set; }
        [StringLength(255, ErrorMessage = "Длинна должна быть от 2 до 255 символов")]
        [RegularExpression("(([A-Z][a-z]+)|([А-ЯЁ][а-яё]+))?", ErrorMessage = "Ошибка формата")]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Range(18,80,ErrorMessage ="Возраст должен быть от 18 до 80 лет")]
        public int Age { get; set; }
    }
}
