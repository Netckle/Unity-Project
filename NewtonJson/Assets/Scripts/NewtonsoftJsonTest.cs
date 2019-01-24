using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Product
{
    public string Name;
    public string ExpiryDate;
    public decimal Price;
    public string[] Sizes;    
}

public class Woman
{
    public string name;
    public int age;
}

public class NewtonsoftJsonTest : MonoBehaviour
{
    private Woman[] womanList = 
    {
        new Woman() {name = "아라", age = 24},
        new Woman() {name = "도희", age = 20},
        new Woman() {name = "현아", age = 22},
    };

    private Product product = new Product();
    private string output;

    public string DateTime(int year, int month, int day)
    {
        string dateTime = year + "-" + month + "-" + day +"T00:00:00"; 
        return dateTime;    
    }

    void Start()
    {      
        var Women = 
            from woman in womanList
            where woman.age > 20
            orderby woman.age ascending
            select woman;

        foreach(var woman in Women)
        {
            Debug.Log(woman.name + " : " + woman.age);
        }

        product.Name = "Apple";
        product.ExpiryDate = DateTime(2008, 12, 28);
        product.Price = 3.99M;
        product.Sizes = new string[] {"Small", "Medium", "Large"};

        output = JsonConvert.SerializeObject(product);

        Debug.Log(output);
    }
}