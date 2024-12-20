﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeSalary.Application.Models
{
    public class SalaryLogDTO
    {
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public DateTime PaymentDate { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
