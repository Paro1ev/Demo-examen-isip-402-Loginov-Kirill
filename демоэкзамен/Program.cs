var builder = WebApplication.CreateBuilder();
var app = builder.Build();


List<order> repo = new List<order>()
{
    new order(1, "Логинов Кирилл Денисович", 89521444789, "Хочу клининг", "Гагаринский дом 3", 52, "Не проживает", 2021, 3, 15, "Андрей Гобзавр"),
};

bool isUpdatedStatus = false;
string massage = "";

app.MapGet("/", () =>
 {
    if (isUpdatedStatus)
    {
        string buffer = massage;
        isUpdatedStatus = false;
        massage = "";
        return Results.Json(new OrderUpdateStatusDTO(repo, buffer));
    }
    else
        return Results.Json(repo);
});
app.MapPost("/",(order o) => repo.Add(o));
app.MapGet("/ {number}", (int number) => repo.Find(o => o.Number == number));
app.Run();
app.MapPut("/{number}", (int number, OrderUpdateDTO dto) =>
{
  
    order buffer = repo.Find(o => o.Number == number);
    if (buffer.Admin != dto.Admin)
        buffer.Admin = dto.Admin;

    if (buffer.StartDate != dto.StartDate)
        buffer.StartDate = dto.StartDate;

    if (buffer.Wishes != dto.Wishes)
        buffer.Wishes = dto.Wishes;

    if (buffer.EndDate != dto.EndDate)
        buffer.EndDate = dto.EndDate;

    {
        buffer.EndDate = dto.EndDate;
        isUpdatedStatus = true;
        massage += "дата выезда" + buffer.Number + "Изменён\n";
        if (buffer.Status == "завершено")
            buffer.EndDate = DateTime.Now;
    }
});

record class OrderUpdateDTO(DateTime StartDate, string Wishes, DateTime EndDate, string Admin);
record class OrderUpdateStatusDTO(List<order> repo, string massage);
public class order
{
    int number;
    string fio;
    long phonenumber;
    string wishes;
    string hoteladdres;
    int appartmentnumber;
    string status;
    string admin;

    public order(int number, string fio, long phonenumber, string wishes, string hoteladdres, int appartmentnumber, string status, int year, int month, int day, string master)
    {
        Number = number;
        Fio = fio;
        Phonenumber = phonenumber;
        Wishes = wishes;
        Hoteladdres = hoteladdres;
        Appartmentnumber = appartmentnumber;
        StartDate = new DateTime(year, month, day);
        EndDate = null;
        Status = status;
        Admin = admin;
    }

    public int Number { get => number; set => number = value; }
    public string Fio { get => fio; set => fio = value; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public long Phonenumber { get => phonenumber; set => phonenumber = value; }
    public string Wishes { get => wishes; set => wishes = value; }
    public string Hoteladdres { get => hoteladdres; set => hoteladdres = value; }
    public int Appartmentnumber { get => appartmentnumber; set => appartmentnumber = value; }
    public string Status { get => status; set => status = value; }
    public string Admin { get => admin; set => admin = value; }
    public List<string> Comments { get; set; } = [];


    public double TimeInDay() => (EndDate - StartDate).Value.TotalDays;
}