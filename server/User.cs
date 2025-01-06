public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }  // לא מומלץ לאחסן סיסמאות בצורה כזו, יש להשתמש בהצפנה
}
