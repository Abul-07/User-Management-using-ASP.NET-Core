﻿namespace UserManagement.Core.Dtos.Log
{
    public class GetLogDto
    {
        public DateTime CreatedAt { get; set; }

        public string? UserName { get; set; }

        public string Description { get; set; }
    }
}
