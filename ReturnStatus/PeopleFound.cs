using System;
using Models;
using System.Collections.Generic;

namespace ReturnStatus
{
    public class PeopleFound
    {
        public bool Status { get; set; }
        public List<Person> People { get; set; }
        public string Message { get; set; }
    }
}
