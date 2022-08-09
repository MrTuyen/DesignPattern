using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeParttern
{
	class Program
	{
		static void Main(string[] args)
		{
			Employee CEO = new Employee("John", "CEO", 30000);

			Employee headSales = new Employee("Robert", "Head Sales", 20000);

			Employee headMarketing = new Employee("Michel", "Head Marketing", 20000);

			Employee clerk1 = new Employee("Laura", "Marketing", 10000);
			Employee clerk2 = new Employee("Bob", "Marketing", 10000);

			Employee salesExecutive1 = new Employee("Richard", "Sales", 10000);
			Employee salesExecutive2 = new Employee("Rob", "Sales", 10000);

			CEO.add(headSales);
			CEO.add(headMarketing);

			headSales.add(salesExecutive1);
			headSales.add(salesExecutive2);

			headMarketing.add(clerk1);
			headMarketing.add(clerk2);

			//print all employees of the organization
			Console.WriteLine("Manager: " + CEO.dept + "-" + CEO.toString());

			foreach (Employee headEmployee in CEO.getSubordinates())
			{
				Console.WriteLine(headEmployee.dept + ": " + headEmployee.toString());
				foreach (Employee employee in headEmployee.getSubordinates())
				{
					Console.WriteLine(employee.toString());
				}
			}

			Console.ReadLine();
		}
	}

	public class Employee
	{
		public String name {get; set;}
		public String dept {get; set;}
		public int salary {get; set;}
		public List<Employee> subordinates {get; set;}

		// constructor
		public Employee(String name, String dept, int sal)
		{
			this.name = name;
			this.dept = dept;
			this.salary = sal;
			subordinates = new List<Employee>();
		}

		public void add(Employee e)
		{
			subordinates.Add(e);
		}

		public void remove(Employee e)
		{
			subordinates.Remove(e);
		}

		public List<Employee> getSubordinates()
		{
			return subordinates;
		}

		public String toString()
		{
			return ("Employee :[ Name : " + name + ", dept : " + dept + ", salary :" + salary + " ]");
		}
	}

	abstract class _Employee
	{
		String name;
		String dept;
		String salary;
		List<_Employee> subordinates;

		void add(_Employee e) {

		}
		void remove(_Employee e) { 

		}
		List<_Employee> getSubordinates(){
			return null;
		}
		String toString() {
			return null;
		}
	}
}
