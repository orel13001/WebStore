

using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public SectionsViewComponent (IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke()
        {
            var sections = _productData.GetSections();

            var parent_sections = sections.Where(o => o.ParentId is null);

            var parent_sections_view = parent_sections.Select(o => new SectionViewModel()
            {
                Id = o.Id,
                Name = o.Name,
                Order = o.Order,
            }).ToList();

            foreach (var parent_section in parent_sections_view)
            {
                var childs = sections.Where(o => o.ParentId == parent_section.Id);

                foreach (var child_section in childs)
                {
                    parent_section.ChildSections.Add(new SectionViewModel
                    {
                        Id = child_section.Id,
                        Name = child_section.Name,
                        Order = child_section.Order,
                        Parent = parent_section,
                    });

                }
                
                parent_section.ChildSections.Sort((a,b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            }
            
            parent_sections_view.Sort((a, b) => Comparer<int>.Default.Compare(a.Order, b.Order));

            return View(parent_sections_view); 
        }
    }
}
