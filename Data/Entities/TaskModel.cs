using System;

namespace RSAHyundai.Data.Entities
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Todo { get; set; }

        public ProjectModel Project { get; set; }
    }
}
