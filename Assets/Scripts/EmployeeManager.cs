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
        TextAsset txt = Resources.Load("NomesFemininos") as TextAsset;
        string[] feminineNames = txt.ToString().Replace("Srta. ", "").Replace("Sra. ", "").Replace("Dra. ", "").Split("\n");
        txt = Resources.Load("NomesMasculinos") as TextAsset;
        string[] masculineNames = txt.ToString().Replace("Sr. ", "").Replace("Dr. ", "").Split("\n");
        txt = Resources.Load("Vagas") as TextAsset;
        string[] vagas = txt.ToString().Split("\n");

        int[] exp = new int[] { 0, 1, 2 };

        List<ContractManager.Vacancy> vacancy = new List<ContractManager.Vacancy>();

        for (int i = 0; i < vagas.Length; i++)
        {
            string[] vaga = vagas[i].Split(",");

            ContractManager.Vacancy v = new ContractManager.Vacancy();
            v.name = vaga[0];
            v.min = int.Parse(vaga[1]);
            v.max = int.Parse(vaga[2]);
            vacancy.Add(v);


        }


        int salaryMax = 20000;

        while(emp.Count > 0)
        {
            float gender = (UnityEngine.Random.value );
            emp[0].name = (gender < 50f) ? feminineNames[UnityEngine.Random.Range(0, feminineNames.Length)] : masculineNames[UnityEngine.Random.Range(0, masculineNames.Length)];
            
            ContractManager.Vacancy v = vacancy[UnityEngine.Random.Range(0, vacancy.Count)];

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
    public int experience;
    public int salary;
}
