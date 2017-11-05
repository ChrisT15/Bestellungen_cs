using System;
using Gtk;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Bestellungen_cs
{
	public class Artikel_suchen_fenster
	{
		/* die Klasse Artikel_suchen_fenster beinhaltet ein Fenster, in das der Artikelname eines Artikels
		 * eingegeben werden kann. Wenn der Knopf "Suchen" gedrueckt wird, werden alle Artikel, die diesen
		 * Namen haben, in einer Datenbank gesucht */
		private Window fenster;

		//Knopf, Schriftzuege und Textfelder für das Fenster werden angelegt
		private Label ueberschrift_label;

		private Label artikelname_label;
		private Entry artikelname_text;



		private Button bestaetigung;
		private Label ergebnisse_label;
		private TextView ausgabe;
		private TextBuffer tb;

		//Konstruktor
		public Artikel_suchen_fenster ()
		{

			//Knopf, Beschriftungen und Textfelder werden zum Fenster hinzugefuegt
			this.fenster = new Window("Artikel suchen");
			this.fenster.Resize (500, 300);

			this.ueberschrift_label = new Label ("Artikeldaten eingeben:");

			this.artikelname_label = new Label ("Artikelname:");
			this.artikelname_text = new Entry ();

			this.ergebnisse_label = new Label("Suchergebnisse");
			this.ausgabe = new TextView();

			this.bestaetigung = new Button ("Suchen");
			/*Wenn der Knopf bestaetigung gedrueckt wird, dann wird die Funktion suchen 
			 * ausgefuehrt */
			bestaetigung.Clicked += new EventHandler(suchen);


			VBox vbox= new VBox ();

			vbox.Add (ueberschrift_label);
			vbox.Add (artikelname_label);
			vbox.Add (artikelname_text);
			vbox.Add (bestaetigung);
			vbox.Add (ergebnisse_label);
			vbox.Add (ausgabe);


			this.fenster.Add (vbox);

			this.fenster.ShowAll ();

		}

		private  void suchen(object sender, EventArgs e)
		{
			/* In dieser Funktion werden alle Artikel in einer Datenbank gesucht, welche den
			 * Artikelnamen haben, der in das Fenster eingegeben wurde */
			string aname = this.artikelname_text.Text;

			//Verbindung zu mysql wird hergestellt
			string connetionString = "server=localhost;uid=root;pwd=;database=Bestellungen;";
			MySqlConnection conn = new MySqlConnection(connetionString);

			try
			{
				conn.Open();

				MySqlCommand sup_query = conn.CreateCommand();

				/*Falls ein Artikelname eingegeben wurde, werden alle Artikel in der Tabelle 
				 * artikel_db gesucht, welche diesen Artikelnamen haben */

				if(!aname.Equals(""))
				{
					string sup = "select name,nummer,preis from artikel_db where name = ?aname";
					sup_query.CommandText = sup;
					sup_query.Parameters.AddWithValue("?aname", aname);


					MySqlDataReader reader = sup_query.ExecuteReader();

					string ergebnis="";
					while (reader.Read()) 
					{
						Artikel a = new Artikel(reader.GetString("name"),reader.GetString("nummer"),reader.GetString("preis"));
						ergebnis = ergebnis + a.to_string()  + System.Environment.NewLine;
					}
					//Suchergebnisse werden in das Textfeld ausgabe geschrieben
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis;

				}

				//Falls kein Artikelname eingegeben wurde, werden alle Artikel aus der Tabelle artikel_db ausgegeben
				else
				{
					string sup = "select name,nummer,preis from artikel_db";
					sup_query.CommandText = sup;


					MySqlDataReader reader = sup_query.ExecuteReader();

					string ergebnis="";
					while (reader.Read()) 
					{
						Artikel a = new Artikel(reader.GetString("name"),reader.GetString("nummer"),reader.GetString("preis"));
						ergebnis = ergebnis + a.to_string()  + System.Environment.NewLine;
					}
					//Suchergebnisse werden in dem Textfeld ausgabe ausgegeben
					this.tb = this.ausgabe.Buffer;
					this.tb.Text = ergebnis;

				}
					

				conn.Close();
			}
			catch (Exception ex)
			{
				//mysql-Fehlermeldungen werden ausgegeben
				Console.WriteLine("MySQL Fehlermeldung: " + ex.ToString());
			}

			//Textfeld wird geleert
			this.artikelname_text.Text = "";


		}

	}
}

