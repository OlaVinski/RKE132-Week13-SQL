using System.Data.SQLite;

ReadData(CreateConnection()); //kutsub selle funktsiooni välja
//InsertCustomer(CreateConnection());
//RemoveCutomer(CreateConnection());
FindCustomer(CreateConnection());
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
    command.CommandText = "SELECT rowid, * FROM customer";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        //string readerRowId = reader.GetString(0);
        string readerRowId = reader["rowid"].ToString(); //teine variant
        string readerStringFristName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringDoB = reader.GetString(3);

        Console.WriteLine($"{readerRowId}. Full name: {readerStringFristName} {readerStringLastName}; DoB: {readerStringDoB}");
    }

    myConnection.Close();
    //ühendus tuleb kinni panna: ressursside kulu ja turvalisuse pärast
}


static void InsertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName, dob; //kastikeste valmis panek vahemällu

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-yyyy):");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer(firstName, lastName, dateOfBirth) " +
        $"VALUES ('{fName}', '{lName}', '{dob}')";

    //peab olema veergude nimedega sama

    int rowInserted = command.ExecuteNonQuery(); //natuke teistmoodi käsk, mis lisab andmeid andmebaasi
    Console.WriteLine($"Row inserted: {rowInserted}");


    ReadData(myConnection);
}

static void RemoveCutomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;

    string idToDelete;
    Console.WriteLine("Enter an id to delete a customer:");
    idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    int rowRemoved = command.ExecuteNonQuery();
    Console.WriteLine($"{rowRemoved} was removed from the table customer.");

    ReadData(myConnection);
}


static void FindCustomer(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");
    searchName = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstName, customer.lastName, status.statusType " +
        $"FROM customerStatus " +
        $"JOIN customer ON customer.rowid = customerStatus.customerId " +
        $"JOIN status ON status.rowid = customerStatus.statusId " +
        $"WHERE firstname LIKE '{searchName}'";

    reader = command.ExecuteReader();

    while (reader.Read())
    {
        string readerRowId = reader["rowid"].ToString();
        string readerStringName = reader.GetString(1);
        string readerStringLastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);

        Console.WriteLine($"Search result: ID: {readerRowId}. {readerStringName} {readerStringLastName}. Status: {readerStringStatus}");
    }

myConnection.Close();
    //ühendus tuleb kinni panna: ressursside kulu ja turvalisuse pärast
}