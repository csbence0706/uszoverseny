using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace uszoverseny
{
    public partial class Form1 : Form
    {
        private List<Versenyzo> resztvevok;

        public Form1()
        {
            InitializeComponent();
            resztvevok = new List<Versenyzo>();
        }

        class Versenyzo
        {
            public string Rajtszam { get; private set; }
            public string Nev { get; private set; }
            public DateTime szuletesi { get; private set; }
            public string Orszag { get; private set; }
            public TimeSpan IdoEredmeny { get; private set; }

            public Versenyzo(string rajtszam, string nev, DateTime szuletesi,
                string orszag, TimeSpan idoEredmeny)
            {
                this.Rajtszam = rajtszam;
                this.Nev = nev;
                this.szuletesi = szuletesi;
                this.Orszag = orszag;
                this.IdoEredmeny = idoEredmeny;
            }

            public override string ToString()
            {
                return Nev;
            }
        }

        private void AdatBeolvasas()
        {
            StreamReader data = new StreamReader("uszoverseny.txt");
            string adat = data.ReadLine();
            while (adat != null)
            {
                Feldolgoz(adat);
                adat = data.ReadLine();
            }
            data.Close();
        }

        private void Feldolgoz(string adat)
        {
            string[] tordelt = adat.Split(';');
            string rajtSzam = tordelt[0];
            string nev = tordelt[1];
            DateTime szuletesi = DateTime.Parse(tordelt[2]);
            string orszag = tordelt[3];
            TimeSpan idoEredmeny = TimeSpan.Parse("0:" + tordelt[4]);
            Versenyzo versenyzo = new Versenyzo(rajtSzam, nev, szuletesi, orszag, idoEredmeny);
            resztvevok.Add(versenyzo);
            elsoV.Items.Add(versenyzo);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Optional: Load data automatically on form load
            // AdatBeolvasas();
        }

        private void btnAdatBe_Click(object sender, EventArgs e)
        {
            elsoV.Items.Clear();
            resztvevok.Clear();
            AdatBeolvasas();
            btnAdatBe.Enabled = false;
            btnGyoztes.Enabled = true;
        }

        private void elsoV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (elsoV.SelectedIndex != -1)
            {
                Versenyzo versenyzo = resztvevok[elsoV.SelectedIndex];
                txtRajtszam.Text = versenyzo.Rajtszam;
                txtOrszag.Text = versenyzo.Orszag;
                txtIdoEredmeny.Text = new DateTime(versenyzo.IdoEredmeny.Ticks).ToString("mm:ss.ff");
                txtEletKor.Text = (DateTime.Now.Year - versenyzo.szuletesi.Year) + " év";
            }
        }

        private void btnGyoztes_Click(object sender, EventArgs e)
        {
            if (resztvevok.Count > 0)
            {
                TimeSpan min = resztvevok[0].IdoEredmeny;
                foreach (var versenyzo in resztvevok)
                {
                    if (versenyzo.IdoEredmeny < min)
                    {
                        min = versenyzo.IdoEredmeny;
                    }
                }
                txtGyoztesIdo.Text = new DateTime(min.Ticks).ToString("mm:ss:ff");
                rchTxtGyoztes.Clear();
                foreach (var versenyzo in resztvevok)
                {
                    if (versenyzo.IdoEredmeny == min)
                    {
                        rchTxtGyoztes.AppendText(versenyzo + "\n");
                    }
                }
            }
        }

        private void lstVersenyzok_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
