using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour
{
    [SerializeField]private int startEmployees;
    [SerializeField] private List<Employee> employees = new List<Employee>();

    private void Start()
    {
        GenerateEmployees(startEmployees);
    }

    public void GenerateEmployees(int qtd)
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


        int salaryMax = 5000;

        while (employees.Count < qtd)
        {
            Employee emp = new Employee();

            float gender = (UnityEngine.Random.value * 100);
            emp.name = (gender < 50f) ? feminineNames[UnityEngine.Random.Range(0, feminineNames.Length)] : masculineNames[UnityEngine.Random.Range(0, masculineNames.Length)];

            ContractManager.Vacancy v = vacancy[UnityEngine.Random.Range(0, vacancy.Count)];

            int experience = UnityEngine.Random.Range(0, exp.Length);
            int salary = (100 * (int)Mathf.Round((UnityEngine.Random.Range(v.min, v.max) + ((experience + 1) * 550) - 550) / 100.0f));

            emp.experience = experience;
            emp.salary = salary;

            if (SalarySum(salary) < salaryMax)
            {
                employees.Add(emp);
            }
        }
    }

    public int SalarySum(int salary)
    {
        int sum = salary;

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
    public int experience;
    public int salary;
}
