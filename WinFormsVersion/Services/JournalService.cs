using Microsoft.Data.Sqlite;
using System.Data.SQLite;
public class JournalService
{
    private readonly string dbPath = "Data Source=journal.db;Version=3;";

    public JournalService()
    {
        using var conn = new System.Data.SQLite.SQLiteConnection(dbPath);
        conn.Open();

        string sql = @"CREATE TABLE IF NOT EXISTS Journal (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT,
            Content TEXT,
            PrimaryMood TEXT,
            SecondaryMood1 TEXT,
            SecondaryMood2 TEXT,
            Category TEXT,
            Tags TEXT,
            CreatedAt TEXT,
            UpdatedAt TEXT
        )";

        new SQLiteCommand(sql, conn).ExecuteNonQuery();
    }

    public void AddEntry(JournalEntry e)
    {
        using var conn = new SQLiteConnection(dbPath);
        conn.Open();

        string sql = @"INSERT INTO Journal 
        (Title, Content, PrimaryMood, SecondaryMood1, SecondaryMood2, Category, Tags, CreatedAt, UpdatedAt)
        VALUES (@t,@c,@pm,@s1,@s2,@cat,@tags,@ca,@ua)";

        var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@t", e.Title);
        cmd.Parameters.AddWithValue("@c", e.Content);
        cmd.Parameters.AddWithValue("@pm", e.PrimaryMood);
        cmd.Parameters.AddWithValue("@s1", e.SecondaryMood1);
        cmd.Parameters.AddWithValue("@s2", e.SecondaryMood2);
        cmd.Parameters.AddWithValue("@cat", e.Category);
        cmd.Parameters.AddWithValue("@tags", e.Tags);
        cmd.Parameters.AddWithValue("@ca", e.CreatedAt);
        cmd.Parameters.AddWithValue("@ua", e.UpdatedAt);

        cmd.ExecuteNonQuery();
    }

    public void UpdateEntry(JournalEntry e)
    {
        using var conn = new SQLiteConnection(dbPath);
        conn.Open();

        string sql = @"UPDATE Journal SET
            Title=@t, Content=@c, PrimaryMood=@pm,
            SecondaryMood1=@s1, SecondaryMood2=@s2,
            Category=@cat, Tags=@tags, UpdatedAt=@ua
            WHERE Id=@id";

        var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", e.Id);
        cmd.Parameters.AddWithValue("@t", e.Title);
        cmd.Parameters.AddWithValue("@c", e.Content);
        cmd.Parameters.AddWithValue("@pm", e.PrimaryMood);
        cmd.Parameters.AddWithValue("@s1", e.SecondaryMood1);
        cmd.Parameters.AddWithValue("@s2", e.SecondaryMood2);
        cmd.Parameters.AddWithValue("@cat", e.Category);
        cmd.Parameters.AddWithValue("@tags", e.Tags);
        cmd.Parameters.AddWithValue("@ua", DateTime.Now);

        cmd.ExecuteNonQuery();
    }

    public void DeleteEntry(int id)
    {
        using var conn = new SQLiteConnection(dbPath);
        conn.Open();

        new SQLiteCommand("DELETE FROM Journal WHERE Id=@id", conn)
        { Parameters = { new SQLiteParameter("@id", id) } }
        .ExecuteNonQuery();
    }

    public List<JournalEntry> GetAllEntries()
    {
        var list = new List<JournalEntry>();
        using var conn = new SQLiteConnection(dbPath);
        conn.Open();

        var cmd = new SQLiteCommand("SELECT * FROM Journal ORDER BY CreatedAt DESC", conn);
        using var r = cmd.ExecuteReader();

        while (r.Read())
        {
            list.Add(new JournalEntry
            {
                Id = Convert.ToInt32(r["Id"]),
                Title = r["Title"].ToString(),
                Content = r["Content"].ToString(),
                PrimaryMood = r["PrimaryMood"].ToString(),
                SecondaryMood1 = r["SecondaryMood1"].ToString(),
                SecondaryMood2 = r["SecondaryMood2"].ToString(),
                Category = r["Category"].ToString(),
                Tags = r["Tags"].ToString(),
                CreatedAt = DateTime.Parse(r["CreatedAt"].ToString()),
                UpdatedAt = DateTime.Parse(r["UpdatedAt"].ToString())
            });
        }
        return list;
    }
}

