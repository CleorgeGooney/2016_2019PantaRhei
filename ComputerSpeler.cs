using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Oxo
{
    public abstract class ComputerSpeler
    {
        public Speler Speler { get; set; }
        public ComputerSpeler(Speler speler) { Speler = speler; }
        public void Speel(Spel spel)
        {
            Veld veld = VerkiestVeld(spel);
            spel.Speel(veld);
        }
        public abstract Veld VerkiestVeld(Spel spel);
    }
    public class Willekeurigaard : ComputerSpeler
    {
        public Willekeurigaard(Speler speler) : base(speler) { }

        protected static Random _random = new Random();
        public override Veld VerkiestVeld(Spel spel)
        {
            //Lever een willekeurig vrij veld op;
            List<Veld> legeVelden = spel.Bord.LegeVelden();
            int willekeurigeIndex = _random.Next(0, legeVelden.Count);
            return legeVelden[willekeurigeIndex];
        }
    }
    public class Pessimist : Willekeurigaard
    {
        public Pessimist(Speler speler) : base(speler) { }

        public Speler Tegenstander { get { return (Speler == Oxo.Speler.O ? Oxo.Speler.X : Oxo.Speler.O); } }

        protected Veld BlokkerendVeld(Spel spel)
        {
            foreach (Lijn lijn in spel.Bord.Lijnen)
            {
                //Indien er op een lijn nog 1 vrij veld is, en 2 velden die door de tegenstander zijn bezet:
                if (lijn.AantalVrijeVelden() == 1 && lijn.AantalVelden(Tegenstander) == 2)
                {
                    //Lever het nog vrij veld op.
                    foreach (Veld v in lijn.Velden) if (v.IsVrij()) return v;
                    //of: return lijn.Velden.First(veld => veld.IsVrij());
                }
            }
            return null;
        }
        public override Veld VerkiestVeld(Spel spel)
        {
            //1) Probeer een reeks van de tegenstander te blokkeren:
            Veld blokkerendVeld = BlokkerendVeld(spel);
            if (blokkerendVeld != null) return blokkerendVeld;

            //2) Indien het middelste veld nog vrij is lever deze op:
            Veld middelsteVeld = spel.Bord[1, 1];
            if (middelsteVeld.IsVrij()) return middelsteVeld;

            //3) Willekeurig vrij veld opleveren:
            return base.VerkiestVeld(spel);
        }
    }
    public class Opportunist : Pessimist
    {
        public Opportunist(Speler speler) : base(speler) { }

        protected Veld WinnendVeld(Spel spel)
        {
            foreach (Lijn lijn in spel.Bord.Lijnen)
            {
                //Indien er op een lijn nog 1 vrij veld is, en 2 velden die door deze speler zijn bezet:
                if (lijn.AantalVrijeVelden() == 1 && lijn.AantalVelden(Speler) == 2)
                {
                    //Lever het nog vrij veld op.
                    foreach (Veld v in lijn.Velden) if (v.IsVrij()) return v;
                    //of: return lijn.Velden.First(veld => veld.IsVrij());
                }
            }
            return null;
        }
        public override Veld VerkiestVeld(Spel spel)
        {
            //1) Probeer zelf een reeks te maken:
            Veld winnendVeld = WinnendVeld(spel);
            if (winnendVeld != null) return winnendVeld;

            //2) Kan er geen reeks gemaakt worden => tegenstander blokkeren:
            return base.VerkiestVeld(spel);
        }
    }
    public class MathijsGLaDOS : Opportunist
    {
        //2e beurt Speler.X en 2e beurt Speler.O eigenlijk niet nodig, maar minimax is anders te traag en mist nog een duel
        public MathijsGLaDOS(Speler speler) : base(speler) { }
        public override Veld VerkiestVeld(Spel spel)
        {
            #region checks en variabelen
            int aantalIngenomen = AantalIngenomen(spel.Bord);
            List<Lijn> randLijnen = GetRandLijnen(spel.Bord);

            Veld doel = null;

            doel = WinnendVeld(spel);
            if (doel != null) return doel;

            doel = BlokkerendVeld(spel);
            if (doel != null) return doel;
            #endregion

            //SPELER 1
            if (Speler == Speler.O)
            {
                #region Eerste beurt speler 1
                //neemt een hoek (hoogste kans), center of rand
                if (aantalIngenomen == 0)
                {
                    int rnd = RandomInt();
                    if (rnd <= 15)
                    {
                        doel = spel.Bord[4];
                        return doel;
                    }
                    else if (rnd >= 85)
                    {
                        doel = RandomRand(spel.Bord);
                        return doel;
                    }
                    doel = RandomHoek(spel.Bord);
                    return doel;
                }
                #endregion

                #region Tweede beurt speler 1
                else if (aantalIngenomen == 2)
                {
                    Veld eigenMove = VeldenSpeler(spel.Bord, Speler)[0];
                    Veld tegenstanderMove = VeldenSpeler(spel.Bord, Tegenstander)[0];

                    #region Speler 1 speelde hoek
                    if (IsHoek(eigenMove, spel.Bord))
                    {
                        Veld gespiegeldeEersteMove = GespiegeldeHoek(spel.Bord, eigenMove);
                        #region Tegenstander speelde hoek of rand
                        //x speelt geen center
                        if (tegenstanderMove != spel.Bord[1, 1])
                        {
                            Veld excludeHoek = null;

                            foreach (Lijn lijn in randLijnen)
                            {
                                if (lijn.AantalVrijeVelden() == 1)
                                {
                                    if (lijn.Velden[0].Speler == Speler)
                                    {
                                        excludeHoek = lijn.Velden[2];
                                    }
                                    else excludeHoek = lijn.Velden[0];
                                }
                            }
                            do
                            {
                                doel = RandomHoek(spel.Bord);
                            } while (doel == excludeHoek || doel == gespiegeldeEersteMove || !doel.IsVrij());
                            return doel;
                        }
                        #endregion
                        #region Tegenstander speelde center
                        //x speelt center
                        else
                        {
                            int rnd = RandomInt();
                            if (rnd <= 10)
                            {
                                foreach (Lijn lijn in randLijnen)
                                {
                                    if (lijn.AantalVrijeVelden() == 3)
                                    {
                                        doel = lijn.Velden[1];
                                        if (doel.IsVrij()) return doel;
                                    }
                                }
                            }
                            return gespiegeldeEersteMove;
                        }
                        #endregion
                    }
                    #endregion
                    #region Speler 1 speelde center
                    else if (IsCenter(eigenMove))
                    {
                        #region Tegenstander speelde hoek
                        //gespiegelde hoek nemen, daarna nog vorkpoging
                        if (IsHoek(tegenstanderMove, spel.Bord))
                        {
                            Veld gespiegeldeHoek = GespiegeldeHoek(spel.Bord, tegenstanderMove);
                            return gespiegeldeHoek;
                        }
                        #endregion
                        #region Tegenstander speelde rand
                        else
                        {
                            do
                            {
                                doel = RandomHoek(spel.Bord);
                            } while (!doel.IsVrij());

                            return doel;
                        }
                        #endregion
                    }
                    #endregion
                    #region Speler 1 speelde rand
                    else if (IsRand(eigenMove, spel.Bord))
                    {
                        #region Tegenstander speelde rand
                        if (IsRand(tegenstanderMove, spel.Bord))
                        {
                            //tegenstander speelt near rand
                            if (!ZijnOpposRanden(tegenstanderMove, eigenMove, spel.Bord))
                            {
                                doel = Vork(tegenstanderMove, eigenMove, spel.Bord);
                                if (doel.IsVrij()) return doel;
                            }
                            //tegenstander speelt opposite rand
                            else
                            {
                                foreach (Lijn lijn in randLijnen)
                                {
                                    foreach (Veld v in lijn.Velden)
                                    {
                                        if (v.Speler == Tegenstander)
                                        {
                                            //2 mogelijke hoeken, neem ééntje at random
                                            int rnd = RandomInt();
                                            if (rnd >= 50)
                                            {
                                                doel = lijn.Velden[0];
                                                if (doel.IsVrij()) return doel;
                                            }
                                            else
                                            {
                                                doel = lijn.Velden[2];
                                                if (doel.IsVrij()) return doel;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        #region Tegenstander speelde hoek
                        else if (IsHoek(tegenstanderMove, spel.Bord))
                        //zoek eerst naar near corner
                        {
                            foreach (Lijn lijn in randLijnen)
                            {
                                if (lijn.AantalVrijeVelden() == 1)//if true=> near corner
                                {
                                    foreach (Veld v in lijn.Velden)
                                    {
                                        if (v.IsVrij())
                                        {
                                            doel = GespiegeldeHoek(spel.Bord, v);
                                            if (doel.IsVrij()) return doel;
                                        }
                                    }
                                }
                            }
                            //niets gevonden? tegenstander speelt far corner 
                            doel = GespiegeldeHoek(spel.Bord, Vork(eigenMove, tegenstanderMove, spel.Bord));
                            if (doel.IsVrij()) return doel;
                        }
                        #endregion
                        #region Tegenstander speelde center
                        int rnd2 = RandomInt();
                        //random hoek
                        if (rnd2 <= 10)
                        {
                            foreach (Lijn lijn in randLijnen)
                            {
                                if (lijn.Velden[1].Speler == Speler)
                                {
                                    Veld excludeHoek1 = lijn.Velden[0];
                                    Veld excludeHoek2 = lijn.Velden[2];
                                    do
                                    {
                                        doel = RandomHoek(spel.Bord);
                                    } while (doel == excludeHoek1 || doel == excludeHoek2);
                                    return doel;
                                }
                            }
                        }
                        //random rand die niet opposite is van eigen move
                        else
                        {
                            do
                            {
                                doel = RandomRand(spel.Bord);
                            } while (!doel.IsVrij() || ZijnOpposRanden(doel, eigenMove, spel.Bord));
                            return doel;
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            //SPELER 2
            else
            {
                #region Eerste beurt speler 2
                if (aantalIngenomen == 1)
                {
                    Veld move1 = VeldenSpeler(spel.Bord, Tegenstander)[0];
                    if (IsCenter(move1))
                    {
                        doel = RandomHoek(spel.Bord);
                        return doel;
                    }
                    else
                    {
                        doel = spel.Bord[4];
                        return doel;
                    }
                }
                #endregion

                #region Tweede beurt speler 2
                else if (aantalIngenomen == 3)
                {
                    Veld move1 = VeldenSpeler(spel.Bord, Tegenstander)[0];
                    Veld move2 = VeldenSpeler(spel.Bord, Tegenstander)[1];
                    //al op vrij checken in method
                    //o neemt 2 hoeken
                    if (IsHoek(move1, spel.Bord) && IsHoek(move2, spel.Bord))
                    {
                        do
                        {
                            doel = RandomRand(spel.Bord);
                        } while (!doel.IsVrij());
                        return doel;
                    }
                    //o neemt center en hoek
                    else if (IsCenter(move1) && IsHoek(move2, spel.Bord) || IsCenter(move2) && IsHoek(move1, spel.Bord))
                    {
                        do
                        {
                            doel = RandomHoek(spel.Bord);
                        } while (!doel.IsVrij());
                        return doel;
                    }
                    //o neemt 2 randen
                    else if (IsRand(move1, spel.Bord) && IsRand(move2, spel.Bord))
                    {
                        if (ZijnOpposRanden(move1, move2, spel.Bord))
                        {
                            do
                            {
                                doel = RandomHoek(spel.Bord);
                            } while (!doel.IsVrij());//checks op is vrij en while lus in method, 
                            return doel;
                        }
                        else
                        {
                            doel = Vork(move1, move2, spel.Bord);
                            if (doel.IsVrij()) return doel;
                        }
                    }
                    //o neemt rand en hoek
                    else if (IsRand(move1, spel.Bord) && IsHoek(move2, spel.Bord) || IsHoek(move1, spel.Bord) && IsRand(move2, spel.Bord))
                    {
                        Veld hoek;
                        Veld rand;
                        hoek = IsHoek(move1, spel.Bord) ? move1 : move2;
                        rand = IsRand(move1, spel.Bord) ? move1 : move2;
                        doel = Vork(rand, hoek, spel.Bord);
                        if (doel.IsVrij()) return doel;
                    }
                }
                #endregion
            }

            #region "Overige beurten of geen returns"
            doel = MinimaxVeld(spel.Bord);
            if (doel == null || (!doel.IsVrij())) return base.VerkiestVeld(spel);
            return doel;
            #endregion
        }

        protected int AantalIngenomen(Bord bord)
        {
            int aantalIngenomen = 0;
            foreach (Veld v in bord) if (v.Speler != null) aantalIngenomen++;
            return aantalIngenomen;
        }
        protected List<Lijn> GetRandLijnen(Bord bord)
        {
            List<Lijn> randLijnen = new List<Lijn>();
            randLijnen.Add(bord.Lijnen[0]);
            randLijnen.Add(bord.Lijnen[2]);
            randLijnen.Add(bord.Lijnen[3]);
            randLijnen.Add(bord.Lijnen[5]);
            return randLijnen;
        }
        protected Veld GespiegeldeHoek(Bord bord, Veld eigenMove)
        {
            Veld gespiegeldeHoek = null;
            for (int iLijnen = 6; iLijnen < 8; iLijnen++)
            {
                for (int iVeld = 0; iVeld < 3; iVeld++)
                {
                    if (bord.Lijnen[iLijnen].Velden[iVeld] == eigenMove)
                    {
                        if (iVeld == 2) gespiegeldeHoek = bord.Lijnen[iLijnen].Velden[0];
                        else gespiegeldeHoek = bord.Lijnen[iLijnen].Velden[2];
                    }
                }
            }
            return gespiegeldeHoek;
        }
        protected List<Veld> VeldenSpeler(Bord bord, Speler speler)
        {
            List<Veld> veldenSpeler = new List<Veld>();
            foreach (Veld veld in bord) if (veld.Speler == speler) veldenSpeler.Add(veld);
            return veldenSpeler;
        }
        protected Veld Vork(Veld randofhoek, Veld hoek, Bord bord)
        {
            if (randofhoek.Kolom == 2)
            {
                return bord[randofhoek.Rij - 1, hoek.Kolom - 1];
            }
            else
            {
                return bord[hoek.Rij - 1, randofhoek.Kolom - 1];
            }
        }
        protected bool IsCenter(Veld veld)
        {
            if (veld.Kolom == 2 && veld.Rij == 2) return true;
            else return false;
        }
        protected bool IsHoek(Veld veld, Bord bord)
        {
            if (veld == bord[0, 0] || veld == bord[2, 2] || veld == bord[2, 0] || veld == bord[0, 2])
                return true;
            return false;
        }
        protected bool IsRand(Veld veld, Bord bord)
        {
            if (veld == bord[0, 1] || veld == bord[1, 0] || veld == bord[1, 2] || veld == bord[2, 1])
                return true;
            return false;
        }
        protected bool ZijnOpposRanden(Veld veld1, Veld veld2, Bord bord)
        {
            if (veld1 == bord[0, 1] && veld2 == bord[2, 1] || veld1 == bord[1, 0] && veld2 == bord[1, 2] ||
                veld2 == bord[0, 1] && veld1 == bord[2, 1] || veld2 == bord[1, 0] && veld1 == bord[1, 2])
                return true;
            return false;
        }
        protected int RandomInt()
        {
            return _random.Next(0, 101);
        }
        protected Veld RandomRand(Bord bord)
        {
            int random = _random.Next(0, 4);
            switch (random)
            {
                case 0:
                    return bord[1];
                case 1:
                    return bord[3];
                case 2:
                    return bord[5];
                default: return bord[7];
            }
        }
        protected Veld RandomHoek(Bord bord)
        {
            int random = _random.Next(0, 4);
            switch (random)
            {
                case 0:
                    return bord[0];
                case 1:
                    return bord[2];
                case 2:
                    return bord[6];
                default: return bord[8];
            }
        }
        //Brute force 
        protected Strategie Minimax(Bord bord, Speler speler, int diepte)
        {
            var kopieBord = new Bord();
            kopieBord = bord;
            if (CheckOpWinst(kopieBord, Speler.O))
            {
                return new Strategie(null, 100 + diepte);
            }
            else if (CheckOpWinst(kopieBord, Speler.X))
            {
                return new Strategie(null, -100 - diepte);
            }
            else if (CheckOfVol(kopieBord)) { return new Strategie(null, 0); }

            else
            {
                diepte = diepte + 10;
                if (speler == Speler.O)
                {
                    Strategie besteStrategie = new Strategie(null, -100000);

                    foreach (Veld v in kopieBord)
                    {
                        if (v.IsVrij())
                        {
                            v.Speler = speler;
                            Strategie strategie = new Strategie(v, Minimax(kopieBord, Speler.X, diepte).Score);
                            if (strategie.Score > besteStrategie.Score)
                            {
                                besteStrategie = strategie;
                            }
                            //als er al een winnende move is ontdekt, hogere score toekennen als "broer" ook winst oplevert
                            else if (strategie.Score >= 100)
                            {
                                besteStrategie.Score = besteStrategie.Score + 50;
                            }
                            v.Speler = null;
                        }
                    }
                    return besteStrategie;
                }
                else
                {
                    Strategie besteStrategie = new Strategie(null, 100000);
                    foreach (Veld v in kopieBord)
                    {
                        if (v.IsVrij())
                        {
                            v.Speler = speler;
                            Strategie strategie = new Strategie(v, Minimax(kopieBord, Speler.O, diepte).Score);
                            if (strategie.Score < besteStrategie.Score)
                            {
                                besteStrategie = strategie;
                            }
                            else if (strategie.Score <= -100)
                            {
                                besteStrategie.Score = besteStrategie.Score - 50;
                            }
                            v.Speler = null;
                        }
                    }
                    return besteStrategie;
                }
            }
        }
        protected Veld MinimaxVeld(Bord bord)
        {
            Veld doel = null;
            Strategie besteStrategie = Minimax(bord, Speler, 0);
            int gekozenRij = besteStrategie.Veld.Rij;
            int gekozenKolom = besteStrategie.Veld.Kolom;
            foreach (Veld v in bord)
            {
                if (v.Kolom == gekozenKolom && v.Rij == gekozenRij)
                {
                    doel = v;
                }
            }
            return doel;
        }
        protected bool CheckOfVol(Bord bord)
        {
            int ingenomenPosities = 0;
            foreach (Veld v in bord)
            {
                if (v.Speler != null) ingenomenPosities++;
            }
            if (ingenomenPosities == 9) return true;
            return false;
        }
        protected bool CheckOpWinst(Bord bord, Speler speler)
        {
            foreach (Lijn lijn in bord.Lijnen)
            {
                if (lijn.IsReeks(speler) == true) return true;
            }
            return false;
        }
        protected class Strategie
        {
            public int Score { get; set; }
            public Veld Veld { get; set; }
            public Strategie(Veld veld, int score)
            {
                Veld = veld;
                Score = score;
            }
        }
    }
}


