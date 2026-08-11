using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.DTOs.Category.Query
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
