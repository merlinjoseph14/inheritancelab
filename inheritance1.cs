using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    public abstract class Employee
    {
        static void Main(string[] args)
        {
            EmployeeManager manager = new EmployeeManager();

            manager.LoadEmployees("employees.txt");

            Console.WriteLine($"Average Weekly Pay: {manager.CalculateAverageWeeklyPay()}");

            var (highestPayName, highestPay) = manager.FindHighestWageEmployeePay();
            Console.WriteLine($"Highest Wage Pay: {highestPay}, Employee Name: {highestPayName}");

            var (lowestSalaryName, lowestSalary) = manager.FindLowestSalariedEmployeeSalary();
            Console.WriteLine($"Lowest Salary: {lowestSalary}, Employee Name: {lowestSalaryName}");

            var percentages = manager.CalculateEmployeeCategoryPercentages();
            foreach (var category in percentages)
            {
                Console.WriteLine($"{category.Key} Employees: {category.Value}%");
            }
        }

        public class EmployeeManager
        {
            private List employees = new List();

            public void LoadEmployees(string filePath)
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var details = line.Split(':');
                    string id = details[0].Trim();
                    string name = details[1].Trim();
                    string address = details[2].Trim();
                    string phone = details[3].Trim();
                    long sin = long.Parse(details[4].Trim());
                    string dob = details[5].Trim();
                    string dept = details[6].Trim();
                    double rateOrSalary = double.Parse(details[7].Trim(), CultureInfo.InvariantCulture);
                    double hours = details.Length > 8 ? double.Parse(details[8].Trim(), CultureInfo.InvariantCulture) : 0;

                    if (id.StartsWith("0") || id.StartsWith("1") || id.StartsWith("2") || id.StartsWith("3") || id.StartsWith("4"))
                    {
                        employees.Add(new Salaried(id, name, address, phone, sin, dob, dept, rateOrSalary));
                    }
                    else if (id.StartsWith("8") || id.StartsWith("9"))
                    {
                        employees.Add(new PartTime(id, name, address, phone, sin, dob, dept, rateOrSalary, hours));
                    }
                    else
                    {
                        employees.Add(new Wages(id, name, address, phone, sin, dob, dept, rateOrSalary, hours));
                    }
                }
            }
            public double CalculateAverageWeeklyPay()
            {
                double totalPay = employees.Sum(employee => employee.GetPay()); 
                return totalPay / employees.Count;
            }

            public (string, double) FindHighestWageEmployeePay() 
            {
                var wageEmployees = employees.OfType(); 
                var highestPaid = wageEmployees.OrderByDescending(emp => emp.GetPay()).FirstOrDefault(); 
                return (highestPaid?.Name, highestPaid?.GetPay() ?? 0);
            }

            public (string, double) FindLowestSalariedEmployeeSalary()
            {
                var salariedEmployees = employees.OfType();
                var lowestPaid = salariedEmployees.OrderBy(emp => emp.GetPay()).FirstOrDefault();
                return (lowestPaid?.Name, lowestPaid?.GetPay() ?? 0);
            }

            public Dictionary CalculateEmployeeCategoryPercentages()
            {
                int totalEmployees = employees.Count;
                var percentages = new Dictionary
                {
                     { "Salaried", employees.OfType().Count() * 100.0 / totalEmployees },
                     { "Wages", employees.OfType().Count() * 100.0 / totalEmployees },
                     { "PartTime", employees.OfType().Count() * 100.0 / totalEmployees }
                };

                return percentages;
            }
        }

        public class PartTime : Employee
        {
            private double rate;
            private double hours;

            public PartTime(string id, string name, string address, string phone, long sin, string dob, string dept, double rate, double hours)
            : base(id, name, address, phone, sin, dob, dept)
            {
                this.rate = rate;
                this.hours = hours;
            }

            public override double GetPay()
            {
                return rate * hours;
            }

            public override string ToString()
            {
                return $"ID: {Id}, Name: {Name}, Address: {Address}, Phone: {Phone}, SIN: {Sin}, DOB: {Dob}, Dept: {Dept}, Hourly Rate: {rate}, Hours Worked: {hours}";
            }

            public double Rate
            {
                get { return rate; }
                set { rate = value; }
            }

            public double Hours
            {
                get { return hours; }
                set { hours = value; }
            }
        }

        public class Salaried : Employee
        {
            private double salary;

            public Salaried(string id, string name, string address, string phone, long sin, string dob, string dept, double salary)
            : base(id, name, address, phone, sin, dob, dept)
            {
                this.salary = salary;
            }

            public override double GetPay()
            {
                return salary;
            }

            public override string ToString()
            {
                return $"ID: {Id}, Name: {Name}, Address: {Address}, Phone: {Phone}, SIN: {Sin}, DOB: {Dob}, Dept: {Dept}, Salary: {salary}";
            }

            public double Salary
            {
                get { return salary; }
                set { salary = value; }
            }
        }

        public class Wages : Employee
        {
            private double rate;
            private double hours;

            public Wages(string id, string name, string address, string phone, long sin, string dob, string dept, double rate, double hours)
            : base(id, name, address, phone, sin, dob, dept)
            {
                this.rate = rate;
                this.hours = hours;
            }

            public override double GetPay()
            {
                if (hours <= 40)
                {
                    return rate * hours;
                }
                else
                {
                    double regularPay = rate * 40;
                    double overtimePay = (hours - 40) * (rate * 1.5);
                    return regularPay + overtimePay;
                }
             }

            public override string ToString()
            {
                return $"ID: {Id}, Name: {Name}, Address: {Address}, Phone: {Phone}, SIN: {Sim}, DOB: {Dob}, Dept: {Dept}, Hourly Rate: {Rate}";
            }

            public double Rate
            {
                get { return rate; }
                set { rate = value; }
            }

            Console.ReadKey()
        }
}