using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour
{

    [SerializeField] private List<Employee> startEmployees;

    [SerializeField] private List<Employee> employees = new List<Employee>();

    private void Start()
    {
        GenerateEmployees(startEmployees);
    }

    public void GenerateEmployees(List<Employee> emp)
    {
        int salaryMax = 20000;

        while(emp.Count > 0)
        {
            float gender = (UnityEngine.Random.value * 100f);
            emp[0].name = (gender < 50f) ? InformationDatabase.i.feminineNames[UnityEngine.Random.Range(0, InformationDatabase.i.feminineNames.Length)] : InformationDatabase.i.masculineNames[UnityEngine.Random.Range(0, InformationDatabase.i.masculineNames.Length)];

            InformationDatabase.Vacancy v = InformationDatabase.i.vacancyList[UnityEngine.Random.Range(0, InformationDatabase.i.vacancyList.Count)];

            string removed = v.name.Replace("(a)", "");

            string femaleSimple = removed + "a";
            string femaleComplex = removed.Remove(removed.Length - 1, 1) + "a";

            string correct = removed;
            if(v.name.Contains("(a)") && gender < 50f)
            {
                if(removed[removed.Length - 1] == 'o')
                {
                    correct = femaleComplex;
                }else{
                    correct = femaleSimple;
                }   
            }

            emp[0].cargo = correct;
            emp[0].cargoID = v.id;

            int experience = emp[0].experience;
            int salary = emp[0].salary;

            if(salary > v.min && salary < v.max )
            {

                if(SalarySum(salary) < salaryMax)
                {
                    employees.Add(emp[0]);
                    emp.RemoveAt(0);
                }else{
                    emp.AddRange(employees);
                    employees.Clear();
                }
            }
        }

    }

    public int SalarySum(int salary)
    {
        int sum = salary;

        if(employees.Count > 0)
            for (int i = 0; i < employees.Count; i++)
            {
                sum += employees[i].salary;
            }

        return sum;
    }
}


[System.Serializable]
public class Employee
{
    public string name;
    public string cargo;
    public int cargoID;
    public int experience;
    public int salary;
}
