using Microsoft.CodeAnalysis;
using System;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Service
{
    public interface IUserPermission
    {
        Boolean CanEditNote(Note note);
    }
}

