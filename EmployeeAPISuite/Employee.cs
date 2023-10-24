using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QE_Tech_Chalange_API_Tests
{
    public class Employee
    {
        private static string[] first_names_ = {"John", "Patrick", "Albus", "Tony", "Han",
        "Lilo", "Hermoine", "Sarah", "Katniss", "James",
        "Stevie", "Valentino", "Max", "Marc", "Marco"  };

        private static string[] last_names_ = {"Rossi", "Marquez", "Dumbledore", "Stark", "Wick",
            "Verstapen", "Wonder", "Everdeen", "Bond", "Connor",
        "Granger", "Pelekai", "Simoncelli", "Bateman", "Solo" };

        private static string[] days_ = { "02/", "03/", "04/", "05/", "06/", "07/", "08/", "09/", "10/",
            "11/", "12/", "13/", "14/", "15/", "16/", "17/", "18/", "19/", "20/", "21/", "22/", "23/"
                , "24/", "25/", "26/", "27/", "28/" };

        private static string[] months_ = { "Jan/", "Feb/", "Mar/", "Apr/", "May/", "Jun/", "Jul/", "Aug/", "Sep/", "Oct/", "Nov/", "Dec/" };
        static string[] birth_year_ = { "1999", "1998", "1997", "1996", "1995", "1994", "1993", "1992", "1991", "1990", "1989", "1988", "1987", "1986", "1985" };

        private static string[] start_year_ = { "2023", "2022", "2021", "2020", "2019", "2018", "2017", "2016", "2015", "2014" };
        private static string[] department_types_ = { "Managment", "HelpDesk", "Sales", "Quality Engineering", "Development", "Escalations", "Human Resources" };
        private static string[,] job_titles_ = {
            {"CEO", "CFO", "CTO" },
            {"HelpDesk Level 1", "HelpDesk Level 2", "HelpDesk Team Lead" },
            {"Sale Consultant", "Sales Consultant", "Sales Team Lead" },
            {"QE Engineer", "Senior QE Engineer", "QE Team Lead" },
            {"Software Engineer", "Senior Software Engineer", "Lead Engineer" },
            {"Junior Escalations Analyst", "Escalations Analyst", "Senior Escalations Analyst"},
            {"HR Consultant", "Talent Aquisition Specialist", "HR Team Lead" }
        };
        private static string[] address_steet = { "Stacey street, 1234 NSW", "Sessame Street, 4321 NSW", "Hiremeplz Avenue, 7654 NSW",
            "Sunnyvale Road, 8976 NSW", "Shaw Street, 5533 NSW", "Fifth Avenue, 9901 NSW", "Fourth Street, 2001 NSW",
            "Little Road, 3003 NSW", "Big Street, 0098 NSW", "Pickett Cressent, 8780 NSW" };


        //FirstName, LastName, DateOfBirth, StartDate, Department, JobTitle, Email, Mobile, Address & BaseSalary

        public string first_name_ { get;  set; }
        public string last_name_ { get;  set; }
        public string date_of_birth_ { get;  set; }
        public string start_date_ { get;  set; }
        public string department_ { get;  set; }
        public string job_title_ { get;  set; }
        public string email_ { get;  set; }
        public string mobile_ { get;  set; }
        public string address_ { get;  set; }
        public string base_salary_ { get;  set; }


        private static string GetRandomFirstName()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int first_name_count = first_names_.Length;
            return first_names_[rand.Next(first_name_count)];
        }

        private static string GetRandomLastName()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int last_name_count = last_names_.Length;
            return last_names_[rand.Next(last_name_count)];
        }

        private static string GetRandomDepartment()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int department_count = department_types_.Length;
            return department_types_[rand.Next(department_count)];
        }

        private static string GetRandomRoleForDepartment(string department_)
        {
            int department_index = 0;
            for (int i = 0; i < department_types_.Length; ++i)
            {
                if (department_.Equals(department_types_[i]))
                {
                    department_index = i;
                    break;
                }
            }
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return job_titles_[department_index, rand.Next(3)];
        }

        private static string GetRandomDateOfBirth()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string birth_day = days_[rand.Next(days_.Length)];
            string birth_month = months_[rand.Next(months_.Length)];
            string birth_year = birth_year_[rand.Next(birth_year_.Length)];
            return string.Concat(birth_day, birth_month, birth_year);
        }

        private static string GetRandomStartDate()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string start_day = days_[rand.Next(days_.Length)];
            string start_month = months_[rand.Next(months_.Length)];
            string start_year = start_year_[rand.Next(start_year_.Length)];
            return string.Concat(start_day, start_month, start_year);
        }

        private static string MakeEmailAddress(string first_name_, string last_name_)
        {
            return string.Concat(first_name_, ".", last_name_, "@fictionalcompany.com.au");
        }

        private static string GeneratePhoneNumber()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int number = rand.Next(10000000, 100000000);
            return string.Concat("04", number.ToString());
        }

        private static string GetRandomAddress()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int street_number = rand.Next(400);
            return string.Concat(street_number.ToString(), " ", address_steet[rand.Next(address_steet.Length)]);
        }

        private static string GenerateBaseSalary()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return string.Concat("$", rand.Next(50000, 180001).ToString());
        }

        public Employee()
        {
            this.first_name_ = GetRandomFirstName();
            this.last_name_ = GetRandomLastName();
            this.department_ = GetRandomDepartment();
            this.job_title_ = GetRandomRoleForDepartment(this.department_);
            this.date_of_birth_ = GetRandomDateOfBirth();
            this.start_date_ = GetRandomStartDate();
            this.email_ = MakeEmailAddress(this.first_name_, this.last_name_);
            this.mobile_ = GeneratePhoneNumber();
            this.base_salary_ = GenerateBaseSalary();
            this.address_ = GetRandomAddress();
        }

        public Employee(JObject employee_json_)
        {
            this.mobile_ = (string)employee_json_["Employee"]["Mobile"];
            this.email_ = (string)employee_json_["Employee"]["Email"];
            this.first_name_ = (string)employee_json_["Employee"]["FirstName"];
            this.last_name_ = (string)employee_json_["Employee"]["LastName"];
            this.job_title_ = (string)employee_json_["Employee"]["JobTitle"];
            this.department_ = (string)employee_json_["Employee"]["Department"];
            this.address_ = (string)employee_json_["Employee"]["Address"];
            this.base_salary_ = (string)employee_json_["Employee"]["BaseSalary"];
            this.start_date_ = (string)employee_json_["Employee"]["StartDate"];
            this.date_of_birth_ = (string)employee_json_["Employee"]["DateOfBirth"];
        }

        public string ConvertEmployeeToJsonString()
        {

            //FirstName, LastName, DateOfBirth, StartDate, Department, JobTitle, Email, Mobile, Address & BaseSalary
            JObject json_employee = new JObject
            (
                    new JProperty("Employee",
                        new JObject
                        (
                            new JProperty("FirstName", this.first_name_),
                            new JProperty("LastName", this.last_name_),
                            new JProperty("DateOfBirth", this.date_of_birth_),
                            new JProperty("StartDate", this.start_date_),
                            new JProperty("Department", this.department_),
                            new JProperty("JobTitle", this.job_title_),
                            new JProperty("Email", this.email_),
                            new JProperty("Mobile", this.mobile_),
                            new JProperty("Address", this.address_),
                            new JProperty("BaseSalary", this.base_salary_)

                        )
                    )
                );
            return json_employee.ToString();
        }

        public static bool Equals(Employee employee1_, Employee employee2_)
        {
            if
            (
                employee1_.first_name_ == employee2_.first_name_ &&
                employee1_.last_name_ == employee2_.last_name_ &&
                employee1_.date_of_birth_ == employee2_.date_of_birth_ &&
                employee1_.start_date_ == employee2_.start_date_ &&
                employee1_.base_salary_ == employee2_.base_salary_ &&
                employee1_.mobile_ == employee2_.mobile_ &&
                employee1_.email_ == employee2_.email_ &&
                employee1_.department_ == employee2_.department_ &&
                employee1_.job_title_ == employee2_.job_title_ &&
                employee1_.address_ == employee2_.address_

            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}