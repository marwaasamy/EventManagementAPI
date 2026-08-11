using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.DTOs.Category.Command
{
    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
