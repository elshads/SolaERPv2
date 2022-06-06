namespace SolaERPv2.Server.Models;

public class Unit : BaseModel
{
    public int UnitId { get; set; }
    public int BusinessUnitId { get; set; }


//public class Person
//{
//    public int Id { get; set; }
//    public string? Name { get; set; }
//    public List<Phone> PhoneList { get; set; }
//    public List<Email> EmailList { get; set; }


//    public async Task<IEnumerable<Person>> GetAll()
//    {
//        using IDbConnection cn = new SqlConnection("ConnectionString");
//        var sqlPerson = "SELECT * FROM Person";
//        var personList = await cn.QueryAsync<Person>(sqlPerson);

//        if (personList != null && personList.Any())
//        {
//            var tempList = new List<Person>();

//            foreach (var person in personList)
//            {
//                var sqlPhone = $"SELECT * FROM PersonPhone WHERE PersonId = {person.Id}";
//                var phoneList = await cn.QueryAsync<Phone>(sqlPhone);
//                person.PhoneList = phoneList.ToList();

//                var sqlEmail = $"SELECT * FROM PersonEmail WHERE PersonId = {person.Id}";
//                var emailList = await cn.QueryAsync<Email>(sqlEmail);
//                person.EmailList = emailList.ToList();

//                tempList.Add(person);
//            }

//            personList = tempList;
//        }

//        return personList;
//    }
//}

//public class Phone
//{
//    public int Id { get; set; }
//    public string? Number { get; set; }
//}

//public class Email
//{
//    public int Id { get; set; }
//    public string? EmailAddress { get; set; }
//}
}
