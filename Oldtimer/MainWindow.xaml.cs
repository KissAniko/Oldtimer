using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySqlConnector;



namespace Oldtimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=Oldtimer;";
        private MySqlConnection connection;
        private List<Auto> adatok;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Betoltes()
        {
            try
            {

                adatok = new List<Auto>();
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string lekerdezesSzoveg = " SELECT rendszam, szin, autok.nev AS autoNev, evjarat, ar, kategoriak.nev AS kategoriaNev" +
                                   " FROM autok " +
                                   " INNER JOIN kategoriak ON kategoriak.id = autok.kategoriaId";

                MySqlCommand lekerdez = new MySqlCommand(lekerdezesSzoveg, connection);
                MySqlDataReader olvaso = lekerdez.ExecuteReader();

                lekerdez.CommandTimeout = 10;             // időtúllépés a lekérdezésnek --> 10 mp


                while (olvaso.Read())
                {

                    string rendszam = olvaso.GetString(0);
                    string szin = olvaso.GetString(1);
                    string nev = olvaso.GetString(2);
                    int evjarat = olvaso.GetInt32(3);
                    int ar = olvaso.GetInt32(4);
                    string kategoriaNev = olvaso.GetString(5);

                    adatok.Add(new Auto(rendszam, szin, nev, evjarat, ar, kategoriaNev));
                }

                olvaso.Close();
                connection.Close();
                adatokTablazat.ItemsSource = adatok;

            }


            catch (Exception hiba)         // ez bármilyen hibát elkap
            {
                MessageBox.Show(hiba.Message);
            }

        }

//-------------------------------------------------------------------------------------------------------------------
       
        private void adatokTablazat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                    
        }
        private void adatokTablazat_Loaded_1(object sender, RoutedEventArgs e)
        {
            Betoltes();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------

        // Kiválasztott elemek törlése:
        private void Torles_Click(object sender, RoutedEventArgs e)
        {
            int kivalasztott = adatokTablazat.SelectedIndex;

            adatok.RemoveAt(kivalasztott);                    // kitörli a kiválasztott elemet.

            adatokTablazat.Items.Refresh();                  // Frissítjük az adatokat, különben törlés után nem fog
                                                             // ..kitörlődni a kiválasztott elem a táblázatból és hibát fog dobni.
                                                             // Viszont ezzel még nem törlődött az adat az adatbázisból.
                                                             // ...Ezt, a következő sorral tudjuk megtenni.


        // Mivel egy adat több táblában is szerepel, ezért úgy kell kiíratni, hogy az összesből is autómatikusan törlődjön.
        // Ehhez az idegenkulcs (foreign key) mogszorítást és a TIGGER-t kell használni. Az "Oldtimer megjegyzések.txt" fájlban
        // megtalálható a beállítás parancssora. 

            string torlesszoveg = $"DELETE FROM autok WHERE rendszam='{adatok[kivalasztott].Rendszam}'";

            connection = new MySqlConnection(connectionString);                          // Újra meg kell nyitni a kapcsolatot.
            connection.Open();
            MySqlCommand torles = new MySqlCommand(torlesszoveg, connection);

            torles.CommandTimeout = 10;                           // ez a sor nem fontos

            torles.ExecuteNonQuery();                         //  Ha nem SELECT-et használunk, vagyis lekérést, akkor ezt a
                                                              //  módosító utasítást használjuk. Ezzel is lehet futtatni a törlést.
                                                                 
            connection.Close() ;

//--------------------------------------------------------------------------------------------------------------------------------

            // Lekérjük a színek listáját és beletesszük a 'színek' comboBox-ba:

            var szinek = adatok.Select(x => new
                   { 
                          szin = x.Szin
                  })     .ToList().Distinct();
               szinLista.ItemsSource = szinek;
        }


        //---------------------------------------------------------------------------------------------------------------------------------


        // Kijelölünk egy autót, majd annak a színét a comboBoxból kiválasztott színre megváltoztatjuk.


        /*     private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {


            connection = new MySqlConnection(connectionString);
            connection.Open();
            string lekerdezesSzoveg = "SELECT nev FROM kategoriak ORDER BY nev";
            MySqlCommand lekerdez = new MySqlCommand(lekerdezesSzoveg, connection);
            try
            {
                MySqlDataReader olvaso = lekerdez.ExecuteReader();
                if (olvaso.HasRows)
                {
                    while (olvaso.Read())
                    {
                        kategoriaBevitel.Items.Add(olvaso.GetString(0));
                    }
                }
            }
            catch (Exception hiba)
            {
                MessageBox.Show(hiba.Message);
            }
            connection.Close();

        }

        */

        //--------------------------------------------------------------------------------------------------------------------------------
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem.ToString() == "felvitelTab")
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string lekerdezesSzoveg = "SELECT nev FROM kategoriak ORDER BY nev";
                MySqlCommand lekerdez = new MySqlCommand(lekerdezesSzoveg, connection);
                try
                {
                    MySqlDataReader olvaso = lekerdez.ExecuteReader();
                    if (olvaso.HasRows)
                    {
                        while (olvaso.Read())
                        {
                            kategoriaBevitel.Items.Add(olvaso.GetString(0));
                        }
                    }
                }
                catch (Exception hiba)
                {
                    MessageBox.Show(hiba.Message);
                }
                connection.Close();
            }

        }

       
    }
}
    
