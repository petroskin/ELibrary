﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ELibrary.Domain.Models
{
    public class EmailMessage : BaseEntity
    {
        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Boolean Status { get; set; }
    }
}
