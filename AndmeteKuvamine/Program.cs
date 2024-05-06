using System.Data.SQLite;

ReadData(CreateConnection()); //kutsub selle funktsiooni välja

static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data Source=mydb.db; Version = 3; New = True; Compress = True;");

    try
    {
        connection.Open();
        Console.WriteLine("DB found.");
    }
    catch //catch tähendab, et try ei õnnestunud
    {
        Console.WriteLine("DB not found.");
    }
    return connection;
}


static void ReadData(SQLiteConnection myConnection)
{
    Console.Clear(); //puhastame konsooli
    SQLiteDataReader reader; //loome objekti, kuhu hakkame andmeid salvestama
    SQLiteCommand command; //see on päring andmebaasist

    command = myConnection.CreateCommand(); //tehakse ühendus lahti
    command.CommandText = "SELECT customer.firstName, customer.lastName, status.statusType FROM customerStatus " +
        "JOIN customer ON customer.rowid = customerStatus.customerId " +
        "JOIN status ON status.rowid = customerStatus.statusId " +
        "ORDER BY status.statustype"; //andmete järjestamine tähestiku järjekorras vastavalt staatusele
    //tühikud andme lõpus on olulised
    reader = command.ExecuteReader(); //pane see käsk käima ja hakka anmeid salvestama "kasti"

    while (reader.Read()) //ridade lugemine algues horisontaalselt lõpuni ja siis vertikaalselt järgmisele reale ja nii kuni vertikaalseslt on ka tabeli lõpus
    {
        string readerStringFristName = reader.GetString(0);
        string readerStringLastName = reader.GetString(1);
        string readerStringStatus = reader.GetString(2);

        Console.WriteLine($"Full name: {readerStringFristName} {readerStringLastName}; Status: {readerStringStatus}");
        //string readerStringDoB = reader.GetString(2);

        //Console.WriteLine($"Full name: {readerStringFristName} {readerStringLastName}; DoB {readerStringDoB}");
    }

    myConnection.Close();
    //ühendus tuleb kinni panna: ressursside kulu ja turvalisuse pärast
}
